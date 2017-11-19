#region Using

using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;
using NMock.Internal;
using NMock.Monitoring;

#endregion

namespace NMock.Proxy.Castle
{
	internal class MockObjectInterceptor : InterceptorBase, IInterceptor
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MockObjectInterceptor"/> class.
		/// </summary>
		/// <param name="mockFactory">The mockFactory.</param>
		/// <param name="mockedType">Type of the mocked.</param>
		/// <param name="name">The name.</param>
		/// <param name="mockStyle">The mock style.</param>
		internal MockObjectInterceptor(MockFactory mockFactory, CompositeType mockedType, string name, MockStyle mockStyle)
			: base(mockFactory, mockedType, name, mockStyle)
		{
		}

		#region IInterceptor Members

		public void Intercept(IInvocation invocation)
		{
			if(InvocationRecorder.Recording)
			{
				InvocationRecorder.Current.Invocation = new Invocation(invocation.Method, invocation.Arguments);
				return;
			}

			// Check for calls to basic NMock infrastructure
			if (MockObjectMethods.ContainsKey(invocation.Method))
			{
				try
				{
					invocation.ReturnValue = invocation.Method.Invoke(this, invocation.Arguments);
				}
				catch (TargetInvocationException tie)
				{
#if !SILVERLIGHT
					// replace stack trace with original stack trace
					FieldInfo remoteStackTraceString = typeof (Exception).GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);
					remoteStackTraceString.SetValue(tie.InnerException, tie.InnerException.StackTrace + Environment.NewLine);

					throw tie.InnerException;
#else
					throw;
#endif
				}

				return;
			}

			// Ok, this call is targeting a member of the mocked class
			object invocationTarget;
			if (MockedTypes.PrimaryType == invocation.Method.DeclaringType && invocation.InvocationTarget != null)
			{
				invocationTarget = invocation.InvocationTarget;
			}
			else
			{
				invocationTarget = invocation.Proxy;
			}

			var invocationForMock = new Invocation(invocationTarget, invocation.Method, invocation.Arguments);

			if (ShouldCallInvokeImplementation(invocationForMock))
			{
				invocation.Proceed();

				if (!MockFactory.HasExpectationFor(invocationForMock))
					return;
			}

			invocation.ReturnValue = ProcessCallAgainstExpectations(invocationForMock);
		}

		#endregion

	}
}
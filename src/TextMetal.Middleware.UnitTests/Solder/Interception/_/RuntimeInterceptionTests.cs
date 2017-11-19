/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Reflection;

using NUnit.Framework;

using TextMetal.Middleware.Solder.Interception;
using TextMetal.Middleware.UnitTests.TestingInfrastructure;

namespace TextMetal.Middleware.UnitTests.Solder.Interception._
{
	[TestFixture]
	public class RuntimeInterceptionTests
	{
		#region Constructors/Destructors

		public RuntimeInterceptionTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCloneInvokeTest()
		{
			MockRuntimeInterception mockRuntimeInterception;
			IRuntimeInvocation mockRuntimeInvocation;
			IRuntimeContext mockRuntimeContext;
			object proxyInstance;
			Type proxiedType;
			MethodInfo invokedMethodInfo;
			object[] invocationParameters;
			object returnValue;

			proxyInstance = new object();
			proxiedType = typeof(IMockCloneable);
			invokedMethodInfo = typeof(IMockCloneable).GetMethod("Clone");
			invocationParameters = new object[] { };
			mockRuntimeInvocation = new RuntimeInvocation(proxyInstance, proxiedType, invokedMethodInfo, invocationParameters, null);
			mockRuntimeContext = null;

			mockRuntimeInterception = new MockRuntimeInterception();
			mockRuntimeInterception.Invoke(mockRuntimeInvocation, mockRuntimeContext);
			returnValue = mockRuntimeInvocation.InvocationReturnValue;

			Assert.IsNull(returnValue);
			Assert.AreEqual("IMockCloneable::Clone", mockRuntimeInterception.LastOperationName);
		}

		[Test]
		public void ShouldCreateTest__todo_mock_invocation_and_context()
		{
			Assert.Ignore("TODO: This test case has not been implemented yet.");
		}

		[Test]
		public void ShouldDisposeInvokeTest()
		{
			MockRuntimeInterception mockRuntimeInterception;
			IRuntimeInvocation mockRuntimeInvocation;
			IRuntimeContext mockRuntimeContext;
			object proxyInstance;
			Type proxiedType;
			MethodInfo invokedMethodInfo;
			object[] invocationParameters;
			object returnValue;

			proxyInstance = new object();
			proxiedType = typeof(IDisposable);
			invokedMethodInfo = typeof(IDisposable).GetMethod("Dispose");
			invocationParameters = new object[] { };
			mockRuntimeInvocation = new RuntimeInvocation(proxyInstance, proxiedType, invokedMethodInfo, invocationParameters, null);
			mockRuntimeContext = null;

			mockRuntimeInterception = new MockRuntimeInterception();
			mockRuntimeInterception.Invoke(mockRuntimeInvocation, mockRuntimeContext);
			returnValue = mockRuntimeInvocation.InvocationReturnValue;

			Assert.IsNull(returnValue);
			Assert.IsTrue(mockRuntimeInterception.Disposed);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullInputParameterInvokeTest()
		{
			MockRuntimeInterception mockRuntimeInterception;
			IRuntimeInvocation mockRuntimeInvocation;
			IRuntimeContext mockRuntimeContext;
			object proxyInstance;
			Type proxiedType;
			MethodInfo invokedMethodInfo;
			object[] invocationParameters;

			proxyInstance = new object();
			proxiedType = typeof(IMockObject);
			invokedMethodInfo = typeof(IMockObject).GetMethod("ToString");
			invocationParameters = null;
			mockRuntimeInvocation = new RuntimeInvocation(proxyInstance, proxiedType, invokedMethodInfo, invocationParameters, null);
			mockRuntimeContext = null;

			mockRuntimeInterception = new MockRuntimeInterception();
			mockRuntimeInterception.Invoke(mockRuntimeInvocation, mockRuntimeContext);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullInvokingTypeInvokeTest()
		{
			MockRuntimeInterception mockRuntimeInterception;
			IRuntimeInvocation mockRuntimeInvocation;
			IRuntimeContext mockRuntimeContext;
			object proxyInstance;
			Type proxiedType;
			MethodInfo invokedMethodInfo;
			object[] invocationParameters;

			proxyInstance = new object();
			proxiedType = null;
			invokedMethodInfo = typeof(IMockObject).GetMethod("ToString");
			invocationParameters = new object[] { };
			mockRuntimeInvocation = new RuntimeInvocation(proxyInstance, proxiedType, invokedMethodInfo, invocationParameters, null);
			mockRuntimeContext = null;

			mockRuntimeInterception = new MockRuntimeInterception();
			mockRuntimeInterception.Invoke(mockRuntimeInvocation, mockRuntimeContext);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTargetInstanceInvokeTest()
		{
			MockRuntimeInterception mockRuntimeInterception;
			IRuntimeInvocation mockRuntimeInvocation;
			IRuntimeContext mockRuntimeContext;
			object proxyInstance;
			Type proxiedType;
			MethodInfo invokedMethodInfo;
			object[] invocationParameters;

			proxyInstance = null;
			proxiedType = typeof(IMockObject);
			invokedMethodInfo = typeof(IMockObject).GetMethod("ToString");
			invocationParameters = new object[] { };
			mockRuntimeInvocation = new RuntimeInvocation(proxyInstance, proxiedType, invokedMethodInfo, invocationParameters, null);
			mockRuntimeContext = null;

			mockRuntimeInterception = new MockRuntimeInterception();
			mockRuntimeInterception.Invoke(mockRuntimeInvocation, mockRuntimeContext);
		}

		[Test]
		public void ShouldNotFailOnDoubleDisposeTest()
		{
			MockRuntimeInterception mockRuntimeInterception;

			mockRuntimeInterception = new MockRuntimeInterception();

			Assert.IsNotNull(mockRuntimeInterception);

			mockRuntimeInterception.Dispose();
			mockRuntimeInterception.Dispose();
		}

		[Test]
		public void ShouldToStringInvokeTest()
		{
			MockRuntimeInterception mockRuntimeInterception;
			IRuntimeInvocation mockRuntimeInvocation;
			IRuntimeContext mockRuntimeContext;
			object proxyInstance;
			Type proxiedType;
			MethodInfo invokedMethodInfo;
			object[] invocationParameters;
			object returnValue;

			proxyInstance = new object();
			proxiedType = typeof(IMockObject);
			invokedMethodInfo = typeof(IMockObject).GetMethod("ToString");
			invocationParameters = new object[] { };
			mockRuntimeInvocation = new RuntimeInvocation(proxyInstance, proxiedType, invokedMethodInfo, invocationParameters, null);
			mockRuntimeContext = null;

			mockRuntimeInterception = new MockRuntimeInterception();
			mockRuntimeInterception.Invoke(mockRuntimeInvocation, mockRuntimeContext);
			returnValue = mockRuntimeInvocation.InvocationReturnValue;

			Assert.AreEqual(typeof(IMockObject).FullName, returnValue);
		}

		#endregion
	}
}
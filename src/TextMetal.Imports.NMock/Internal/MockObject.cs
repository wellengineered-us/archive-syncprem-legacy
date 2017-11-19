#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NMock.Monitoring;
using NMock.Proxy;

#endregion

/* ^ */ using System.Linq; /* dpbullington@gmail.com ^ */

namespace NMock.Internal
{
	/// <summary>
	/// 
	/// </summary>
	internal class MockObject : IInvokable, IMockObject
	{
		/// <summary>
		/// Results that have been explicitly assigned via a call to a property setter.
		/// These will be returned for all subsequent calls to the matching property getter.
		/// </summary>
		private readonly Dictionary<string, object> assignedPropertyResults = new Dictionary<string, object>();

		/// <summary>
		/// Stores the event handlers that could be added to the mock object.
		/// </summary>
		private readonly Dictionary<string, List<Delegate>> eventHandlers;

		/// <summary>
		/// Results that have been generated for methods or property getters.
		/// These will be returned for all subsequent calls to the same member.
		/// </summary>
		private readonly Dictionary<MethodInfo, object> rememberedMethodResults = new Dictionary<MethodInfo, object>();

		private readonly Dictionary<string, object> rememberedResults = new Dictionary<string, object>();

		private Dictionary<string, object> _interceptedValues;
		Dictionary<string, object> IMockObject.InterceptedValues
		{
			get { return _interceptedValues ?? (_interceptedValues = new Dictionary<string, object>()); }
			set { _interceptedValues = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MockObject"/> class.
		/// </summary>
		/// <param name="mockFactory">The mockFactory.</param>
		/// <param name="mockedType">Type of the mocked.</param>
		/// <param name="name">The name.</param>
		/// <param name="mockStyle">The mock style.</param>
		protected MockObject(MockFactory mockFactory, CompositeType mockedType, string name, MockStyle mockStyle)
		{
			MockFactory = mockFactory;
			MockStyle = mockStyle;
			MockName = name;
			eventHandlers = new Dictionary<string, List<Delegate>>();
			MockedTypes = mockedType;
		}

		protected MockFactory MockFactory { get; private set; }

		/// <summary>
		/// Gets the mock style of this mock.
		/// </summary>
		protected MockStyle MockStyle { get; private set; }

		#region IInvokable Members

		void IInvokable.Invoke(Invocation invocation)
		{
			switch (MockStyle)
			{
				case MockStyle.Default:
				case MockStyle.Transparent:
					try
					{
						//call up the the factory to route this invocation
						MockFactory.Dispatch(invocation);
					}
					catch (UnexpectedInvocationException)
					{
						if (IgnoreUnexpectedInvocations)
						{
							//clear the factory expectation
							MockFactory.ClearException();
						}
						else
						{
							throw;
						}
					}
					break;

				case MockStyle.Stub:
					{
						if (MockFactory.HasExpectationForIgnoringIsActive(invocation))
						{
							goto case MockStyle.Default;
						}

						// check whether we already have a value for this call
						object result;

						if (invocation.IsPropertySetAccessor)// remember values set in a property setter
						{
							if (assignedPropertyResults.ContainsKey(invocation.MethodName)) //TODO: stubs don't support indexers! need Item[int] or Item[string,int]
							{
								assignedPropertyResults[invocation.MethodName] = invocation.Parameters[0];
							}
							else
							{
								assignedPropertyResults.Add(invocation.MethodName, invocation.Parameters[0]);
							}

							return;
						}
						else if (invocation.IsEventAccessor)
						{
							ProcessEventHandlers(invocation);
							return;
						}
						else if (invocation.IsPropertyGetAccessor && assignedPropertyResults.ContainsKey(invocation.MethodName))
						{
							result = assignedPropertyResults[invocation.MethodName];
						}
						else if (rememberedMethodResults.ContainsKey(invocation.Method))
						{
							result = rememberedMethodResults[invocation.Method];
						}
						else
						{
							result = GetStubResult(invocation);
							rememberedMethodResults.Add(invocation.Method, result);
						}

						if (result != Missing.Value)
						{
							invocation.Result = result;
						}

						break;
					}
				case MockStyle.RecursiveStub:
					{
						if (MockFactory.HasExpectationFor(invocation))
						{
							goto case MockStyle.Default;
						}

						object result;
						if (rememberedResults.ContainsKey(invocation.MethodName))
						{
							result = rememberedResults[invocation.MethodName];
						}
						else
						{
							result = GetStubResult(invocation);
							rememberedResults.Add(invocation.MethodName, result);
						}

						if (result != Missing.Value)
						{
							invocation.Result = result;
						}

						break;
					}
			}
		}

		#endregion

		#region IMockObject Members

		public string MockName { get; private set; }

		public CompositeType MockedTypes { get; private set; }

		public IList<MethodInfo> GetMethodsMatching(Matcher methodMatcher)
		{
			return MockedTypes.GetMatchingMethods(methodMatcher, false);
		}

		public void AddExpectation(IExpectation expectation)
		{
			MockFactory.AddExpectation(expectation);
		}

		public void ProcessEventHandlers(Invocation invocation)
		{
			var handler = invocation.Parameters[0] as Delegate;
			var name = invocation.MethodName;

			if (name.StartsWith(Constants.ADD))
			{
				AddEventHandler(name.Substring(Constants.ADD.Length), handler);
			}
			else if (name.StartsWith(Constants.REMOVE))
			{
				RemoveEventHandler(name.Substring(Constants.REMOVE.Length), handler);
			}
		}

		public void AddEventHandler(string eventName, Delegate handler)
		{
			if (eventHandlers.ContainsKey(eventName))
			{
				var handlers = eventHandlers[eventName];

				if (!handlers.Contains(handler))
				{
					handlers.Add(handler);
				}
			}
			else
			{
				eventHandlers.Add(eventName, new List<Delegate> { handler });
			}
		}

		public void RemoveEventHandler(string eventName, Delegate handler)
		{
			if (eventHandlers.ContainsKey(eventName))
			{
				List<Delegate> handlers = eventHandlers[eventName];
				handlers.Remove(handler);

				if (handlers.Count == 0)
				{
					eventHandlers.Remove(eventName);
				}
			}
		}

		public void RaiseEvent(string eventName, params object[] args)
		{
			if (eventName.StartsWith(Constants.ADD))
				eventName = eventName.Substring(Constants.ADD.Length);

			if (eventHandlers.ContainsKey(eventName))
			{
				IEnumerable handlers = eventHandlers[eventName];
				// copy handlers before invocation to prevent colection modified exception if event handler is removed within event handler itself.
				List<Delegate> delegates = new List<Delegate>();
				foreach (Delegate handler in handlers)
				{
					delegates.Add(handler);
				}

				DynamicInvoke(delegates, args);
			}
		}

		private void DynamicInvoke(List<Delegate> delegates, object[] args)
		{
			foreach (Delegate d in delegates)
			{
				try
				{
					d.DynamicInvoke(args);
				}
				catch (TargetInvocationException tie)
				{
#if !SILVERLIGHT
					// replace stack trace with original stack trace
					FieldInfo remoteStackTraceString = typeof(Exception).GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);
					remoteStackTraceString.SetValue(tie.InnerException, tie.InnerException.StackTrace + Environment.NewLine);
					throw tie.InnerException;
#else
						throw;
#endif
				}
			}
		}

		#endregion

		public override string ToString()
		{
			return MockName;
		}

		/// <summary>
		/// Gets the default result for an invocation.
		/// </summary>
		/// <param name="invocation">The invocation.</param>
		/// <returns>The default value to return as result of the invocation. 
		/// <see cref="Missing.Value"/> if no default value was provided.</returns>
		private object GetStubResult(Invocation invocation)
		{
			Type returnType = invocation.MethodReturnType;
			/* ^ */ var _returnTypeInfo = returnType.GetTypeInfo(); /* dpbullington@gmail.com ^ */

			// void method
			if (returnType == typeof(void))
			{
				return Missing.Value;
			}

			// see if developer provides a return value
			object returnValue = MockFactory.ResolveType(invocation.Receiver, returnType);

			if (returnValue != Missing.Value)
			{
				return returnValue;
			}

			if (/* ^ */ _returnTypeInfo.IsValueType /* dpbullington@gmail.com ^ */)
			{
				// use default contructor for value types
				return Activator.CreateInstance(returnType);
			}

			if (returnType == typeof(string))
			{
				// string empty for strings
				return string.Empty;
			}

			if (/* ^ */ _returnTypeInfo.IsClass && _returnTypeInfo.ImplementedInterfaces.Any(t => t == typeof(IEnumerable)) /* dpbullington@gmail.com ^ */)
			{
				// for enumerables (List, Dictionary) we create an empty object
				return Activator.CreateInstance(returnType);
			}

			if (/* ^ */ _returnTypeInfo.IsSealed /* dpbullington@gmail.com ^ */)
			{
				// null for sealed classes
				return null;
			}

			// a mock for interfaces and all cases no covered above
			return MockFactory.NewMock(returnType, GetMemberName(invocation), MockFactory.GetDependencyMockStyle(invocation.Receiver, returnType) ?? MockStyle);
		}

		/// <summary>
		/// Gets the name of the member to be used as the name for a mock returned an a call to a stub.
		/// </summary>
		/// <param name="invocation">The invocation.</param>
		/// <returns>Name of the mock created as a result value on a call to a stub.</returns>
		private string GetMemberName(ISelfDescribing invocation)
		{
			var writer = new StringWriter();
			invocation.DescribeTo(writer);

			var name = writer.ToString();
			return name.Replace(Environment.NewLine, string.Empty);
		}

		public bool IgnoreUnexpectedInvocations { get; set; }
	}
}
#region Using

using System;
using System.Diagnostics;
using System.IO;
using NMock.Internal;
using NMock.Monitoring;
using NMock.Syntax;

#endregion

/* ^ */ using System.Reflection; /* dpbullington@gmail.com ^ */

namespace NMock
{
	/// <summary>
	/// Base class for Invoker classes that raise events.
	/// </summary>
	public abstract class EventInvokerBase : ICommentSyntax
	{
		private IActionSyntax Expectation { get; set; }
		private string EventName { get; set; }

		/// <summary>
		/// Holds a reference to the delegate that will be invoked.
		/// </summary>
		private Delegate Handler;

		internal EventInvokerBase(string eventName, IActionSyntax expectation, bool isDelegate)
		{
			EventName = eventName;
			Expectation = expectation;
			Expectation.Will(Hookup(isDelegate));
		}

		#region ICommentSyntax Members

		/// <summary>
		/// Adds a comment for the expectation that is added to the error message if the expectation is not met.
		/// </summary>
		/// <param name="comment">The comment that is shown in the error message if this expectation is not met.
		/// You can describe here why this expectation has to be met.</param>
		public IVerifyableExpectation Comment(string comment)
		{
			return Expectation.Comment(comment);
		}

		#endregion

		#region IVerifyableExpectation

		public void Assert()
		{
			Expectation.Assert();
		}

		#endregion

		/// <summary>
		/// Hooks up with the action that will be taken once a handler is added to the event.
		/// </summary>
		/// <returns>The action to hook the incoming handler to the event.</returns>
		[DebuggerStepThrough]
		private IAction Hookup(bool isDelegate)
		{
			return new MockEventHookup(this, isDelegate);
		}

		private void Initialize(Delegate handler)
		{
			Handler = handler;
		}

		/// <summary>
		/// Raises the event that created the expectations.
		/// </summary>
		/// <param name="args">Arguments for the event.</param>
		protected void RaiseEvent(params object[] args)
		{
			var builder = (ExpectationBuilder)Expectation;
			var mockObject = builder.MockObject;

			/* ^ */ var _methodInfo = Handler.GetMethodInfo(); /* dpbullington@gmail.com ^ */

			/* ^ */ var parameterCount = _methodInfo.GetParameters().Length; /* dpbullington@gmail.com ^ */
			if (parameterCount != args.Length)
			{
				string eventName;
				if (EventName.Contains(Constants.ADD))
					eventName = EventName.Substring(Constants.ADD.Length);
				else if (EventName.Contains(Constants.REMOVE))
					eventName = EventName.Substring(Constants.REMOVE.Length);
				else
					eventName = EventName;

				throw new InvalidOperationException(eventName + " expects " + parameterCount + " parameter(s) but Invoke was supplied " + args.Length + " parameter(s).");
			}

			mockObject.RaiseEvent(EventName, args);
		}

		#region private

		private class MockEventHookup : IAction
		{
			private readonly EventInvokerBase invoker;
			private readonly bool isDelegateBinding;

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="invoker"></param>
			/// <param name="isDelegate"></param>
			public MockEventHookup(EventInvokerBase invoker, bool isDelegate)
			{
				this.invoker = invoker;
				isDelegateBinding = isDelegate;
			}

			#region IAction Members

			/// <summary>
			/// Invokes this object.
			/// </summary>
			/// <param name="invocation">The invocation.</param>
			void IAction.Invoke(Invocation invocation)
			{
				var eventHandler = invocation.Parameters[0] as Delegate;

				if (eventHandler != null)
				{
					invoker.Initialize(eventHandler);
					return;
				}

				throw new ArgumentNullException(string.Concat(@"invocation.", @"Parameters[0]"), string.Format("Event handler parameter is of the wrong type: {0}. EventHandler type expected.", invocation.Parameters[0].GetType()));
			}

			/// <summary>
			/// Describes this object.
			/// </summary>
			/// <param name="writer">The text writer the description is added to.</param>
			void ISelfDescribing.DescribeTo(TextWriter writer)
			{
				string bindingKind = "n event";

				if (isDelegateBinding)
					bindingKind = " delegate";

				writer.Write(string.Format("bind a{0}, returning an invoker.", bindingKind));
			}

			#endregion
		}

		#endregion
	}
}
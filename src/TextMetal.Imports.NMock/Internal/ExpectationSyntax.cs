using System;
using NMock.Actions;
using NMock.Matchers;
using NMock.Syntax;

namespace NMock.Internal
{
	internal class ExpectationSyntax<T> : IExpectMatchSyntax where T : class
	{
		private readonly ExpectationBuilder<T> _builder;
		private readonly string _eventName;

		public ExpectationSyntax(ExpectationBuilder<T> expectationBuilder)
		{
			_builder = expectationBuilder;
		}

		public ExpectationSyntax(ExpectationBuilder<T> expectationBuilder, string eventName)
			: this(expectationBuilder)
		{
			_eventName = eventName;
		}

		#region IExpectActionEventSyntax
		public DelegateInvoker WillReturnDelegateInvoker()
		{
			return new DelegateInvoker(_eventName, this);
		}

		public EventInvoker<TEventArgs> WillReturnEventInvoker<TEventArgs>() where TEventArgs : EventArgs
		{
			return new EventInvoker<TEventArgs>(_eventName, this);
		}
		#endregion

		#region IActionSyntax
		public ICommentSyntax Will(params IAction[] actions)
		{
			return _builder.Will(actions);
		}
		#endregion

		#region IComment
		public IVerifyableExpectation Comment(string comment)
		{
			return _builder.Comment(comment);
		}
		#endregion

		#region IExpectMatchSyntax

		public IActionSyntax With(params object[] expectedArguments)
		{
			_builder.With(expectedArguments);
			return this;
		}

		public IActionSyntax WithAnyArguments()
		{
			_builder.WithAnyArguments();
			return this;
		}

		#endregion

		#region IVerifyableExpectation

		void IVerifyableExpectation.Assert()
		{
			_builder.Assert();
		}

		#endregion

	}

	internal class ExpectationSyntax<T, TResult> : IExpectMatchSyntax<TResult> where T : class
	{
		private readonly ExpectationBuilder<T> _builder;

		public ExpectationSyntax(ExpectationBuilder<T> expectationBuilder)
		{
			_builder = expectationBuilder;
		}

		#region IExpectActionSyntax

		ICommentSyntax IExpectActionSyntax<TResult>.WillReturn(TResult actualValue)
		{
			_builder.Will(Return.Value(actualValue));
			return this;
		}

		ICommentSyntax IExpectActionSyntax.Will(params IAction[] actions)
		{
			_builder.Will(actions);
			return this;
		}

		ICommentSyntax IExpectActionSetterSyntax.WillReturnSetterValue()
		{
			_builder.Will(new ReturnPropertyValueAction(_builder.MockObject));
			return this;
		}

		#endregion

		#region IExpectMatchSyntax<T>

		IExpectActionSyntax<TResult> IExpectMatchSyntax<TResult>.WithAnyArguments()
		{
			_builder.WithAnyArguments();
			return this;
		}

		IExpectActionSyntax<TResult> IExpectMatchSyntax<TResult>.With(params object[] expectedArguments)
		{
			_builder.With(expectedArguments);
			return this;
		}

		IExpectActionSyntax IExpectMatchSyntax<TResult>.To(Matcher matcher)
		{
			return To(matcher);
		}

		IExpectActionSyntax IExpectMatchSyntax<TResult>.To(TResult value)
		{
			return To(Is.EqualTo(value));
		}

		IExpectActionSyntax IExpectMatchSyntax<TResult>.ToAnything()
		{
			return To(Is.Anything);
		}

		IExpectActionSyntax To(Matcher matcher)
		{
			//make sure the method expectation was set up as a setter.
			//it may not be when the expectation is used like mock.Expect(_=>_.prop).To(...)

			string propertyName = _builder.BuildableExpectation.MethodMatcher.Description;

			if (!propertyName.Contains("=")) //get properties will not have an "=", so the expectation needs to change
			{
				_builder.SetProperty(propertyName);
			}

			_builder.With(new ArgumentsMatcher(matcher));

			// capture the value during the assignment
			_builder.Will(new CaptureValueAction());

			return this;
		}

		#endregion

		#region ICommentSyntax

		IVerifyableExpectation ICommentSyntax.Comment(string comment)
		{
			return _builder.Comment(comment);
		}

		#endregion

		#region IVerifyableExpectation

		void IVerifyableExpectation.Assert()
		{
			_builder.Assert();
		}

		#endregion

	}
}

#region Using

using NMock.Syntax;

#endregion

namespace NMock.Internal
{
	internal class PropertyArgumentBuilder<TResult> : IAutoArgumentSyntax<TResult>
	{
		private readonly ExpectationBuilder _builder;

		public PropertyArgumentBuilder(ExpectationBuilder builder)
		{
			_builder = builder;
		}

		#region IAutoArgumentSyntax<TResult> Members

		public ICommentSyntax WillReturn(TResult actualValue)
		{
			return _builder.Will(Return.Value(actualValue));
		}

		public IVerifyableExpectation Comment(string comment)
		{
			return _builder.Comment(comment);
		}

		public ICommentSyntax Will(params IAction[] actions)
		{
			return _builder.Will(actions);
		}

		public IAutoActionSyntax<TResult> WithArguments(params Matcher[] argumentMatchers)
		{
			_builder.With(argumentMatchers);

			return this;
		}

		public IAutoMatchSyntax<TResult> With(params object[] expectedArguments)
		{
			_builder.With(expectedArguments);

			return new PropertyMatchBuilder<TResult>(_builder);
		}

		public IAutoMatchSyntax<TResult> WithNoArguments()
		{
			return new PropertyMatchBuilder<TResult>(_builder);
		}

		public IAutoMatchSyntax<TResult> WithAnyArguments()
		{
			_builder.WithAnyArguments();

			return new PropertyMatchBuilder<TResult>(_builder);
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
#region Using

using System.Collections.Generic;
using NMock.Actions;
using NMock.Syntax;

#endregion

namespace NMock.Internal
{
	internal class PropertyMatchBuilder<T> : IAutoMatchSyntax<T>
	{
		private readonly ExpectationBuilder _builder;

		public PropertyMatchBuilder(ExpectationBuilder builder)
		{
			_builder = builder;
		}

		#region IAutoMatchSyntax<T> Members

		public ICommentSyntax WillReturn(T actualValue)
		{
			return Will(Return.Value(actualValue));
		}

		public ICommentSyntax WillReturnSetterValue()
		{
			return _builder.Will(new ReturnPropertyValueAction(_builder.MockObject));
		}

		public IVerifyableExpectation Comment(string comment)
		{
			return _builder.Comment(comment);
		}

		public ICommentSyntax Will(params IAction[] actions)
		{
			return _builder.Will(actions);
		}

		public IActionSyntax Matching(Matcher matcher)
		{
			return _builder.Matching(matcher);
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
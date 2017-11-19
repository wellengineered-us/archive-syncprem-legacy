#region Using

using NMock.Actions;
using NMock.Matchers;
using NMock.Syntax;

#endregion

namespace NMock.Internal
{
	internal class PropertyValueBuilder<T> : IAutoValueSyntax<T>
	{
		private readonly IValueSyntaxBuilder _builder;

		public PropertyValueBuilder(IValueSyntaxBuilder builder)
		{
			_builder = builder;
		}

		#region IAutoValueSyntax<T> Members

		public IActionSyntax To(T equalValue)
		{
			return To(Is.EqualTo(equalValue));
		}

		public IActionSyntax To(Matcher valueMatcher)
		{
			// capture the value during the assignment
			_builder.Will(new CaptureValueAction());

			return _builder.To(valueMatcher);
		}

		public IActionSyntax ToAnything()
		{
			return _builder.To(Is.Anything);
		}

		#endregion
	}
}
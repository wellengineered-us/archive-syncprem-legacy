#region Using

using NMock.Matchers;

#endregion

namespace NMock
{
	/// <summary>
	/// Provides shortcuts to matchers.
	/// </summary>
	public class Has
	{
		/// <summary>
		/// Returns a matcher for testing string representation of objects.
		/// </summary>
		/// <param name="matcher">The wrapped matcher.</param>
		/// <returns>Returns a <see cref="ToStringMatcher"/> for testing string representation of objects.</returns>
		public static Matcher ToString(Matcher matcher)
		{
			return new ToStringMatcher(matcher);
		}

		/// <summary>
		/// Returns a matcher for checking property values.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="valueMatcher">The value matcher.</param>
		/// <returns>Returns a <see cref="PropertyMatcher"/> for checking property values.</returns>
		public static Matcher Property(string propertyName, Matcher valueMatcher)
		{
			return new PropertyMatcher(propertyName, valueMatcher);
		}

		/// <summary>
		/// Returns a matcher for checking field values.
		/// </summary>
		/// <param name="fieldName">Name of the field.</param>
		/// <param name="valueMatcher">The value matcher.</param>
		/// <returns>Returns a <see cref="FieldMatcher"/> for checking field values.</returns>
		public static Matcher Field(string fieldName, Matcher valueMatcher)
		{
			return new FieldMatcher(fieldName, valueMatcher);
		}
	}
}
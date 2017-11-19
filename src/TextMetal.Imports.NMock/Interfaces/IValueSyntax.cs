namespace NMock.Syntax
{
	/// <summary>
	/// Syntax defining a value.
	/// </summary>
	public interface IValueSyntax
	{
		/// <summary>
		/// Defines a value.
		/// </summary>
		/// <param name="valueMatcher">The value matcher.</param>
		/// <returns>Match syntax defining the behavior of the value.</returns>
		IActionSyntax To(Matcher valueMatcher);
	}
}
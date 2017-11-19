namespace NMock.Syntax
{
	/// <summary>
	/// Extends <see cref="IValueSyntax"/> by adding other syntax methods.
	/// </summary>
	public interface IAutoValueSyntax<TProperty> : IValueSyntax
	{
		/// <summary>
		/// Creates an expectation that this property can be set to any value.
		/// </summary>
		/// <returns></returns>
		IActionSyntax ToAnything();

		/// <summary>
		/// Creates an expectation that the property will be set to this value.
		/// </summary>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		IActionSyntax To(TProperty equalValue);
	}
}
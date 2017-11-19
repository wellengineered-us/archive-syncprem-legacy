namespace NMock.Syntax
{
	/// <summary>
	/// Syntax defining a method, property or event (de)registration.
	/// </summary>
	public interface IMethodSyntax
	{
		/// <summary>
		/// Sets up an expectation that a protected method will be called.
		/// </summary>
		/// <param name="name">The name of the method.</param>
		/// <returns>
		/// Argument syntax defining the arguments of the method.
		/// </returns>
		IArgumentSyntax ProtectedMethod(string name);

		//TODO: vNext
		//IAutoArgumentSyntax<T> Method<T>(Expression<Func<T>> expression) where T : class;
	}
}
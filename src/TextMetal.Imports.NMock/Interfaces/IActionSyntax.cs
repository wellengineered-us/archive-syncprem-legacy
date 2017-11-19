namespace NMock.Syntax
{
	/// <summary>
	/// Syntax for defining actions.
	/// </summary>
	public interface IActionSyntax : ICommentSyntax
	{
		/// <summary>
		/// Creates the expectation of what to do when this member is called.
		/// </summary>
		/// <param name="actions">Common <see cref="IAction" />s can be found in the <see cref="Return"/> and <see cref="Throw"/> classes.</param>
		/// <returns>Returns the comment syntax defined after will.</returns>
		/// <remarks>
		/// For void methods this action does not need to be used.
		/// For non-void methods this action is typically set to use the <see cref="Return"/> class.
		/// For all methods this action can be used to throw an exception using the <see cref="Throw"/> class.
		/// </remarks>
		ICommentSyntax Will(params IAction[] actions);
	}
}
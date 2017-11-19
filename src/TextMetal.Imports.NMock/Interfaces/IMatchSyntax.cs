namespace NMock.Syntax
{
	/// <summary>
	/// Syntax defining matching criterias.
	/// </summary>
	public interface IMatchSyntax : IActionSyntax
	{
		/// <summary>
		/// Defines a matching criteria.
		/// </summary>
		/// <param name="matcher">The matcher.</param>
		/// <returns>Action syntax defining the action to take.</returns>
		IActionSyntax Matching(Matcher matcher);
	}
}
namespace NMock.Syntax
{
	/// <summary>
	/// Syntax for adding an explanation for the expectation.
	/// </summary>
	public interface ICommentSyntax : IVerifyableExpectation
	{
		/// <summary>
		/// Adds a comment for the expectation that is added to the error message if the expectation is not met.
		/// </summary>
		/// <param name="comment">The comment that is shown in the error message if this expectation is not met.
		/// You can describe here why this expectation has to be met.</param>
		IVerifyableExpectation Comment(string comment);
	}
}
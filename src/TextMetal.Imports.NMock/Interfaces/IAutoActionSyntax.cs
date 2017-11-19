namespace NMock.Syntax
{
	/// <summary>
	/// Extends the <see cref="IArgumentSyntax"/> interface to include the <see cref="WillReturn"/> method.
	/// </summary>
	public interface IAutoActionSyntax<TResult> : IActionSyntax
	{
		/// <summary>
		/// This is the strongly-typed version of the <see cref="IActionSyntax.Will"/> method.  Use this method when
		/// the value to return <b>is</b> this value (and not a matcher).
		/// </summary>
		/// <param name="actualValue"></param>
		/// <returns></returns>
		/// <remarks>
		/// This version of the "Will" methods is used in place of <c>.Will(Return.Value(obj))</c> when <c>obj</c> is known
		/// and should be checked at compile time.  The benefit is that the method takes a strongly-typed argument.  The 
		/// drawback is that you can't specify a matcher.
		/// </remarks>
		ICommentSyntax WillReturn(TResult actualValue);
	}
}
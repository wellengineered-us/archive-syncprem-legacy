using NMock.Matchers;

namespace NMock.Syntax
{
	/// <summary>
	/// This interface adds a special method for property expectations.
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public interface IAutoPropertyActionSyntax<TResult> : IAutoActionSyntax<TResult>
	{
		/// <summary>
		/// Sets up an expectation that calls an action to return the value of the property
		/// that was previously set in code or prior expectation.
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// It is useful in cases where a value is assigned to a property internally in a method
		/// and only a <see cref="TypeMatcher"/> could be used to match the assignment.  This method
		/// is called on a getter expectation to get that underlying value.
		/// </remarks>
		ICommentSyntax WillReturnSetterValue();
	}
}

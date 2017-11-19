using System;

namespace NMock.Syntax
{
	/// <summary>
	/// Syntax for defining expected arguments of a method call.
	/// </summary>
	public interface IArgumentSyntax : IActionSyntax
	{
		/// <summary>
		/// Defines the arguments that are expected on the method call.
		/// </summary>
		/// <param name="expectedArguments">The expected arguments.</param>
		/// <returns>Matcher syntax.</returns>
		/// <remarks>
		/// The specified arguments are converted to <see chref="EqualMatcher" />s.  Matchers
		/// as well as non-Matchers can be used interchangably in the method.
		/// </remarks>
		IActionSyntax With(params object[] expectedArguments);

		/// <summary>
		/// Defines that no arguments are expected on the method call.
		/// </summary>
		/// <returns>Matcher syntax.</returns>
		[Obsolete("Method signature matching is now exact.  Use the correct types when matching the method signature.", true)]
		IActionSyntax WithNoArguments();

		/// <summary>
		/// Defines that any arguments are allowed on the method call.
		/// </summary>
		/// <returns>Matcher syntax.</returns>
		IActionSyntax WithAnyArguments();

		/// <summary>
		/// Defines matching criteria for arguments.
		/// </summary>
		/// <param name="argumentMatchers">A list of matchers to match the arguments of a method.</param>
		/// <returns>Matcher syntax.</returns>
		/// <remarks>
		/// The matchers will be automatically wrapped in an ArgumentsMatcher.
		/// </remarks>
		[Obsolete("Use the With method.  It accepts values and matchers.")]
		IMatchSyntax WithArguments(params Matcher[] argumentMatchers);
	}
}
using System;

namespace NMock.Syntax
{
	/// <summary>
	/// This interface provides the syntax used for "Method" methods on the <see cref="IMethodSyntax{TInterface}"/>
	/// interface that don't return void and don't use argument values explicitly.
	/// </summary>
	/// <typeparam name="TResult">The return type of the method.</typeparam>
	/// <remarks>
	/// This interface provides the syntax needed to specify method arguments or match
	/// method arguments.
	/// </remarks>
	public interface IAutoArgumentSyntax<TResult> : IAutoActionSyntax<TResult>
	{
		/// <summary>
		/// Defines the arguments that are expected on the method call.
		/// </summary>
		/// <param name="expectedArguments">The expected arguments.</param>
		/// <returns>Matcher syntax.</returns>
		IAutoMatchSyntax<TResult> With(params object[] expectedArguments);

		/// <summary>
		/// Defines that no arguments are expected on the method call.
		/// </summary>
		/// <returns>Matcher syntax.</returns>
		[Obsolete("Method signature matching is now exact.  Use the correct types when matching the method signature.", true)]
		IAutoMatchSyntax<TResult> WithNoArguments();

		/// <summary>
		/// Defines that all arguments are allowed on the method call.
		/// </summary>
		/// <returns>Matcher syntax.</returns>
		IAutoMatchSyntax<TResult> WithAnyArguments();

		/// <summary>
		/// Defines matching criteria for arguments.
		/// </summary>
		/// <param name="argumentMatchers">A list of matchers to match the arguments of a method.</param>
		/// <returns>Action syntax defining the action to take.</returns>
		/// <remarks>
		/// The matchers will be automatically wrapped in an ArgumentsMatcher.
		/// </remarks>
		[Obsolete("Use the With method.  It accepts values and matchers." /* ^ */ /*, true*/ /* dpbullington@gmail.com ^ */)]
		IAutoActionSyntax<TResult> WithArguments(params Matcher[] argumentMatchers);
	}
}
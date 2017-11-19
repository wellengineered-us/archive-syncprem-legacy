using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMock.Matchers;
using NMock.Syntax;

namespace NMock.Syntax
{
	public interface IExpectMatchSyntax : IExpectActionEventSyntax, IActionSyntax
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
		/// Defines that any arguments are allowed on the method call.
		/// </summary>
		/// <returns>Matcher syntax.</returns>
		IActionSyntax WithAnyArguments();
	}

	public interface IExpectMatchSyntax<TResult> : IExpectActionSetterSyntax, IExpectActionSyntax<TResult>, ICommentSyntax
	{
		/// <summary>
		/// Defines the arguments that are expected on the method call.
		/// </summary>
		/// <param name="expectedArguments">The expected arguments.</param>
		/// <returns>Matcher syntax.</returns>
		IExpectActionSyntax<TResult> With(params object[] expectedArguments);

		/// <summary>
		/// Defines that all arguments are allowed on the method call.
		/// </summary>
		/// <returns>Matcher syntax.</returns>
		IExpectActionSyntax<TResult> WithAnyArguments();

		/// <summary>
		/// Defines a value.
		/// </summary>
		/// <param name="valueMatcher">The value matcher.</param>
		/// <returns>Match syntax defining the behavior of the value.</returns>
		IExpectActionSyntax To(Matcher valueMatcher);

		/// <summary>
		/// Creates an expectation that this property can be set to any value.
		/// </summary>
		/// <returns></returns>
		IExpectActionSyntax ToAnything();

		/// <summary>
		/// Creates an expectation that the property will be set to this value.
		/// </summary>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		IExpectActionSyntax To(TResult equalValue);
	}

	public interface IExpectActionSyntax : IVerifyableExpectation
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

	public interface IExpectActionSetterSyntax : IExpectActionSyntax
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

	public interface IExpectActionSyntax<TResult> : IExpectActionSyntax
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

	public interface IExpectActionEventSyntax
	{
		DelegateInvoker WillReturnDelegateInvoker();

		EventInvoker<TEventArgs> WillReturnEventInvoker<TEventArgs>() where TEventArgs : EventArgs;
	}
}
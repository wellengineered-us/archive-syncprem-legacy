using System;
using System.Linq.Expressions;

namespace NMock.Syntax
{
	/// <summary>
	/// Syntax methods that are used specifically for stubbing.
	/// </summary>
	/// <typeparam name="TInterface"></typeparam>
	public interface IStubSyntax<TInterface>
	{
		#region Method

		/// <summary>
		/// Creates an expectation for the method specified when the method returns <see langword="void"/>.
		/// The arguments are ignored, use the <see cref="IAutoArgumentSyntax{TProperty}"/> result of this 
		/// method to specify the expected arguments.
		/// </summary>
		/// <param name="expression">A method call expression that specifies the expected method.</param>
		/// <returns>An instance of a class used to specify the parameters.</returns>
		/// <remarks>
		/// <note type="note">
		/// This overload is chosen by the compiler when the method specified in the <b>action</b> returns
		/// <see langword="void"/>.
		/// </note>
		/// <note type="caution">
		/// An expectation created with this method will not use the parameters supplied to the method call.
		/// Parameters must be supplied in order for the code to compile.  Use the <see cref="IArgumentSyntax.With(object[])"/> or 
		/// <see cref="IArgumentSyntax.WithAnyArguments"/> methods
		/// as part of the return type of this method.  As an alternative, use the <see cref="MethodWith"/> method which
		/// will use the parameters supplied to the method as part of the expectation.
		/// </note>
		/// <note type="tip">
		/// The <see cref="IActionSyntax.Will"/> method should not be used from this method.  This method
		/// is used for <see langword="void" /> return types, therefore a return expectation is not needed.
		/// </note>
		/// </remarks>
		/// <seealso cref="MethodWith"/>
		/// <overloads>
		/// Creates an expectation for the method specified.
		/// </overloads>
		/// <example>
		/// In this example, the method returns void.
		/// <code>
		/// mock.Expects.One.Method(m=>m.MethodName("IgnoredParam")).With("RealParam");
		/// </code>
		/// </example>
		IArgumentSyntax Method(Expression<Action<TInterface>> expression);

		/// <summary>
		/// Creates an expectation that this method will be called.  The arguments are ignored, use the 
		/// <see cref="IAutoArgumentSyntax{TProperty}"/> result of this method to specify the expected arguments.
		/// </summary>
		/// <param name="expression">A method call expression that specifies the expected method.</param>
		/// <returns>An <see cref="IAutoArgumentSyntax{TResult}"/> object used to specify expected arguments or return value.</returns>
		/// <example>
		/// 
		/// <code>
		/// mock.Expects.One.Method(m=>m.MethodName("", 3)).Will(Return.Value(true));
		/// mock.Expects.One.Method(m=>m.MethodName()).WillReturn(7);
		/// </code>
		/// </example>
		IAutoArgumentSyntax<TResult> Method<TResult>(Expression<Func<TInterface, TResult>> expression);

		/// <summary>
		/// Creates an expectation for the method specified when the method returns <see langword="void"/>.
		/// The arguments are used as the expected values.
		/// </summary>
		/// <param name="expression">A method call expression that specifies the expected method.</param>
		/// <returns>An instance of a class used to specify the parameters.</returns>
		/// <remarks>
		/// <note type="note">
		/// This overload is chosen by the compiler when the method specified in the <b>action</b> returns
		/// <see langword="void"/>.
		/// </note>
		/// <note type="note">
		/// An expectation created with this method will use the parameters supplied to the method call.
		/// </note>
		/// </remarks>
		/// <overloads>
		/// Creates an expectation for the method specified.
		/// </overloads>
		/// <example>
		/// In this example, the method returns void.
		/// <code>
		/// mock.Expects.One.MethodWith(m=>m.MethodName("RealParam"));
		/// </code>
		/// </example>
		IActionSyntax MethodWith(Expression<Action<TInterface>> expression);

		/// <summary>
		/// Creates an expectation that this method will be called.  The arguments are used as the expected values.
		/// </summary>
		/// <param name="expression">A method call expression that specifies the expected method.</param>
		/// <returns>An <see cref="IAutoActionSyntax{TResult}"/> object used to specify the return value of the method.</returns>
		IAutoActionSyntax<TResult> MethodWith<TResult>(Expression<Func<TInterface, TResult>> expression);

		/// <summary>
		/// Creates an expectation that this method will be called.  The arguments are used as the expected values.
		/// </summary>
		/// <param name="expression">A method call expression that specifies the expected method.</param>
		/// <param name="returnValue">The value to be returned when the method is called.</param>
		/// <returns>An <see cref="ICommentSyntax"/> object used to specify the explanation for the expectation.</returns>
		/// <remarks>Compare this method to other versions of this overloaded method in the See Also section.</remarks>
		ICommentSyntax MethodWith<TProperty>(Expression<Func<TInterface, TProperty>> expression, TProperty returnValue);

		#endregion

		#region GetProperty

		/// <summary>
		/// Creates an expectation that this property will be accessed.
		/// </summary>
		/// <typeparam name="TProperty">The property data type.</typeparam>
		/// <param name="expression">The expression to extract the property name.</param>
		/// <returns>An <see cref="IAutoMatchSyntax{TProperty}"/> that can be used to set the return value.</returns>
		IAutoPropertyActionSyntax<TProperty> GetProperty<TProperty>(Expression<Func<TInterface, TProperty>> expression);

		/// <summary>
		/// Creates an expectation that this property will be accessed and it should return the specified value.
		/// </summary>
		/// <typeparam name="TProperty">The property data type.</typeparam>
		/// <param name="expression">A lambda expression to extract the property name.</param>
		/// <param name="returnValue">The value to be returned when the property is accessed.</param>
		/// <returns>An object to add comments about this expectation.</returns>
		/// <remarks>
		/// Use this method as a shorthand to <see cref="GetProperty{TProperty}(Expression{Func{TInterface,TProperty}})"/>.
		/// It sacrifices syntactic sugar but reduces the number of characters to type.
		/// </remarks>
		ICommentSyntax GetProperty<TProperty>(Expression<Func<TInterface, TProperty>> expression, TProperty returnValue);

		#endregion

		#region Events
		/// <summary>
		/// Creates an expectation that a <see cref="Delegate"/> will be bound to an event.  The type of binding will be infered from the use of += or -=.
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		/// <remarks>
		/// If a binding expectation uses <c>null</c>, any arguments will be allowed.
		/// If a binding references a specific <see cref="Delegate"/>, the expectation will expect that delegate.
		/// </remarks>
		DelegateInvoker DelegateBinding(Action<TInterface> action);

		/// <summary>
		/// Creates an expectation that an <see cref="EventHandler"/> will be bound.  The type of binding will be infered from the use of += or -=.
		/// </summary>
		/// <param name="action">A lambda expression: e =&gt; e.Event += null</param>
		/// <returns></returns>
		/// <remarks>
		/// Use <see cref="EventBinding{T}" /> to bind to <see cref="EventHandler{T}" /> is used.
		/// </remarks>
		EventInvoker EventBinding(Action<TInterface> action);

		/// <summary>
		/// Creates an expectation that an <see cref="EventHandler{T}"/> will be bound.  The type of binding will be infered from the use of += or -=.
		/// </summary>
		/// <param name="action">A lambda expression: e =&gt; e.Event += null</param>
		/// <typeparam name="TEventArgs">The type of event args for the <see cref="EventHandler" /></typeparam>
		/// <returns></returns>
		EventInvoker<TEventArgs> EventBinding<TEventArgs>(Action<TInterface> action) where TEventArgs : EventArgs;

		#endregion


	}
}

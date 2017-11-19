#region Using

using System;

#endregion

namespace NMock.Syntax
{
	/// <summary>
	/// Contains the methods that define the expectation for either a property, method, or event.
	/// </summary>
	/// <typeparam name="TInterface">The interface or class being mocked.</typeparam>
	/// <remarks>
	/// This interface defines generic methods that take lambda expressions.
	/// </remarks>
	public interface IMethodSyntax<TInterface> : IStubSyntax<TInterface>, IMethodSyntax
	{
		#region SetProperty

		/// <summary>
		/// Creates an expectation that this property will be set to a value specified in the 
		/// <see cref="IAutoValueSyntax{TProperty}"/> result of this method.  The value used in the expression
		/// is ignored.
		/// </summary>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="expression">A set property expression that specifies the property to be set.</param>
		/// <remarks>
		/// If the property specified in the expression has a getter, a value isn't required in the expression.
		/// <code>
		/// mock.Expects.One.SetProperty(p => p.Prop)
		/// </code>
		/// instead of
		/// <code>
		/// mock.Expects.One.SetProperty(p => p.Prop = "Ignored Value")
		/// </code>
		/// The code above only needs to be used in cases where the property is write-only.
		/// </remarks>
		/// <returns></returns>
		IAutoValueSyntax<TProperty> SetProperty<TProperty>(Func<TInterface, TProperty> expression);

		/// <summary>
		/// Creates an expectation that this property will be set to the specified value.
		/// </summary>
		/// <param name="action">z => z.prop = 0</param>
		/// <returns>An <see cref="ICommentSyntax"/> object to specify the comment for the expectation. </returns>
		IActionSyntax SetPropertyTo(Action<TInterface> action);

		#endregion

	}
}
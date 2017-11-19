#region Using

using System;

#endregion

namespace NMock.Syntax
{
	/// <summary>
	/// Syntax describing the initial characteristics of a new mock object.
	/// </summary>
	public interface IMockDefinitionSyntax : IMockDefinition
	{
		/// <summary>
		/// Specifies a type that this mock should implement. This may be a class or interface,
		/// but there can only be a maximum of one class implemented by a mock.
		/// </summary>
		/// <typeparam name="T">The type to implement.</typeparam>
		/// <returns>The mock object definition.</returns>
		IMockDefinitionSyntax Implementing<T>();

		/// <summary>
		/// Specifies the types that this mock should implement. These may be a class or interface,
		/// but there can only be a maximum of one class implemented by a mock.
		/// </summary>
		/// <param name="additionalTypesToMock">The types to implement.</param>
		/// <returns>The mock object definition.</returns>
		IMockDefinitionSyntax Implementing(params Type[] additionalTypesToMock);

		/// <summary>
		/// Specifies how the mock object should behave when first created.
		/// It is invalid to set the MockStyle of a mock more than once.
		/// </summary>
		/// <param name="style">A MockStyle value.</param>
		/// <returns>The mock object definition.</returns>
		IMockDefinitionSyntax OfStyle(MockStyle style);

		/// <summary>
		/// Specifies the arguments for the constructor of the class to be mocked.
		/// Only applicable when mocking a class with a non-default constructor.
		/// It is invalid to specify the constructor arguments of a mock more than once.
		/// </summary>
		/// <param name="args">The arguments for the class constructor.</param>
		/// <returns>The mock object definition.</returns>
		IMockDefinitionSyntax WithArgs(params object[] args);

		/// <summary>
		/// Specifies a name for the mock. This will be used in error messages,
		/// and as the return value of ToString() if not mocking a class.
		/// It is invalid to specify the name of a mock more than once.
		/// </summary>
		/// <param name="name">The name for the mock.</param>
		/// <returns>The mock object definition.</returns>
		IMockDefinitionSyntax Named(string name);
	}
}
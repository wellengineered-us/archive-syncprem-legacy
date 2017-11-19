#region Using

using System;
using NMock.Internal;
using NMock.Syntax;

#endregion

namespace NMock
{
	/// <summary>
	/// Defines the initial characteristics of a new mock object.
	/// This is normally used in conjunction with <see cref="MockFactory.CreateMock{T}(NMock.IMockDefinition)"/>
	/// </summary>
	public static class DefinedAs
	{
		/// <summary>
		/// Specifies a type that this mock should implement. This may be a class or interface,
		/// but there can only be a maximum of one class implemented by a mock.
		/// </summary>
		/// <typeparam name="T">The type to implement.</typeparam>
		/// <returns>The mock object definition.</returns>
		public static IMockDefinitionSyntax Implementing<T>()
		{
			return new MockBuilder().Implementing<T>();
		}

		/// <summary>
		/// Specifies the types that this mock should implement. These may be a class or interface,
		/// but there can only be a maximum of one class implemented by a mock.
		/// </summary>
		/// <param name="types">The types to implement.</param>
		/// <returns>The mock object definition.</returns>
		public static IMockDefinitionSyntax Implementing(params Type[] types)
		{
			return new MockBuilder().Implementing(types);
		}

		/// <summary>
		/// Specifies how the mock object should behave when first created.
		/// </summary>
		/// <param name="mockStyle">A MockStyle value.</param>
		/// <returns>The mock object definition.</returns>
		public static IMockDefinitionSyntax OfStyle(MockStyle mockStyle)
		{
			return new MockBuilder().OfStyle(mockStyle);
		}

		/// <summary>
		/// Specifies the arguments for the constructor of the class to be mocked.
		/// Only applicable when mocking a class with a non-default constructor.
		/// </summary>
		/// <param name="args">The arguments for the class constructor.</param>
		/// <returns>The mock object definition.</returns>
		public static IMockDefinitionSyntax WithArgs(params object[] args)
		{
			return new MockBuilder().WithArgs(args);
		}

		/// <summary>
		/// Specifies a name for the mock. This will be used in error messages,
		/// and as the return value of ToString() if not mocking a class.
		/// </summary>
		/// <param name="name">The name for the mock.</param>
		/// <returns>The mock object definition.</returns>
		public static IMockDefinitionSyntax Named(string name)
		{
			return new MockBuilder().Named(name);
		}

		/// <summary>
		/// Returns a default implementation of <see cref="IMockDefinitionSyntax"/>.
		/// </summary>
		public static IMockDefinitionSyntax Default
		{
			get
			{
				return new MockBuilder();
			}
		}
	}
}
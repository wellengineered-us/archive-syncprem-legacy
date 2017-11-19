using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NMock.Internal;
using NMock.Proxy;
using NMock.Proxy.Castle;
using NMock.Syntax;

namespace NMock
{
	//TODO: vNext
	/*
	public static class Mock
	{
		/// <summary>
		/// The mock object factory that is being used by this MockFactory instance.
		/// </summary>
		private static readonly IMockObjectFactory currentMockObjectFactory = new CastleMockObjectFactory();

		private static readonly MockFactory mockFactory = new MockFactory();

		#region CreateInstance
		/// <summary>
		/// Creates a new dynamic mock of the specified type using the supplied definition.
		/// </summary>
		/// <typeparam name="TMockedType">The type to mock.</typeparam>
		/// <param name="definition">An <see cref="IMockDefinition"/> to create the mock from.</param>
		/// <returns>A dynamic mock for the specified type.</returns>
		public static TMockedType Create<TMockedType>(IMockDefinition definition)
		{
			return (TMockedType)definition.Create(typeof(TMockedType), mockFactory, currentMockObjectFactory);
		}

		/// <summary>
		/// Creates a new dynamic mock of the specified type using the supplied definition.
		/// </summary>
		/// <typeparam name="TMockedType">The type to mock.</typeparam>
		/// <param name="name">The name of the mock.</param>
		/// <returns>A dynamic mock for the specified type.</returns>
		public static TMockedType Create<TMockedType>(string name)
		{
			return Create<TMockedType>(DefinedAs.Named(name));
		}

		/// <summary>
		/// Creates a new dynamic mock of the specified type.
		/// </summary>
		/// <typeparam name="TMockedType">The type to mock.</typeparam>
		/// <param name="constructorArgs">The arguments for the constructor of the class to be mocked.
		/// Only applicable when mocking classes with non-default constructors.</param>
		/// <returns>A dynamic mock for the specified type.</returns>
		public static TMockedType Create<TMockedType>(params object[] constructorArgs)
		{
			return Create<TMockedType>(DefinedAs.WithArgs(constructorArgs));
		}

		/// <summary>
		/// Creates a new dynamic mock of the specified type.
		/// </summary>
		/// <typeparam name="TMockedType">The type to mock.</typeparam>
		/// <param name="mockStyle">Specifies how the mock object should behave when first created.</param>
		/// <param name="constructorArgs">The arguments for the constructor of the class to be mocked.
		/// Only applicable when mocking classes with non-default constructors.</param>
		/// <returns>A dynamic mock for the specified type.</returns>
		public static TMockedType Create<TMockedType>(MockStyle mockStyle, params object[] constructorArgs)
		{
			return Create<TMockedType>(DefinedAs.OfStyle(mockStyle).WithArgs(constructorArgs));
		}

		#endregion

		public static IAutoActionSyntax<T> Expects<T>(Func<T> func)
		{
			return null;
		}

		public static IAutoActionSyntax<T> Expects<T>(Action<T> expression)
		{
			return null;
		}
	}
	*/
}

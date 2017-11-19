#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using NMock.Matchers;
using NMock.Proxy;
using NMock.Syntax;

#endregion

namespace NMock.Internal
{
	/// <summary>
	/// Allows a mock object to be incrementally defined, and then finally created.
	/// </summary>
	internal class MockBuilder : IMockDefinitionSyntax
	{
		private readonly object locker = new object();
		/// <summary>
		/// A single empty array instance that is used as a default value
		/// for constructor arguments.
		/// </summary>
		private static readonly object[] EmptyArgsArray = new object[0];

		/// <summary>
		/// The types that the mock object needs to implement.
		/// </summary>
		private readonly List<Type> _types = new List<Type>();

		/// <summary>
		/// Constructor arguments for any class type that this mock might subclass.
		/// If not subclassing, or if using a default constructor, then this should
		/// be an empty array.
		/// </summary>
		private object[] _constructorArgs = EmptyArgsArray;

		/// <summary>
		/// The MockStyle for the mock. If not specified, this will ultimately be
		/// assumed to be MockStyle.Default.
		/// </summary>
		private MockStyle? _mockStyle;

		/// <summary>
		/// The name of the mock object. Null is a valid value.
		/// </summary>
		private string _name;

		#region IMockDefinitionSyntax Members

		/// <summary>
		/// Specifies a type that this mock should implement. This may be a class or interface,
		/// but there can only be a maximum of one class implemented by a mock.
		/// </summary>
		/// <typeparam name="T">The type to implement.</typeparam>
		/// <returns>The mock object definition.</returns>
		public IMockDefinitionSyntax Implementing<T>()
		{
			lock(locker)
			{
				_types.Add(typeof(T));
			}

			return this;
		}

		/// <summary>
		/// Specifies the types that this mock should implement. These may be a class or interface,
		/// but there can only be a maximum of one class implemented by a mock.
		/// </summary>
		/// <param name="additionalTypesToMock">The types to implement.</param>
		/// <returns>The mock object definition.</returns>
		public IMockDefinitionSyntax Implementing(params Type[] additionalTypesToMock)
		{
			// <pex>
			if (additionalTypesToMock == null)
				throw new ArgumentNullException("additionalTypesToMock");
			// </pex>
			lock(locker)
			{
				_types.AddRange(additionalTypesToMock);
			}

			return this;
		}

		/// <summary>
		/// Specifies how the mock object should behave when first created.
		/// It is invalid to set the MockStyle of a mock more than once.
		/// </summary>
		/// <param name="mockStyle">A MockStyle value.</param>
		/// <returns>The mock object definition.</returns>
		public IMockDefinitionSyntax OfStyle(MockStyle mockStyle)
		{
			if (_mockStyle.HasValue)
			{
				throw new InvalidOperationException("MockStyle has already been set for this mock definition.");
			}

			_mockStyle = mockStyle;

			return this;
		}

		/// <summary>
		/// Specifies the arguments for the constructor of the class to be mocked.
		/// Only applicable when mocking a class with a non-default constructor.
		/// It is invalid to specify the constructor arguments of a mock more than once.
		/// </summary>
		/// <param name="args">The arguments for the class constructor.</param>
		/// <returns>The mock object definition.</returns>
		public IMockDefinitionSyntax WithArgs(params object[] args)
		{
			if (_constructorArgs != EmptyArgsArray)
			{
				throw new InvalidOperationException("Constructor arguments have already been specified for this mock definition.");
			}

			_constructorArgs = args;

			return this;
		}

		/// <summary>
		/// Specifies a name for the mock. This will be used in error messages,
		/// and as the return value of ToString() if not mocking a class.
		/// It is invalid to specify the name of a mock more than once.
		/// </summary>
		/// <param name="name">The name for the mock.</param>
		/// <returns>The mock object definition.</returns>
		public IMockDefinitionSyntax Named(string name)
		{
			if (_name != null)
			{
				throw new InvalidOperationException("A name has already been specified for this mock definition.");
			}

			_name = name;

			return this;
		}

		/// <summary>
		/// This method supports NMock infrastructure and is not intended to be called directly from your code.
		/// </summary>
		/// <param name="primaryType">The primary type that is being mocked.</param>
		/// <param name="mockFactory">The current <see cref="MockFactory"/> instance.</param>
		/// <param name="mockObjectFactory">An <see cref="IMockObjectFactory"/> to use when creating the mock.</param>
		/// <returns>A new mock instance.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public object Create(Type primaryType, MockFactory mockFactory, IMockObjectFactory mockObjectFactory)
		{
			if (_name == null)
			{
				_name = primaryType.DefaultNameFor();
			}

			DiscoverInterfaces(primaryType);

			var compositeType = new CompositeType(primaryType, _types.ToArray());

			/* ^ */ var _typeInfo = compositeType.PrimaryType.GetTypeInfo(); /* dpbullington@gmail.com ^ */

			if (/* ^ */ _typeInfo.IsInterface /* dpbullington@gmail.com ^ */)
			{
				if (_constructorArgs.Length > 0)
				{
					throw new InvalidOperationException("Cannot specify constructor arguments when mocking an interface");
				}
			}

			return mockObjectFactory.CreateMock(
				mockFactory,
				compositeType,
				_name,
				_mockStyle ?? MockStyle.Default,
				_constructorArgs);
		}

		#endregion

		private void DiscoverInterfaces(Type type)
		{
			foreach (Type t in type.GetInterfaces())
			{
				if (!_types.Contains(t))
				{
					lock(locker)
					{
						_types.Add(t);
					}
				}
				DiscoverInterfaces(t);
			}
		}

		/// <summary>
		/// Checks that interfaces do not contain ToString method declarations.
		/// </summary>
		/// <param name="mockedTypes">The types that are to be mocked.</param>
		private static void CheckInterfacesDoNotContainToStringMethodDeclaration(CompositeType mockedTypes)
		{
			foreach (var method in mockedTypes.GetMatchingMethods(new MethodNameMatcher("ToString"), false))
			{
				/* ^ */ var _typeInfo = method.DeclaringType.GetTypeInfo(); /* dpbullington@gmail.com ^ */

				if (/* ^ */ _typeInfo.IsInterface && method.GetParameters().Length == 0 /* dpbullington@gmail.com ^ */)
				{
					throw new ArgumentException("Interfaces must not contain a declaration for ToString().");
				}
			}
		}
	}
}
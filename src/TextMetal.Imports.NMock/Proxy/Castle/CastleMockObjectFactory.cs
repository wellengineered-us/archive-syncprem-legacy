#region Using

using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using NMock.Internal;

#endregion

/* ^ */ using System.Reflection; /* dpbullington@gmail.com ^ */

namespace NMock.Proxy.Castle
{
	/// <summary>
	/// Class that creates mocks for interfaces and classes (virtual members only) using the
	/// Castle proxy generator.
	/// </summary>
	public class CastleMockObjectFactory : MockObjectFactoryBase
	{
		/// <summary>
		/// A collection of types
		/// </summary>
		private static readonly Dictionary<CompositeType, Type> CachedProxyTypes = new Dictionary<CompositeType, Type>();
		object locker = new object();
		readonly IProxyBuilder ProxyBuilder;
		//protected ProxyGenerator Generator;

		/// <summary>
		/// Initializes a new instance of a <see cref="CastleMockObjectFactory"/>
		/// </summary>
		public CastleMockObjectFactory() : this(false)
		{
			
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="CastleMockObjectFactory"/>
		/// </summary>
		/// <param name="usePersistentProxyBuilder">A value indicating if the ProxyBuilder should be persistent</param>
		public CastleMockObjectFactory(bool usePersistentProxyBuilder)
		{
#if !SILVERLIGHT && FALSE // dpbullington@gmail.com
			// no PersistentProxyBuilder in Silverlight
			if (usePersistentProxyBuilder)
			{
				ProxyBuilder = new PersistentProxyBuilder();
			}
			else
			{
#endif
			ProxyBuilder = new DefaultProxyBuilder();
#if !SILVERLIGHT && FALSE // dpbullington@gmail.com
			}
#endif
			//Generator = new ProxyGenerator(ProxyBuilder);
		}

		#region IMockObjectFactory Members

		/// <summary>
		/// Creates a mock of the specified type(s).
		/// </summary>
		/// <param name="mockFactory">The mockFactory used to create this mock instance.</param>
		/// <param name="typesToMock">The type(s) to include in the mock.</param>
		/// <param name="name">The name to use for the mock instance.</param>
		/// <param name="mockStyle">The behaviour of the mock instance when first created.</param>
		/// <param name="constructorArgs">Constructor arguments for the class to be mocked. Only valid if mocking a class type.</param>
		/// <returns>A mock instance of the specified type(s).</returns>
		public override object CreateMock(MockFactory mockFactory, CompositeType typesToMock, string name, MockStyle mockStyle, object[] constructorArgs)
		{
			Type proxyType = GetProxyType(typesToMock);

			return InstantiateProxy(typesToMock, proxyType, mockFactory, mockStyle, name, constructorArgs);
		}

		#endregion

		private object InstantiateProxy(CompositeType compositeType, Type proxyType, MockFactory mockFactory, MockStyle mockStyle, string name, object[] constructorArgs)
		{
			IInterceptor interceptor = new MockObjectInterceptor(mockFactory, compositeType, name, mockStyle);
			object[] activationArgs;

			/* ^ */ var _typeInfo = compositeType.PrimaryType.GetTypeInfo(); /* dpbullington@gmail.com ^ */

			if (/* ^ */ _typeInfo.IsClass /* dpbullington@gmail.com ^ */)
			{
				activationArgs = new object[constructorArgs.Length + 1];
				constructorArgs.CopyTo(activationArgs, 1);
				activationArgs[0] = new[] { interceptor };
			}
			else
			{
				activationArgs = new[] { new[] { interceptor }, new object(), name };
			}

			return Activator.CreateInstance(proxyType, activationArgs);
		}

		private Type GetProxyType(CompositeType compositeType)
		{
			if (compositeType == null)
				throw new ArgumentNullException("compositeType");

			if (!CachedProxyTypes.ContainsKey(compositeType))
			{
				Type[] additionalInterfaceTypes = BuildAdditionalTypeArrayForProxyType(compositeType);
				Type proxyType;

				/* ^ */ var _typeInfo = compositeType.PrimaryType.GetTypeInfo(); /* dpbullington@gmail.com ^ */

				if (/* ^ */ _typeInfo.IsClass /* dpbullington@gmail.com ^ */)
				{
					if (/* ^ */ _typeInfo.IsSealed /* dpbullington@gmail.com ^ */)
					{
						throw new ArgumentException("Cannot mock sealed classes.");
					}

					proxyType = ProxyBuilder.CreateClassProxyType(compositeType.PrimaryType, additionalInterfaceTypes, ProxyGenerationOptions.Default);
				}
				else
				{
					proxyType = ProxyBuilder.CreateInterfaceProxyTypeWithoutTarget(compositeType.PrimaryType, additionalInterfaceTypes, new ProxyGenerationOptions { BaseTypeForInterfaceProxy = typeof(InterfaceMockBase) });
				}

				lock (locker)
				{
					if (!CachedProxyTypes.ContainsKey(compositeType))
						CachedProxyTypes[compositeType] = proxyType;
				}
				return proxyType;
			}

			return CachedProxyTypes[compositeType];
		}
	}
}
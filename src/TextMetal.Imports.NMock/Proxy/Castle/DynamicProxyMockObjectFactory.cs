#region Using

using System;
using Castle.DynamicProxy;
using NMock.Internal;
using NMock.Monitoring;

#endregion

/* ^ */ using System.Reflection; /* dpbullington@gmail.com ^ */

namespace NMock.Proxy.Castle
{
	//used for testing
	//trying to remove as much DP logic as possible
	//use the other until this becomes stable
	internal class DynamicProxyMockObjectFactory : MockObjectFactoryBase
	{
		protected IProxyBuilder builder;
		protected ProxyGenerator generator;

		public DynamicProxyMockObjectFactory()
		{
			builder = new DefaultProxyBuilder();
			generator = new ProxyGenerator(builder);
		}

		#region IMockObjectFactory Members

		public override object CreateMock(MockFactory mockFactory, CompositeType typesToMock, string name, MockStyle mockStyle, object[] constructorArgs)
		{
			if (typesToMock == null)
				throw new ArgumentNullException("typesToMock");

			Type primaryType = typesToMock.PrimaryType;
			Type[] additionalInterfaces = BuildAdditionalTypeArrayForProxyType(typesToMock);
			IInterceptor mockInterceptor = new MockObjectInterceptor(mockFactory, typesToMock, name, mockStyle);
			object result;

			/* ^ */ var _typeInfo = primaryType.GetTypeInfo(); /* dpbullington@gmail.com ^ */

			if (/* ^ */ _typeInfo.IsInterface /* dpbullington@gmail.com ^ */)
			{
				result = generator.CreateInterfaceProxyWithoutTarget(primaryType, additionalInterfaces, new ProxyGenerationOptions {BaseTypeForInterfaceProxy = typeof (InterfaceMockBase)}, mockInterceptor);
				((InterfaceMockBase) result).Name = name;
			}
			else
			{
				result = generator.CreateClassProxy(primaryType, additionalInterfaces, ProxyGenerationOptions.Default, constructorArgs, mockInterceptor);
				//return generator.CreateClassProxy(primaryType, new []{typeof(IMockObject)}, mockInterceptor);
			}

			return result;
		}

		#endregion
	}
}
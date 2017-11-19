using System;
using System.Collections.Generic;

namespace NMock.Proxy
{
	/// <summary>
	/// A base implementation of the <see cref="IMockObjectFactory"/> interface
	/// </summary>
	public abstract class MockObjectFactoryBase : IMockObjectFactory
	{
		/// <summary>
		/// Combines the specified types with the <see cref="IMockObject"/> into an array.
		/// </summary>
		/// <returns>An array of the specified types and <see cref="IMockObject"/>,</returns>
		protected static Type[] BuildAdditionalTypeArrayForProxyType(CompositeType compositeType)
		{
			List<Type> types = new List<Type>();

			//IMockObject always needs to be first
			types.Add(typeof(IMockObject));

			types.AddRange(compositeType.AdditionalInterfaceTypes);

			return types.ToArray();

			/*
			if (compositeType.PrimaryType.IsInterface)
			{
				//The primary type is an interface so there is no need to check for explicit mapping
				types.AddRange(compositeType.AdditionalInterfaceTypes);
			}
			else
			{
				foreach (Type t in compositeType.AdditionalInterfaceTypes)
				{
					if (t.IsAssignableFrom(compositeType.PrimaryType))
					{
						InterfaceMapping map = compositeType.PrimaryType.GetInterfaceMap(t);

						//if any members are public, the interface is not using explicit implementation
						if (map.TargetMethods.Any(info => info.IsPublic))
						{
							//castle dynamic proxy can only have additionalInterfaceTypes that are not explicit implementation
							//see: http://www.mail-archive.com/castle-project-devel@googlegroups.com/msg04685.html
							types.Add(t);
						}
					}
					else
					{
						//add the type so it is generated in the proxy
						types.Add(t);
					}
				}
			}

			return types.ToArray();
			*/
		}

		/// <summary>
		/// Creates a mock of the specified type(s).
		/// </summary>
		/// <param name="mockFactory">The mockFactory used to create this mock instance.</param>
		/// <param name="typesToMock">The type(s) to include in the mock.</param>
		/// <param name="name">The name to use for the mock instance.</param>
		/// <param name="mockStyle">The behaviour of the mock instance when first created.</param>
		/// <param name="constructorArgs">Constructor arguments for the class to be mocked. Only valid if mocking a class type.</param>
		/// <returns>A mock instance of the specified type(s).</returns>
		public abstract object CreateMock(MockFactory mockFactory, CompositeType typesToMock, string name, MockStyle mockStyle, object[] constructorArgs);
	}
}

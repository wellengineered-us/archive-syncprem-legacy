/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using NMock;

using NUnit.Framework;

using TextMetal.Middleware.Solder.Injection;
using TextMetal.Middleware.UnitTests.TestingInfrastructure;

namespace TextMetal.Middleware.UnitTests.Solder.Injection._
{
	[TestFixture]
	public class SingletonWrapperDependencyResolutionTests
	{
		#region Constructors/Destructors

		public SingletonWrapperDependencyResolutionTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateAndEvaluateTest()
		{
			SingletonWrapperDependencyResolution singletonWrapperDependencyResolution;
			IDependencyManager mockDependencyManager;
			IDependencyResolution mockDependencyResolution;
			IDependencyManager _unusedDependencyManager = null;
			Type _unusedType = null;
			string _unusedString = null;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			Expect.On(mockDependencyResolution).One.Method(m => m.Resolve(_unusedDependencyManager, _unusedType, _unusedString)).With(mockDependencyManager, typeof(object), string.Empty).WillReturn(11);
			Expect.On(mockDependencyResolution).One.Method(m => m.Dispose());

			singletonWrapperDependencyResolution = new SingletonWrapperDependencyResolution(mockDependencyResolution);

			Assert.AreEqual(DependencyLifetime.Singleton, singletonWrapperDependencyResolution.DependencyLifetime);

			// should be thawed at this point
			result = singletonWrapperDependencyResolution.Resolve(mockDependencyManager, typeof(object), string.Empty);

			Assert.IsNotNull(result);
			Assert.AreEqual(11, result);

			// should be frozen at this point
			result = singletonWrapperDependencyResolution.Resolve(mockDependencyManager, typeof(object), string.Empty);

			Assert.IsNotNull(result);
			Assert.AreEqual(11, result);

			singletonWrapperDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyManagerResolveTest()
		{
			SingletonWrapperDependencyResolution singletonWrapperDependencyResolution;
			IDependencyManager mockDependencyManager;
			IDependencyResolution mockDependencyResolution;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = null;
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			singletonWrapperDependencyResolution = new SingletonWrapperDependencyResolution(mockDependencyResolution);

			result = singletonWrapperDependencyResolution.Resolve(mockDependencyManager, typeof(object), string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyResolutionCreateTest()
		{
			SingletonWrapperDependencyResolution singletonWrapperDependencyResolution;
			IDependencyResolution mockDependencyResolution;

			mockDependencyResolution = null;

			singletonWrapperDependencyResolution = new SingletonWrapperDependencyResolution(mockDependencyResolution);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveTest()
		{
			SingletonWrapperDependencyResolution singletonWrapperDependencyResolution;
			IDependencyManager mockDependencyManager;
			IDependencyResolution mockDependencyResolution;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			singletonWrapperDependencyResolution = new SingletonWrapperDependencyResolution(mockDependencyResolution);

			result = singletonWrapperDependencyResolution.Resolve(mockDependencyManager, typeof(object), null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeResolveTest()
		{
			SingletonWrapperDependencyResolution singletonWrapperDependencyResolution;
			IDependencyManager mockDependencyManager;
			IDependencyResolution mockDependencyResolution;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			singletonWrapperDependencyResolution = new SingletonWrapperDependencyResolution(mockDependencyResolution);

			result = singletonWrapperDependencyResolution.Resolve(mockDependencyManager, null, string.Empty);
		}

		#endregion
	}
}
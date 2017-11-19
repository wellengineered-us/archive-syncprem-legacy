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
	public class TransientDefaultConstructorDependencyResolutionTests
	{
		#region Constructors/Destructors

		public TransientDefaultConstructorDependencyResolutionTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateAndEvaluateTest()
		{
			TransientDefaultConstructorDependencyResolution transientDefaultConstructorDependencyResolution;
			IDependencyManager mockDependencyManager;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			transientDefaultConstructorDependencyResolution = new TransientDefaultConstructorDependencyResolution(typeof(MockDependantObject));

			Assert.AreEqual(DependencyLifetime.Transient, transientDefaultConstructorDependencyResolution.DependencyLifetime);

			result = transientDefaultConstructorDependencyResolution.Resolve(mockDependencyManager, typeof(object), string.Empty);

			Assert.IsNotNull(result);
			Assert.IsInstanceOf<MockDependantObject>(result);
			Assert.IsNull(((MockDependantObject)result).Text);

			transientDefaultConstructorDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullActivatorTypeCreateTest()
		{
			TransientDefaultConstructorDependencyResolution transientFactoryMethodDependencyResolution;
			Type type;

			type = null;

			transientFactoryMethodDependencyResolution = new TransientDefaultConstructorDependencyResolution(type);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyManagerResolveTest()
		{
			TransientDefaultConstructorDependencyResolution transientDefaultConstructorDependencyResolution;
			IDependencyManager mockDependencyManager;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = null;

			transientDefaultConstructorDependencyResolution = new TransientDefaultConstructorDependencyResolution(typeof(int));

			result = transientDefaultConstructorDependencyResolution.Resolve(mockDependencyManager, typeof(object), string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveTest()
		{
			TransientDefaultConstructorDependencyResolution transientDefaultConstructorDependencyResolution;
			IDependencyManager mockDependencyManager;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			transientDefaultConstructorDependencyResolution = new TransientDefaultConstructorDependencyResolution(typeof(int));

			result = transientDefaultConstructorDependencyResolution.Resolve(mockDependencyManager, typeof(object), null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeResolveTest()
		{
			TransientDefaultConstructorDependencyResolution transientDefaultConstructorDependencyResolution;
			IDependencyManager mockDependencyManager;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			transientDefaultConstructorDependencyResolution = new TransientDefaultConstructorDependencyResolution(typeof(int));

			result = transientDefaultConstructorDependencyResolution.Resolve(mockDependencyManager, null, string.Empty);
		}

		#endregion
	}
}
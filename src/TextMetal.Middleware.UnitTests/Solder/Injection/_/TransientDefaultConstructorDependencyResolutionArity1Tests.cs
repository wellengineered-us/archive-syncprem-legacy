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
	public class TransientDefaultConstructorDependencyResolutionArity1Tests
	{
		#region Constructors/Destructors

		public TransientDefaultConstructorDependencyResolutionArity1Tests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateAndEvaluateTest()
		{
			TransientDefaultConstructorDependencyResolution<MockDependantObject> transientDefaultConstructorDependencyResolution;
			IDependencyManager mockDependencyManager;
			MockDependantObject result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			transientDefaultConstructorDependencyResolution = new TransientDefaultConstructorDependencyResolution<MockDependantObject>();

			Assert.AreEqual(DependencyLifetime.Transient, transientDefaultConstructorDependencyResolution.DependencyLifetime);

			result = transientDefaultConstructorDependencyResolution.Resolve(mockDependencyManager, string.Empty);

			Assert.IsNotNull(result);
			Assert.IsNull(result.Text);

			transientDefaultConstructorDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldCreateAndEvaluateUntypedTest()
		{
			TransientDefaultConstructorDependencyResolution<MockDependantObject> transientDefaultConstructorDependencyResolution;
			IDependencyManager mockDependencyManager;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			transientDefaultConstructorDependencyResolution = new TransientDefaultConstructorDependencyResolution<MockDependantObject>();

			Assert.AreEqual(DependencyLifetime.Transient, transientDefaultConstructorDependencyResolution.DependencyLifetime);

			result = transientDefaultConstructorDependencyResolution.Resolve(mockDependencyManager, typeof(MockDependantObject), string.Empty);

			Assert.IsNotNull(result);
			Assert.IsInstanceOf<MockDependantObject>(result);
			Assert.IsNull(((MockDependantObject)result).Text);

			transientDefaultConstructorDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyManagerResolveTest()
		{
			TransientDefaultConstructorDependencyResolution<int> transientDefaultConstructorDependencyResolution;
			IDependencyManager mockDependencyManager;
			int result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = null;

			transientDefaultConstructorDependencyResolution = new TransientDefaultConstructorDependencyResolution<int>();

			result = transientDefaultConstructorDependencyResolution.Resolve(mockDependencyManager, string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyManagerResolveUntypedTest()
		{
			TransientDefaultConstructorDependencyResolution<int> transientDefaultConstructorDependencyResolution;
			IDependencyManager mockDependencyManager;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = null;

			transientDefaultConstructorDependencyResolution = new TransientDefaultConstructorDependencyResolution<int>();

			result = transientDefaultConstructorDependencyResolution.Resolve(mockDependencyManager, typeof(int), string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveTest()
		{
			TransientDefaultConstructorDependencyResolution<int> transientDefaultConstructorDependencyResolution;
			IDependencyManager mockDependencyManager;
			int result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			transientDefaultConstructorDependencyResolution = new TransientDefaultConstructorDependencyResolution<int>();

			result = transientDefaultConstructorDependencyResolution.Resolve(mockDependencyManager, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveUntypedTest()
		{
			TransientDefaultConstructorDependencyResolution<int> transientDefaultConstructorDependencyResolution;
			IDependencyManager mockDependencyManager;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			transientDefaultConstructorDependencyResolution = new TransientDefaultConstructorDependencyResolution<int>();

			result = transientDefaultConstructorDependencyResolution.Resolve(mockDependencyManager, typeof(int), null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeResolveUntypedTest()
		{
			TransientDefaultConstructorDependencyResolution<int> transientDefaultConstructorDependencyResolution;
			IDependencyManager mockDependencyManager;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			transientDefaultConstructorDependencyResolution = new TransientDefaultConstructorDependencyResolution<int>();

			result = transientDefaultConstructorDependencyResolution.Resolve(mockDependencyManager, null, string.Empty);
		}

		#endregion
	}
}
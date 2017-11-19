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
	public class TransientFactoryMethodDependencyResolutionArity1Tests
	{
		#region Constructors/Destructors

		public TransientFactoryMethodDependencyResolutionArity1Tests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateAndEvaluateTest()
		{
			TransientFactoryMethodDependencyResolution<int> transientFactoryMethodDependencyResolution;
			IDependencyManager mockDependencyManager;
			Func<int> value;
			int result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			value = () => 11;

			transientFactoryMethodDependencyResolution = new TransientFactoryMethodDependencyResolution<int>(value);

			Assert.AreEqual(DependencyLifetime.Transient, transientFactoryMethodDependencyResolution.DependencyLifetime);

			result = transientFactoryMethodDependencyResolution.Resolve(mockDependencyManager, string.Empty);

			Assert.IsNotNull(result);
			Assert.AreEqual(11, result);

			transientFactoryMethodDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldCreateAndEvaluateUntypedTest()
		{
			TransientFactoryMethodDependencyResolution<int> transientFactoryMethodDependencyResolution;
			IDependencyManager mockDependencyManager;
			Func<int> value;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			value = () => 11;

			transientFactoryMethodDependencyResolution = new TransientFactoryMethodDependencyResolution<int>(value);

			Assert.AreEqual(DependencyLifetime.Transient, transientFactoryMethodDependencyResolution.DependencyLifetime);

			result = transientFactoryMethodDependencyResolution.Resolve(mockDependencyManager, typeof(int), string.Empty);

			Assert.IsNotNull(result);
			Assert.AreEqual(11, result);

			transientFactoryMethodDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyManagerResolveTest()
		{
			TransientFactoryMethodDependencyResolution<int> transientFactoryMethodDependencyResolution;
			IDependencyManager mockDependencyManager;
			Func<int> value;
			int result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = null;

			value = () => 11;

			transientFactoryMethodDependencyResolution = new TransientFactoryMethodDependencyResolution<int>(value);

			result = transientFactoryMethodDependencyResolution.Resolve(mockDependencyManager, string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyManagerResolveUntypedTest()
		{
			TransientFactoryMethodDependencyResolution<int> transientFactoryMethodDependencyResolution;
			IDependencyManager mockDependencyManager;
			Func<int> value;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = null;

			value = () => 11;

			transientFactoryMethodDependencyResolution = new TransientFactoryMethodDependencyResolution<int>(value);

			result = transientFactoryMethodDependencyResolution.Resolve(mockDependencyManager, typeof(int), string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullFactoryMethodCreateTest()
		{
			TransientFactoryMethodDependencyResolution<int> transientFactoryMethodDependencyResolution;
			Func<int> value;

			value = null;

			transientFactoryMethodDependencyResolution = new TransientFactoryMethodDependencyResolution<int>(value);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveTest()
		{
			TransientFactoryMethodDependencyResolution<int> transientFactoryMethodDependencyResolution;
			IDependencyManager mockDependencyManager;
			Func<int> value;
			int result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			value = () => 11;

			transientFactoryMethodDependencyResolution = new TransientFactoryMethodDependencyResolution<int>(value);

			result = transientFactoryMethodDependencyResolution.Resolve(mockDependencyManager, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveUntypedTest()
		{
			TransientFactoryMethodDependencyResolution<int> transientFactoryMethodDependencyResolution;
			IDependencyManager mockDependencyManager;
			Func<int> value;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			value = () => 11;

			transientFactoryMethodDependencyResolution = new TransientFactoryMethodDependencyResolution<int>(value);

			result = transientFactoryMethodDependencyResolution.Resolve(mockDependencyManager, typeof(int), null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeResolveUntypedTest()
		{
			TransientFactoryMethodDependencyResolution<int> transientFactoryMethodDependencyResolution;
			IDependencyManager mockDependencyManager;
			Func<int> value;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();

			value = () => 11;

			transientFactoryMethodDependencyResolution = new TransientFactoryMethodDependencyResolution<int>(value);

			result = transientFactoryMethodDependencyResolution.Resolve(mockDependencyManager, null, string.Empty);
		}

		#endregion
	}
}
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
	public class SingletonWrapperDependencyResolutionArity1Tests
	{
		#region Constructors/Destructors

		public SingletonWrapperDependencyResolutionArity1Tests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateAndEvaluateTest()
		{
			SingletonWrapperDependencyResolution<int> singletonWrapperDependencyResolution;
			IDependencyManager mockDependencyManager;
			IDependencyResolution<int> mockDependencyResolution;
			IDependencyManager _unusedDependencyManager = null;
			string _unusedString = null;
			int result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<int>>();

			Expect.On(mockDependencyResolution).One.Method(m => m.Resolve(_unusedDependencyManager, _unusedString)).With(mockDependencyManager, string.Empty).WillReturn(11);
			Expect.On(mockDependencyResolution).One.Method(m => m.Dispose());

			singletonWrapperDependencyResolution = new SingletonWrapperDependencyResolution<int>(mockDependencyResolution);

			Assert.AreEqual(DependencyLifetime.Singleton, singletonWrapperDependencyResolution.DependencyLifetime);

			// should be thawed at this point
			result = singletonWrapperDependencyResolution.Resolve(mockDependencyManager, string.Empty);

			Assert.IsNotNull(result);
			Assert.AreEqual(11, result);

			// should be frozen at this point
			result = singletonWrapperDependencyResolution.Resolve(mockDependencyManager, string.Empty);

			Assert.IsNotNull(result);
			Assert.AreEqual(11, result);

			singletonWrapperDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldCreateAndEvaluateUntypedTest()
		{
			SingletonWrapperDependencyResolution<int> singletonWrapperDependencyResolution;
			IDependencyManager mockDependencyManager;
			IDependencyResolution<int> mockDependencyResolution;
			IDependencyManager _unusedDependencyManager = null;
			string _unusedString = null;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<int>>();

			Expect.On(mockDependencyResolution).One.Method(m => m.Resolve(_unusedDependencyManager, _unusedString)).With(mockDependencyManager, string.Empty).WillReturn(11);
			Expect.On(mockDependencyResolution).One.Method(m => m.Dispose());

			singletonWrapperDependencyResolution = new SingletonWrapperDependencyResolution<int>(mockDependencyResolution);

			Assert.AreEqual(DependencyLifetime.Singleton, singletonWrapperDependencyResolution.DependencyLifetime);

			// should be thawed at this point
			result = singletonWrapperDependencyResolution.Resolve(mockDependencyManager, typeof(int), string.Empty);

			Assert.IsNotNull(result);
			Assert.AreEqual(11, result);

			// should be frozen at this point
			result = singletonWrapperDependencyResolution.Resolve(mockDependencyManager, typeof(int), string.Empty);

			Assert.IsNotNull(result);
			Assert.AreEqual(11, result);

			singletonWrapperDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyManagerResolveTest()
		{
			SingletonWrapperDependencyResolution<int> singletonWrapperDependencyResolution;
			IDependencyManager mockDependencyManager;
			IDependencyResolution<int> mockDependencyResolution;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = null;
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<int>>();

			singletonWrapperDependencyResolution = new SingletonWrapperDependencyResolution<int>(mockDependencyResolution);

			result = singletonWrapperDependencyResolution.Resolve(mockDependencyManager, string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyManagerResolveUntypedTest()
		{
			SingletonWrapperDependencyResolution<int> singletonWrapperDependencyResolution;
			IDependencyManager mockDependencyManager;
			IDependencyResolution<int> mockDependencyResolution;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = null;
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<int>>();

			singletonWrapperDependencyResolution = new SingletonWrapperDependencyResolution<int>(mockDependencyResolution);

			result = singletonWrapperDependencyResolution.Resolve(mockDependencyManager, typeof(int), string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyResolutionCreateTest()
		{
			SingletonWrapperDependencyResolution<int> singletonWrapperDependencyResolution;
			IDependencyResolution<int> mockDependencyResolution;

			mockDependencyResolution = null;

			singletonWrapperDependencyResolution = new SingletonWrapperDependencyResolution<int>(mockDependencyResolution);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveTest()
		{
			SingletonWrapperDependencyResolution<object> singletonWrapperDependencyResolution;
			IDependencyManager mockDependencyManager;
			IDependencyResolution<object> mockDependencyResolution;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<object>>();

			singletonWrapperDependencyResolution = new SingletonWrapperDependencyResolution<object>(mockDependencyResolution);

			result = singletonWrapperDependencyResolution.Resolve(mockDependencyManager, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveUntypedTest()
		{
			SingletonWrapperDependencyResolution<int> singletonWrapperDependencyResolution;
			IDependencyManager mockDependencyManager;
			IDependencyResolution<int> mockDependencyResolution;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<int>>();

			singletonWrapperDependencyResolution = new SingletonWrapperDependencyResolution<int>(mockDependencyResolution);

			result = singletonWrapperDependencyResolution.Resolve(mockDependencyManager, typeof(int), null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeResolveUntypedTest()
		{
			SingletonWrapperDependencyResolution<int> singletonWrapperDependencyResolution;
			IDependencyManager mockDependencyManager;
			IDependencyResolution<int> mockDependencyResolution;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<int>>();

			singletonWrapperDependencyResolution = new SingletonWrapperDependencyResolution<int>(mockDependencyResolution);

			result = singletonWrapperDependencyResolution.Resolve(mockDependencyManager, null, string.Empty);
		}

		#endregion
	}
}
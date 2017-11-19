/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using NMock;

using NUnit.Framework;

using TextMetal.Middleware.Solder.Context;
using TextMetal.Middleware.Solder.Injection;
using TextMetal.Middleware.UnitTests.TestingInfrastructure;

namespace TextMetal.Middleware.UnitTests.Solder.Injection._
{
	[TestFixture]
	public class ContextWrapperDependencyResolutionArity1Tests
	{
		#region Constructors/Destructors

		public ContextWrapperDependencyResolutionArity1Tests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateAndEvaluateTest()
		{
			ContextWrapperDependencyResolution<int> contextWrapperDependencyResolution;
			IDependencyManager mockDependencyManager;
			IDependencyResolution<int> mockDependencyResolution;
			IDependencyManager _unusedDependencyManager = null;
			string _unusedString = null;
			int result;
			const int EXPECTED = 11;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<int>>();

			Expect.On(mockDependencyResolution).One.Method(m => m.Resolve(_unusedDependencyManager, _unusedString)).With(mockDependencyManager, string.Empty).WillReturn(EXPECTED);
			Expect.On(mockDependencyResolution).One.Method(m => m.Dispose());

			contextWrapperDependencyResolution = new ContextWrapperDependencyResolution<int>(ContextScope.LocalThreadSafe, mockDependencyResolution);

			Assert.AreEqual(DependencyLifetime.Singleton, contextWrapperDependencyResolution.DependencyLifetime);

			// should be thawed at this point
			result = contextWrapperDependencyResolution.Resolve(mockDependencyManager, string.Empty);

			Assert.IsNotNull(result);
			Assert.AreEqual(EXPECTED, result);

			// should be frozen at this point
			result = contextWrapperDependencyResolution.Resolve(mockDependencyManager, string.Empty);

			Assert.IsNotNull(result);
			Assert.AreEqual(EXPECTED, result);

			contextWrapperDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldCreateAndEvaluateUntypedTest()
		{
			ContextWrapperDependencyResolution<int> contextWrapperDependencyResolution;
			IDependencyManager mockDependencyManager;
			IDependencyResolution<int> mockDependencyResolution;
			IDependencyManager _unusedDependencyManager = null;
			string _unusedString = null;
			object result;
			const int EXPECTED = 11;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<int>>();

			Expect.On(mockDependencyResolution).One.Method(m => m.Resolve(_unusedDependencyManager, _unusedString)).With(mockDependencyManager, string.Empty).WillReturn(EXPECTED);
			Expect.On(mockDependencyResolution).One.Method(m => m.Dispose());

			contextWrapperDependencyResolution = new ContextWrapperDependencyResolution<int>(ContextScope.LocalThreadSafe, mockDependencyResolution);

			Assert.AreEqual(DependencyLifetime.Singleton, contextWrapperDependencyResolution.DependencyLifetime);

			// should be thawed at this point
			result = contextWrapperDependencyResolution.Resolve(mockDependencyManager, typeof(int), string.Empty);

			Assert.IsNotNull(result);
			Assert.AreEqual(EXPECTED, result);

			// should be frozen at this point
			result = contextWrapperDependencyResolution.Resolve(mockDependencyManager, typeof(int), string.Empty);

			Assert.IsNotNull(result);
			Assert.AreEqual(EXPECTED, result);

			contextWrapperDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldCreateTest__todo_mock_contextual_storage_strategy()
		{
			Assert.Ignore("TODO: This test case has not been implemented yet.");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullContextualStorageStrategyCreateTest()
		{
			ContextWrapperDependencyResolution<int> contextWrapperDependencyResolution;
			IContextualStorageStrategy mockContextualStorageStrategy;
			IDependencyResolution<int> mockDependencyResolution;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockContextualStorageStrategy = null;
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<int>>();

			contextWrapperDependencyResolution = new ContextWrapperDependencyResolution<int>(mockContextualStorageStrategy, mockDependencyResolution);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyManagerResolveTest()
		{
			ContextWrapperDependencyResolution<int> contextWrapperDependencyResolution;
			IDependencyManager mockDependencyManager;
			IDependencyResolution<int> mockDependencyResolution;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = null;
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<int>>();

			contextWrapperDependencyResolution = new ContextWrapperDependencyResolution<int>(ContextScope.LocalThreadSafe, mockDependencyResolution);

			result = contextWrapperDependencyResolution.Resolve(mockDependencyManager, string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyManagerResolveUntypedTest()
		{
			ContextWrapperDependencyResolution<int> contextWrapperDependencyResolution;
			IDependencyManager mockDependencyManager;
			IDependencyResolution<int> mockDependencyResolution;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = null;
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<int>>();

			contextWrapperDependencyResolution = new ContextWrapperDependencyResolution<int>(ContextScope.LocalThreadSafe, mockDependencyResolution);

			result = contextWrapperDependencyResolution.Resolve(mockDependencyManager, typeof(int), string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyResolutionCreateTest()
		{
			ContextWrapperDependencyResolution<int> contextWrapperDependencyResolution;
			IContextualStorageStrategy mockContextualStorageStrategy;
			IDependencyResolution<int> mockDependencyResolution;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockContextualStorageStrategy = mockFactory.CreateInstance<IContextualStorageStrategy>();
			mockDependencyResolution = null;

			contextWrapperDependencyResolution = new ContextWrapperDependencyResolution<int>(mockContextualStorageStrategy, mockDependencyResolution);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveTest()
		{
			ContextWrapperDependencyResolution<object> contextWrapperDependencyResolution;
			IDependencyManager mockDependencyManager;
			IDependencyResolution<object> mockDependencyResolution;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<object>>();

			contextWrapperDependencyResolution = new ContextWrapperDependencyResolution<object>(ContextScope.LocalThreadSafe, mockDependencyResolution);

			result = contextWrapperDependencyResolution.Resolve(mockDependencyManager, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveUntypedTest()
		{
			ContextWrapperDependencyResolution<int> contextWrapperDependencyResolution;
			IDependencyManager mockDependencyManager;
			IDependencyResolution<int> mockDependencyResolution;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<int>>();

			contextWrapperDependencyResolution = new ContextWrapperDependencyResolution<int>(ContextScope.LocalThreadSafe, mockDependencyResolution);

			result = contextWrapperDependencyResolution.Resolve(mockDependencyManager, typeof(int), null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeResolveUntypedTest()
		{
			ContextWrapperDependencyResolution<int> contextWrapperDependencyResolution;
			IDependencyManager mockDependencyManager;
			IDependencyResolution<int> mockDependencyResolution;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<int>>();

			contextWrapperDependencyResolution = new ContextWrapperDependencyResolution<int>(ContextScope.LocalThreadSafe, mockDependencyResolution);

			result = contextWrapperDependencyResolution.Resolve(mockDependencyManager, null, string.Empty);
		}

		#endregion
	}
}
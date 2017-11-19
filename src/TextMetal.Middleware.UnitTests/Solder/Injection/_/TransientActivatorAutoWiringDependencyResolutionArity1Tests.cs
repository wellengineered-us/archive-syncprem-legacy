/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using NMock;

using NUnit.Framework;

using TextMetal.Middleware.Solder.Injection;
using TextMetal.Middleware.Solder.Utilities;
using TextMetal.Middleware.UnitTests.TestingInfrastructure;

namespace TextMetal.Middleware.UnitTests.Solder.Injection._
{
	[TestFixture]
	public class TransientActivatorAutoWiringDependencyResolutionArity1Tests
	{
		#region Constructors/Destructors

		public TransientActivatorAutoWiringDependencyResolutionArity1Tests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateAndEvaluateSelectorKeyTest()
		{
			TransientActivatorAutoWiringDependencyResolution<MockDependantObject> transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			IReflectionFascade mockReflectionFascade;
			MockDependantObject result;
			MockFactory mockFactory;
			string _unusedString = null;
			bool _unusedBoolean = false;
			Type _unusedType = null;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockReflectionFascade = mockFactory.CreateInstance<IReflectionFascade>();

			Expect.On(mockDependencyManager).One.Method(m => m.ResolveDependency(_unusedType, _unusedString, _unusedBoolean)).With(typeof(MockDependantObject), string.Empty, true).Will(Return.Value(new MockDependantObject("both")));

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution<MockDependantObject>(mockReflectionFascade);

			Assert.AreEqual(DependencyLifetime.Transient, transientActivatorAutoWiringDependencyResolution.DependencyLifetime);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, "named_dep_obj");

			Assert.IsNotNull(result);
			Assert.IsInstanceOf<MockDependantObject>(result);
			Assert.IsNotNull(result.Text);
			Assert.AreEqual(string.Empty, result.Text);
			Assert.IsNotNull(result.Left);
			Assert.IsNotNull(result.Right);
			Assert.AreEqual("both", result.Left.Text);
			Assert.AreEqual("both", result.Right.Text);

			transientActivatorAutoWiringDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldCreateAndEvaluateTest()
		{
			TransientActivatorAutoWiringDependencyResolution<MockDependantObject> transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			IReflectionFascade mockReflectionFascade;
			MockDependantObject result;
			MockFactory mockFactory;
			string _unusedString = null;
			bool _unusedBoolean = false;
			Type _unusedType = null;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockReflectionFascade = mockFactory.CreateInstance<IReflectionFascade>();

			Expect.On(mockDependencyManager).One.Method(m => m.ResolveDependency(_unusedType, _unusedString, _unusedBoolean)).With(typeof(MockDependantObject), "named_dep_obj", true).Will(Return.Value(new MockDependantObject("left")));
			Expect.On(mockDependencyManager).One.Method(m => m.ResolveDependency(_unusedType, _unusedString, _unusedBoolean)).With(typeof(MockDependantObject), string.Empty, true).Will(Return.Value(new MockDependantObject("right")));

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution<MockDependantObject>(mockReflectionFascade);

			Assert.AreEqual(DependencyLifetime.Transient, transientActivatorAutoWiringDependencyResolution.DependencyLifetime);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, string.Empty);

			Assert.IsNotNull(result);
			Assert.IsInstanceOf<MockDependantObject>(result);
			Assert.IsNotNull(result.Text);
			Assert.AreEqual(string.Empty, result.Text);
			Assert.IsNotNull(result.Left);
			Assert.IsNotNull(result.Right);
			Assert.AreEqual("left", result.Left.Text);
			Assert.AreEqual("right", result.Right.Text);

			transientActivatorAutoWiringDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldCreateAndEvaluateUntypedTest()
		{
			TransientActivatorAutoWiringDependencyResolution<MockDependantObject> transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			IReflectionFascade mockReflectionFascade;
			object result;
			MockFactory mockFactory;
			string _unusedString = null;
			bool _unusedBoolean = false;
			Type _unusedType = null;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockReflectionFascade = mockFactory.CreateInstance<IReflectionFascade>();

			Expect.On(mockDependencyManager).One.Method(m => m.ResolveDependency(_unusedType, _unusedString, _unusedBoolean)).With(typeof(MockDependantObject), "named_dep_obj", true).Will(Return.Value(new MockDependantObject("left")));
			Expect.On(mockDependencyManager).One.Method(m => m.ResolveDependency(_unusedType, _unusedString, _unusedBoolean)).With(typeof(MockDependantObject), string.Empty, true).Will(Return.Value(new MockDependantObject("right")));

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution<MockDependantObject>(mockReflectionFascade);

			Assert.AreEqual(DependencyLifetime.Transient, transientActivatorAutoWiringDependencyResolution.DependencyLifetime);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, typeof(MockDependantObject), string.Empty);

			Assert.IsNotNull(result);
			Assert.IsInstanceOf<MockDependantObject>(result);
			Assert.IsNotNull(((MockDependantObject)result).Text);
			Assert.AreEqual(string.Empty, ((MockDependantObject)result).Text);
			Assert.IsNotNull(((MockDependantObject)result).Left);
			Assert.IsNotNull(((MockDependantObject)result).Right);
			Assert.AreEqual("left", ((MockDependantObject)result).Left.Text);
			Assert.AreEqual("right", ((MockDependantObject)result).Right.Text);

			transientActivatorAutoWiringDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyManagerResolveTest()
		{
			TransientActivatorAutoWiringDependencyResolution<MockDependantObject> transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			IReflectionFascade mockReflectionFascade;
			MockDependantObject result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = null;
			mockReflectionFascade = mockFactory.CreateInstance<IReflectionFascade>();

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution<MockDependantObject>(mockReflectionFascade);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyManagerResolveUntypedTest()
		{
			TransientActivatorAutoWiringDependencyResolution<MockDependantObject> transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			IReflectionFascade mockReflectionFascade;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = null;
			mockReflectionFascade = mockFactory.CreateInstance<IReflectionFascade>();

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution<MockDependantObject>(mockReflectionFascade);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, typeof(MockDependantObject), string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveTest()
		{
			TransientActivatorAutoWiringDependencyResolution<MockDependantObject> transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			IReflectionFascade mockReflectionFascade;
			MockDependantObject result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockReflectionFascade = mockFactory.CreateInstance<IReflectionFascade>();

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution<MockDependantObject>(mockReflectionFascade);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveUntypedTest()
		{
			TransientActivatorAutoWiringDependencyResolution<MockDependantObject> transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			IReflectionFascade mockReflectionFascade;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockReflectionFascade = mockFactory.CreateInstance<IReflectionFascade>();

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution<MockDependantObject>(mockReflectionFascade);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, typeof(MockDependantObject), null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullReflectionFascadeCreateTest()
		{
			TransientActivatorAutoWiringDependencyResolution<MockDependantObject> transientActivatorAutoWiringDependencyResolution;
			IReflectionFascade mockReflectionFascade;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockReflectionFascade = null;

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution<MockDependantObject>(mockReflectionFascade);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeResolveUntypedTest()
		{
			TransientActivatorAutoWiringDependencyResolution<MockDependantObject> transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			IReflectionFascade mockReflectionFascade;
			object result;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockReflectionFascade = mockFactory.CreateInstance<IReflectionFascade>();

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution<MockDependantObject>(mockReflectionFascade);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, null, string.Empty);
		}

		#endregion
	}
}
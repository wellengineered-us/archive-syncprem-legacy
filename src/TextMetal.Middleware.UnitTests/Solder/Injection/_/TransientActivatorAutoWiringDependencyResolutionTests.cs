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
	public class TransientActivatorAutoWiringDependencyResolutionTests
	{
		#region Constructors/Destructors

		public TransientActivatorAutoWiringDependencyResolutionTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateAndEvaluateSelectorKeyTest()
		{
			TransientActivatorAutoWiringDependencyResolution transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			IReflectionFascade mockReflectionFascade;
			Type activatorType;
			object result;
			MockFactory mockFactory;
			string _unusedString = null;
			bool _unusedBoolean = false;
			Type _unusedType = null;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockReflectionFascade = mockFactory.CreateInstance<IReflectionFascade>();

			activatorType = typeof(MockDependantObject);

			Expect.On(mockDependencyManager).One.Method(m => m.ResolveDependency(_unusedType, _unusedString, _unusedBoolean)).With(typeof(MockDependantObject), string.Empty, true).Will(Return.Value(new MockDependantObject("both")));

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution(mockReflectionFascade, activatorType);

			Assert.AreEqual(DependencyLifetime.Transient, transientActivatorAutoWiringDependencyResolution.DependencyLifetime);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, typeof(object), "named_dep_obj");

			Assert.IsNotNull(result);
			Assert.IsInstanceOf<MockDependantObject>(result);
			Assert.IsNotNull(((MockDependantObject)result).Text);
			Assert.AreEqual(string.Empty, ((MockDependantObject)result).Text);
			Assert.IsNotNull(((MockDependantObject)result).Left);
			Assert.IsNotNull(((MockDependantObject)result).Right);
			Assert.AreEqual("both", ((MockDependantObject)result).Left.Text);
			Assert.AreEqual("both", ((MockDependantObject)result).Right.Text);

			transientActivatorAutoWiringDependencyResolution.Dispose();

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldCreateAndEvaluateTest()
		{
			TransientActivatorAutoWiringDependencyResolution transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			IReflectionFascade mockReflectionFascade;
			Type activatorType;
			object result;
			MockFactory mockFactory;
			string _unusedString = null;
			bool _unusedBoolean = false;
			Type _unusedType = null;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockReflectionFascade = mockFactory.CreateInstance<IReflectionFascade>();

			activatorType = typeof(MockDependantObject);

			Expect.On(mockDependencyManager).One.Method(m => m.ResolveDependency(_unusedType, _unusedString, _unusedBoolean)).With(typeof(MockDependantObject), "named_dep_obj", true).Will(Return.Value(new MockDependantObject("left")));
			Expect.On(mockDependencyManager).One.Method(m => m.ResolveDependency(_unusedType, _unusedString, _unusedBoolean)).With(typeof(MockDependantObject), string.Empty, true).Will(Return.Value(new MockDependantObject("right")));

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution(mockReflectionFascade, activatorType);

			Assert.AreEqual(DependencyLifetime.Transient, transientActivatorAutoWiringDependencyResolution.DependencyLifetime);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, typeof(object), string.Empty);

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
		[ExpectedException(typeof(DependencyException))]
		public void ShouldFailOnMockAmbiguousConstructorMatchTest()
		{
			TransientActivatorAutoWiringDependencyResolution transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			IReflectionFascade mockReflectionFascade;
			Type activatorType;
			object result;
			MockFactory mockFactory;
			string _unusedString = null;
			bool _unusedBoolean = false;
			Type _unusedType = null;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockReflectionFascade = mockFactory.CreateInstance<IReflectionFascade>();

			activatorType = typeof(MockAmbiguousCtorMatchDependantObject);

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution(mockReflectionFascade, activatorType);

			Assert.AreEqual(DependencyLifetime.Transient, transientActivatorAutoWiringDependencyResolution.DependencyLifetime);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, typeof(object), "named_dep_obj");
		}

		[Test]
		[ExpectedException(typeof(DependencyException))]
		public void ShouldFailOnMockUnmarkedParametersMatchTest()
		{
			TransientActivatorAutoWiringDependencyResolution transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			IReflectionFascade mockReflectionFascade;
			Type activatorType;
			object result;
			MockFactory mockFactory;
			string _unusedString = null;
			bool _unusedBoolean = false;
			Type _unusedType = null;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockReflectionFascade = mockFactory.CreateInstance<IReflectionFascade>();

			activatorType = typeof(MockAmbiguousCtorMatchDependantObject);

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution(mockReflectionFascade, activatorType);

			Assert.AreEqual(DependencyLifetime.Transient, transientActivatorAutoWiringDependencyResolution.DependencyLifetime);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, typeof(object), "i_haz_no_marked_params");
		}

		[Test]
		[ExpectedException(typeof(DependencyException))]
		public void ShouldFailOnNoMatchingConstructorTest()
		{
			TransientActivatorAutoWiringDependencyResolution transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			IReflectionFascade mockReflectionFascade;
			Type activatorType;
			object result;
			MockFactory mockFactory;
			string _unusedString = null;
			bool _unusedBoolean = false;
			Type _unusedType = null;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockReflectionFascade = mockFactory.CreateInstance<IReflectionFascade>();

			activatorType = typeof(MockAmbiguousCtorMatchDependantObject);

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution(mockReflectionFascade, activatorType);

			Assert.AreEqual(DependencyLifetime.Transient, transientActivatorAutoWiringDependencyResolution.DependencyLifetime);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, typeof(object), "boaty_mcboatface_NO_MATCH_right?");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullActualTypeCreateTest()
		{
			TransientActivatorAutoWiringDependencyResolution transientActivatorAutoWiringDependencyResolution;
			Type activatorType;
			IReflectionFascade mockReflectionFascade;
			MockFactory mockFactory;

			mockFactory = new MockFactory();

			mockReflectionFascade = mockFactory.CreateInstance<IReflectionFascade>();
			activatorType = null;

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution(mockReflectionFascade, activatorType);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyManagerResolveTest()
		{
			TransientActivatorAutoWiringDependencyResolution transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			IReflectionFascade mockReflectionFascade;
			object result;
			MockFactory mockFactory;
			Type activatorType;

			mockFactory = new MockFactory();
			mockDependencyManager = null;
			mockReflectionFascade = mockFactory.CreateInstance<IReflectionFascade>();
			activatorType = typeof(int);

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution(mockReflectionFascade, activatorType);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, typeof(object), string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveTest()
		{
			TransientActivatorAutoWiringDependencyResolution transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			IReflectionFascade mockReflectionFascade;
			object result;
			MockFactory mockFactory;
			Type activatorType;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockReflectionFascade = mockFactory.CreateInstance<IReflectionFascade>();
			activatorType = typeof(int);

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution(mockReflectionFascade, activatorType);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, typeof(object), null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullReflectionFascadeCreateTest()
		{
			TransientActivatorAutoWiringDependencyResolution transientActivatorAutoWiringDependencyResolution;
			IReflectionFascade mockReflectionFascade;
			Type activatorType;
			MockFactory mockFactory;

			mockFactory = new MockFactory();
			mockReflectionFascade = null;
			activatorType = typeof(IReflectionFascade);

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution(mockReflectionFascade, activatorType);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeResolveTest()
		{
			TransientActivatorAutoWiringDependencyResolution transientActivatorAutoWiringDependencyResolution;
			IDependencyManager mockDependencyManager;
			IReflectionFascade mockReflectionFascade;
			object result;
			MockFactory mockFactory;
			Type activatorType;

			mockFactory = new MockFactory();
			mockDependencyManager = mockFactory.CreateInstance<IDependencyManager>();
			mockReflectionFascade = mockFactory.CreateInstance<IReflectionFascade>();
			activatorType = typeof(int);

			transientActivatorAutoWiringDependencyResolution = new TransientActivatorAutoWiringDependencyResolution(mockReflectionFascade, activatorType);

			result = transientActivatorAutoWiringDependencyResolution.Resolve(mockDependencyManager, null, string.Empty);
		}

		#endregion
	}
}
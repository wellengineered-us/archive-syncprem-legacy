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
	public class DependencyManagerTests
	{
		#region Constructors/Destructors

		public DependencyManagerTests()
		{
		}

		#endregion

		#region Fields/Constants

		private const string COMMON_SELECTOR_KEY = "x";
		private const string UNCOMMON_SELECTOR_KEY = "bob";

		#endregion

		#region Methods/Operators

		[DependencyMagicMethod]
		public static void IsValidDependencyMagicMethod(IDependencyManager dependencyManager)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;

			IDependencyManager _unusedDependencyManager = null;
			Type _unusedType = null;
			string _unusedString = null;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			targetType = typeof(IFormattable);
			selectorKey = UNCOMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			dependencyManager.AddResolution(targetType, selectorKey, includeAssignableTypes, mockDependencyResolution);

			Expect.On(mockDependencyResolution).Any.Method(m => m.Resolve(_unusedDependencyManager, _unusedType, _unusedString)).With(dependencyManager, targetType, selectorKey).WillReturn(1234.5678);

			Expect.On(mockDependencyResolution).One.Method(m => m.Dispose());
		}

		public static void NotMarkedAsDependencyMagicMethod(IDependencyManager dependencyManager)
		{
			throw new Exception();
		}

		[DependencyMagicMethod]
		private static void ShouldNotMatchPrivateDependencyMagicMethod(IDependencyManager dependencyManager)
		{
			throw new Exception();
		}

		[DependencyMagicMethod]
		public static void ShouldNotMatchSignatureDependencyMagicMethod(int unused)
		{
			throw new Exception();
		}

		[Test]
		public void ShouldAddResolution1Test()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution<object> mockDependencyResolution;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<object>>();

			dependencyManager = new DependencyManager();
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			dependencyManager.AddResolution<object>(selectorKey, includeAssignableTypes, mockDependencyResolution);

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldAddResolutionTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			targetType = typeof(object);
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			dependencyManager.AddResolution(targetType, selectorKey, includeAssignableTypes, mockDependencyResolution);

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldCheckIfHasResolution1Test()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution<IDisposable> mockDependencyResolution;
			string selectorKey;
			bool includeAssignableTypes;
			bool result;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<IDisposable>>();

			Expect.On(mockDependencyResolution).One.Method(m => m.Dispose());

			dependencyManager = new DependencyManager();

			selectorKey = null;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution<IDisposable>(selectorKey, includeAssignableTypes);
			Assert.IsFalse(result);

			selectorKey = string.Empty;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution<IDisposable>(selectorKey, includeAssignableTypes);
			Assert.IsFalse(result);

			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution<IDisposable>(selectorKey, includeAssignableTypes);
			Assert.IsFalse(result);

			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			dependencyManager.AddResolution<IDisposable>(selectorKey, includeAssignableTypes, mockDependencyResolution);

			selectorKey = null;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution<IDisposable>(selectorKey, includeAssignableTypes);
			Assert.IsTrue(result);

			selectorKey = string.Empty;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution<IDisposable>(selectorKey, includeAssignableTypes);
			Assert.IsFalse(result);

			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution<IDisposable>(selectorKey, includeAssignableTypes);
			Assert.IsTrue(result);

			dependencyManager.ClearAllResolutions();

			selectorKey = null;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution<IDisposable>(selectorKey, includeAssignableTypes);
			Assert.IsFalse(result);

			selectorKey = string.Empty;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution<IDisposable>(selectorKey, includeAssignableTypes);
			Assert.IsFalse(result);

			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution<IDisposable>(selectorKey, includeAssignableTypes);
			Assert.IsFalse(result);

			selectorKey = string.Empty;
			includeAssignableTypes = false;

			dependencyManager.AddResolution<IDisposable>(selectorKey, includeAssignableTypes, mockDependencyResolution);

			result = dependencyManager.HasTypeResolution<IDisposable>(selectorKey, includeAssignableTypes);
			Assert.IsTrue(result);

			selectorKey = string.Empty;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution<IDisposable>(selectorKey, includeAssignableTypes);
			Assert.IsTrue(result);

			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution<IDisposable>(selectorKey, includeAssignableTypes);
			Assert.IsFalse(result);

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldCheckIfHasResolutionTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;
			bool result;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			Expect.On(mockDependencyResolution).One.Method(m => m.Dispose());

			dependencyManager = new DependencyManager();

			targetType = typeof(IDisposable);
			selectorKey = null;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution(targetType, selectorKey, includeAssignableTypes);
			Assert.IsFalse(result);

			targetType = typeof(IDisposable);
			selectorKey = string.Empty;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution(targetType, selectorKey, includeAssignableTypes);
			Assert.IsFalse(result);

			targetType = typeof(IDisposable);
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution(targetType, selectorKey, includeAssignableTypes);
			Assert.IsFalse(result);

			targetType = typeof(IDisposable);
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			dependencyManager.AddResolution(targetType, selectorKey, includeAssignableTypes, mockDependencyResolution);

			targetType = typeof(IDisposable);
			selectorKey = null;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution(targetType, selectorKey, includeAssignableTypes);
			Assert.IsTrue(result);

			targetType = typeof(IDisposable);
			selectorKey = string.Empty;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution(targetType, selectorKey, includeAssignableTypes);
			Assert.IsFalse(result);

			targetType = typeof(IDisposable);
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution(targetType, selectorKey, includeAssignableTypes);
			Assert.IsTrue(result);

			dependencyManager.ClearAllResolutions();

			targetType = typeof(IDisposable);
			selectorKey = null;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution(targetType, selectorKey, includeAssignableTypes);
			Assert.IsFalse(result);

			targetType = typeof(IDisposable);
			selectorKey = string.Empty;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution(targetType, selectorKey, includeAssignableTypes);
			Assert.IsFalse(result);

			targetType = typeof(IDisposable);
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution(targetType, selectorKey, includeAssignableTypes);
			Assert.IsFalse(result);

			targetType = typeof(IDisposable);
			selectorKey = string.Empty;
			includeAssignableTypes = false;

			dependencyManager.AddResolution(targetType, selectorKey, includeAssignableTypes, mockDependencyResolution);

			targetType = typeof(IDisposable);
			selectorKey = null;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution(targetType, selectorKey, includeAssignableTypes);
			Assert.IsTrue(result);

			targetType = typeof(IDisposable);
			selectorKey = string.Empty;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution(targetType, selectorKey, includeAssignableTypes);
			Assert.IsTrue(result);

			targetType = typeof(IDisposable);
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			result = dependencyManager.HasTypeResolution(targetType, selectorKey, includeAssignableTypes);
			Assert.IsFalse(result);

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldClearAllResolutionsTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;
			bool result;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			targetType = typeof(object);
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			Expect.On(mockDependencyResolution).One.Method(m => m.Dispose());

			dependencyManager = new DependencyManager();

			result = dependencyManager.ClearAllResolutions();

			Assert.IsFalse(result);

			dependencyManager.AddResolution(targetType, selectorKey, includeAssignableTypes, mockDependencyResolution);

			result = dependencyManager.ClearAllResolutions();

			Assert.IsTrue(result);

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldClearTypeResolutions1Test()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution<object> mockDependencyResolution;
			string selectorKey;
			bool includeAssignableTypes;
			bool result;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<object>>();

			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			Expect.On(mockDependencyResolution).One.Method(m => m.Dispose());

			dependencyManager = new DependencyManager();

			result = dependencyManager.ClearTypeResolutions<object>(includeAssignableTypes);

			Assert.IsFalse(result);

			dependencyManager.AddResolution<object>(selectorKey, includeAssignableTypes, mockDependencyResolution);

			result = dependencyManager.ClearTypeResolutions<object>(includeAssignableTypes);

			Assert.IsTrue(result);

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldClearTypeResolutionsTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;
			bool result;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			Expect.On(mockDependencyResolution).One.Method(m => m.Dispose());

			dependencyManager = new DependencyManager();
			targetType = typeof(object);
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			result = dependencyManager.ClearTypeResolutions(targetType, includeAssignableTypes);

			Assert.IsFalse(result);

			dependencyManager.AddResolution(targetType, selectorKey, includeAssignableTypes, mockDependencyResolution);

			result = dependencyManager.ClearTypeResolutions(targetType, includeAssignableTypes);

			Assert.IsTrue(result);

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldCreateTest()
		{
			DependencyManager dependencyManager;

			dependencyManager = new DependencyManager();
		}

		[Test]
		[ExpectedException(typeof(ObjectDisposedException))]
		public void ShouldFailOnDisposedAddResolution1Test()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution<object> mockDependencyResolution;
			IDependencyManager _unusedDependencyManager = null;
			Type _unusedType = null;
			string _unusedString = null;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<object>>();

			dependencyManager = new DependencyManager();
			targetType = typeof(object);
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			Expect.On(mockDependencyResolution).One.Method(x => x.Resolve(_unusedDependencyManager, _unusedType, _unusedString)).With(dependencyManager, targetType, selectorKey).Will(Return.Value(null));

			dependencyManager.Dispose();
			dependencyManager.AddResolution<object>(selectorKey, includeAssignableTypes, mockDependencyResolution);
		}

		[Test]
		[ExpectedException(typeof(ObjectDisposedException))]
		public void ShouldFailOnDisposedAddResolutionTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			IDependencyManager _unusedDependencyManager = null;
			Type _unusedType = null;
			string _unusedString = null;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			targetType = typeof(object);
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			Expect.On(mockDependencyResolution).One.Method(x => x.Resolve(_unusedDependencyManager, _unusedType, _unusedString)).With(dependencyManager, targetType, selectorKey).Will(Return.Value(null));

			dependencyManager.Dispose();
			dependencyManager.AddResolution(targetType, selectorKey, includeAssignableTypes, mockDependencyResolution);
		}

		[Test]
		[ExpectedException(typeof(ObjectDisposedException))]
		public void ShouldFailOnDisposedCheckIfHasResolution1Test()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			selectorKey = null;
			includeAssignableTypes = false;

			dependencyManager = new DependencyManager();

			dependencyManager.Dispose();
			dependencyManager.HasTypeResolution<object>(selectorKey, includeAssignableTypes);
		}

		[Test]
		[ExpectedException(typeof(ObjectDisposedException))]
		public void ShouldFailOnDisposedCheckIfHasResolutionTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			targetType = typeof(object);
			selectorKey = null;
			includeAssignableTypes = false;

			dependencyManager.Dispose();
			dependencyManager.HasTypeResolution(targetType, selectorKey, includeAssignableTypes);
		}

		[Test]
		[ExpectedException(typeof(ObjectDisposedException))]
		public void ShouldFailOnDisposedClearAllResolutionsTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			bool result;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();

			dependencyManager.Dispose();
			result = dependencyManager.ClearAllResolutions();
		}

		[Test]
		[ExpectedException(typeof(ObjectDisposedException))]
		public void ShouldFailOnDisposedClearTypeResolutions1Test()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			bool result;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();

			includeAssignableTypes = false;

			dependencyManager.Dispose();
			result = dependencyManager.ClearTypeResolutions<object>(includeAssignableTypes);
		}

		[Test]
		[ExpectedException(typeof(ObjectDisposedException))]
		public void ShouldFailOnDisposedClearTypeResolutionsTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			Type targetType;
			bool result;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			targetType = typeof(object);
			includeAssignableTypes = false;

			dependencyManager.Dispose();
			result = dependencyManager.ClearTypeResolutions(targetType, includeAssignableTypes);
		}

		[Test]
		[ExpectedException(typeof(ObjectDisposedException))]
		public void ShouldFailOnDisposedRemoveResolution1Test()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			dependencyManager.Dispose();
			dependencyManager.RemoveResolution<object>(selectorKey, includeAssignableTypes);
		}

		[Test]
		[ExpectedException(typeof(ObjectDisposedException))]
		public void ShouldFailOnDisposedRemoveResolutionTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			targetType = typeof(object);
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			dependencyManager.Dispose();
			dependencyManager.RemoveResolution(targetType, selectorKey, includeAssignableTypes);
		}

		[Test]
		[ExpectedException(typeof(ObjectDisposedException))]
		public void ShouldFailOnDisposedResolveDependency1Test()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			string selectorKey;
			object value;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			dependencyManager.Dispose();
			value = dependencyManager.ResolveDependency<object>(selectorKey, includeAssignableTypes);
		}

		[Test]
		[ExpectedException(typeof(ObjectDisposedException))]
		public void ShouldFailOnDisposedResolveDependencyTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			Type targetType;
			string selectorKey;
			object value;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			targetType = typeof(object);
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			dependencyManager.Dispose();
			value = dependencyManager.ResolveDependency(targetType, selectorKey, includeAssignableTypes);
		}

		[Test]
		[ExpectedException(typeof(DependencyException))]
		public void ShouldFailOnKeyExistsAddResolutionTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			IDependencyManager _unusedDependencyManager = null;
			Type _unusedType = null;
			string _unusedString = null;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			targetType = typeof(object);
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			Expect.On(mockDependencyResolution).One.Method(x => x.Resolve(_unusedDependencyManager, _unusedType, _unusedString)).With(dependencyManager, targetType, selectorKey).Will(Return.Value(null));

			dependencyManager.AddResolution(targetType, selectorKey, includeAssignableTypes, mockDependencyResolution);
			dependencyManager.AddResolution(targetType, selectorKey, includeAssignableTypes, mockDependencyResolution);
		}

		[Test]
		[ExpectedException(typeof(DependencyException))]
		public void ShouldFailOnKeyNotExistsRemoveResolutionTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			targetType = typeof(object);
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			dependencyManager.RemoveResolution(targetType, selectorKey, includeAssignableTypes);
		}

		[Test]
		[ExpectedException(typeof(DependencyException))]
		public void ShouldFailOnKeyNotExistsResolveDependencyTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			Type targetType;
			string selectorKey;
			object value;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			targetType = typeof(object);
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			value = dependencyManager.ResolveDependency(targetType, selectorKey, includeAssignableTypes);
		}

		[Test]
		[ExpectedException(typeof(DependencyException))]
		public void ShouldFailOnNotAssignableResolveDependencyTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			IDependencyManager _unusedDependencyManager = null;
			Type _unusedType = null;
			string _unusedString = null;
			Type targetType;
			string selectorKey;
			object value;
			bool includeAssignableTypes;

			dependencyManager = new DependencyManager();

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			targetType = typeof(IDisposable);
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			Expect.On(mockDependencyResolution).One.Method(x => x.Resolve(_unusedDependencyManager, _unusedType, _unusedString)).With(dependencyManager, targetType, selectorKey).Will(Return.Value(1));

			dependencyManager.AddResolution(targetType, selectorKey, includeAssignableTypes, mockDependencyResolution);
			value = dependencyManager.ResolveDependency<IDisposable>(selectorKey, includeAssignableTypes);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyResolutionAddResolution1Test()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution<object> mockDependencyResolution;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = null;

			dependencyManager = new DependencyManager();
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			dependencyManager.AddResolution<object>(selectorKey, includeAssignableTypes, mockDependencyResolution);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullDependencyResolutionAddResolutionTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = null;

			dependencyManager = new DependencyManager();
			targetType = typeof(object);
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			dependencyManager.AddResolution(targetType, selectorKey, includeAssignableTypes, mockDependencyResolution);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyAddResolution1Test()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution<object> mockDependencyResolution;
			IDependencyManager _unusedDependencyManager = null;
			Type _unusedType = null;
			string _unusedString = null;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<object>>();

			dependencyManager = new DependencyManager();
			targetType = typeof(object);
			selectorKey = null;
			includeAssignableTypes = false;

			Expect.On(mockDependencyResolution).One.Method(x => x.Resolve(_unusedDependencyManager, _unusedType, _unusedString)).With(dependencyManager, targetType, selectorKey).Will(Return.Value(null));

			dependencyManager.AddResolution<object>(selectorKey, includeAssignableTypes, mockDependencyResolution);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyAddResolutionTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			IDependencyManager _unusedDependencyManager = null;
			Type _unusedType = null;
			string _unusedString = null;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			targetType = typeof(object);
			selectorKey = null;
			includeAssignableTypes = false;

			Expect.On(mockDependencyResolution).One.Method(x => x.Resolve(_unusedDependencyManager, _unusedType, _unusedString)).With(dependencyManager, targetType, selectorKey).Will(Return.Value(null));

			dependencyManager.AddResolution(targetType, selectorKey, includeAssignableTypes, mockDependencyResolution);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyRemoveResolution1Test()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			selectorKey = null;
			includeAssignableTypes = false;

			dependencyManager.RemoveResolution<object>(selectorKey, includeAssignableTypes);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyRemoveResolutionTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			targetType = typeof(object);
			selectorKey = null;
			includeAssignableTypes = false;

			dependencyManager.RemoveResolution(targetType, selectorKey, includeAssignableTypes);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveDependency1Test()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			string selectorKey;
			object value;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			selectorKey = null;
			includeAssignableTypes = false;

			value = dependencyManager.ResolveDependency<object>(selectorKey, includeAssignableTypes);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullKeyResolveDependencyTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			Type targetType;
			string selectorKey;
			object value;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			targetType = typeof(object);
			selectorKey = null;
			includeAssignableTypes = false;

			value = dependencyManager.ResolveDependency(targetType, selectorKey, includeAssignableTypes);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeAddResolutionTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			IDependencyManager _unusedDependencyManager = null;
			Type _unusedType = null;
			string _unusedString = null;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			targetType = null;
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			Expect.On(mockDependencyResolution).One.Method(x => x.Resolve(_unusedDependencyManager, _unusedType, _unusedString)).With(dependencyManager, targetType, selectorKey).Will(Return.Value(null));

			dependencyManager.AddResolution(targetType, selectorKey, includeAssignableTypes, mockDependencyResolution);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeCheckIfHasResolutionTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			targetType = null;
			selectorKey = string.Empty;
			includeAssignableTypes = false;

			dependencyManager.HasTypeResolution(targetType, selectorKey, includeAssignableTypes);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeClearTypeResolutionsTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			Type targetType;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			targetType = null;
			includeAssignableTypes = false;

			dependencyManager.ClearTypeResolutions(targetType, includeAssignableTypes);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeRemoveResolutionTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			targetType = null;
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			dependencyManager.RemoveResolution(targetType, selectorKey, includeAssignableTypes);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTypeResolveDependencyTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;
			object value;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();
			targetType = null;
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			value = dependencyManager.ResolveDependency(targetType, selectorKey, includeAssignableTypes);
		}

		[Test]
		public void ShouldNotFailOnDoubleDisposedAddResolution1Test()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			dependencyManager = new DependencyManager();

			Assert.IsFalse(dependencyManager.Disposed);

			dependencyManager.Dispose();

			Assert.IsTrue(dependencyManager.Disposed);

			dependencyManager.Dispose();

			Assert.IsTrue(dependencyManager.Disposed);

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldRemoveResolution1Test()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution<object> mockDependencyResolution;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<object>>();

			Expect.On(mockDependencyResolution).One.Method(m => m.Dispose());

			dependencyManager = new DependencyManager();
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			dependencyManager.AddResolution<object>(selectorKey, includeAssignableTypes, mockDependencyResolution);
			dependencyManager.RemoveResolution<object>(selectorKey, includeAssignableTypes);

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldRemoveResolutionTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			Expect.On(mockDependencyResolution).One.Method(m => m.Dispose());

			dependencyManager = new DependencyManager();
			targetType = typeof(object);
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			dependencyManager.AddResolution(targetType, selectorKey, includeAssignableTypes, mockDependencyResolution);
			dependencyManager.RemoveResolution(targetType, selectorKey, includeAssignableTypes);

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldResolveDependency1Test()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution<object> mockDependencyResolution;
			IDependencyManager _unusedDependencyManager = null;
			Type _unusedType = null;
			string _unusedString = null;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;
			object value;

			dependencyManager = new DependencyManager();

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution<object>>();

			targetType = typeof(object);
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			Expect.On(mockDependencyResolution).One.Method(x => x.Resolve(_unusedDependencyManager, _unusedType, _unusedString)).With(dependencyManager, targetType, selectorKey).Will(Return.Value(1));

			dependencyManager.AddResolution<object>(selectorKey, includeAssignableTypes, mockDependencyResolution);
			value = dependencyManager.ResolveDependency<object>(selectorKey, includeAssignableTypes);

			Assert.IsNotNull(value);
			Assert.AreEqual(1, value);

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldResolveDependencyTest()
		{
			DependencyManager dependencyManager;
			MockFactory mockFactory;
			IDependencyResolution mockDependencyResolution;
			IDependencyManager _unusedDependencyManager = null;
			Type _unusedType = null;
			string _unusedString = null;
			Type targetType;
			string selectorKey;
			bool includeAssignableTypes;
			object value;

			dependencyManager = new DependencyManager();

			mockFactory = new MockFactory();
			mockDependencyResolution = mockFactory.CreateInstance<IDependencyResolution>();

			targetType = typeof(object);
			selectorKey = COMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			Expect.On(mockDependencyResolution).One.Method(x => x.Resolve(_unusedDependencyManager, _unusedType, _unusedString)).With(dependencyManager, targetType, selectorKey).Will(Return.Value(1));

			dependencyManager.AddResolution(targetType, selectorKey, includeAssignableTypes, mockDependencyResolution);
			value = dependencyManager.ResolveDependency(targetType, selectorKey, includeAssignableTypes);

			Assert.IsNotNull(value);
			Assert.AreEqual(1, value);

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		public void ShouldVerifyAutoWiringTest()
		{
			IFormattable formattable;
			string selectorKey;
			bool includeAssignableTypes;

			selectorKey = UNCOMMON_SELECTOR_KEY;
			includeAssignableTypes = false;

			formattable = AssemblyDomain.Default.DependencyManager.ResolveDependency<IFormattable>(selectorKey, includeAssignableTypes);

			Assert.IsNotNull(formattable);
			Assert.IsInstanceOf<double>(formattable);
			Assert.AreEqual(1234.5678, (double)formattable);
		}

		#endregion

		#region Classes/Structs/Interfaces/Enums/Delegates

		private static class Inner
		{
			#region Methods/Operators

			[DependencyMagicMethod]
			public static void ShouldNotMatchPrivateType()
			{
				throw new Exception();
			}

			#endregion
		}

		#endregion
	}
}
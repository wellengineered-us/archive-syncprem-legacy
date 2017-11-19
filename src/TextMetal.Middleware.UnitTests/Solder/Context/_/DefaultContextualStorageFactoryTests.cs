/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using NUnit.Framework;

using TextMetal.Middleware.Solder.Context;
using TextMetal.Middleware.UnitTests.TestingInfrastructure;

namespace TextMetal.Middleware.UnitTests.Solder.Context._
{
	[TestFixture]
	public class DefaultContextualStorageFactoryTests
	{
		#region Constructors/Destructors

		public DefaultContextualStorageFactoryTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateTest()
		{
			DefaultContextualStorageFactory defaultContextualStorageFactory;
			IContextualStorageStrategy contextualStorageStrategy;

			defaultContextualStorageFactory = new DefaultContextualStorageFactory(ContextScope.GlobalStaticUnsafe);
			contextualStorageStrategy = defaultContextualStorageFactory.GetContextualStorage();

			Assert.IsNotNull(contextualStorageStrategy);

			defaultContextualStorageFactory = new DefaultContextualStorageFactory(ContextScope.LocalThreadSafe);
			contextualStorageStrategy = defaultContextualStorageFactory.GetContextualStorage();

			Assert.IsNotNull(contextualStorageStrategy);

			defaultContextualStorageFactory = new DefaultContextualStorageFactory(ContextScope.LocalAsyncSafe);
			contextualStorageStrategy = defaultContextualStorageFactory.GetContextualStorage();

			Assert.IsNotNull(contextualStorageStrategy);

			defaultContextualStorageFactory = new DefaultContextualStorageFactory(ContextScope.LocalRequestSafe);
			contextualStorageStrategy = defaultContextualStorageFactory.GetContextualStorage();

			Assert.IsNotNull(contextualStorageStrategy);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ShouldFailOnInvalidContextScopeCreateTest()
		{
			DefaultContextualStorageFactory defaultContextualStorageFactory;

			defaultContextualStorageFactory = new DefaultContextualStorageFactory(ContextScope.Unknown);

			defaultContextualStorageFactory.GetContextualStorage();
		}

		#endregion
	}
}
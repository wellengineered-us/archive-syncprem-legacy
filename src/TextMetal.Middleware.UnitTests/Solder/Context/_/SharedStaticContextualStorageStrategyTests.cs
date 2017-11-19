/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading;

using NUnit.Framework;

using TextMetal.Middleware.Solder.Context;
using TextMetal.Middleware.UnitTests.TestingInfrastructure;

namespace TextMetal.Middleware.UnitTests.Solder.Context._
{
	[TestFixture]
	public class SharedStaticContextualStorageStrategyTests
	{
		#region Constructors/Destructors

		public SharedStaticContextualStorageStrategyTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateCrossThreadTest()
		{
			GlobalStaticContextualStorageStrategy globalStaticContextualStorageStrategy;
			const string KEY = "somekey";
			bool result;
			string value;
			string expected;

			Thread thread;
			int managedThreadId;

			managedThreadId = Thread.CurrentThread.ManagedThreadId;
			globalStaticContextualStorageStrategy = new GlobalStaticContextualStorageStrategy();
			globalStaticContextualStorageStrategy.ResetValues();

			// set unset
			expected = Guid.NewGuid().ToString("N");
			globalStaticContextualStorageStrategy.SetValue(KEY, expected);

			// has isset
			result = globalStaticContextualStorageStrategy.HasValue(KEY);
			Assert.IsTrue(result);

			// get isset
			value = globalStaticContextualStorageStrategy.GetValue<string>(KEY);
			Assert.IsNotNull(value);
			Assert.AreEqual(expected, value);

			thread = new Thread(() =>
								{
									int _managedThreadId = Thread.CurrentThread.ManagedThreadId;

									Assert.AreNotEqual(_managedThreadId, managedThreadId);

									// has isset(other thread)
									result = globalStaticContextualStorageStrategy.HasValue(KEY);
									Assert.IsTrue(result);
								});

			thread.Start();
			thread.Join();
		}

		[Test]
		public void ShouldCreateTest()
		{
			GlobalStaticContextualStorageStrategy globalStaticContextualStorageStrategy;
			const string KEY = "somekey";
			bool result;
			string value;
			string expected;

			globalStaticContextualStorageStrategy = new GlobalStaticContextualStorageStrategy();
			globalStaticContextualStorageStrategy.ResetValues();

			// has unset
			result = globalStaticContextualStorageStrategy.HasValue(KEY);
			Assert.IsFalse(result);

			// get unset
			value = globalStaticContextualStorageStrategy.GetValue<string>(KEY);
			Assert.IsNull(value);

			// remove unset
			globalStaticContextualStorageStrategy.RemoveValue(KEY);

			// set unset
			expected = Guid.NewGuid().ToString("N");
			globalStaticContextualStorageStrategy.SetValue(KEY, expected);

			// has isset
			result = globalStaticContextualStorageStrategy.HasValue(KEY);
			Assert.IsTrue(result);

			// get isset
			value = globalStaticContextualStorageStrategy.GetValue<string>(KEY);
			Assert.IsNotNull(value);
			Assert.AreEqual(expected, value);

			// set isset
			expected = Guid.NewGuid().ToString("N");
			globalStaticContextualStorageStrategy.SetValue(KEY, expected);

			result = globalStaticContextualStorageStrategy.HasValue(KEY);
			Assert.IsTrue(result);

			value = globalStaticContextualStorageStrategy.GetValue<string>(KEY);
			Assert.IsNotNull(value);
			Assert.AreEqual(expected, value);

			// remove isset
			globalStaticContextualStorageStrategy.RemoveValue(KEY);

			// verify remove
			result = globalStaticContextualStorageStrategy.HasValue(KEY);
			Assert.IsFalse(result);

			value = globalStaticContextualStorageStrategy.GetValue<string>(KEY);
			Assert.IsNull(value);
		}

		[Test]
		public void ShouldCreateTest__todo_mock_shared_static()
		{
			Assert.Ignore("TODO: This test case has not been implemented yet.");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullSharedStaticCreateTest()
		{
			IDictionary<string, object> mockSharedStatic;
			GlobalStaticContextualStorageStrategy globalStaticContextualStorageStrategy;

			mockSharedStatic = null;

			globalStaticContextualStorageStrategy = new GlobalStaticContextualStorageStrategy(mockSharedStatic);
		}

		#endregion
	}
}
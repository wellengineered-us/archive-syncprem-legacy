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
	public class ThreadLocalContextualStorageStrategyTests
	{
		#region Constructors/Destructors

		public ThreadLocalContextualStorageStrategyTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateCrossThreadTest()
		{
			ThreadLocalContextualStorageStrategy threadLocalContextualStorageStrategy;
			const string KEY = "somekey";
			bool result;
			string value;
			string expected;

			Thread thread;
			int managedThreadId;

			managedThreadId = Thread.CurrentThread.ManagedThreadId;
			threadLocalContextualStorageStrategy = new ThreadLocalContextualStorageStrategy();

			// set unset
			expected = Guid.NewGuid().ToString("N");
			threadLocalContextualStorageStrategy.SetValue(KEY, expected);

			// has isset
			result = threadLocalContextualStorageStrategy.HasValue(KEY);
			Assert.IsTrue(result);

			// get isset
			value = threadLocalContextualStorageStrategy.GetValue<string>(KEY);
			Assert.IsNotNull(value);
			Assert.AreEqual(expected, value);

			thread = new Thread(() =>
								{
									int _managedThreadId = Thread.CurrentThread.ManagedThreadId;

									Assert.AreNotEqual(_managedThreadId, managedThreadId);

									// has isset(other thread)
									result = threadLocalContextualStorageStrategy.HasValue(KEY);
									Assert.IsFalse(result);
								});

			thread.Start();
			thread.Join();
		}

		[Test]
		public void ShouldCreateTest()
		{
			ThreadLocalContextualStorageStrategy threadLocalContextualStorageStrategy;
			const string KEY = "somekey";
			bool result;
			string value;
			string expected;

			threadLocalContextualStorageStrategy = new ThreadLocalContextualStorageStrategy();

			// has unset
			result = threadLocalContextualStorageStrategy.HasValue(KEY);
			Assert.IsFalse(result);

			// get unset
			value = threadLocalContextualStorageStrategy.GetValue<string>(KEY);
			Assert.IsNull(value);

			// remove unset
			threadLocalContextualStorageStrategy.RemoveValue(KEY);

			// set unset
			expected = Guid.NewGuid().ToString("N");
			threadLocalContextualStorageStrategy.SetValue(KEY, expected);

			// has isset
			result = threadLocalContextualStorageStrategy.HasValue(KEY);
			Assert.IsTrue(result);

			// get isset
			value = threadLocalContextualStorageStrategy.GetValue<string>(KEY);
			Assert.IsNotNull(value);
			Assert.AreEqual(expected, value);

			// set isset
			expected = Guid.NewGuid().ToString("N");
			threadLocalContextualStorageStrategy.SetValue(KEY, expected);

			result = threadLocalContextualStorageStrategy.HasValue(KEY);
			Assert.IsTrue(result);

			value = threadLocalContextualStorageStrategy.GetValue<string>(KEY);
			Assert.IsNotNull(value);
			Assert.AreEqual(expected, value);

			// remove isset
			threadLocalContextualStorageStrategy.RemoveValue(KEY);

			// verify remove
			result = threadLocalContextualStorageStrategy.HasValue(KEY);
			Assert.IsFalse(result);

			value = threadLocalContextualStorageStrategy.GetValue<string>(KEY);
			Assert.IsNull(value);
		}

		[Test]
		public void ShouldCreateTest__todo_mock_tls_context_factory_method()
		{
			Assert.Ignore("TODO: This test case has not been implemented yet.");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldFailOnNullTlsContextFactoryMethodCreateTest()
		{
			Func<IDictionary<string, object>> mockTlsContextFactoryMethod;
			ThreadLocalContextualStorageStrategy threadLocalContextualStorageStrategy;

			mockTlsContextFactoryMethod = null;

			threadLocalContextualStorageStrategy = new ThreadLocalContextualStorageStrategy(mockTlsContextFactoryMethod);
		}

		#endregion
	}
}
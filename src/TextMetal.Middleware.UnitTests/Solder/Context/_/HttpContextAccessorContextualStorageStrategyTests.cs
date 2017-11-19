/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Http;

using NMock;

using NUnit.Framework;

using TextMetal.Middleware.Solder.Context;
using TextMetal.Middleware.UnitTests.TestingInfrastructure;

namespace TextMetal.Middleware.UnitTests.Solder.Context._
{
	[TestFixture]
	public class HttpContextAccessorContextualStorageStrategyTests
	{
		#region Constructors/Destructors

		public HttpContextAccessorContextualStorageStrategyTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateTest()
		{
			HttpContextAccessorContextualStorageStrategy httpContextAccessorContextualStorageStrategy;
			const string KEY = "somekey";
			bool result;
			string value;
			string expected, __expected;
			IHttpContextAccessor mockHttpContextAccessor;
			HttpContext mockHttpContext;
			IDictionary<object, object> mockDictionary;

			MockFactory mockFactory;

			const string _unsusedString = null;

			mockFactory = new MockFactory();
			mockHttpContextAccessor = mockFactory.CreateInstance<IHttpContextAccessor>();
			mockHttpContext = mockFactory.CreateInstance<HttpContext>();
			mockDictionary = mockFactory.CreateInstance<IDictionary<object, object>>();

			expected = Guid.NewGuid().ToString("N");
			__expected = new string(expected.ToCharArray().Reverse().ToArray());

			Expect.On(mockHttpContextAccessor).Any.GetProperty(p => p.HttpContext).WillReturn(mockHttpContext);
			Expect.On(mockHttpContext).Any.GetProperty(p => p.Items).WillReturn(mockDictionary);

			// has unset
			Expect.On(mockDictionary).One.Method(m => m.ContainsKey(_unsusedString)).With(KEY).WillReturn(false);
			// get unset
			Expect.On(mockDictionary).One.GetProperty(m => m[KEY]).WillReturn(null);
			// remove unset
			Expect.On(mockDictionary).One.Method(m => m.Remove(_unsusedString)).With(KEY).WillReturn(true);
			// set unset
			Expect.On(mockDictionary).One.SetProperty(m => m[KEY] = _unsusedString).To(expected);
			// has isset
			Expect.On(mockDictionary).One.Method(m => m.ContainsKey(_unsusedString)).With(KEY).WillReturn(true);
			// get isset
			Expect.On(mockDictionary).One.GetProperty(m => m[KEY]).WillReturn(expected);
			// set isset
			Expect.On(mockDictionary).One.SetProperty(m => m[KEY] = _unsusedString).To(__expected);
			Expect.On(mockDictionary).One.Method(m => m.ContainsKey(_unsusedString)).With(KEY).WillReturn(true);
			Expect.On(mockDictionary).One.GetProperty(m => m[KEY]).WillReturn(__expected);
			// remove isset
			Expect.On(mockDictionary).One.Method(m => m.Remove(_unsusedString)).With(KEY).WillReturn(true);
			// verify remove
			Expect.On(mockDictionary).One.Method(m => m.ContainsKey(_unsusedString)).With(KEY).WillReturn(false);
			Expect.On(mockDictionary).One.GetProperty(m => m[KEY]).WillReturn(null);

			httpContextAccessorContextualStorageStrategy = new HttpContextAccessorContextualStorageStrategy(mockHttpContextAccessor);

			// has unset
			result = httpContextAccessorContextualStorageStrategy.HasValue(KEY);
			Assert.IsFalse(result);

			// get unset
			value = httpContextAccessorContextualStorageStrategy.GetValue<string>(KEY);
			Assert.IsNull(value);

			// remove unset
			httpContextAccessorContextualStorageStrategy.RemoveValue(KEY);

			// set unset
			httpContextAccessorContextualStorageStrategy.SetValue(KEY, expected);

			// has isset
			result = httpContextAccessorContextualStorageStrategy.HasValue(KEY);
			Assert.IsTrue(result);

			// get isset
			value = httpContextAccessorContextualStorageStrategy.GetValue<string>(KEY);
			Assert.IsNotNull(value);
			Assert.AreEqual(expected, value);

			// set isset
			httpContextAccessorContextualStorageStrategy.SetValue(KEY, __expected);

			result = httpContextAccessorContextualStorageStrategy.HasValue(KEY);
			Assert.IsTrue(result);

			value = httpContextAccessorContextualStorageStrategy.GetValue<string>(KEY);
			Assert.IsNotNull(value);
			Assert.AreEqual(__expected, value);

			// remove isset
			httpContextAccessorContextualStorageStrategy.RemoveValue(KEY);

			// verify remove
			result = httpContextAccessorContextualStorageStrategy.HasValue(KEY);
			Assert.IsFalse(result);

			value = httpContextAccessorContextualStorageStrategy.GetValue<string>(KEY);
			Assert.IsNull(value);

			mockFactory.VerifyAllExpectationsHaveBeenMet();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ShouldFailOnNullSharedStaticCreateTest()
		{
			HttpContextAccessorContextualStorageStrategy httpContextAccessorContextualStorageStrategy;
			IHttpContextAccessor mockHttpContextAccessor;

			mockHttpContextAccessor = null;

			httpContextAccessorContextualStorageStrategy = new HttpContextAccessorContextualStorageStrategy(mockHttpContextAccessor);

			httpContextAccessorContextualStorageStrategy.HasValue("test");
		}

		[Test]
		public void ShouldNotFailOnNullSharedStaticIsNotValidHttpContextCreateTest()
		{
			HttpContextAccessorContextualStorageStrategy httpContextAccessorContextualStorageStrategy;
			IHttpContextAccessor mockHttpContextAccessor;

			mockHttpContextAccessor = null;

			httpContextAccessorContextualStorageStrategy = new HttpContextAccessorContextualStorageStrategy(mockHttpContextAccessor);

			Assert.IsFalse(httpContextAccessorContextualStorageStrategy.IsValidHttpContext);
		}

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using NUnit.Framework;

using TextMetal.Middleware.Solder.Injection;

namespace TextMetal.Middleware.UnitTests.Solder.Injection._
{
	[TestFixture]
	public class DependencyInjectionAttributeTests
	{
		#region Constructors/Destructors

		public DependencyInjectionAttributeTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateTest()
		{
			DependencyInjectionAttribute attribute;
			string expected;

			expected = "selid";
			attribute = new DependencyInjectionAttribute();
			Assert.IsNotNull(attribute);

			attribute.SelectorKey = expected;
			Assert.AreEqual(expected, attribute.SelectorKey);
		}

		#endregion
	}
}
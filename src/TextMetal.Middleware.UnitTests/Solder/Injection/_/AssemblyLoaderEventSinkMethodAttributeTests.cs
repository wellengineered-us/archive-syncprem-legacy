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
	public class AssemblyLoaderEventSinkMethodAttributeTests
	{
		#region Constructors/Destructors

		public AssemblyLoaderEventSinkMethodAttributeTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateTest()
		{
			DependencyMagicMethodAttribute attribute;

			attribute = new DependencyMagicMethodAttribute();
			Assert.IsNotNull(attribute);
		}

		#endregion
	}
}
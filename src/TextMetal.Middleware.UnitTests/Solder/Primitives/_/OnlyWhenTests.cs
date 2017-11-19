/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using NUnit.Framework;

using TextMetal.Middleware.Solder.Primitives;

namespace TextMetal.Middleware.UnitTests.Solder.Primitives._
{
	[TestFixture]
	public class OnlyWhenTests
	{
		#region Constructors/Destructors

		public OnlyWhenTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldOnlyWhenTest()
		{
			OnlyWhen._DEBUG_ThenPrint(string.Empty);
		}

		#endregion
	}
}
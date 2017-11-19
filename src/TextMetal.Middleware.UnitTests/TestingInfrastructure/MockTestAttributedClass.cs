/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using TextMetal.Middleware.UnitTests.TestingInfrastructure;

[assembly: MockSingleTestAttibute(int.MaxValue)]
[module: MockSingleTestAttibute(int.MinValue)]

namespace TextMetal.Middleware.UnitTests.TestingInfrastructure
{
	[MockSingleTestAttibute(1)]
	[MockMultipleTestAttibute(10)]
	[MockMultipleTestAttibute(20)]
	public class MockTestAttributedClass
	{
		#region Constructors/Destructors

		public MockTestAttributedClass()
		{
		}

		#endregion

		#region Methods/Operators

		[MockSingleTestAttibute(2)]
		[return: MockSingleTestAttibute(8)]
		public object MyMethod([MockSingleTestAttibute(4)] object obj)
		{
			return obj;
		}

		#endregion
	}
}
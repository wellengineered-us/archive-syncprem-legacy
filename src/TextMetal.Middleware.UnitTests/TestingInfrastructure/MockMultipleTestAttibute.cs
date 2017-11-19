/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace TextMetal.Middleware.UnitTests.TestingInfrastructure
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public class MockMultipleTestAttibute : Attribute
	{
		#region Constructors/Destructors

		public MockMultipleTestAttibute(int value)
		{
			this.value = value;
		}

		#endregion

		#region Fields/Constants

		private int value;

		#endregion

		#region Properties/Indexers/Events

		public int Value
		{
			get
			{
				return this.value;
			}
		}

		#endregion
	}
}
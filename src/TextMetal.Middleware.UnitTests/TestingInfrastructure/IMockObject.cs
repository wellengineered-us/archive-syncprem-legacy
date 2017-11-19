/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace TextMetal.Middleware.UnitTests.TestingInfrastructure
{
	public interface IMockObject
	{
		#region Properties/Indexers/Events

		object SomeProp
		{
			get;
			set;
		}

		#endregion

		#region Methods/Operators

		byte SomeMethodWithVarietyOfParameters(int inparam, out string outparam, ref object refparam);

		#endregion
	}
}
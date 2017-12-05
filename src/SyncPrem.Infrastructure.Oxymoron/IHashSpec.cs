/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace SyncPrem.Infrastructure.Oxymoron
{
	public interface IHashSpec
	{
		#region Properties/Indexers/Events

		long? Multiplier
		{
			get;
			set;
		}

		long? Seed
		{
			get;
			set;
		}

		#endregion
	}
}
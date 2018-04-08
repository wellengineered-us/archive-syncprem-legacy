/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace SyncPrem.Infrastructure.Configuration
{
	public interface IConfigurable<TConfiguration>
		where TConfiguration : IConfigurationObject
	{
		#region Properties/Indexers/Events

		TConfiguration Configuration
		{
			get;
			set;
		}

		#endregion
	}
}
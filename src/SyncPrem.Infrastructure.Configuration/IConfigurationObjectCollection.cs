/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections;

namespace SyncPrem.Infrastructure.Configuration
{
	/// <summary>
	/// Represents an configuration object collection.
	/// </summary>
	public interface IConfigurationObjectCollection : IList //<IConfigurationObject>
	{
		#region Properties/Indexers/Events

		/// <summary>
		/// Gets the site configuration object or null if this instance is unattached.
		/// </summary>
		IConfigurationObject Site
		{
			get;
		}

		#endregion
	}
}
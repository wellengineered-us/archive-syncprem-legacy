/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace SyncPrem.Infrastructure.Configuration
{
	/// <summary>
	/// Represents an configuration object collection.
	/// NOTE: This interface is invariant due to its use of IList`1,
	/// thus it should NOT derive/implement the non-generic version.
	/// This will be left to the class implementation for which to solve.
	/// </summary>
	/// <typeparam name="TItemConfigurationObject"> </typeparam>
	public interface IConfigurationObjectCollection<TItemConfigurationObject> /*: IConfigurationObjectCollection*/
		where TItemConfigurationObject : IConfigurationObject
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
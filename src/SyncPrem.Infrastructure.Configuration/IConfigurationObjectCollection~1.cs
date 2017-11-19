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
	/// <typeparam name="TConfigurationObject"> </typeparam>
	public interface IConfigurationObjectCollection<TConfigurationObject> /*: IConfigurationObjectCollection*/
		where TConfigurationObject : IConfigurationObject
	{
	}
}
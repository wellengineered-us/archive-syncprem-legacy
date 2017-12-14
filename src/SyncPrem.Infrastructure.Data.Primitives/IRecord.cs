/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

namespace SyncPrem.Infrastructure.Data.Primitives
{
	public interface IRecord : IDictionary<string, object>
	{
		#region Properties/Indexers/Events

		object ContextData
		{
			get;
		}

		long RecordIndex
		{
			get;
		}

		#endregion
	}
}
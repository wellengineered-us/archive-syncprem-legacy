/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.StreamingIO.ProxyWrappers;

using __IRecord = System.Collections.Generic.IDictionary<string, object>;
using __Record = System.Collections.Generic.Dictionary<string, object>;

namespace SyncPrem.StreamingIO.AdoNet
{
	public interface IAdoNetResult : IApplyWrap<IAdoNetResult, __IRecord>
	{
		#region Properties/Indexers/Events

		IEnumerable<__IRecord> Records
		{
			get;
		}

		int RecordsAffected
		{
			get;
		}

		long ResultIndex
		{
			get;
		}

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.StreamingIO.Relational
{
	public interface IAdoNetStreamingResult
	{
		#region Properties/Indexers/Events

		IAsyncEnumerable<IPayload> AsyncRecords
		{
			get;
		}

		IEnumerable<IPayload> Records
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
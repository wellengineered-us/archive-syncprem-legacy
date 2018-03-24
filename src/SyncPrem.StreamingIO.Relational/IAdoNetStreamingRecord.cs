/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.StreamingIO.Relational
{
	public interface IAdoNetStreamingRecord : IPayload
	{
		#region Properties/Indexers/Events

		long RecordIndex
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
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Runtime;

namespace SyncPrem.Pipeline.Abstractions.Sync.Runtime
{
	public interface ISyncChannel : ISyncComponent, IChannel
	{
		#region Properties/Indexers/Events

		ISyncStream Records
		{
			get;
		}

		#endregion
	}
}
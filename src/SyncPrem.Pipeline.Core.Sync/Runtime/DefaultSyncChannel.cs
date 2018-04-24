/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions.Sync;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;

namespace SyncPrem.Pipeline.Core.Sync.Runtime
{
	public sealed class DefaultSyncChannel : SyncComponent, ISyncChannel
	{
		#region Constructors/Destructors

		public DefaultSyncChannel(IEnumerable<ISyncRecord> records)
		{
			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			this.records = new DefaultSyncStream(records);
		}

		#endregion

		#region Fields/Constants

		private readonly ISyncStream records;

		#endregion

		#region Properties/Indexers/Events

		public ISyncStream Records
		{
			get
			{
				return this.records;
			}
		}

		#endregion
	}
}
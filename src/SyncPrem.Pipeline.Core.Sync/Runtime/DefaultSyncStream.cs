using System;
using System.Collections;
using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions.Sync;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;

namespace SyncPrem.Pipeline.Core.Sync.Runtime
{
	public sealed class DefaultSyncStream : SyncComponent, ISyncStream
	{
		#region Constructors/Destructors

		public DefaultSyncStream(IEnumerable<ISyncRecord> records)
		{
			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			this.records = records;
		}

		#endregion

		#region Fields/Constants

		private readonly IEnumerable<ISyncRecord> records;

		#endregion

		#region Properties/Indexers/Events

		public IEnumerable<ISyncRecord> Records
		{
			get
			{
				return this.records;
			}
		}

		#endregion

		#region Methods/Operators

		public IEnumerator<ISyncRecord> GetEnumerator()
		{
			return this.Records.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
using System;

using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;
using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.Pipeline.Core.Sync.Runtime
{
	public sealed class DefaultSyncRecord : Record, ISyncRecord
	{
		#region Constructors/Destructors

		public DefaultSyncRecord(ISchema schema, IPayload payload, string topic, IPartition partition, IOffset offset)
			: base(schema, payload, topic, partition, offset)
		{
		}

		#endregion
	}
}
using System;

using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.Pipeline.Core.Async.Runtime
{
	public sealed class DefaultAsyncRecord : Record, IAsyncRecord
	{
		#region Constructors/Destructors

		public DefaultAsyncRecord(ISchema schema, IPayload payload, string topic, IPartition partition, IOffset offset)
			: base(schema, payload, topic, partition, offset)
		{
		}

		#endregion
	}
}
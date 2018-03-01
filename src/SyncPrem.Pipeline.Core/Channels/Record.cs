/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Channel;
using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.Pipeline.Core.Channels
{
	public sealed class Record : IRecord
	{
		#region Constructors/Destructors

		public Record(ISchema schema, IPayload payload, string topic, int partition, IPayload offset)
		{
			if ((object)schema == null)
				throw new ArgumentNullException(nameof(schema));

			if ((object)payload == null)
				throw new ArgumentNullException(nameof(payload));

			if ((object)offset == null)
				throw new ArgumentNullException(nameof(offset));

			this.schema = schema;
			this.payload = payload;
			this.topic = topic;
			this.partition = partition;
			this.offset = offset;
			this.timestamp = DateTimeOffset.UtcNow;
		}

		#endregion

		#region Fields/Constants

		private readonly IPayload offset;
		private readonly int partition;
		private readonly IPayload payload;

		private readonly ISchema schema;
		private readonly DateTimeOffset timestamp;
		private readonly string topic;

		#endregion

		#region Properties/Indexers/Events

		public IPayload Offset
		{
			get
			{
				return this.offset;
			}
		}

		public int Partition
		{
			get
			{
				return this.partition;
			}
		}

		public IPayload Payload
		{
			get
			{
				return this.payload;
			}
		}

		public ISchema Schema
		{
			get
			{
				return this.schema;
			}
		}

		public DateTimeOffset Timestamp
		{
			get
			{
				return this.timestamp;
			}
		}

		public string Topic
		{
			get
			{
				return this.topic;
			}
		}

		#endregion

		#region Methods/Operators

		public override string ToString()
		{
			return $"{this.Schema} | {this.Payload}";
		}

		#endregion
	}
}
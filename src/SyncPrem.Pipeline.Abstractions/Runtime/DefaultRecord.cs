/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.Pipeline.Abstractions.Runtime
{
	public abstract class Record : IRecord
	{
		#region Constructors/Destructors

		protected Record(ISchema schema, IPayload payload, string topic, IPartition partition, IOffset offset)
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

		private readonly IOffset offset;
		private readonly IPartition partition;
		private readonly IPayload payload;
		private readonly ISchema schema;
		private readonly DateTimeOffset timestamp;
		private readonly string topic;
		private long? index;

		#endregion

		#region Properties/Indexers/Events

		public IOffset Offset
		{
			get
			{
				return this.offset;
			}
		}

		public IPartition Partition
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

		public long? Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override string ToString()
		{
			return $"{this.Index} | {this.Payload}";
		}

		#endregion
	}
}
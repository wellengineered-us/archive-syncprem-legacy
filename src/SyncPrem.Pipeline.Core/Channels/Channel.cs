/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Channel;
using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.Pipeline.Core.Channels
{
	public sealed class Channel : Component, IChannel
	{
		#region Constructors/Destructors

		public Channel(ISchema schema, IEnumerable<IRecord> records)
		{
			if ((object)schema == null)
				throw new ArgumentNullException(nameof(schema));

			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			this.schema = schema;
			this.records = records;
			this.timestamp = DateTimeOffset.UtcNow;
		}

		#endregion

		#region Fields/Constants

		private readonly IEnumerable<IRecord> records;
		private readonly ISchema schema;
		private readonly DateTimeOffset timestamp;

		#endregion

		#region Properties/Indexers/Events

		public IEnumerable<IRecord> Records
		{
			get
			{
				return this.records;
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

		#endregion
	}
}
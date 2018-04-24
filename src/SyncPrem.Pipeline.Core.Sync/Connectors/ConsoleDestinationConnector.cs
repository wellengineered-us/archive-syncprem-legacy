/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;
using SyncPrem.Pipeline.Abstractions.Sync.Stage.Connector.Destination;
using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.Pipeline.Core.Sync.Connectors
{
	public class ConsoleDestinationConnector : SyncDestinationConnector<StageSpecificConfiguration>
	{
		#region Constructors/Destructors

		public ConsoleDestinationConnector()
		{
		}

		#endregion

		#region Fields/Constants

		private static readonly TextWriter textWriter = Console.Out;

		#endregion

		#region Properties/Indexers/Events

		private static TextWriter TextWriter
		{
			get
			{
				return textWriter;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void ConsumeInternal(ISyncContext context, RecordConfiguration configuration, ISyncChannel channel)
		{
			IEnumerable<IRecord> records;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)channel == null)
				throw new ArgumentNullException(nameof(channel));

			this.AssertValidConfiguration();

			records = channel.Records;

			if ((object)records == null)
				throw new SyncPremException(nameof(records));

			foreach (IRecord record in records)
				TextWriter.WriteLine(record);
		}

		protected override void Create(bool creating)
		{
			// do nothing
			base.Create(creating);
		}

		protected override void Dispose(bool disposing)
		{
			// do nothing
			base.Dispose(disposing);
		}

		protected override void PostExecuteInternal(ISyncContext context, RecordConfiguration configuration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();
		}

		protected override void PreExecuteInternal(ISyncContext context, RecordConfiguration configuration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();
		}

		#endregion
	}
}
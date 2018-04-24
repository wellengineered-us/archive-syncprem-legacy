/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Async.Stage.Connector.Destination;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.Pipeline.Core.Async.Connectors
{
	public class ConsoleDestinationConnector : AsyncDestinationConnector<StageSpecificConfiguration>
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

		protected override async Task ConsumeAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, IAsyncChannel asyncChannel, CancellationToken cancellationToken)
		{
			IAsyncEnumerable<IAsyncRecord> records;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)asyncChannel == null)
				throw new ArgumentNullException(nameof(asyncChannel));

			this.AssertValidConfiguration();

			records = asyncChannel.Records;

			if ((object)records == null)
				throw new SyncPremException(nameof(records));

			IAsyncEnumerator<IAsyncRecord> recordz;
			recordz = records.GetEnumerator();

			if ((object)recordz == null)
				throw new InvalidOperationException(nameof(recordz));

			while (await recordz.MoveNext(cancellationToken))
			{
				IAsyncRecord record = recordz.Current;
				TextWriter.WriteLine(record);
			}
		}

		protected override async Task CreateAsync(bool creating, CancellationToken cancellationToken)
		{
			// do nothing
			await base.CreateAsync(creating, cancellationToken);
		}

		protected override async Task DisposeAsync(bool disposing, CancellationToken cancellationToken)
		{
			// do nothing
			await base.DisposeAsync(disposing, cancellationToken);
		}

		protected override Task PostExecuteAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			return Task.CompletedTask;
		}

		protected override Task PreExecuteAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			return Task.CompletedTask;
		}

		#endregion
	}
}
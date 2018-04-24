/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Async.Stage.Connector.Destination;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Core.Configurations;
using SyncPrem.StreamingIO.Primitives;
using SyncPrem.StreamingIO.Textual;

namespace SyncPrem.Pipeline.Core.Async.Connectors
{
	public abstract class TextualDestinationConnector<TTextualFieldConfiguration, TTextualConfiguration, TTextualFieldSpec, TTextualSpec, TTextualConnectorSpecificConfiguration, TTextualWriter> : AsyncDestinationConnector<TTextualConnectorSpecificConfiguration>
		where TTextualFieldConfiguration : TextualFieldConfiguration
		where TTextualConfiguration : TextualConfiguration<TTextualFieldConfiguration, TTextualFieldSpec, TTextualSpec>
		where TTextualFieldSpec : ITextualFieldSpec
		where TTextualSpec : ITextualSpec<TTextualFieldSpec>
		where TTextualConnectorSpecificConfiguration : TextualConnectorSpecificConfiguration<TTextualFieldConfiguration, TTextualConfiguration, TTextualFieldSpec, TTextualSpec>, new()
		where TTextualWriter : TextualWriter<TTextualFieldSpec, TTextualSpec>
	{
		#region Constructors/Destructors

		protected TextualDestinationConnector()
		{
		}

		#endregion

		#region Fields/Constants

		private TTextualWriter textualWriter;

		#endregion

		#region Properties/Indexers/Events

		protected TTextualWriter TextualWriter
		{
			get
			{
				return this.textualWriter;
			}
			private set
			{
				this.textualWriter = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override async Task ConsumeAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, IAsyncChannel asyncChannel, CancellationToken cancellationToken)
		{
			IAsyncEnumerable<IRecord> records;

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

			var payloads = asyncChannel.Records.Select(p => p.Payload);

			await this.TextualWriter.WriteRecordsAsync(payloads, cancellationToken);
		}

		protected override async Task CreateAsync(bool creating, CancellationToken cancellationToken)
		{
			// do nothing
			await base.CreateAsync(creating, cancellationToken);
		}

		protected abstract TTextualWriter CreateTextualWriter(StreamWriter streamWriter, TTextualSpec textualSpec);

		protected override async Task DisposeAsync(bool disposing, CancellationToken cancellationToken)
		{
			if (disposing)
			{
				if ((object)this.TextualWriter != null)
				{
					this.TextualWriter.Dispose(); // wish list: DisposeAsync()
					this.TextualWriter = null;
				}
			}

			await base.DisposeAsync(disposing, cancellationToken);
		}

		protected override async Task PostExecuteAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			TTextualConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			if ((object)this.TextualWriter != null)
			{
				await this.TextualWriter.FlushAsync(cancellationToken);
				this.TextualWriter.Dispose(); // wish list: DisposeAsync()
				this.TextualWriter = null;
			}
		}

		protected override Task PreExecuteAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			TTextualSpec spec;
			string filePath;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			TTextualConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			spec = fsConfig.TextualConfiguration.MapToSpec();

			if ((object)spec == null)
				throw new SyncPremException(nameof(spec));

			//if (!spec.TextualHeaderSpecs.Any())
			//throw new InvalidOperationException(string.Format("Configuration missing: [Source and/or Destination]...{0}.{1}", nameof(DelimitedTextualSpec), nameof(DelimitedTextualSpec.TextualHeaderSpecs)));

			if (Directory.Exists(fsConfig.TextualFilePath))
				filePath = Path.Combine(fsConfig.TextualFilePath, Path.GetRandomFileName());
			else
				filePath = fsConfig.TextualFilePath;

			this.TextualWriter = this.CreateTextualWriter(new StreamWriter(new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous)), spec);

			// wish list: more async file system operations
			return Task.CompletedTask;
		}

		#endregion
	}
}
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

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Async.Stage.Connector.Source;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Core.Async.Runtime;
using SyncPrem.Pipeline.Core.Configurations;
using SyncPrem.StreamingIO.Primitives;
using SyncPrem.StreamingIO.ProxyWrappers;
using SyncPrem.StreamingIO.Textual;

namespace SyncPrem.Pipeline.Core.Async.Connectors
{
	public abstract class TextualSourceConnector<TTextualFieldConfiguration, TTextualConfiguration, TTextualFieldSpec, TTextualSpec, TTextualConnectorSpecificConfiguration, TTextualReader> : AsyncSourceConnector<TTextualConnectorSpecificConfiguration>
		where TTextualFieldConfiguration : TextualFieldConfiguration
		where TTextualConfiguration : TextualConfiguration<TTextualFieldConfiguration, TTextualFieldSpec, TTextualSpec>
		where TTextualFieldSpec : ITextualFieldSpec
		where TTextualSpec : ITextualSpec<TTextualFieldSpec>
		where TTextualConnectorSpecificConfiguration : TextualConnectorSpecificConfiguration<TTextualFieldConfiguration, TTextualConfiguration, TTextualFieldSpec, TTextualSpec>, new()
		where TTextualReader : TextualReader<TTextualFieldSpec, TTextualSpec>
	{
		#region Constructors/Destructors

		protected TextualSourceConnector()
		{
		}

		#endregion

		#region Fields/Constants

		private TTextualReader textualReader;

		#endregion

		#region Properties/Indexers/Events

		protected TTextualReader TextualReader
		{
			get
			{
				return this.textualReader;
			}
			private set
			{
				this.textualReader = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override async Task CreateAsync(bool creating, CancellationToken cancellationToken)
		{
			// do nothing
			await base.CreateAsync(creating, cancellationToken);
		}

		protected abstract TTextualReader CreateTextualReader(StreamReader streamReader, TTextualSpec textualSpec);

		protected override async Task DisposeAsync(bool disposing, CancellationToken cancellationToken)
		{
			if (disposing)
			{
				if ((object)this.TextualReader != null)
				{
					this.TextualReader.Dispose(); // wish list: DisposeAsync()
					this.TextualReader = null;
				}
			}

			await base.DisposeAsync(disposing, cancellationToken);
		}

		protected override Task PostExecuteAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			TTextualConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			if ((object)this.TextualReader != null)
			{
				this.TextualReader.Dispose(); // wish list: DisposeAsync()
				this.TextualReader = null;
			}

			return Task.CompletedTask;
		}

		protected override async Task PreExecuteAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			SchemaBuilder schemaBuilder;
			ISchema schema;
			IAsyncEnumerable<TTextualFieldSpec> headers;
			TTextualSpec spec;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			TTextualConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			spec = fsConfig.TextualConfiguration.MapToSpec();

			if ((object)spec == null)
				throw new SyncPremException(nameof(spec));

			this.TextualReader = this.CreateTextualReader(new StreamReader(new FileStream(fsConfig.TextualFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous)), spec);

			headers = this.TextualReader.ReadHeaderFieldsAsync(cancellationToken);

			if ((object)headers == null)
				throw new SyncPremException(nameof(headers));

			if (!asyncContext.LocalState.TryGetValue(this, out IDictionary<string, object> localState))
			{
				localState = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
				asyncContext.LocalState.Add(this, localState);
			}

			schemaBuilder = SchemaBuilder.Create();

			await headers.ForceAsyncEnumeration(cancellationToken);

			foreach (TTextualFieldSpec header in spec.TextualHeaderSpecs)
				schemaBuilder.AddField(header.FieldTitle, header.FieldType.ToClrType(), header.IsFieldRequired, header.IsFieldIdentity);

			schema = schemaBuilder.Build();

			if ((object)schema == null)
				throw new SyncPremException(nameof(schema));

			localState.Add(Constants.ContextComponentScopedSchema, schema);
		}

		protected override Task<IAsyncChannel> ProduceAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			IAsyncChannel asyncChannel;
			ISchema schema;

			IAsyncEnumerable<IPayload> payloads;
			IAsyncEnumerable<IAsyncRecord> records;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			TTextualConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			if (!asyncContext.LocalState.TryGetValue(this, out IDictionary<string, object> localState))
			{
				localState = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
				asyncContext.LocalState.Add(this, localState);
			}

			schema = localState[Constants.ContextComponentScopedSchema] as ISchema;

			if ((object)schema == null)
				throw new SyncPremException(nameof(schema));

			schema = localState[Constants.ContextComponentScopedSchema] as ISchema;

			if ((object)schema == null)
				throw new SyncPremException(nameof(schema));

			payloads = this.TextualReader.ReadRecordsAsync(cancellationToken);

			if ((object)payloads == null)
				throw new SyncPremException(nameof(payloads));

			records = payloads.Select(rec => new DefaultAsyncRecord(schema, rec, string.Empty, Partition.None, Offset.None));

			if ((object)records == null)
				throw new SyncPremException(nameof(records));

			asyncChannel = asyncContext.CreateChannelAsync(records);

			return Task.FromResult(asyncChannel);
		}

		#endregion
	}
}
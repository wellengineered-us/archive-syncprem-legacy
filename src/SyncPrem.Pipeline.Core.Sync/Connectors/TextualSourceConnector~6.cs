/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;
using SyncPrem.Pipeline.Abstractions.Sync.Stage.Connector.Source;
using SyncPrem.Pipeline.Core.Configurations;
using SyncPrem.Pipeline.Core.Sync.Runtime;
using SyncPrem.StreamingIO.Primitives;
using SyncPrem.StreamingIO.ProxyWrappers;
using SyncPrem.StreamingIO.Textual;

namespace SyncPrem.Pipeline.Core.Sync.Connectors
{
	public abstract class TextualSourceConnector<TTextualFieldConfiguration, TTextualConfiguration, TTextualFieldSpec, TTextualSpec, TTextualConnectorSpecificConfiguration, TTextualReader> : SyncSourceConnector<TTextualConnectorSpecificConfiguration>
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

		protected override void Create(bool creating)
		{
			// do nothing
			base.Create(creating);
		}

		protected abstract TTextualReader CreateTextualReader(StreamReader streamReader, TTextualSpec textualSpec);

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if ((object)this.TextualReader != null)
				{
					this.TextualReader.Dispose(); // wish list: DisposeAsync()
					this.TextualReader = null;
				}
			}

			base.Dispose(disposing);
		}

		protected override void PostExecuteInternal(ISyncContext context, RecordConfiguration configuration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			TTextualConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			if ((object)this.TextualReader != null)
			{
				this.TextualReader.Dispose();
				this.TextualReader = null;
			}
		}

		protected override void PreExecuteInternal(ISyncContext context, RecordConfiguration configuration)
		{
			SchemaBuilder schemaBuilder;
			ISchema schema;
			IEnumerable<TTextualFieldSpec> headers;
			TTextualSpec spec;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			TTextualConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			spec = fsConfig.TextualConfiguration.MapToSpec();

			if ((object)spec == null)
				throw new SyncPremException(nameof(spec));

			this.TextualReader = this.CreateTextualReader(new StreamReader(File.Open(fsConfig.TextualFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)), spec);

			headers = this.TextualReader.ReadHeaderFields();

			if ((object)headers == null)
				throw new SyncPremException(nameof(headers));

			if (!context.LocalState.TryGetValue(this, out IDictionary<string, object> localState))
			{
				localState = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
				context.LocalState.Add(this, localState);
			}

			schemaBuilder = SchemaBuilder.Create();

			headers.ForceEnumeration();

			foreach (TTextualFieldSpec header in spec.TextualHeaderSpecs)
				schemaBuilder.AddField(header.FieldTitle, header.FieldType.ToClrType(), header.IsFieldRequired, header.IsFieldIdentity);

			schema = schemaBuilder.Build();

			if ((object)schema == null)
				throw new SyncPremException(nameof(schema));

			localState.Add(Constants.ContextComponentScopedSchema, schema);
		}

		protected override ISyncChannel ProduceInternal(ISyncContext context, RecordConfiguration configuration)
		{
			ISyncChannel channel;
			ISchema schema;

			IEnumerable<IPayload> payloads;
			IEnumerable<ISyncRecord> records;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			TTextualConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			if (!context.LocalState.TryGetValue(this, out IDictionary<string, object> localState))
			{
				localState = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
				context.LocalState.Add(this, localState);
			}

			schema = localState[Constants.ContextComponentScopedSchema] as ISchema;

			if ((object)schema == null)
				throw new SyncPremException(nameof(schema));

			schema = localState[Constants.ContextComponentScopedSchema] as ISchema;

			if ((object)schema == null)
				throw new SyncPremException(nameof(schema));

			payloads = this.TextualReader.ReadRecords();

			if ((object)payloads == null)
				throw new SyncPremException(nameof(payloads));

			records = payloads.Select(rec => new DefaultSyncRecord(schema, rec, string.Empty, Partition.None, Offset.None));

			if ((object)records == null)
				throw new SyncPremException(nameof(records));

			channel = context.CreateChannel(records);

			return channel;
		}

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Channel;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Source;
using SyncPrem.Pipeline.Core.Channels;
using SyncPrem.Pipeline.Core.Configurations.FlatText;
using SyncPrem.StreamingIO.Primitives;
using SyncPrem.StreamingIO.Textual.Delimited;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Connectors.FlatText
{
	public class DelimitedTextSourceConnector : SourceConnector<DelimitedTextConnectorSpecificConfiguration>
	{
		#region Constructors/Destructors

		public DelimitedTextSourceConnector()
		{
		}

		#endregion

		#region Fields/Constants

		private DelimitedTextualReader delimitedTextualReader;

		#endregion

		#region Properties/Indexers/Events

		private DelimitedTextualReader DelimitedTextualReader
		{
			get
			{
				return this.delimitedTextualReader;
			}
			set
			{
				this.delimitedTextualReader = value;
			}
		}

		#endregion

		#region Methods/Operators

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

		protected override void PostExecuteRecord(IContext context, RecordConfiguration configuration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)this.StageConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration));

			if ((object)this.StageConfiguration.StageSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration.StageSpecificConfiguration));

			DelimitedTextConnectorSpecificConfiguration fsConfig = this.StageConfiguration.StageSpecificConfiguration;

			if ((object)this.DelimitedTextualReader != null)
				this.DelimitedTextualReader.Dispose();

			this.DelimitedTextualReader = null;
		}

		protected override void PreExecuteRecord(IContext context, RecordConfiguration configuration)
		{
			SchemaBuilder schemaBuilder;
			ISchema schema;
			IEnumerable<IDelimitedTextualFieldSpec> headers;
			DelimitedTextualSpec spec;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)this.StageConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration));

			if ((object)this.StageConfiguration.StageSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration.StageSpecificConfiguration));

			DelimitedTextConnectorSpecificConfiguration fsConfig = this.StageConfiguration.StageSpecificConfiguration;

			if ((object)fsConfig.DelimitedTextSpecConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(fsConfig.DelimitedTextSpecConfiguration)));

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.DelimitedTextFilePath))
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(fsConfig.DelimitedTextFilePath)));

			spec = DelimitedTextSpecConfiguration.ToSpec(fsConfig.DelimitedTextSpecConfiguration);

			if ((object)spec == null)
				throw new SyncPremException(nameof(spec));

			this.DelimitedTextualReader = new DelimitedTextualReader(new StreamReader(File.Open(fsConfig.DelimitedTextFilePath, FileMode.Open, FileAccess.Read, FileShare.None)), spec);

			headers = this.DelimitedTextualReader.ReadHeaderFields();

			if ((object)headers == null)
				throw new SyncPremException(nameof(headers));

			if (!context.LocalState.TryGetValue(this, out IDictionary<string, object> localState))
			{
				localState = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
				context.LocalState.Add(this, localState);
			}

			schemaBuilder = SchemaBuilder.Create();

			foreach (IDelimitedTextualFieldSpec header in spec.DelimitedTextHeaderSpecs)
				schemaBuilder.AddField(header.FieldTitle, header.FieldType, header.IsFieldRequired, header.IsFieldIdentity);

			schema = schemaBuilder.Build();

			if ((object)schema == null)
				throw new SyncPremException(nameof(schema));

			localState.Add(Constants.ContextComponentScopedSchema, schema);
		}

		protected override IChannel ProduceRecord(IContext context, RecordConfiguration configuration)
		{
			IChannel channel;
			ISchema schema;

			IEnumerable<IPayload> payloads;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)this.StageConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration));

			if ((object)this.StageConfiguration.StageSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration.StageSpecificConfiguration));

			DelimitedTextConnectorSpecificConfiguration fsConfig = this.StageConfiguration.StageSpecificConfiguration;

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

			payloads = this.DelimitedTextualReader.ReadRecords();

			if ((object)payloads == null)
				throw new SyncPremException(nameof(payloads));

			var records = payloads.Select(rec => new Record(schema, rec, string.Empty, 0, new Payload()));

			channel = context.CreateChannel(records);

			return channel;
		}

		#endregion
	}
}
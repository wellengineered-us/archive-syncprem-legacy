/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Payload;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Source;
using SyncPrem.Pipeline.Core.Configurations.FlatText;
using SyncPrem.StreamingIO.FlatText.Delimited;
using SyncPrem.StreamingIO.Primitives;

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

		private DelimitedTextReader delimitedTextReader;

		#endregion

		#region Properties/Indexers/Events

		private DelimitedTextReader DelimitedTextReader
		{
			get
			{
				return this.delimitedTextReader;
			}
			set
			{
				this.delimitedTextReader = value;
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

		protected override void PostExecuteRecord(IContext context, RecordConfiguration recordConfiguration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			if ((object)this.StageConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration));

			if ((object)this.StageConfiguration.StageSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration.StageSpecificConfiguration));

			DelimitedTextConnectorSpecificConfiguration fsConfig = this.StageConfiguration.StageSpecificConfiguration;

			if ((object)this.DelimitedTextReader != null)
				this.DelimitedTextReader.Dispose();

			this.DelimitedTextReader = null;
		}

		protected override void PreExecuteRecord(IContext context, RecordConfiguration recordConfiguration)
		{
			IEnumerable<IField> fields;
			IPipelineMetadata pipelineMetadata;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			if ((object)this.StageConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration));

			if ((object)this.StageConfiguration.StageSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration.StageSpecificConfiguration));

			DelimitedTextConnectorSpecificConfiguration fsConfig = this.StageConfiguration.StageSpecificConfiguration;

			if ((object)fsConfig.DelimitedTextSpecConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(fsConfig.DelimitedTextSpecConfiguration)));

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.DelimitedTextFilePath))
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(fsConfig.DelimitedTextFilePath)));

			this.DelimitedTextReader = new DelimitedTextReader(new StreamReader(File.Open(fsConfig.DelimitedTextFilePath, FileMode.Open, FileAccess.Read, FileShare.None)), fsConfig.DelimitedTextSpecConfiguration);

			fields = this.DelimitedTextReader.ReadHeaderFields();

			if ((object)fields == null)
				throw new InvalidOperationException(string.Format("Fields were invalid."));

			pipelineMetadata = context.CreateMetadata(fields);
			context.MetadataChain.Push(pipelineMetadata);
		}

		protected override IPipelineMessage ProduceRecord(IContext context, RecordConfiguration recordConfiguration)
		{
			IPipelineMessage pipelineMessage;
			IEnumerable<IRecord> sourceDataEnumerable;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			if ((object)this.StageConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration));

			if ((object)this.StageConfiguration.StageSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration.StageSpecificConfiguration));

			DelimitedTextConnectorSpecificConfiguration fsConfig = this.StageConfiguration.StageSpecificConfiguration;

			sourceDataEnumerable = this.DelimitedTextReader.ReadRecords();

			if ((object)sourceDataEnumerable == null)
				throw new InvalidOperationException(string.Format("Records were invalid."));

			pipelineMessage = context.CreateMessage(sourceDataEnumerable);

			return pipelineMessage;
		}

		#endregion
	}
}
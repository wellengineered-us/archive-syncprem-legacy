/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Payload;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Destination;
using SyncPrem.Pipeline.Core.Configurations.FlatText;
using SyncPrem.StreamingIO.FlatText.Delimited;
using SyncPrem.StreamingIO.Primitives;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Connectors.FlatText
{
	public class DelimitedTextDestinationConnector : DestinationConnector<DelimitedTextConnectorSpecificConfiguration>
	{
		#region Constructors/Destructors

		public DelimitedTextDestinationConnector()
		{
		}

		#endregion

		#region Fields/Constants

		private DelimitedTextWriter delimitedTextWriter;

		#endregion

		#region Properties/Indexers/Events

		private DelimitedTextWriter DelimitedTextWriter
		{
			get
			{
				return this.delimitedTextWriter;
			}
			set
			{
				this.delimitedTextWriter = value;
			}
		}

		#endregion

		#region Methods/Operators

		[Obsolete]
		private void BrittlePropagation(DelimitedTextSpec effectiveDelimitedTextSpec, IField[] upstreamFields)
		{
			StageConfiguration<DelimitedTextConnectorSpecificConfiguration> sourceConfiguration;

			if ((object)effectiveDelimitedTextSpec == null)
				throw new ArgumentNullException(nameof(effectiveDelimitedTextSpec));

			if ((object)upstreamFields == null)
				throw new ArgumentNullException(nameof(upstreamFields));

			sourceConfiguration = new StageConfiguration<DelimitedTextConnectorSpecificConfiguration>(((PipelineConfiguration)this.StageConfiguration.Parent).SourceConfiguration);

			// 2016-07-12 / dpbullington@gmail.com: fix NRE bug in the below check
			// attempt to "flow" the DTM spec from source to destination if not specified on destination
			if ((object)sourceConfiguration.StageSpecificConfiguration != null &&
				(object)sourceConfiguration.StageSpecificConfiguration.DelimitedTextSpecConfiguration != null)
			{
				DelimitedTextSpecConfiguration srcDts = sourceConfiguration.StageSpecificConfiguration.DelimitedTextSpecConfiguration;
				List<IDelimitedTextFieldSpec> effectiveDelimitedTextFieldSpecs;

				effectiveDelimitedTextFieldSpecs = effectiveDelimitedTextSpec.DelimitedTextFieldSpecs.ToList();

				if (effectiveDelimitedTextFieldSpecs.Count <= 0)
				{
					if (upstreamFields.Length <= 0)
						effectiveDelimitedTextFieldSpecs.AddRange(srcDts.DelimitedTextFieldConfigurations);
					else
						effectiveDelimitedTextFieldSpecs.AddRange(upstreamFields.OrderBy(us => us.FieldIndex).Select((io, ix) => new DelimitedTextFieldSpec()
																																{
																																	FieldIndex = ix,
																																	FieldName = io.FieldName,
																																	IsFieldKeyComponent = io.IsFieldKeyComponent,
																																	IsFieldOptional = io.IsFieldOptional,
																																	FieldType = io.FieldType
																																}));

					effectiveDelimitedTextSpec.DelimitedTextFieldSpecs = effectiveDelimitedTextFieldSpecs;
				}

				if (effectiveDelimitedTextSpec.FieldDelimiter == null)
					effectiveDelimitedTextSpec.FieldDelimiter = srcDts.FieldDelimiter;

				if (effectiveDelimitedTextSpec.OpenQuoteValue == null)
					effectiveDelimitedTextSpec.OpenQuoteValue = srcDts.OpenQuoteValue;

				if (effectiveDelimitedTextSpec.CloseQuoteValue == null)
					effectiveDelimitedTextSpec.CloseQuoteValue = srcDts.CloseQuoteValue;

				if (effectiveDelimitedTextSpec.RecordDelimiter == null)
					effectiveDelimitedTextSpec.RecordDelimiter = srcDts.RecordDelimiter; // fix bug using wrong property

				//if (effectiveDelimitedTextSpec.FirstRecordIsHeader == null)
				//effectiveDelimitedTextSpec.FirstRecordIsHeader = srcDts.FirstRecordIsHeader;

				//if (effectiveDelimitedTextSpec.LastRecordIsFooter == null)
				//effectiveDelimitedTextSpec.LastRecordIsFooter = srcDts.LastRecordIsFooter;
			}
		}

		protected override void ConsumeRecord(IContext context, RecordConfiguration recordConfiguration, IPipelineMessage pipelineMessage)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			if ((object)pipelineMessage == null)
				throw new ArgumentNullException(nameof(pipelineMessage));

			this.DelimitedTextWriter.WriteRecords(pipelineMessage.Records);
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

			if ((object)this.DelimitedTextWriter != null)
			{
				this.DelimitedTextWriter.Flush();
				this.DelimitedTextWriter.Dispose();
			}

			this.DelimitedTextWriter = null;
		}

		protected override void PreExecuteRecord(IContext context, RecordConfiguration recordConfiguration)
		{
			IPipelineMetadata pipelineMetadata;
			IField[] upstreamFields;
			DelimitedTextSpec effectiveDelimitedTextSpec;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			if ((object)this.StageConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration));

			if ((object)this.StageConfiguration.StageSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration.StageSpecificConfiguration));

			DelimitedTextConnectorSpecificConfiguration fsConfig = this.StageConfiguration.StageSpecificConfiguration;

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.DelimitedTextFilePath))
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(fsConfig.DelimitedTextFilePath)));

			pipelineMetadata = context.MetadataChain.Peek();
			upstreamFields = pipelineMetadata.UpstreamFields.ToArray();

			effectiveDelimitedTextSpec = new DelimitedTextSpec();

			if (fsConfig.DelimitedTextSpecConfiguration != null)
			{
				List<IDelimitedTextFieldSpec> effectiveDelimitedTextFieldSpecs;

				effectiveDelimitedTextFieldSpecs = fsConfig.DelimitedTextSpecConfiguration.DelimitedTextFieldConfigurations.Cast<IDelimitedTextFieldSpec>().ToList();

				effectiveDelimitedTextSpec.RecordDelimiter = fsConfig.DelimitedTextSpecConfiguration.RecordDelimiter;
				effectiveDelimitedTextSpec.FieldDelimiter = fsConfig.DelimitedTextSpecConfiguration.FieldDelimiter;
				effectiveDelimitedTextSpec.FirstRecordIsHeader = fsConfig.DelimitedTextSpecConfiguration.FirstRecordIsHeader ?? false;
				effectiveDelimitedTextSpec.LastRecordIsFooter = fsConfig.DelimitedTextSpecConfiguration.LastRecordIsFooter ?? false;
				effectiveDelimitedTextSpec.OpenQuoteValue = fsConfig.DelimitedTextSpecConfiguration.OpenQuoteValue;
				effectiveDelimitedTextSpec.CloseQuoteValue = fsConfig.DelimitedTextSpecConfiguration.CloseQuoteValue;

				effectiveDelimitedTextSpec.DelimitedTextFieldSpecs = effectiveDelimitedTextFieldSpecs;
			}

			// refactor this nasty legacy code out into its own method
			this.BrittlePropagation(effectiveDelimitedTextSpec, upstreamFields);

			if (!effectiveDelimitedTextSpec.DelimitedTextFieldSpecs.Any())
				throw new InvalidOperationException(string.Format("Configuration missing: [Source and/or Destination]...{0}.{1}", nameof(DelimitedTextSpec), nameof(DelimitedTextSpec.DelimitedTextFieldSpecs)));

			this.DelimitedTextWriter = new DelimitedTextWriter(new StreamWriter(File.Open(fsConfig.DelimitedTextFilePath, FileMode.Create, FileAccess.Write, FileShare.None)), effectiveDelimitedTextSpec);
		}

		#endregion
	}
}
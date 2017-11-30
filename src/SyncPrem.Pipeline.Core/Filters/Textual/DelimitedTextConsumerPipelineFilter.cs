/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SyncPrem.Infrastructure.Data.Primitives;
using SyncPrem.Infrastructure.Textual.Delimited;
using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configurations;
using SyncPrem.Pipeline.Abstractions.Filters.Consumer;
using SyncPrem.Pipeline.Abstractions.Messages;
using SyncPrem.Pipeline.Core.Configurations.Textual;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Filters.Textual
{
	public class DelimitedTextConsumerPipelineFilter : ConsumerPipelineFilter<DelimitedTextFilterSpecificConfiguration>
	{
		#region Constructors/Destructors

		public DelimitedTextConsumerPipelineFilter()
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
			FilterConfiguration<DelimitedTextFilterSpecificConfiguration> sourceFilterConfiguration;

			if ((object)effectiveDelimitedTextSpec == null)
				throw new ArgumentNullException(nameof(effectiveDelimitedTextSpec));

			if ((object)upstreamFields == null)
				throw new ArgumentNullException(nameof(upstreamFields));

			sourceFilterConfiguration = new FilterConfiguration<DelimitedTextFilterSpecificConfiguration>(((RealPipelineConfiguration)this.FilterConfiguration.Parent).ProducerFilterConfiguration);

			// 2016-07-12 / dpbullington@gmail.com: fix NRE bug in the below check
			// attempt to "flow" the DTM spec from source to destination if not specified on destination
			if ((object)sourceFilterConfiguration.FilterSpecificConfiguration != null &&
				(object)sourceFilterConfiguration.FilterSpecificConfiguration.DelimitedTextSpecConfiguration != null)
			{
				DelimitedTextSpecConfiguration srcDts = sourceFilterConfiguration.FilterSpecificConfiguration.DelimitedTextSpecConfiguration;
				List<IDelimitedTextFieldSpec> effectiveDelimitedTextFieldSpecs;

				effectiveDelimitedTextFieldSpecs = effectiveDelimitedTextSpec.DelimitedTextFieldSpecs.ToList();

				if (effectiveDelimitedTextFieldSpecs.Count <= 0)
				{
					if (upstreamFields.Length <= 0)
						effectiveDelimitedTextFieldSpecs.AddRange(srcDts.DelimitedTextFieldConfigurations);
					else
						effectiveDelimitedTextFieldSpecs.AddRange(upstreamFields.OrderBy(us => us.FieldIndex).Select((io, ix) => new DelimitedTextFieldSpec(ix)
																																{
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

				if (effectiveDelimitedTextSpec.FirstRecordIsHeader == null)
					effectiveDelimitedTextSpec.FirstRecordIsHeader = srcDts.FirstRecordIsHeader;

				if (effectiveDelimitedTextSpec.LastRecordIsFooter == null)
					effectiveDelimitedTextSpec.LastRecordIsFooter = srcDts.LastRecordIsFooter;
			}
		}

		protected override void ConsumeMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration, IPipelineMessage pipelineMessage)
		{
			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)pipelineMessage == null)
				throw new ArgumentNullException(nameof(pipelineMessage));

			this.DelimitedTextWriter.WriteRecords(pipelineMessage.Results.SelectMany(r => r.Records));
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

		protected override void PostProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)this.FilterConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration));

			if ((object)this.FilterConfiguration.FilterSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration.FilterSpecificConfiguration));

			DelimitedTextFilterSpecificConfiguration fsConfig = this.FilterConfiguration.FilterSpecificConfiguration;

			if ((object)this.DelimitedTextWriter != null)
			{
				this.DelimitedTextWriter.Flush();
				this.DelimitedTextWriter.Dispose();
			}

			this.DelimitedTextWriter = null;
		}

		protected override void PreProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			IPipelineMetadata pipelineMetadata;
			IField[] upstreamFields;
			DelimitedTextSpec effectiveDelimitedTextSpec;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)this.FilterConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration));

			if ((object)this.FilterConfiguration.FilterSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration.FilterSpecificConfiguration));

			DelimitedTextFilterSpecificConfiguration fsConfig = this.FilterConfiguration.FilterSpecificConfiguration;

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.DelimitedTextFilePath))
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(fsConfig.DelimitedTextFilePath)));

			pipelineMetadata = pipelineContext.MetadataChain.Peek();
			upstreamFields = pipelineMetadata.UpstreamFields.ToArray();

			effectiveDelimitedTextSpec = new DelimitedTextSpec();

			if (fsConfig.DelimitedTextSpecConfiguration != null)
			{
				List<IDelimitedTextFieldSpec> effectiveDelimitedTextFieldSpecs;

				effectiveDelimitedTextFieldSpecs = fsConfig.DelimitedTextSpecConfiguration.DelimitedTextFieldConfigurations.Cast<IDelimitedTextFieldSpec>().ToList();

				effectiveDelimitedTextSpec.RecordDelimiter = fsConfig.DelimitedTextSpecConfiguration.RecordDelimiter;
				effectiveDelimitedTextSpec.FieldDelimiter = fsConfig.DelimitedTextSpecConfiguration.FieldDelimiter;
				effectiveDelimitedTextSpec.FirstRecordIsHeader = fsConfig.DelimitedTextSpecConfiguration.FirstRecordIsHeader;
				effectiveDelimitedTextSpec.LastRecordIsFooter = fsConfig.DelimitedTextSpecConfiguration.LastRecordIsFooter;
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
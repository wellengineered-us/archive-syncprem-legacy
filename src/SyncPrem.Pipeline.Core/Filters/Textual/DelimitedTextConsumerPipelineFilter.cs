/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;
using System.Linq;

using SyncPrem.Infrastructure.Data.Primitives;
using SyncPrem.Infrastructure.Textual;
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

		private DelimitedTexttWriter delimitedTextWriter;

		#endregion

		#region Properties/Indexers/Events

		private DelimitedTexttWriter DelimitedTexttWriter
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

		protected override void ConsumeMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration, IPipelineMessage pipelineMessage)
		{
			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)pipelineMessage == null)
				throw new ArgumentNullException(nameof(pipelineMessage));

			this.DelimitedTexttWriter.WriteRecords(pipelineMessage.Results.SelectMany(r => r.Records));
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

			if ((object)this.DelimitedTexttWriter != null)
			{
				this.DelimitedTexttWriter.Flush();
				this.DelimitedTexttWriter.Dispose();
			}

			this.DelimitedTexttWriter = null;
		}

		protected override void PreProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			IPipelineMetadata pipelineMetadata;
			Field[] upstreamHeaderSpec;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)this.FilterConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration));

			if ((object)this.FilterConfiguration.FilterSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration.FilterSpecificConfiguration));

			DelimitedTextFilterSpecificConfiguration fsConfig = this.FilterConfiguration.FilterSpecificConfiguration;
			FilterConfiguration<DelimitedTextFilterSpecificConfiguration> sourceFilterConfiguration;
			DelimitedTextSpec effectiveDelimitedTextSpec;

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.DelimitedTextFilePath))
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(fsConfig.DelimitedTextFilePath)));

			pipelineMetadata = pipelineContext.MetadataChain.Peek();
			upstreamHeaderSpec = pipelineMetadata.UpstreamFields.Select(mc => mc.ContextData).OfType<Field>().ToArray();

			sourceFilterConfiguration = new FilterConfiguration<DelimitedTextFilterSpecificConfiguration>(((RealPipelineConfiguration)this.FilterConfiguration.Parent).ProducerFilterConfiguration);

			if (fsConfig.DelimitedTextSpec != null)
				effectiveDelimitedTextSpec = fsConfig.DelimitedTextSpec;
			else
				effectiveDelimitedTextSpec = new DelimitedTextSpec();

			// 2016-07-12 / dpbullington@gmail.com: fix NRE bug in the below check
			// attempt to "flow" the DTM spec from source to destination if not specified on destination
			if ((object)sourceFilterConfiguration != null &&
				(object)sourceFilterConfiguration.FilterSpecificConfiguration != null &&
				(object)sourceFilterConfiguration.FilterSpecificConfiguration.DelimitedTextSpec != null)
			{
				DelimitedTextSpec srcDts = sourceFilterConfiguration.FilterSpecificConfiguration.DelimitedTextSpec;

				if (effectiveDelimitedTextSpec.HeaderSpecs.Count <= 0)
				{
					if ((object)upstreamHeaderSpec != null &&
						upstreamHeaderSpec.Length <= 0)
						effectiveDelimitedTextSpec.HeaderSpecs.AddRange(srcDts.HeaderSpecs);
					else
						effectiveDelimitedTextSpec.HeaderSpecs.AddRange(upstreamHeaderSpec);
				}

				if (effectiveDelimitedTextSpec.FieldDelimiter == null)
					effectiveDelimitedTextSpec.FieldDelimiter = srcDts.FieldDelimiter;

				if (effectiveDelimitedTextSpec.QuoteValue == null)
					effectiveDelimitedTextSpec.QuoteValue = srcDts.QuoteValue;

				if (effectiveDelimitedTextSpec.RecordDelimiter == null)
					effectiveDelimitedTextSpec.FieldDelimiter = srcDts.RecordDelimiter;

				if (effectiveDelimitedTextSpec.FirstRecordIsHeader == null)
					effectiveDelimitedTextSpec.FirstRecordIsHeader = srcDts.FirstRecordIsHeader;
			}

			if ((object)effectiveDelimitedTextSpec == null ||
				effectiveDelimitedTextSpec.HeaderSpecs.Count <= 0)
				throw new InvalidOperationException(string.Format("Configuration missing: [Source and/or Destination]...{0}.{1}", nameof(DelimitedTextSpec), nameof(DelimitedTextSpec.HeaderSpecs)));

			this.DelimitedTexttWriter = new DelimitedTexttWriter(new StreamWriter(File.Open(fsConfig.DelimitedTextFilePath, FileMode.Create, FileAccess.Write, FileShare.None)), effectiveDelimitedTextSpec);
		}

		#endregion
	}
}
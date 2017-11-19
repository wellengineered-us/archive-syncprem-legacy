/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SyncPrem.Infrastructure.Data.Primitives;
using SyncPrem.Infrastructure.Textual;
using SyncPrem.Infrastructure.Textual.Delimited;
using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configurations;
using SyncPrem.Pipeline.Abstractions.Filters.Producer;
using SyncPrem.Pipeline.Abstractions.Messages;
using SyncPrem.Pipeline.Core.Configurations.Textual;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Filters.Textual
{
	public class DelimitedTextProducerPipelineFilter : ProducerPipelineFilter<DelimitedTextFilterSpecificConfiguration>
	{
		#region Constructors/Destructors

		public DelimitedTextProducerPipelineFilter()
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

			if ((object)this.DelimitedTextReader != null)
				this.DelimitedTextReader.Dispose();

			this.DelimitedTextReader = null;
		}

		protected override void PreProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			IEnumerable<IField> headers;
			IPipelineMetadata pipelineMetadata;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)this.FilterConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration));

			if ((object)this.FilterConfiguration.FilterSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration.FilterSpecificConfiguration));

			DelimitedTextFilterSpecificConfiguration fsConfig = this.FilterConfiguration.FilterSpecificConfiguration;

			if ((object)fsConfig.DelimitedTextSpec == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(fsConfig.DelimitedTextSpec)));

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.DelimitedTextFilePath))
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(fsConfig.DelimitedTextFilePath)));

			this.DelimitedTextReader = new DelimitedTextReader(new StreamReader(File.Open(fsConfig.DelimitedTextFilePath, FileMode.Open, FileAccess.Read, FileShare.None)), fsConfig.DelimitedTextSpec);

			headers = this.DelimitedTextReader.ReadHeaders();

			if ((object)headers == null)
				throw new InvalidOperationException(string.Format("Headers were invalid."));

			pipelineMetadata = pipelineContext.CreateMetadata(headers);
			pipelineContext.MetadataChain.Push(pipelineMetadata);
		}

		protected override IPipelineMessage ProduceMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			IPipelineMessage pipelineMessage;

			IEnumerable<IResult> sourceDataEnumerable;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)this.FilterConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration));

			if ((object)this.FilterConfiguration.FilterSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration.FilterSpecificConfiguration));

			DelimitedTextFilterSpecificConfiguration fsConfig = this.FilterConfiguration.FilterSpecificConfiguration;

			sourceDataEnumerable = this.DelimitedTextReader.ReadResults();

			if ((object)sourceDataEnumerable == null)
				throw new InvalidOperationException(string.Format("Records were invalid."));

			pipelineMessage = pipelineContext.CreateMessage(sourceDataEnumerable);

			return pipelineMessage;
		}

		#endregion
	}
}
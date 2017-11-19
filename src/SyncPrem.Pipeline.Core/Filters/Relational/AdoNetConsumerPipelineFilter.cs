/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using SyncPrem.Infrastructure.Data.AdoNet.UoW;
using SyncPrem.Infrastructure.Data.Primitives;
using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configurations;
using SyncPrem.Pipeline.Abstractions.Filters.Consumer;
using SyncPrem.Pipeline.Abstractions.Messages;
using SyncPrem.Pipeline.Core.Configurations.Relational;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Filters.Relational
{
	public abstract class AdoNetConsumerPipelineFilter : ConsumerPipelineFilter<AdoNetFilterSpecificConfiguration>
	{
		#region Constructors/Destructors

		public AdoNetConsumerPipelineFilter()
		{
		}

		#endregion

		#region Fields/Constants

		private IUnitOfWork destinationUnitOfWork;

		#endregion

		#region Properties/Indexers/Events

		protected IUnitOfWork DestinationUnitOfWork
		{
			get
			{
				return this.destinationUnitOfWork;
			}
			private set
			{
				this.destinationUnitOfWork = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void ConsumeMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration, IPipelineMessage pipelineMessage)
		{
			IPipelineMetadata pipelineMetadata;
			DbDataReader sourceDataReader;
			long rowsCopied;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)pipelineMessage == null)
				throw new ArgumentNullException(nameof(pipelineMessage));

			pipelineMetadata = pipelineContext.MetadataChain.Peek();
			sourceDataReader = new ResultRecordDataReader(pipelineMetadata.UpstreamFields, pipelineMessage.Results);

			try
			{
				this.ConsumeMessageReader(pipelineContext, tableConfiguration, sourceDataReader, out rowsCopied);
			}
			finally
			{
				// just in case ConsumeMessageReader did not or could not enumerate to completion for disposal...
				while (sourceDataReader.NextResult())
					;
			}
		}

		protected abstract void ConsumeMessageReader(IPipelineContext pipelineContext, TableConfiguration tableConfiguration, DbDataReader sourceDataReader, out long rowsCopied);

		protected override void PostProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			IEnumerable<IResult> results;
			IEnumerable<DbParameter> dbParameters;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)this.FilterConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration));

			if ((object)this.FilterConfiguration.FilterSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration.FilterSpecificConfiguration));

			AdoNetFilterSpecificConfiguration fsConfig = this.FilterConfiguration.FilterSpecificConfiguration;

			if (fsConfig.PostExecuteCommand != null &&
				!SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.PostExecuteCommand.CommandText))
			{
				dbParameters = fsConfig.PostExecuteCommand.GetDbDataParameters(this.DestinationUnitOfWork);

				results = this.DestinationUnitOfWork.ExecuteSchemaResults(fsConfig.PostExecuteCommand.CommandType ?? CommandType.Text, fsConfig.PostExecuteCommand.CommandText, dbParameters);

				if ((object)results == null)
					throw new InvalidOperationException(string.Format("Results were invalid."));

				results.ToArray(); // force execution
			}

			if ((object)this.DestinationUnitOfWork != null)
				this.DestinationUnitOfWork.Dispose();

			this.DestinationUnitOfWork = null;
		}

		protected override void PreProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			IEnumerable<IResult> results;
			IEnumerable<DbParameter> dbParameters;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)this.FilterConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration));

			if ((object)this.FilterConfiguration.FilterSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration.FilterSpecificConfiguration));

			AdoNetFilterSpecificConfiguration fsConfig = this.FilterConfiguration.FilterSpecificConfiguration;

			this.DestinationUnitOfWork = fsConfig.GetUnitOfWork();

			if (fsConfig.PreExecuteCommand != null &&
				!SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.PreExecuteCommand.CommandText))
			{
				dbParameters = fsConfig.PreExecuteCommand.GetDbDataParameters(this.DestinationUnitOfWork);

				results = this.DestinationUnitOfWork.ExecuteResults(fsConfig.PreExecuteCommand.CommandType ?? CommandType.Text, fsConfig.PreExecuteCommand.CommandText, dbParameters);

				if ((object)results == null)
					throw new InvalidOperationException(string.Format("Results were invalid."));

				results.ToArray(); // force execution
			}
		}

		#endregion
	}
}
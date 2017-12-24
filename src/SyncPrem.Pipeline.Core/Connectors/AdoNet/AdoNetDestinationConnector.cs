/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Payload;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Destination;
using SyncPrem.Pipeline.Core.Configurations.AdoNet;
using SyncPrem.StreamingIO.AdoNet;
using SyncPrem.StreamingIO.AdoNet.UoW;

using IField = SyncPrem.StreamingIO.Primitives.IField;
using Field = SyncPrem.StreamingIO.Primitives.Field;

using __IRecord = System.Collections.Generic.IDictionary<string, object>;
using __Record = System.Collections.Generic.Dictionary<string, object>;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Connectors.AdoNet
{
	public abstract class AdoNetDestinationConnector : DestinationConnector<AdoNetConnectorSpecificConfiguration>
	{
		#region Constructors/Destructors

		public AdoNetDestinationConnector()
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

		protected abstract void ConsumeMessageReader(IContext context, RecordConfiguration recordConfiguration, DbDataReader sourceDataReader, out long rowsCopied);

		protected override void ConsumeRecord(IContext context, RecordConfiguration recordConfiguration, IPipelineMessage pipelineMessage)
		{
			IPipelineMetadata pipelineMetadata;
			DbDataReader sourceDataReader;
			long rowsCopied;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			if ((object)pipelineMessage == null)
				throw new ArgumentNullException(nameof(pipelineMessage));

			pipelineMetadata = context.MetadataChain.Peek();
			sourceDataReader = new ResultRecordDataReader(pipelineMetadata.UpstreamFields, pipelineMessage.Records);

			try
			{
				this.ConsumeMessageReader(context, recordConfiguration, sourceDataReader, out rowsCopied);
			}
			finally
			{
				// just in case ConsumeMessageReader did not or could not enumerate to completion for disposal...
				while (sourceDataReader.NextResult())
					;
			}
		}

		protected override void PostExecuteRecord(IContext context, RecordConfiguration recordConfiguration)
		{
			IEnumerable<IAdoNetResult> results;
			IEnumerable<DbParameter> dbParameters;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			if ((object)this.StageConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration));

			if ((object)this.StageConfiguration.StageSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration.StageSpecificConfiguration));

			AdoNetConnectorSpecificConfiguration fsConfig = this.StageConfiguration.StageSpecificConfiguration;

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

		protected override void PreExecuteRecord(IContext context, RecordConfiguration recordConfiguration)
		{
			IEnumerable<IAdoNetResult> results;
			IEnumerable<DbParameter> dbParameters;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			if ((object)this.StageConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration));

			if ((object)this.StageConfiguration.StageSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration.StageSpecificConfiguration));

			AdoNetConnectorSpecificConfiguration fsConfig = this.StageConfiguration.StageSpecificConfiguration;

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
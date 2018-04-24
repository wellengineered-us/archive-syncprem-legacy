/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;
using SyncPrem.Pipeline.Abstractions.Sync.Stage.Connector.Destination;
using SyncPrem.Pipeline.Core.Configurations;
using SyncPrem.StreamingIO.Primitives;
using SyncPrem.StreamingIO.ProxyWrappers;
using SyncPrem.StreamingIO.Relational;
using SyncPrem.StreamingIO.Relational.UoW;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Sync.Connectors
{
	public abstract class AdoNetDestinationConnector : SyncDestinationConnector<AdoNetConnectorSpecificConfiguration>
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

		protected override void ConsumeInternal(ISyncContext context, RecordConfiguration configuration, ISyncChannel channel)
		{
			ISchema schema = null;
			IEnumerable<ISyncRecord> records;

			DbDataReader sourceDataReader;
			long rowsCopied;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)channel == null)
				throw new ArgumentNullException(nameof(channel));

			this.AssertValidConfiguration();

			records = channel.Records;

			if ((object)records == null)
				throw new SyncPremException(nameof(records));

			sourceDataReader = null; //new AdoNetStreamingPayloadDataReader(schema.Fields.Values, records.Select(r => r.Payload));

			try
			{
				this.ConsumeMessageReader(context, configuration, sourceDataReader);
			}
			finally
			{
				// just in case ConsumeMessageReader did not or could not enumerate to completion for disposal...
				do
					while (sourceDataReader.Read())
						;
				while (sourceDataReader.NextResult())
					;
			}
		}

		protected abstract void ConsumeMessageReader(ISyncContext context, RecordConfiguration configuration, DbDataReader sourceDataReader);

		protected override void PostExecuteInternal(ISyncContext context, RecordConfiguration configuration)
		{
			IEnumerable<IAdoNetStreamingResult> results;
			IEnumerable<DbParameter> dbParameters;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			AdoNetConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			if (fsConfig.PostExecuteCommand != null &&
				!SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.PostExecuteCommand.CommandText))
			{
				dbParameters = fsConfig.PostExecuteCommand.GetDbDataParameters(this.DestinationUnitOfWork);

				results = this.DestinationUnitOfWork.ExecuteSchemaResults(fsConfig.PostExecuteCommand.CommandType ?? CommandType.Text, fsConfig.PostExecuteCommand.CommandText, dbParameters);

				if ((object)results == null)
					throw new SyncPremException(nameof(results));

				results.ForceEnumeration(); // force execution
			}

			if ((object)this.DestinationUnitOfWork != null)
				this.DestinationUnitOfWork.Dispose();

			this.DestinationUnitOfWork = null;
		}

		protected override void PreExecuteInternal(ISyncContext context, RecordConfiguration configuration)
		{
			IEnumerable<IAdoNetStreamingResult> results;
			IEnumerable<DbParameter> dbParameters;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			AdoNetConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			this.DestinationUnitOfWork = fsConfig.GetUnitOfWork();

			if (fsConfig.PreExecuteCommand != null &&
				!SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.PreExecuteCommand.CommandText))
			{
				dbParameters = fsConfig.PreExecuteCommand.GetDbDataParameters(this.DestinationUnitOfWork);

				results = this.DestinationUnitOfWork.ExecuteResults(fsConfig.PreExecuteCommand.CommandType ?? CommandType.Text, fsConfig.PreExecuteCommand.CommandText, dbParameters);

				if ((object)results == null)
					throw new SyncPremException(nameof(results));

				results.ForceEnumeration(); // force execution
			}
		}

		#endregion
	}
}
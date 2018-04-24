/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Async.Stage.Connector.Destination;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Core.Configurations;
using SyncPrem.StreamingIO.Primitives;
using SyncPrem.StreamingIO.ProxyWrappers;
using SyncPrem.StreamingIO.Relational;
using SyncPrem.StreamingIO.Relational.UoW;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Async.Connectors
{
	public abstract class AdoNetDestinationConnector : AsyncDestinationConnector<AdoNetConnectorSpecificConfiguration>
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

		protected override async Task ConsumeAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, IAsyncChannel asyncChannel, CancellationToken cancellationToken)
		{
			ISchema schema = null;
			IAsyncEnumerable<IRecord> records;

			DbDataReader sourceDataReader;
			long rowsCopied;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)asyncChannel == null)
				throw new ArgumentNullException(nameof(asyncChannel));

			this.AssertValidConfiguration();

			records = asyncChannel.Records;

			if ((object)records == null)
				throw new SyncPremException(nameof(records));

			/*IAsyncEnumerable<IAsyncGrouping<ISchema, IRecord>> groups;

			groups = records.GroupBy(r => r.Schema);

			foreach (IAsyncGrouping<ISchema, IRecord> grouping in groups)
			{
				foreach (IRecord record in grouping)
				{
				}
			}*/

			sourceDataReader = null; //new AdoNetStreamingPayloadDataReaderAsync(schema.Fields.Values, records.Select(r => r.Payload));

			try
			{
				await this.ConsumeMessageReaderAsync(asyncContext, configuration, sourceDataReader, cancellationToken);
			}
			finally
			{
				// just in case ConsumeMessageReaderAsync did not or could not enumerate to completion for disposal...
				do
					while (await sourceDataReader.ReadAsync(cancellationToken))
						;
				while (await sourceDataReader.NextResultAsync(cancellationToken))
					;
			}
		}

		protected abstract Task ConsumeMessageReaderAsync(IAsyncContext asyncContext, RecordConfiguration configuration, DbDataReader sourceDataReader, CancellationToken cancellationToken);

		protected override async Task CreateAsync(bool creating, CancellationToken cancellationToken)
		{
			// do nothing
			await base.CreateAsync(creating, cancellationToken);
		}

		protected override async Task DisposeAsync(bool disposing, CancellationToken cancellationToken)
		{
			// do nothing
			await base.DisposeAsync(disposing, cancellationToken);
		}

		protected override async Task PostExecuteAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			IAsyncEnumerable<IAdoNetStreamingResult> results;
			IEnumerable<DbParameter> dbParameters;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			AdoNetConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			if (fsConfig.PostExecuteCommand != null &&
				!SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.PostExecuteCommand.CommandText))
			{
				dbParameters = fsConfig.PostExecuteCommand.GetDbDataParameters(this.DestinationUnitOfWork);

				results = this.DestinationUnitOfWork.ExecuteSchemaResultsAsync(fsConfig.PostExecuteCommand.CommandType ?? CommandType.Text, fsConfig.PostExecuteCommand.CommandText, dbParameters, cancellationToken);

				if ((object)results == null)
					throw new SyncPremException(nameof(results));

				await results.ForceAsyncEnumeration(cancellationToken); // force execution
			}

			if ((object)this.DestinationUnitOfWork != null)
				this.DestinationUnitOfWork.Dispose();

			this.DestinationUnitOfWork = null;
		}

		protected override async Task PreExecuteAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			IAsyncEnumerable<IAdoNetStreamingResult> results;
			IEnumerable<DbParameter> dbParameters;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			AdoNetConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			this.DestinationUnitOfWork = fsConfig.GetUnitOfWork();

			if (fsConfig.PreExecuteCommand != null &&
				!SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.PreExecuteCommand.CommandText))
			{
				dbParameters = fsConfig.PreExecuteCommand.GetDbDataParameters(this.DestinationUnitOfWork);

				results = this.DestinationUnitOfWork.ExecuteResultsAsync(fsConfig.PreExecuteCommand.CommandType ?? CommandType.Text, fsConfig.PreExecuteCommand.CommandText, dbParameters, cancellationToken);

				if ((object)results == null)
					throw new SyncPremException(nameof(results));

				await results.ForceAsyncEnumeration(cancellationToken); // force execution
			}
		}

		#endregion
	}
}
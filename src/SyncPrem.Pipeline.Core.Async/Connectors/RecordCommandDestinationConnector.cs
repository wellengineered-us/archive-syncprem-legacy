/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Core.Configurations;
using SyncPrem.StreamingIO.ProxyWrappers;
using SyncPrem.StreamingIO.Relational;
using SyncPrem.StreamingIO.Relational.UoW;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Async.Connectors
{
	public class RecordCommandDestinationConnector : AdoNetDestinationConnector
	{
		#region Constructors/Destructors

		public RecordCommandDestinationConnector()
		{
		}

		#endregion

		#region Methods/Operators

		protected override async Task ConsumeMessageReaderAsync(IAsyncContext asyncContext, RecordConfiguration configuration, DbDataReader sourceDataReader, CancellationToken cancellationToken)
		{
			IAsyncEnumerable<IAdoNetStreamingResult> results;
			IEnumerable<DbParameter> dbParameters;
			long recordCount = 0;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)sourceDataReader == null)
				throw new ArgumentNullException(nameof(sourceDataReader));

			this.AssertValidConfiguration();

			AdoNetConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			if ((object)fsConfig.ExecuteCommand == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(fsConfig.ExecuteCommand)));

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.ExecuteCommand.CommandText))
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}.{1}'.", nameof(fsConfig.ExecuteCommand), nameof(fsConfig.ExecuteCommand.CommandText)));

			do
			{
				while (await sourceDataReader.ReadAsync(cancellationToken))
				{
					dbParameters = fsConfig.ExecuteCommand.GetDbDataParameters(this.DestinationUnitOfWork);

					dbParameters = dbParameters.Select(p =>
														{
															// prevent modified closure bug
															DbDataReader _sourceDataReader = sourceDataReader;
															// lazy load
															p.Value = _sourceDataReader[p.SourceColumn];
															return p;
														});

					results = this.DestinationUnitOfWork.ExecuteResultsAsync(fsConfig.ExecuteCommand.CommandType ?? CommandType.Text, fsConfig.ExecuteCommand.CommandText, dbParameters, cancellationToken);

					await results.ForceAsyncEnumeration(cancellationToken); // force execution

					recordCount++;
				}
			}
			while (await sourceDataReader.NextResultAsync(cancellationToken));

			//System.Console.WriteLine("DESTINATION (update): recordCount={0}", recordCount);
		}

		#endregion
	}
}
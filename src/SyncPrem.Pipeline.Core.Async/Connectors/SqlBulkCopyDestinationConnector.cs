/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Core.Configurations;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Async.Connectors
{
	public class SqlBulkCopyDestinationConnector : AdoNetDestinationConnector
	{
		#region Constructors/Destructors

		public SqlBulkCopyDestinationConnector()
		{
		}

		#endregion

		#region Methods/Operators

		protected override async Task ConsumeMessageReaderAsync(IAsyncContext asyncContext, RecordConfiguration configuration, DbDataReader sourceDataReader, CancellationToken cancellationToken)
		{
			long recordCount = 0;
			//SqlRowsCopiedEventHandler callback;

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

			using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy((SqlConnection)this.DestinationUnitOfWork.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)this.DestinationUnitOfWork.Transaction))
			{
				//callback = (sender, e) => Console.WriteLine(_rowsCopied = e.RowsCopied);

				foreach (FieldConfiguration columnConfiguration in configuration.ColumnConfigurations)
					sqlBulkCopy.ColumnMappings.Add(columnConfiguration.FieldName, columnConfiguration.FieldName);

				sqlBulkCopy.EnableStreaming = true;
				sqlBulkCopy.BatchSize = 2500;
				//sqlBulkCopy.NotifyAfter = 2500;
				//sqlBulkCopy.SqlRowsCopied += callback;
				sqlBulkCopy.DestinationTableName = fsConfig.ExecuteCommand.CommandText;

				await sqlBulkCopy.WriteToServerAsync(sourceDataReader, cancellationToken);

				//sqlBulkCopy.SqlRowsCopied -= callback;
			}
		}

		#endregion
	}
}
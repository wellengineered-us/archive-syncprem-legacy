/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Data.Common;
using System.Data.SqlClient;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Core.Configurations.AdoNet;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Connectors.AdoNet
{
	public class SqlBulkCopyDestinationConnector : AdoNetDestinationConnector
	{
		#region Constructors/Destructors

		public SqlBulkCopyDestinationConnector()
		{
		}

		#endregion

		#region Methods/Operators

		protected override void ConsumeMessageReader(IContext context, RecordConfiguration configuration, DbDataReader sourceDataReader, out long rowsCopied)
		{
			long _rowsCopied = 0;
			//SqlRowsCopiedEventHandler callback;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)sourceDataReader == null)
				throw new ArgumentNullException(nameof(sourceDataReader));

			if ((object)this.StageConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration));

			if ((object)this.StageConfiguration.StageSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration.StageSpecificConfiguration));

			AdoNetConnectorSpecificConfiguration fsConfig = this.StageConfiguration.StageSpecificConfiguration;

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
				sqlBulkCopy.NotifyAfter = 2500;
				//sqlBulkCopy.SqlRowsCopied += callback;
				sqlBulkCopy.DestinationTableName = fsConfig.ExecuteCommand.CommandText;

				sqlBulkCopy.WriteToServer(sourceDataReader);

				//sqlBulkCopy.SqlRowsCopied -= callback;
			}

			rowsCopied = _rowsCopied;
		}

		#endregion
	}
}
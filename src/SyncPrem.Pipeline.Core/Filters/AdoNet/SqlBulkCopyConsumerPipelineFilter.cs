/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Data.Common;
using System.Data.SqlClient;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configurations;
using SyncPrem.Pipeline.Core.Configurations.AdoNet;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Filters.AdoNet
{
	public class SqlBulkCopyConsumerPipelineFilter : AdoNetConsumerPipelineFilter
	{
		#region Constructors/Destructors

		public SqlBulkCopyConsumerPipelineFilter()
		{
		}

		#endregion

		#region Methods/Operators

		protected override void ConsumeMessageReader(IPipelineContext pipelineContext, TableConfiguration tableConfiguration, DbDataReader sourceDataReader, out long rowsCopied)
		{
			long _rowsCopied = 0;
			//SqlRowsCopiedEventHandler callback;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)sourceDataReader == null)
				throw new ArgumentNullException(nameof(sourceDataReader));

			if ((object)this.FilterConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration));

			if ((object)this.FilterConfiguration.FilterSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration.FilterSpecificConfiguration));

			AdoNetFilterSpecificConfiguration fsConfig = this.FilterConfiguration.FilterSpecificConfiguration;

			if ((object)fsConfig.ExecuteCommand == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(fsConfig.ExecuteCommand)));

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.ExecuteCommand.CommandText))
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}.{1}'.", nameof(fsConfig.ExecuteCommand), nameof(fsConfig.ExecuteCommand.CommandText)));

			using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy((SqlConnection)this.DestinationUnitOfWork.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)this.DestinationUnitOfWork.Transaction))
			{
				//callback = (sender, e) => Console.WriteLine(_rowsCopied = e.RowsCopied);

				foreach (ColumnConfiguration columnConfiguration in tableConfiguration.ColumnConfigurations)
					sqlBulkCopy.ColumnMappings.Add(columnConfiguration.ColumnName, columnConfiguration.ColumnName);

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
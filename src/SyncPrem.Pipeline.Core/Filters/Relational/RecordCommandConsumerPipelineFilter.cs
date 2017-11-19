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
using SyncPrem.Pipeline.Core.Configurations.Relational;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Filters.Relational
{
	public class RecordCommandConsumerPipelineFilter : AdoNetConsumerPipelineFilter
	{
		#region Constructors/Destructors

		public RecordCommandConsumerPipelineFilter()
		{
		}

		#endregion

		#region Methods/Operators

		protected override void ConsumeMessageReader(IPipelineContext pipelineContext, TableConfiguration tableConfiguration, DbDataReader sourceDataReader, out long rowsCopied)
		{
			IEnumerable<IResult> results;
			IEnumerable<DbParameter> dbParameters;
			long _rowsCopied = 0;

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

			do
			{
				while (sourceDataReader.Read())
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

					results = this.DestinationUnitOfWork.ExecuteResults(fsConfig.ExecuteCommand.CommandType ?? CommandType.Text, fsConfig.ExecuteCommand.CommandText, dbParameters);

					results.ToArray(); // force execution

					_rowsCopied++;
				}
			}
			while (sourceDataReader.NextResult());

			rowsCopied = _rowsCopied;

			//System.Console.WriteLine("DESTINATION (update): rowsCopied={0}", rowsCopied);
		}

		#endregion
	}
}
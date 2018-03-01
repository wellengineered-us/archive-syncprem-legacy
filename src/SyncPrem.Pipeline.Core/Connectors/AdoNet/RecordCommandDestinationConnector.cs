﻿/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Core.Configurations.AdoNet;
using SyncPrem.StreamingIO.ProxyWrappers;
using SyncPrem.StreamingIO.Relational;
using SyncPrem.StreamingIO.Relational.UoW;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Connectors.AdoNet
{
	public class RecordCommandDestinationConnector : AdoNetDestinationConnector
	{
		#region Constructors/Destructors

		public RecordCommandDestinationConnector()
		{
		}

		#endregion

		#region Methods/Operators

		protected override void ConsumeMessageReader(IContext context, RecordConfiguration configuration, DbDataReader sourceDataReader, out long rowsCopied)
		{
			IEnumerable<IAdoNetStreamingResult> results;
			IEnumerable<DbParameter> dbParameters;
			long _rowsCopied = 0;

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

					results.ForceEnumeration(); // force execution

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
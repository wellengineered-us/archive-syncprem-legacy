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
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Source;
using SyncPrem.Pipeline.Core.Configurations.AdoNet;
using SyncPrem.StreamingIO.AdoNet;
using SyncPrem.StreamingIO.AdoNet.UoW;

using IField = SyncPrem.StreamingIO.Primitives.IField;
using Field = SyncPrem.StreamingIO.Primitives.Field;

using __IRecord = System.Collections.Generic.IDictionary<string, object>;
using __Record = System.Collections.Generic.Dictionary<string, object>;

using TextMetal.Middleware.Solder.Extensions;
using TextMetal.Middleware.Solder.Utilities;

namespace SyncPrem.Pipeline.Core.Connectors.AdoNet
{
	public class AdoNetSourceConnector : SourceConnector<AdoNetConnectorSpecificConfiguration>
	{
		#region Constructors/Destructors

		public AdoNetSourceConnector()
		{
		}

		#endregion

		#region Fields/Constants

		private IUnitOfWork sourceUnitOfWork;

		#endregion

		#region Properties/Indexers/Events

		protected IUnitOfWork SourceUnitOfWork
		{
			get
			{
				return this.sourceUnitOfWork;
			}
			private set
			{
				this.sourceUnitOfWork = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void Create(bool creating)
		{
			// do nothing
			base.Create(creating);
		}

		protected override void Dispose(bool disposing)
		{
			// do nothing
			base.Dispose(disposing);
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
				dbParameters = fsConfig.PostExecuteCommand.GetDbDataParameters(this.SourceUnitOfWork);

				results = this.SourceUnitOfWork.ExecuteSchemaResults(fsConfig.PostExecuteCommand.CommandType ?? CommandType.Text,
					fsConfig.PostExecuteCommand.CommandText,
					dbParameters);

				if ((object)results == null)
					throw new InvalidOperationException(string.Format("Results were invalid."));

				results.ToArray(); // force execution
			}

			if ((object)this.SourceUnitOfWork != null)
				this.SourceUnitOfWork.Dispose();

			this.SourceUnitOfWork = null;
		}

		protected override void PreExecuteRecord(IContext context, RecordConfiguration recordConfiguration)
		{
			IEnumerable<IAdoNetResult> results;
			IEnumerable<__IRecord> records;
			IEnumerable<DbParameter> dbParameters;

			IList<Field> fields;
			IPipelineMetadata pipelineMetadata;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			if ((object)this.StageConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration));

			if ((object)this.StageConfiguration.StageSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.StageConfiguration.StageSpecificConfiguration));

			AdoNetConnectorSpecificConfiguration fsConfig = this.StageConfiguration.StageSpecificConfiguration;

			fields = new List<Field>();
			this.SourceUnitOfWork = fsConfig.GetUnitOfWork();

			if (fsConfig.PreExecuteCommand != null &&
				!SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.PreExecuteCommand.CommandText))
			{
				dbParameters = fsConfig.PreExecuteCommand.GetDbDataParameters(this.SourceUnitOfWork);

				records = this.SourceUnitOfWork.ExecuteRecords(fsConfig.PreExecuteCommand.CommandType ?? CommandType.Text,
					fsConfig.PreExecuteCommand.CommandText,
					dbParameters, null);

				if ((object)records == null)
					throw new InvalidOperationException(string.Format("Records were invalid."));

				records.ToArray();
			}

			if (fsConfig.ExecuteCommand != null ||
				!SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.ExecuteCommand.CommandText))
			{
				dbParameters = fsConfig.ExecuteCommand.GetDbDataParameters(this.SourceUnitOfWork);

				results = this.SourceUnitOfWork.ExecuteSchemaResults(fsConfig.ExecuteCommand.CommandType ?? CommandType.Text,
					fsConfig.ExecuteCommand.CommandText,
					dbParameters);

				if ((object)results == null)
					throw new InvalidOperationException(string.Format("Results were invalid."));

				foreach (IAdoNetResult result in results)
				{
					records = result.Records;

					if ((object)records == null)
						throw new InvalidOperationException(string.Format("Records were invalid."));

					long fieldIndex = 0;
					foreach (__IRecord record in records)
					{
						fields.Add(new Field()
									{
										FieldName = (string)record[nameof(DbColumn.ColumnName)],
										FieldType = (Type)record[nameof(DbColumn.DataType)],
										IsFieldOptional = (bool?)record[nameof(DbColumn.AllowDBNull)] ?? true,
										FieldIndex = fieldIndex++
						});
					}
				}
			}

			pipelineMetadata = context.CreateMetadata(fields);
			context.MetadataChain.Push(pipelineMetadata);
		}

		protected override IPipelineMessage ProduceRecord(IContext context, RecordConfiguration recordConfiguration)
		{
			IPipelineMessage pipelineMessage;
			IEnumerable<IAdoNetResult> sourceDataEnumerable;
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

			if ((object)fsConfig.ExecuteCommand == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(fsConfig.ExecuteCommand)));

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.ExecuteCommand.CommandText))
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}.{1}'.", nameof(fsConfig.ExecuteCommand), nameof(fsConfig.ExecuteCommand.CommandText)));

			dbParameters = fsConfig.ExecuteCommand.GetDbDataParameters(this.SourceUnitOfWork);

			sourceDataEnumerable = this.SourceUnitOfWork.ExecuteResults(fsConfig.ExecuteCommand.CommandType ?? CommandType.Text, fsConfig.ExecuteCommand.CommandText, dbParameters);

			if ((object)sourceDataEnumerable == null)
				throw new InvalidOperationException(string.Format("Results were invalid."));

			pipelineMessage = context.CreateMessage(null /* sourceDataEnumerable */);

			return pipelineMessage;
		}

		#endregion
	}
}
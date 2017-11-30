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
using SyncPrem.Pipeline.Abstractions.Filters.Producer;
using SyncPrem.Pipeline.Abstractions.Messages;
using SyncPrem.Pipeline.Core.Configurations.AdoNet;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Filters.AdoNet
{
	public class AdoNetProducerPipelineFilter : ProducerPipelineFilter<AdoNetFilterSpecificConfiguration>
	{
		#region Constructors/Destructors

		public AdoNetProducerPipelineFilter()
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

		protected override void PostProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			IEnumerable<IResult> results;
			IEnumerable<DbParameter> dbParameters;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)this.FilterConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration));

			if ((object)this.FilterConfiguration.FilterSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration.FilterSpecificConfiguration));

			AdoNetFilterSpecificConfiguration fsConfig = this.FilterConfiguration.FilterSpecificConfiguration;

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

		protected override void PreProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			IEnumerable<IResult> results;
			IEnumerable<IRecord> records;
			IEnumerable<DbParameter> dbParameters;

			IList<Field> fields;
			IPipelineMetadata pipelineMetadata;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)this.FilterConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration));

			if ((object)this.FilterConfiguration.FilterSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration.FilterSpecificConfiguration));

			AdoNetFilterSpecificConfiguration fsConfig = this.FilterConfiguration.FilterSpecificConfiguration;

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

				foreach (IResult result in results)
				{
					records = result.Records;

					if ((object)records == null)
						throw new InvalidOperationException(string.Format("Records were invalid."));

					int i = 0;
					foreach (IRecord record in records)
					{
						DbColumn dbColumn = (DbColumn)record.ContextData;

						fields.Add(new Field(i++)
									{
										FieldName = dbColumn.ColumnName,
										FieldType = dbColumn.DataType,
										IsFieldOptional = dbColumn.AllowDBNull ?? true,
										ContextData = record
									});
					}
				}
			}

			pipelineMetadata = pipelineContext.CreateMetadata(fields);
			pipelineContext.MetadataChain.Push(pipelineMetadata);
		}

		protected override IPipelineMessage ProduceMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			IPipelineMessage pipelineMessage;
			IEnumerable<IResult> sourceDataEnumerable;
			IEnumerable<DbParameter> dbParameters;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)this.FilterConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration));

			if ((object)this.FilterConfiguration.FilterSpecificConfiguration == null)
				throw new InvalidOperationException(nameof(this.FilterConfiguration.FilterSpecificConfiguration));

			AdoNetFilterSpecificConfiguration fsConfig = this.FilterConfiguration.FilterSpecificConfiguration;

			if ((object)fsConfig.ExecuteCommand == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(fsConfig.ExecuteCommand)));

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.ExecuteCommand.CommandText))
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}.{1}'.", nameof(fsConfig.ExecuteCommand), nameof(fsConfig.ExecuteCommand.CommandText)));

			dbParameters = fsConfig.ExecuteCommand.GetDbDataParameters(this.SourceUnitOfWork);

			sourceDataEnumerable = this.SourceUnitOfWork.ExecuteResults(fsConfig.ExecuteCommand.CommandType ?? CommandType.Text, fsConfig.ExecuteCommand.CommandText, dbParameters);

			if ((object)sourceDataEnumerable == null)
				throw new InvalidOperationException(string.Format("Results were invalid."));

			pipelineMessage = pipelineContext.CreateMessage(sourceDataEnumerable);

			return pipelineMessage;
		}

		#endregion
	}
}
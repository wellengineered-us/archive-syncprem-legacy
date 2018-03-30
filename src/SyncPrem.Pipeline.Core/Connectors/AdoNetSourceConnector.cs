/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Source;
using SyncPrem.Pipeline.Core.Configurations;
using SyncPrem.Pipeline.Core.Runtime;
using SyncPrem.StreamingIO.Primitives;
using SyncPrem.StreamingIO.ProxyWrappers;
using SyncPrem.StreamingIO.Relational;
using SyncPrem.StreamingIO.Relational.UoW;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Connectors
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

		private IEnumerable<IChannel> GetMultiplexedChannels(IContext context, IEnumerable<IAdoNetStreamingResult> results)
		{
			IChannel channel;

			IEnumerable<IPayload> payloads;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)results == null)
				throw new ArgumentNullException(nameof(results));

			foreach (IAdoNetStreamingResult result in results)
			{
				ISchema schema; // prevent closure bug

				if (!context.LocalState.TryGetValue(this, out IDictionary<string, object> localState))
				{
					localState = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
					context.LocalState.Add(this, localState);
				}

				schema = localState[Constants.ContextComponentScopedSchema] as ISchema;

				if ((object)schema == null)
					throw new SyncPremException(nameof(schema));

				payloads = result.Records;

				if ((object)payloads == null)
					throw new SyncPremException(nameof(payloads));

				var records = payloads.Select(rec => new Record(schema, rec, string.Empty, Partition.None, Offset.None));

				channel = context.CreateChannel(records);

				yield return channel;
			}
		}

		protected override void PostExecuteRecord(IContext context, RecordConfiguration configuration)
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
				dbParameters = fsConfig.PostExecuteCommand.GetDbDataParameters(this.SourceUnitOfWork);

				results = this.SourceUnitOfWork.ExecuteSchemaResults(fsConfig.PostExecuteCommand.CommandType ?? CommandType.Text,
					fsConfig.PostExecuteCommand.CommandText,
					dbParameters);

				if ((object)results == null)
					throw new SyncPremException(nameof(results));

				results.ForceEnumeration(); // force execution
			}

			if ((object)this.SourceUnitOfWork != null)
				this.SourceUnitOfWork.Dispose();

			this.SourceUnitOfWork = null;
		}

		protected override void PreExecuteRecord(IContext context, RecordConfiguration configuration)
		{
			SchemaBuilder schemaBuilder;
			ISchema schema;
			IEnumerable<IPayload> records;

			IEnumerable<IAdoNetStreamingResult> results;
			IEnumerable<DbParameter> dbParameters;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			AdoNetConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			this.SourceUnitOfWork = fsConfig.GetUnitOfWork();

			if (fsConfig.PreExecuteCommand != null &&
				!SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.PreExecuteCommand.CommandText))
			{
				dbParameters = fsConfig.PreExecuteCommand.GetDbDataParameters(this.SourceUnitOfWork);

				results = this.SourceUnitOfWork.ExecuteResults(fsConfig.PreExecuteCommand.CommandType ?? CommandType.Text,
					fsConfig.PreExecuteCommand.CommandText,
					dbParameters);

				if ((object)results == null)
					throw new SyncPremException(nameof(results));

				results.ForceEnumeration(); // force execution
			}

			// execute schema only
			if (fsConfig.ExecuteCommand != null &&
				!SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.ExecuteCommand.CommandText))
			{
				dbParameters = fsConfig.ExecuteCommand.GetDbDataParameters(this.SourceUnitOfWork);

				results = this.SourceUnitOfWork.ExecuteSchemaResults(fsConfig.ExecuteCommand.CommandType ?? CommandType.Text,
					fsConfig.ExecuteCommand.CommandText,
					dbParameters);

				if ((object)results == null)
					throw new SyncPremException(nameof(results));

				foreach (IAdoNetStreamingResult result in results)
				{
					records = result.Records;

					if ((object)records == null)
						throw new SyncPremException(nameof(results));

					if (!context.LocalState.TryGetValue(this, out IDictionary<string, object> localState))
					{
						localState = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
						context.LocalState.Add(this, localState);
					}

					schemaBuilder = SchemaBuilder.Create();

					foreach (IPayload record in records)
					{
						string fieldName;
						Type fieldType;
						bool isKey;
						bool isNullable;

						fieldName = (string)record[nameof(DbColumn.ColumnName)];
						fieldType = (Type)record[nameof(DbColumn.DataType)];
						isKey = (bool?)record[nameof(DbColumn.IsKey)] ?? false;
						isNullable = (bool?)record[nameof(DbColumn.AllowDBNull)] ?? true;

						// TODO ensure nullable type
						schemaBuilder.AddField(fieldName, fieldType, isNullable, isKey);
					}

					schema = schemaBuilder.Build();

					if ((object)schema == null)
						throw new SyncPremException(nameof(schema));

					localState.Add(Constants.ContextComponentScopedSchema, schema);
				}
			}
		}

		protected override IChannel ProduceRecord(IContext context, RecordConfiguration configuration)
		{
			IChannel channel = null;
			ISchema schema;

			IEnumerable<IPayload> payloads;
			IEnumerable<IAdoNetStreamingResult> results;
			IEnumerable<DbParameter> dbParameters;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			AdoNetConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			if ((object)fsConfig.ExecuteCommand == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(fsConfig.ExecuteCommand)));

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.ExecuteCommand.CommandText))
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}.{1}'.", nameof(fsConfig.ExecuteCommand), nameof(fsConfig.ExecuteCommand.CommandText)));

			dbParameters = fsConfig.ExecuteCommand.GetDbDataParameters(this.SourceUnitOfWork);

			results = this.SourceUnitOfWork.ExecuteResults(fsConfig.ExecuteCommand.CommandType ?? CommandType.Text, fsConfig.ExecuteCommand.CommandText, dbParameters);

			if ((object)results == null)
				throw new SyncPremException(nameof(results));

			var channels = this.GetMultiplexedChannels(context, results);
			var records = channels.SelectMany(x => x.Records);
			channel = context.CreateChannel(records);

			return channel;
		}

		#endregion
	}
}
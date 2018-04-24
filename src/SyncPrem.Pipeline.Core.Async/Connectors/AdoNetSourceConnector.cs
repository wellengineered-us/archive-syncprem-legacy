/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Async.Stage.Connector.Source;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Core.Async.Runtime;
using SyncPrem.Pipeline.Core.Configurations;
using SyncPrem.StreamingIO.Primitives;
using SyncPrem.StreamingIO.ProxyWrappers;
using SyncPrem.StreamingIO.Relational;
using SyncPrem.StreamingIO.Relational.UoW;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Async.Connectors
{
	public class AdoNetSourceConnector : AsyncSourceConnector<AdoNetConnectorSpecificConfiguration>
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

		private IAsyncEnumerable<IAsyncRecord> GetMultiplexedRecords(IAsyncContext asyncContext, IAsyncEnumerable<IAdoNetStreamingResult> results, CancellationToken cancellationToken)
		{
			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)results == null)
				throw new ArgumentNullException(nameof(results));

			return Observable.Create<IAsyncRecord>(
				async _ =>
				{
					IAsyncEnumerator<IAdoNetStreamingResult> resultz;

					IAsyncEnumerable<IPayload> payloads;
					IAsyncEnumerator<IPayload> payloadz;

					IList<ISchema> schemas;
					ISchema schema = null;

					resultz = results.GetEnumerator();

					if ((object)resultz == null)
						throw new InvalidOperationException(nameof(resultz));

					while (await resultz.MoveNext(cancellationToken))
					{
						IAdoNetStreamingResult result = resultz.Current;

						// this is wrong for multi-schema resultsets!!!!!
						if (!asyncContext.LocalState.TryGetValue(this, out IDictionary<string, object> localState))
						{
							localState = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
							asyncContext.LocalState.Add(this, localState);
						}

						schemas = localState[Constants.ContextComponentScopedSchema] as IList<ISchema>;

						if ((object)schemas == null)
						{
							_.OnError(new SyncPremException(nameof(schemas)));
							return;
						}

						if (schemas.Count > result.ResultIndex)
							schema = schemas[(int)result.ResultIndex];

						if ((object)schema == null)
						{
							_.OnError(new SyncPremException(nameof(schema)));
							return;
						}

						payloads = result.AsyncRecords;

						if ((object)payloads == null)
						{
							_.OnError(new SyncPremException(nameof(payloads)));
							return;
						}

						payloadz = payloads.GetEnumerator();

						if ((object)payloadz == null)
						{
							_.OnError(new SyncPremException(nameof(payloadz)));
							return;
						}

						while (await payloadz.MoveNext(cancellationToken))
						{
							IPayload payload = payloadz.Current;
							IAsyncRecord record;

							record = new DefaultAsyncRecord(schema, payload, string.Empty, Partition.None, Offset.None);
							_.OnNext(record);
						}

						_.OnCompleted();
					}
				}).ToAsyncEnumerable();
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
				dbParameters = fsConfig.PostExecuteCommand.GetDbDataParameters(this.SourceUnitOfWork);

				results = this.SourceUnitOfWork.ExecuteSchemaResultsAsync(fsConfig.PostExecuteCommand.CommandType ?? CommandType.Text,
					fsConfig.PostExecuteCommand.CommandText,
					dbParameters, cancellationToken);

				if ((object)results == null)
					throw new SyncPremException(nameof(results));

				await results.ForceAsyncEnumeration(cancellationToken); // force execution
			}

			if ((object)this.SourceUnitOfWork != null)
				this.SourceUnitOfWork.Dispose();

			this.SourceUnitOfWork = null;
		}

		protected override async Task PreExecuteAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			SchemaBuilder schemaBuilder;
			ISchema schema;
			IList<ISchema> schemas;

			IAsyncEnumerable<IAdoNetStreamingResult> results;
			IAsyncEnumerator<IAdoNetStreamingResult> resultz;

			IAsyncEnumerable<IPayload> records;
			IAsyncEnumerator<IPayload> recordz;

			IEnumerable<DbParameter> dbParameters;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			AdoNetConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			this.SourceUnitOfWork = fsConfig.GetUnitOfWork();

			if (fsConfig.PreExecuteCommand != null &&
				!SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.PreExecuteCommand.CommandText))
			{
				dbParameters = fsConfig.PreExecuteCommand.GetDbDataParameters(this.SourceUnitOfWork);

				results = this.SourceUnitOfWork.ExecuteResultsAsync(fsConfig.PreExecuteCommand.CommandType ?? CommandType.Text,
					fsConfig.PreExecuteCommand.CommandText,
					dbParameters, cancellationToken);

				if ((object)results == null)
					throw new SyncPremException(nameof(results));

				await results.ForceAsyncEnumeration(cancellationToken); // force execution
			}

			// execute schema only
			if (fsConfig.ExecuteCommand != null &&
				!SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.ExecuteCommand.CommandText))
			{
				dbParameters = fsConfig.ExecuteCommand.GetDbDataParameters(this.SourceUnitOfWork);

				results = this.SourceUnitOfWork.ExecuteSchemaResultsAsync(fsConfig.ExecuteCommand.CommandType ?? CommandType.Text,
					fsConfig.ExecuteCommand.CommandText,
					dbParameters, cancellationToken);

				if ((object)results == null)
					throw new SyncPremException(nameof(results));

				resultz = results.GetEnumerator();

				if ((object)resultz == null)
					throw new SyncPremException(nameof(resultz));

				schemas = new List<ISchema>();

				while (await resultz.MoveNext(cancellationToken))
				{
					IAdoNetStreamingResult result = resultz.Current;

					records = result.AsyncRecords;

					if ((object)records == null)
						throw new SyncPremException(nameof(results));

					recordz = records.GetEnumerator();

					if ((object)recordz == null)
						throw new SyncPremException(nameof(recordz));

					schemaBuilder = SchemaBuilder.Create();

					while (await recordz.MoveNext(cancellationToken))
					{
						IPayload record = recordz.Current;

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

					schemas.Add(schema);
				}

				if (!asyncContext.LocalState.TryGetValue(this, out IDictionary<string, object> localState))
				{
					localState = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
					asyncContext.LocalState.Add(this, localState);
				}

				localState.Add(Constants.ContextComponentScopedSchema, schemas);
			}
		}

		protected override Task<IAsyncChannel> ProduceAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			IAsyncChannel asyncChannel;

			IAsyncEnumerable<IAdoNetStreamingResult> results;
			IEnumerable<DbParameter> dbParameters;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			AdoNetConnectorSpecificConfiguration fsConfig = this.Configuration.StageSpecificConfiguration;

			if ((object)fsConfig.ExecuteCommand == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(fsConfig.ExecuteCommand)));

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fsConfig.ExecuteCommand.CommandText))
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}.{1}'.", nameof(fsConfig.ExecuteCommand), nameof(fsConfig.ExecuteCommand.CommandText)));

			dbParameters = fsConfig.ExecuteCommand.GetDbDataParameters(this.SourceUnitOfWork);

			results = this.SourceUnitOfWork.ExecuteResultsAsync(fsConfig.ExecuteCommand.CommandType ?? CommandType.Text, fsConfig.ExecuteCommand.CommandText, dbParameters, cancellationToken);

			if ((object)results == null)
				throw new SyncPremException(nameof(results));

			var records = this.GetMultiplexedRecords(asyncContext, results, cancellationToken);
			asyncChannel = asyncContext.CreateChannelAsync(records);

			return Task.FromResult(asyncChannel);
		}

		#endregion
	}
}
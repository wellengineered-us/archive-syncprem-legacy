/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Async.Stage.Connector.Source;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Core.Async.Runtime;
using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.Pipeline.Core.Async.Connectors
{
	public class NullSourceConnector : AsyncSourceConnector<StageSpecificConfiguration>
	{
		#region Constructors/Destructors

		public NullSourceConnector()
		{
		}

		#endregion

		#region Fields/Constants

		private const string FIELD_NAME = "RandomValue_{0:00}";
		private const int INVALID_RANDOM_VALUE = 0;
		private const int MAX_FIELD_COUNT = 10;
		private const int MAX_RECORD_COUNT = 100000;
		private const int MIN_FIELD_COUNT = 1;
		private const int MIN_RECORD_COUNT = 1;
		private static readonly Random random = new Random();
		private static readonly SemaphoreSlim syncLock = new SemaphoreSlim(1, 1);

		#endregion

		#region Properties/Indexers/Events

		private static Random Random
		{
			get
			{
				return random;
			}
		}

		private static SemaphoreSlim SyncLock
		{
			get
			{
				return syncLock;
			}
		}

		#endregion

		#region Methods/Operators

		private static IAsyncEnumerable<IPayload> GetRandomPayloadsAsync(ISchema schema, CancellationToken cancellationToken)
		{
			if ((object)schema == null)
				throw new ArgumentNullException(nameof(schema));

			return Observable.Create<IPayload>(
				async _ =>
				{
					IPayload payload;
					IField[] fields;

					long recordCount;

					fields = schema.Fields.Values.ToArray();

					await SyncLock.WaitAsync(cancellationToken);

					try
					{
						recordCount = Random.Next(MIN_RECORD_COUNT, MAX_RECORD_COUNT);
						recordCount = MAX_RECORD_COUNT;

						if (recordCount == INVALID_RANDOM_VALUE)
						{
							_.OnError(new SyncPremException());
							return;
						}

						for (long recordIndex = 0; recordIndex < recordCount; recordIndex++)
						{
							payload = new Payload();

							for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
							{
								if (fields[fieldIndex].IsFieldKeyComponent)
									payload.Add(fields[fieldIndex].FieldName, Guid.NewGuid());
								else
									payload.Add(fields[fieldIndex].FieldName, Random.NextDouble());
							}

							_.OnNext(payload);
						}

						_.OnCompleted();
					}
					finally
					{
						SyncLock.Release();
					}
				}).ToAsyncEnumerable();
		}

		private static async Task<ISchema> GetRandomSchemaAsync(CancellationToken cancellationToken)
		{
			long fieldCount;
			SchemaBuilder schemaBuilder;

			await SyncLock.WaitAsync(cancellationToken);

			try
			{
				schemaBuilder = SchemaBuilder.Create();

				schemaBuilder.AddField(string.Empty, typeof(Guid), false, true);

				fieldCount = Random.Next(MIN_FIELD_COUNT, MAX_FIELD_COUNT);

				if (fieldCount == INVALID_RANDOM_VALUE)
					throw new SyncPremException();

				for (long fieldIndex = 0; fieldIndex < fieldCount; fieldIndex++)
				{
					string fieldName = string.Format(FIELD_NAME, fieldIndex);

					schemaBuilder.AddField(fieldName, typeof(double), false, false);
				}

				return schemaBuilder.Build();
			}
			finally
			{
				SyncLock.Release();
			}
		}

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

		protected override Task PostExecuteAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			return Task.CompletedTask;
		}

		protected override async Task PreExecuteAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			ISchema schema;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			if (!asyncContext.LocalState.TryGetValue(this, out IDictionary<string, object> localState))
			{
				localState = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
				asyncContext.LocalState.Add(this, localState);
			}

			schema = await GetRandomSchemaAsync(cancellationToken);

			if ((object)schema == null)
				throw new SyncPremException(nameof(schema));

			localState.Add(Constants.ContextComponentScopedSchema, schema);
		}

		protected override Task<IAsyncChannel> ProduceAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			IAsyncChannel asyncChannel;
			ISchema schema;

			IAsyncEnumerable<IPayload> payloads;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			if (!asyncContext.LocalState.TryGetValue(this, out IDictionary<string, object> localState))
			{
				localState = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
				asyncContext.LocalState.Add(this, localState);
			}

			schema = localState[Constants.ContextComponentScopedSchema] as ISchema;

			if ((object)schema == null)
				throw new SyncPremException(nameof(schema));

			payloads = GetRandomPayloadsAsync(schema, cancellationToken);

			if ((object)payloads == null)
				throw new SyncPremException(nameof(payloads));

			var asyncRecords = payloads.Select(rec => new DefaultAsyncRecord(schema, rec, string.Empty, Partition.None, Offset.None));

			asyncChannel = asyncContext.CreateChannelAsync(asyncRecords);

			return Task.FromResult(asyncChannel);
		}

		#endregion
	}
}
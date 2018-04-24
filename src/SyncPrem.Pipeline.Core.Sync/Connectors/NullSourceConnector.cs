/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;
using SyncPrem.Pipeline.Abstractions.Sync.Stage.Connector.Source;
using SyncPrem.Pipeline.Core.Sync.Runtime;
using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.Pipeline.Core.Sync.Connectors
{
	public class NullSourceConnector : SyncSourceConnector<StageSpecificConfiguration>
	{
		#region Constructors/Destructors

		static NullSourceConnector()
		{
			schema = GetRandomSchema();
		}

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
		private static readonly ISchema schema;
		private static readonly object syncLock = new object();

		#endregion

		#region Properties/Indexers/Events

		private static Random Random
		{
			get
			{
				return random;
			}
		}

		private static ISchema Schema
		{
			get
			{
				return schema;
			}
		}

		#endregion

		#region Methods/Operators

		private static IEnumerable<IPayload> GetRandomPayloads(ISchema schema)
		{
			IPayload payload;
			IField[] fields;

			long recordCount;

			if ((object)schema == null)
				throw new ArgumentNullException(nameof(schema));

			fields = schema.Fields.Values.ToArray();

			lock (syncLock)
				recordCount = Random.Next(MIN_RECORD_COUNT, MAX_RECORD_COUNT);

			recordCount = MAX_RECORD_COUNT;

			if (recordCount == INVALID_RANDOM_VALUE)
				throw new SyncPremException();

			for (long recordIndex = 0; recordIndex < recordCount; recordIndex++)
			{
				payload = new Payload();

				for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
				{
					lock (syncLock)
					{
						if (fields[fieldIndex].IsFieldKeyComponent)
							payload.Add(fields[fieldIndex].FieldName, Guid.NewGuid());
						else
							payload.Add(fields[fieldIndex].FieldName, Random.NextDouble());
					}
				}

				yield return payload;
			}
		}

		private static ISchema GetRandomSchema()
		{
			long fieldCount;
			SchemaBuilder schemaBuilder;

			schemaBuilder = SchemaBuilder.Create();

			schemaBuilder.AddField(string.Empty, typeof(Guid), false, true);

			lock (syncLock)
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

		protected override void PostExecuteInternal(ISyncContext context, RecordConfiguration configuration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();
		}

		protected override void PreExecuteInternal(ISyncContext context, RecordConfiguration configuration)
		{
			ISchema schema;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			if (!context.LocalState.TryGetValue(this, out IDictionary<string, object> localState))
			{
				localState = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
				context.LocalState.Add(this, localState);
			}

			schema = Schema;

			if ((object)schema == null)
				throw new SyncPremException(nameof(schema));

			localState.Add(Constants.ContextComponentScopedSchema, schema);
		}

		protected override ISyncChannel ProduceInternal(ISyncContext context, RecordConfiguration configuration)
		{
			ISyncChannel channel;
			ISchema schema;

			IEnumerable<IPayload> payloads;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			if (!context.LocalState.TryGetValue(this, out IDictionary<string, object> localState))
			{
				localState = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
				context.LocalState.Add(this, localState);
			}

			schema = localState[Constants.ContextComponentScopedSchema] as ISchema;

			if ((object)schema == null)
				throw new SyncPremException(nameof(schema));

			payloads = GetRandomPayloads(schema);

			if ((object)payloads == null)
				throw new SyncPremException(nameof(payloads));

			var records = payloads.Select(rec => new DefaultSyncRecord(schema, rec, string.Empty, Partition.None, Offset.None));

			channel = context.CreateChannel(records);

			return channel;
		}

		#endregion
	}
}
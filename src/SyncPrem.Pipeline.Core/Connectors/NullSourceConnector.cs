/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Channel;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Source;
using SyncPrem.Pipeline.Core.Channels;
using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.Pipeline.Core.Connectors
{
	public class NullSourceConnector : SourceConnector<StageSpecificConfiguration>
	{
		#region Constructors/Destructors

		public NullSourceConnector()
		{
		}

		#endregion

		#region Fields/Constants

		private const int FIELD_COUNT = 5;
		private const string FIELD_NAME = "RandomValue_{0:00}";
		private static readonly Random random = new Random();
		private static readonly ISchema schema = GetSchema();

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
			recordCount = Random.Next(0, 100000);

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

				yield return payload;
			}
		}

		private static ISchema GetSchema()
		{
			SchemaBuilder schemaBuilder;

			schemaBuilder = SchemaBuilder.Create();

			schemaBuilder.AddField(string.Empty, typeof(Guid), false, true);

			for (long fieldIndex = 0; fieldIndex < FIELD_COUNT; fieldIndex++)
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

		protected override void PostExecuteRecord(IContext context, RecordConfiguration configuration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));
		}

		protected override void PreExecuteRecord(IContext context, RecordConfiguration configuration)
		{
			ISchema schema;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

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

		protected override IChannel ProduceRecord(IContext context, RecordConfiguration configuration)
		{
			IChannel channel;
			ISchema schema;

			IEnumerable<IPayload> payloads;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

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

			var records = payloads.Select(rec => new Record(schema, rec, string.Empty, Partition.None, Offset.None));

			channel = context.CreateChannel(records);

			return channel;
		}

		#endregion
	}
}
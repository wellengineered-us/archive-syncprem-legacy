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
using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.Pipeline.Core.Connectors.Null
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

		private static readonly ISchema randomSchema = GetRandomSchema();

		#endregion

		#region Properties/Indexers/Events

		private static Random Random
		{
			get
			{
				return random;
			}
		}

		private static ISchema RandomSchema
		{
			get
			{
				return randomSchema;
			}
		}

		#endregion

		#region Methods/Operators

		private static IEnumerable<IRecord> GetRandomRecords(ISchema schema)
		{
			IRecord record;
			IField[] fields;

			long recordCount;

			if ((object)schema == null)
				throw new ArgumentNullException(nameof(schema));

			fields = schema.Fields.Values.ToArray();
			recordCount = Random.Next(0, 100000);

			for (long recordIndex = 0; recordIndex < recordCount; recordIndex++)
			{
				record = new Record() { RecordIndex = recordIndex };

				for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
				{
					record.Add(fields[fieldIndex].FieldName, Random.NextDouble());
				}

				yield return record;
			}
		}

		private static ISchema GetRandomSchema()
		{
			SchemaBuilder schemaBuilder;

			schemaBuilder = new SchemaBuilder();

			for (long fieldIndex = 0; fieldIndex < FIELD_COUNT; fieldIndex++)
			{
				string fieldName = string.Format(FIELD_NAME, fieldIndex);

				schemaBuilder.AddField(fieldName, typeof(double));
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

			schema = RandomSchema;

			if ((object)schema == null)
				throw new SyncPremException(nameof(schema));

			if (!context.LocalState.TryGetValue(this, out IDictionary<string, object> localState))
			{
				localState = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
				context.LocalState.Add(this, localState);
			}

			localState.Add(Constants.ContextComponentScopedSchema, schema);
		}

		protected override IChannel ProduceRecord(IContext context, RecordConfiguration configuration)
		{
			IChannel channel;
			ISchema schema;
			IEnumerable<IRecord> records;

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

			records = GetRandomRecords(schema);

			if ((object)records == null)
				throw new SyncPremException(nameof(records));

			channel = context.CreateChannel(schema, records);

			return channel;
		}

		#endregion
	}
}
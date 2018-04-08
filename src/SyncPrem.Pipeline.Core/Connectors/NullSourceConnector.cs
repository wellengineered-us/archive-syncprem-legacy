/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Source;
using SyncPrem.Pipeline.Core.Runtime;
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
		private const int MAX_RECORD_COUNT = 100000;
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
			recordCount = Random.Next(0, MAX_RECORD_COUNT);

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

		protected override Task PostExecuteAsyncInternal(IContext context, RecordConfiguration configuration, CancellationToken cancellationToken, IProgress<int> progress)
		{
			throw new NotImplementedException();
		}

		protected override void PostExecuteInternal(IContext context, RecordConfiguration configuration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();
		}

		protected override Task PreExecuteAsyncInternal(IContext context, RecordConfiguration configuration, CancellationToken cancellationToken, IProgress<int> progress)
		{
			throw new NotImplementedException();
		}

		protected override void PreExecuteInternal(IContext context, RecordConfiguration configuration)
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

		protected override Task<IChannel> ProduceAsyncInternal(IContext context, RecordConfiguration configuration, CancellationToken cancellationToken, IProgress<int> progress)
		{
			throw new NotImplementedException();
		}

		protected override IChannel ProduceInternal(IContext context, RecordConfiguration configuration)
		{
			IChannel channel;
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

			var records = payloads.Select(rec => new Record(schema, rec, string.Empty, Partition.None, Offset.None));

			channel = context.CreateChannel(records);

			return channel;
		}

		#endregion
	}
}
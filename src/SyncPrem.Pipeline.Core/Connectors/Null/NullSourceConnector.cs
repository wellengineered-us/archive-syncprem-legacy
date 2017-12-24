/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Payload;
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

		private const int FIELD_COUNT = 100;

		private const string FIELD_NAME = "RandomValue_{0:0000}";

		private static readonly Random random = new Random();

		#endregion

		#region Properties/Indexers/Events

		private static Random Random
		{
			get
			{
				return random;
			}
		}

		#endregion

		#region Methods/Operators

		private static IEnumerable<IRecord> GetRandomRecords()
		{
			long recordCount;

			recordCount = 10000; //Random.Next(0, 100);

			for (long recordIndex = 0; recordIndex < recordCount; recordIndex++)
			{
				IRecord record;

				record = new Record();

				for (int fieldIndex = 0; fieldIndex < FIELD_COUNT; fieldIndex++)
				{
					record.Add(string.Format(FIELD_NAME, fieldIndex), Random.NextDouble());
				}

				yield return record;
			}
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

		protected override void PostExecuteRecord(IContext context, RecordConfiguration recordConfiguration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));
		}

		protected override void PreExecuteRecord(IContext context, RecordConfiguration recordConfiguration)
		{
			IField[] fields;
			IPipelineMetadata pipelineMetadata;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			fields = new IField[FIELD_COUNT];
			for (long fieldIndex = 0; fieldIndex < FIELD_COUNT; fieldIndex++)
			{
				fields[fieldIndex] = new Field()
									{
										FieldName = string.Format(FIELD_NAME, fieldIndex),
										FieldType = typeof(double),
										IsFieldOptional = true,
										IsFieldKeyComponent = false,
										FieldIndex = fieldIndex
				};
			}

			pipelineMetadata = context.CreateMetadata(fields);
			context.MetadataChain.Push(pipelineMetadata);
		}

		protected override IPipelineMessage ProduceRecord(IContext context, RecordConfiguration recordConfiguration)
		{
			IEnumerable<IRecord> records;
			IPipelineMessage pipelineMessage;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			records = GetRandomRecords();

			pipelineMessage = context.CreateMessage(records);

			return pipelineMessage;
		}

		#endregion
	}
}
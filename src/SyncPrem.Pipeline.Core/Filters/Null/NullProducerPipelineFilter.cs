/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Data.Primitives;
using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configurations;
using SyncPrem.Pipeline.Abstractions.Filters.Producer;
using SyncPrem.Pipeline.Abstractions.Messages;

namespace SyncPrem.Pipeline.Core.Filters.Null
{
	public class NullProducerPipelineFilter : ProducerPipelineFilter<FilterSpecificConfiguration>
	{
		#region Constructors/Destructors

		public NullProducerPipelineFilter()
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

				record = new Record(recordIndex);

				for (int fieldIndex = 0; fieldIndex < FIELD_COUNT; fieldIndex++)
				{
					record.Add(string.Format(FIELD_NAME, fieldIndex), Random.NextDouble());
				}

				yield return record;
			}
		}

		private static IEnumerable<IResult> GetRandomResults()
		{
			int resultCount;

			resultCount = 100; //Random.Next(0, 10);

			for (long resultIndex = 0; resultIndex < resultCount; resultIndex++)
			{
				yield return new Result(resultIndex) { RecordsAffected = -1, Records = GetRandomRecords() };
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

		protected override void PostProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));
		}

		protected override void PreProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			IField[] fields;
			IPipelineMetadata pipelineMetadata;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			fields = new IField[FIELD_COUNT];
			for (int fieldIndex = 0; fieldIndex < FIELD_COUNT; fieldIndex++)
			{
				fields[fieldIndex] = new Field(fieldIndex)
									{
										FieldName = string.Format(FIELD_NAME, fieldIndex),
										FieldType = typeof(double),
										IsFieldOptional = true,
										IsFieldKeyComponent = false
									};
			}

			pipelineMetadata = pipelineContext.CreateMetadata(fields);
			pipelineContext.MetadataChain.Push(pipelineMetadata);
		}

		protected override IPipelineMessage ProduceMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			IEnumerable<IResult> results;
			IPipelineMessage pipelineMessage;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			results = GetRandomResults();

			pipelineMessage = pipelineContext.CreateMessage(results);

			return pipelineMessage;
		}

		#endregion
	}
}
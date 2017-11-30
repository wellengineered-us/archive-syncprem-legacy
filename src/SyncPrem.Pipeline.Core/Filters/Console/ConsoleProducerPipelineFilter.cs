/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;

using SyncPrem.Infrastructure.Data.Primitives;
using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configurations;
using SyncPrem.Pipeline.Abstractions.Filters.Producer;
using SyncPrem.Pipeline.Abstractions.Messages;

namespace SyncPrem.Pipeline.Core.Filters.Console
{
	public class ConsoleProducerPipelineFilter : ProducerPipelineFilter<FilterSpecificConfiguration>
	{
		#region Constructors/Destructors

		public ConsoleProducerPipelineFilter()
		{
		}

		#endregion

		#region Fields/Constants

		private static readonly TextReader textReader = System.Console.In;
		private static readonly TextWriter textWriter = System.Console.Out;

		#endregion

		#region Properties/Indexers/Events

		private static TextReader TextReader
		{
			get
			{
				return textReader;
			}
		}

		private static TextWriter TextWriter
		{
			get
			{
				return textWriter;
			}
		}

		#endregion

		#region Methods/Operators

		private static IEnumerable<IResult> GetYieldViaConsole(IEnumerable<IField> fields)
		{
			int index = 0;

			if ((object)fields == null)
				throw new ArgumentNullException(nameof(fields));

			while (index < 1)
			{
				yield return new Result(index++)
							{
								RecordsAffected = 0,
								Records = YieldViaConsole(fields)
							};
			}
		}

		private static IEnumerable<IRecord> YieldViaConsole(IEnumerable<IField> fields)
		{
			IRecord record;
			int recordIndex = 0;
			string line;
			string[] fieldValues;

			if ((object)fields == null)
				throw new ArgumentNullException(nameof(fields));

			while (true)
			{
				line = TextReader.ReadLine();
				line = (line ?? string.Empty).Trim();

				if (line == string.Empty)
					yield break;

				fieldValues = line.Split('|');

				record = new Record(recordIndex++)
						{
							ContextData = line
						};

				foreach (IField field in fields)
				{
					record.Add(field.FieldName, fieldValues[field.FieldIndex]);
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

		protected override void PostProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));
		}

		protected override void PreProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			IList<IField> fields;
			IField field;
			IPipelineMetadata pipelineMetadata;

			string line;
			string[] fieldNames;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			TextWriter.WriteLine("Enter list of field names separated by pipe character: ");
			fields = new List<IField>();
			line = TextReader.ReadLine();
			line = (line ?? string.Empty).Trim();

			if (line != string.Empty)
			{
				fieldNames = line.Split('|');

				if ((object)fieldNames == null || fieldNames.Length <= 0)
				{
					TextWriter.WriteLine("List of field names was invalid; using default.");
					field = new Field(0) { IsFieldOptional = false, FieldName = "line", FieldType = typeof(string), ContextData = null };
					fields.Add(field);
				}
				else
				{
					// does not check for unique field names - quick and dirty example (perhaps a dictionary?)
					for (int fieldIndex = 0; fieldIndex < fieldNames.Length; fieldIndex++)
					{
						string fieldName = fieldNames[fieldIndex];

						if ((fieldName ?? string.Empty).Trim() == string.Empty)
							continue;

						field = new Field(fieldIndex) { IsFieldOptional = false, FieldName = fieldName, FieldType = typeof(string), ContextData = null };
						fields.Add(field);
					}

					TextWriter.WriteLine("List of field names was valid; building field schema: {0}", string.Join(" | ", fieldNames));
				}
			}

			pipelineMetadata = pipelineContext.CreateMetadata(fields);
			pipelineContext.MetadataChain.Push(pipelineMetadata);
		}

		protected override IPipelineMessage ProduceMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			IPipelineMessage pipelineMessage;
			IEnumerable<IResult> sourceDataEnumerable;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			sourceDataEnumerable = GetYieldViaConsole(pipelineContext.MetadataChain.Peek().UpstreamFields);
			pipelineMessage = pipelineContext.CreateMessage(sourceDataEnumerable);

			return pipelineMessage;
		}

		#endregion
	}
}
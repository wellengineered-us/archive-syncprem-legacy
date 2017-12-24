/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Payload;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Source;
using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.Pipeline.Core.Connectors.Console
{
	public class ConsoleSourceConnector : SourceConnector<StageSpecificConfiguration>
	{
		#region Constructors/Destructors

		public ConsoleSourceConnector()
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

		private static IEnumerable<IRecord> GetYieldViaConsole(IEnumerable<IField> fields)
		{
			IRecord record;
			long fieldIndex;
			long recordIndex;
			string line;
			string[] fieldValues;

			if ((object)fields == null)
				throw new ArgumentNullException(nameof(fields));

			recordIndex = 0;
			while (true)
			{
				line = TextReader.ReadLine();
				line = (line ?? string.Empty).Trim();

				if (line == string.Empty)
					yield break;

				fieldValues = line.Split('|');

				record = new Record();

				fieldIndex = 0;
				foreach (IField field in fields)
				{
					record.Add(field.FieldName, fieldValues[fieldIndex]);
					fieldIndex++;
				}

				recordIndex++;
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
			IList<IField> fields;
			IField field;
			IPipelineMetadata pipelineMetadata;

			string line;
			string[] fieldNames;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

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
					field = new Field()
							{
								IsFieldOptional = false,
								FieldName = "line",
								FieldType = typeof(string),
								FieldIndex = 0
							};
					fields.Add(field);
				}
				else
				{
					// does not check for unique field names - quick and dirty example (perhaps a dictionary?)
					for (long fieldIndex = 0; fieldIndex < fieldNames.Length; fieldIndex++)
					{
						string fieldName = fieldNames[fieldIndex];

						if ((fieldName ?? string.Empty).Trim() == string.Empty)
							continue;

						field = new Field()
								{
									IsFieldOptional = false,
									FieldName = fieldName,
									FieldType = typeof(string),
									FieldIndex = fieldIndex
						};
						fields.Add(field);
					}

					TextWriter.WriteLine("List of field names was valid; building field schema: {0}", string.Join(" | ", fieldNames));
				}
			}

			pipelineMetadata = context.CreateMetadata(fields);
			context.MetadataChain.Push(pipelineMetadata);
		}

		protected override IPipelineMessage ProduceRecord(IContext context, RecordConfiguration recordConfiguration)
		{
			IPipelineMessage pipelineMessage;
			IEnumerable<IRecord> sourceDataEnumerable;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			sourceDataEnumerable = GetYieldViaConsole(context.MetadataChain.Peek().UpstreamFields);
			pipelineMessage = context.CreateMessage(sourceDataEnumerable);

			return pipelineMessage;
		}

		#endregion
	}
}
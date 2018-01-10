/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Channel;
using SyncPrem.Pipeline.Abstractions.Configuration;
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

		private IEnumerable<IRecord> GetYieldViaConsole(ISchema schema)
		{
			IRecord record;

			long recordIndex;
			string line;
			string[] fieldValues;
			IField[] fields;

			if ((object)schema == null)
				throw new ArgumentNullException(nameof(schema));

			fields = schema.Fields.Values.ToArray();

			recordIndex = 0;
			while (true)
			{
				line = TextReader.ReadLine();

				if ((object)line == null)
					yield break;

				fieldValues = line.Split('|');

				record = new Record() { RecordIndex = recordIndex };

				for (long fieldIndex = 0; fieldIndex < Math.Min(fieldValues.Length, fields.Length); fieldIndex++)
					record.Add(fields[fieldIndex].FieldName, fieldValues[fieldIndex]);

				recordIndex++;

				yield return record;
			}
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
			SchemaBuilder schemaBuilder;
			ISchema schema;

			string line;
			string[] fieldNames;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			schemaBuilder = new SchemaBuilder();

			TextWriter.WriteLine("Enter list of field names separated by pipe character: ");
			line = TextReader.ReadLine();

			if ((object)line != null)
			{
				fieldNames = line.Split('|');

				if ((object)fieldNames == null || fieldNames.Length <= 0)
				{
					TextWriter.WriteLine("List of field names was invalid; using default (blank).");
					schemaBuilder.AddField(string.Empty, typeof(string));
				}
				else
				{
					for (long fieldIndex = 0; fieldIndex < fieldNames.Length; fieldIndex++)
					{
						string fieldName;

						fieldName = fieldNames[fieldIndex];

						if ((fieldName ?? string.Empty).Trim() == string.Empty)
							continue;

						schemaBuilder.AddField(fieldName, typeof(string));
					}

					TextWriter.WriteLine("Building schema: '{0}'", string.Join(" | ", fieldNames));
				}
			}

			schema = schemaBuilder.Build();

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

			records = this.GetYieldViaConsole(schema);

			if ((object)records == null)
				throw new SyncPremException(nameof(records));

			channel = context.CreateChannel(schema, records);

			return channel;
		}

		#endregion
	}
}
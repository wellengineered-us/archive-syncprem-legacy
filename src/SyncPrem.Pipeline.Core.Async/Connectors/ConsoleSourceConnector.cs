/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
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

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Async.Connectors
{
	public class ConsoleSourceConnector : AsyncSourceConnector<StageSpecificConfiguration>
	{
		#region Constructors/Destructors

		public ConsoleSourceConnector()
		{
		}

		#endregion

		#region Fields/Constants

		private static readonly SemaphoreSlim syncLock = new SemaphoreSlim(0, 1);

		private static readonly TextReader textReader = Console.In;
		private static readonly TextWriter textWriter = Console.Out;

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

		private IAsyncEnumerable<IPayload> GetYieldViaConsoleAsync(ISchema schema, CancellationToken cancellationToken)
		{
			if ((object)schema == null)
				throw new ArgumentNullException(nameof(schema));

			return Observable.Create<IPayload>(
				async _ =>
				{
					IPayload payload;

					long recordIndex;
					string line;
					string[] fieldValues;
					IField[] fields;

					fields = schema.Fields.Values.ToArray();

					await syncLock.WaitAsync(cancellationToken);

					try
					{
						recordIndex = 0;
						while (true)
						{
							line = await TextReader.ReadLineAsync();

							if (SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(line))
								break;

							fieldValues = line.Split('|');

							payload = new Payload();

							for (long fieldIndex = 0; fieldIndex < Math.Min(fieldValues.Length, fields.Length); fieldIndex++)
								payload.Add(fields[fieldIndex].FieldName, fieldValues[fieldIndex]);

							recordIndex++;

							_.OnNext(payload);
						}

						_.OnCompleted();
					}
					finally
					{
						syncLock.Release();
					}
				}).ToAsyncEnumerable();
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
			SchemaBuilder schemaBuilder;
			ISchema schema;

			string line;
			string[] fieldNames;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			schemaBuilder = SchemaBuilder.Create();

			await TextWriter.WriteLineAsync("Enter list of schema field names separated by pipe character: ");
			line = await TextReader.ReadLineAsync();

			if (!SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(line))
			{
				fieldNames = line.Split('|');

				if ((object)fieldNames == null || fieldNames.Length <= 0)
				{
					await TextWriter.WriteLineAsync("List of schema field names was invalid; using default (blank).");
					schemaBuilder.AddField(string.Empty, typeof(string), false, true);
				}
				else
				{
					for (long fieldIndex = 0; fieldIndex < fieldNames.Length; fieldIndex++)
					{
						string fieldName;

						fieldName = fieldNames[fieldIndex];

						if ((fieldName ?? string.Empty).Trim() == string.Empty)
							continue;

						schemaBuilder.AddField(fieldName, typeof(string), false, true);
					}

					await TextWriter.WriteLineAsync(string.Format("Building KEY schema: '{0}'", string.Join(" | ", fieldNames)));
				}
			}

			if (!asyncContext.LocalState.TryGetValue(this, out IDictionary<string, object> localState))
			{
				localState = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
				asyncContext.LocalState.Add(this, localState);
			}

			schema = schemaBuilder.Build();

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

			payloads = this.GetYieldViaConsoleAsync(schema, cancellationToken);

			if ((object)payloads == null)
				throw new SyncPremException(nameof(payloads));

			var records = payloads.Select(rec => new DefaultAsyncRecord(schema, rec, string.Empty, Partition.None, Offset.None));

			asyncChannel = asyncContext.CreateChannelAsync(records);

			return Task.FromResult(asyncChannel);
		}

		#endregion
	}
}
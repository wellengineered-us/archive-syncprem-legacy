/*
	Copyright ?2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Stage.Processor;
using SyncPrem.StreamingIO.Primitives;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Processors
{
	public class ReshapeProcessor : Processor<StageSpecificConfiguration>
	{
		#region Constructors/Destructors

		public ReshapeProcessor()
		{
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

		protected override Task PostExecuteAsyncInternal(IContext context, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			return Task.CompletedTask;
		}

		protected override Task PreExecuteAsyncInternal(IContext context, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			return Task.CompletedTask;
		}

		protected override Task<IChannel> ProcessAsyncInternal(IContext context, RecordConfiguration configuration, IChannel channel, ProcessAsyncDelegate nextAsync, CancellationToken cancellationToken)
		{
			Task<IChannel> newChannelTask;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)channel == null)
				throw new ArgumentNullException(nameof(channel));

			//Func<IEnumerable<IMessage>, IEnumerable<IMessage>> reshapeRecords = (r) => this.ReshapeRecords(context, r);

			// simply wrap
			//channel.ApplyWrap(reshapeRecords);


			if ((object)nextAsync != null)
				newChannelTask = nextAsync(context, configuration, channel, cancellationToken);
			else
				newChannelTask = Task.FromResult(channel);

			return newChannelTask;
		}

		private IEnumerable<IPayload> ReshapeRecords(IContext context, IEnumerable<IPayload> records)
		{
			long fieldIndex;
			long recordIndex;
			string fieldName;
			object recordValue;

			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			//foreach (IField x in context.MetadataChain.Peek().UpstreamFields)
			//{
			//}

			recordIndex = 0;
			foreach (IPayload record in records)
			{
				Payload reshapedPayload = null;

				if ((object)record != null)
				{
					reshapedPayload = new Payload();

					fieldIndex = 0;
					foreach (KeyValuePair<string, object> item in record)
					{
						fieldName = item.Key;
						recordValue = record[item.Key];

						char[] charArray = recordValue.SafeToString().ToCharArray();
						Array.Reverse(charArray);
						recordValue = new string(charArray);

						reshapedPayload.Add(fieldName, recordValue);
						fieldIndex++;
					}

					recordIndex++;
				}

				yield return reshapedPayload;
			}
		}

		#endregion

		#region Classes/Structs/Interfaces/Enums/Delegates

		public enum MessageReshapeOption
		{
			None = 0,
			Create,
			Alter,
			Drop
		}

		#endregion
	}
}
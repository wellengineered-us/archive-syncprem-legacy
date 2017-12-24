/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Payload;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Stage.Processor;
using SyncPrem.StreamingIO.DataMasking;
using SyncPrem.StreamingIO.Primitives;
using SyncPrem.StreamingIO.ProxyWrappers;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Processors
{
	public class MessageReshapeProcessor : Processor<StageSpecificConfiguration>
	{
		#region Constructors/Destructors

		public MessageReshapeProcessor()
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

		protected override void PostExecuteRecord(IContext context, RecordConfiguration recordConfiguration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));
		}

		protected override void PreExecuteRecord(IContext context, RecordConfiguration recordConfiguration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));
		}

		protected override IPipelineMessage ProcessRecord(IContext context, RecordConfiguration recordConfiguration, IPipelineMessage pipelineMessage, ProcessDelegate next)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			if ((object)pipelineMessage == null)
				throw new ArgumentNullException(nameof(pipelineMessage));

			Func<IEnumerable<IRecord>, IEnumerable<IRecord>> reshapeRecords = (r) => this.ReshapeRecords(context, r);

			// simply wrap
			pipelineMessage.ApplyWrap(reshapeRecords);

			return next(context, recordConfiguration, pipelineMessage);
		}

		private IEnumerable<IRecord> ReshapeRecords(IContext context, IEnumerable<IRecord> records)
		{
			long fieldIndex;
			long recordIndex;
			string fieldName;
			object recordValue;

			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			foreach (IField x in context.MetadataChain.Peek().UpstreamFields)
			{
			}

			recordIndex = 0;
			foreach (IRecord record in records)
			{
				Record reshapedRecord = null;

				if ((object)record != null)
				{
					reshapedRecord = new Record();

					fieldIndex = 0;
					foreach (KeyValuePair<string, object> item in record)
					{
						fieldName = item.Key;
						recordValue = record[item.Key];

						char[] charArray = recordValue.SafeToString().ToCharArray();
						Array.Reverse(charArray);
						recordValue = new string(charArray);

						reshapedRecord.Add(fieldName, recordValue);
						fieldIndex++;
					}

					recordIndex++;
				}

				yield return reshapedRecord;
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
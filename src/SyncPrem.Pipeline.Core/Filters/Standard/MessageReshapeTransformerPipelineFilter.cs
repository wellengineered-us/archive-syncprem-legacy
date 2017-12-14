/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Data.Primitives;
using SyncPrem.Infrastructure.Oxymoron;
using SyncPrem.Infrastructure.Wrappers;
using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configurations;
using SyncPrem.Pipeline.Abstractions.Filters.Transformer;
using SyncPrem.Pipeline.Abstractions.Messages;

namespace SyncPrem.Pipeline.Core.Filters.Standard
{
	public class MessageReshapeTransformerPipelineFilter : TransformerPipelineFilter<FilterSpecificConfiguration>
	{
		#region Constructors/Destructors

		public MessageReshapeTransformerPipelineFilter()
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

		protected override void PostProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));
		}

		protected override void PreProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			ResolveDictionaryValueDelegate resolveDictionaryValueCallback;
			IObfuscationSpec obfuscationSpec;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));
		}

		private IEnumerable<IRecord> ReshapeRecords(IPipelineContext pipelineContext, IEnumerable<IRecord> records)
		{
			int fieldIndex;
			string fieldName;
			object recordValue;

			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			foreach (IField x in pipelineContext.MetadataChain.Peek().UpstreamFields)
			{
			}

			//char[] charArray = fieldName.ToCharArray();
			//Array.Reverse(charArray);
			//fieldName = new string(charArray);

			foreach (IRecord record in records)
			{
				Record reshapedRecord = null;

				if ((object)record != null)
				{
					reshapedRecord = new Record(record.RecordIndex) { ContextData = record };

					fieldIndex = 0;
					foreach (KeyValuePair<string, object> item in record)
					{
						fieldName = item.Key;
						recordValue = record[item.Key];

						reshapedRecord.Add(fieldName, recordValue);
					}
				}

				yield return reshapedRecord;
			}
		}

		protected override IPipelineMessage TransformMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration, IPipelineMessage pipelineMessage, TransformDelegate next)
		{
			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)pipelineMessage == null)
				throw new ArgumentNullException(nameof(pipelineMessage));

			Func<IEnumerable<IRecord>, IEnumerable<IRecord>> reshapeRecords = (r) => this.ReshapeRecords(pipelineContext, r);

			// simply wrap
			pipelineMessage.ApplyWrap((x) => x.GetWrappedEnumerable(r => r.ApplyWrap(reshapeRecords)));

			return next(pipelineContext, tableConfiguration, pipelineMessage);
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
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

		#endregion

		#region Properties/Indexers/Events

		private static TextReader TextReader
		{
			get
			{
				return textReader;
			}
		}

		#endregion

		#region Methods/Operators

		private static IEnumerable<IResult> GetYieldViaConsole()
		{
			int index = 0;
			while (index < 1)
			{
				yield return new Result(index++)
							{
								RecordsAffected = 0,
								Records = YieldViaConsole()
							};
			}
		}

		private static IEnumerable<IRecord> YieldViaConsole()
		{
			IRecord record;
			int recordIndex = 0;
			string line;

			while (true)
			{
				line = TextReader.ReadLine();

				if ((line ?? string.Empty).Trim() == string.Empty)
					yield break;

				record = new Record(recordIndex++)
						{
							ContextData = line
						};

				record.Add("line", line);
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
			IEnumerable<IField> columns;
			IPipelineMetadata pipelineMetadata;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			columns = new IField[] { new Field() { FieldIndex = 0, IsFieldOptional = false, FieldName = "line", FieldType = typeof(string), ContextData = null } };

			pipelineMetadata = pipelineContext.CreateMetadata(columns);
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

			sourceDataEnumerable = GetYieldViaConsole();
			pipelineMessage = pipelineContext.CreateMessage(sourceDataEnumerable);

			return pipelineMessage;
		}

		#endregion
	}
}
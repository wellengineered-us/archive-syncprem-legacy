/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;
using System.Linq;

using SyncPrem.Infrastructure.Data.Primitives;
using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configurations;
using SyncPrem.Pipeline.Abstractions.Filters.Consumer;
using SyncPrem.Pipeline.Abstractions.Messages;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Filters.Console
{
	public class ConsoleConsumerPipelineFilter : ConsumerPipelineFilter<FilterSpecificConfiguration>
	{
		#region Constructors/Destructors

		public ConsoleConsumerPipelineFilter()
		{
		}

		#endregion

		#region Fields/Constants

		private static readonly TextWriter textWriter = System.Console.Out;

		#endregion

		#region Properties/Indexers/Events

		private static TextWriter TextWriter
		{
			get
			{
				return textWriter;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void ConsumeMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration, IPipelineMessage pipelineMessage)
		{
			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)pipelineMessage == null)
				throw new ArgumentNullException(nameof(pipelineMessage));

			foreach (IResult result in pipelineMessage.Results)
			{
				foreach (IRecord record in result.Records)
				{
					string temp;
					temp = string.Format("[{0}]\t", result.ResultIndex) + string.Join("|", record.Select(kvp => string.Format("{0}={1}", kvp.Key, kvp.Value.SafeToString("null"))).ToArray());
					TextWriter.WriteLine(temp);
				}
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
			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));
		}

		#endregion
	}
}
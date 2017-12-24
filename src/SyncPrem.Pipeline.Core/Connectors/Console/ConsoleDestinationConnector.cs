/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;
using System.Linq;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Payload;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Destination;
using SyncPrem.StreamingIO.Primitives;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Core.Connectors.Console
{
	public class ConsoleDestinationConnector : DestinationConnector<StageSpecificConfiguration>
	{
		#region Constructors/Destructors

		public ConsoleDestinationConnector()
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

		protected override void ConsumeRecord(IContext context, RecordConfiguration recordConfiguration, IPipelineMessage pipelineMessage)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			if ((object)pipelineMessage == null)
				throw new ArgumentNullException(nameof(pipelineMessage));

			foreach (IRecord record in pipelineMessage.Records)
			{
				string temp;
				temp = string.Format("[{0}]\t", 0) + string.Join("|", record.Select(kvp => string.Format("{0}={1}", kvp.Key, kvp.Value.SafeToString("null"))).ToArray());
				TextWriter.WriteLine(temp);
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
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));
		}

		#endregion
	}
}
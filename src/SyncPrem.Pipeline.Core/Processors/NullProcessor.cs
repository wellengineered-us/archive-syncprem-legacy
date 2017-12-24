/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Payload;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Stage.Processor;

namespace SyncPrem.Pipeline.Core.Processors
{
	public class NullProcessor : Processor<StageSpecificConfiguration>
	{
		#region Constructors/Destructors

		public NullProcessor()
		{
		}

		#endregion

		#region Methods/Operators

		public static ProcessDelegate NullMiddlewareMethod(ProcessDelegate next)
		{
			ProcessDelegate retval;

			Console.WriteLine("NullMiddlewareMethod (before GetMiddlewareChain): '{0}'", nameof(NullProcessor));
			retval = ProcessorClosure.GetMiddlewareChain(NullProcessorMethod, next);
			Console.WriteLine("NullMiddlewareMethod (after GetMiddlewareChain): '{0}'", nameof(NullProcessor));
			return retval;
		}

		private static IPipelineMessage NullProcessorMethod(IContext ctx, RecordConfiguration cfg, IPipelineMessage msg, ProcessDelegate next)
		{
			if ((object)ctx == null)
				throw new ArgumentNullException(nameof(ctx));

			if ((object)cfg == null)
				throw new ArgumentNullException(nameof(cfg));

			if ((object)msg == null)
				throw new ArgumentNullException(nameof(msg));

			Console.WriteLine("NullProcessorMethod (before next) processor: '{0}'", nameof(NullProcessor));

			msg = next(ctx, cfg, msg);

			Console.WriteLine("NullProcessorMethod (after next) processor: '{0}'", nameof(NullProcessor));

			return msg;
		}

		protected override void Create(bool creating)
		{
			Console.WriteLine("Creating processor: '{0}'", nameof(NullProcessor));
			// do nothing
			base.Create(creating);
		}

		protected override void Dispose(bool disposing)
		{
			Console.WriteLine("Disposing processor: '{0}'", nameof(NullProcessor));
			// do nothing
			base.Dispose(disposing);
		}

		protected override void PostExecuteRecord(IContext context, RecordConfiguration recordConfiguration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			Console.WriteLine("PostExecuteRecord processor: '{0}'", nameof(NullProcessor));
		}

		protected override void PreExecuteRecord(IContext context, RecordConfiguration recordConfiguration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			Console.WriteLine("PreExecuteRecord processor: '{0}'", nameof(NullProcessor));
		}

		protected override IPipelineMessage ProcessRecord(IContext context, RecordConfiguration recordConfiguration, IPipelineMessage pipelineMessage, ProcessDelegate next)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			if ((object)pipelineMessage == null)
				throw new ArgumentNullException(nameof(pipelineMessage));

			Console.WriteLine("ProcessRecord (before next) processor: '{0}'", nameof(NullProcessor));

			pipelineMessage = next(context, recordConfiguration, pipelineMessage);

			Console.WriteLine("ProcessRecord (after next) processor: '{0}'", nameof(NullProcessor));

			return pipelineMessage;
		}

		#endregion
	}
}
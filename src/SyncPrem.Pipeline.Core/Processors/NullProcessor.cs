/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;
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

		private static IChannel NullProcessorMethod(IContext context, RecordConfiguration configuration, IChannel channel, ProcessDelegate next)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)channel == null)
				throw new ArgumentNullException(nameof(channel));

			Console.WriteLine("NullProcessorMethod (before next) processor: '{0}'", nameof(NullProcessor));

			channel = next(context, configuration, channel);

			Console.WriteLine("NullProcessorMethod (after next) processor: '{0}'", nameof(NullProcessor));

			return channel;
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

		protected override void PostExecuteRecord(IContext context, RecordConfiguration configuration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			Console.WriteLine("PostExecuteRecord processor: '{0}'", nameof(NullProcessor));
		}

		protected override void PreExecuteRecord(IContext context, RecordConfiguration configuration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			Console.WriteLine("PreExecuteRecord processor: '{0}'", nameof(NullProcessor));
		}

		protected override IChannel ProcessRecord(IContext context, RecordConfiguration configuration, IChannel channel, ProcessDelegate next)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)channel == null)
				throw new ArgumentNullException(nameof(channel));

			Console.WriteLine("ProcessRecord (before next) processor: '{0}'", nameof(NullProcessor));

			channel = next(context, configuration, channel);

			Console.WriteLine("ProcessRecord (after next) processor: '{0}'", nameof(NullProcessor));

			return channel;
		}

		#endregion
	}
}
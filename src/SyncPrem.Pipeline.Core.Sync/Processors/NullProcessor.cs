/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;
using SyncPrem.Pipeline.Abstractions.Sync.Stage.Processor;

namespace SyncPrem.Pipeline.Core.Sync.Processors
{
	public class NullProcessor : SyncProcessor<StageSpecificConfiguration>
	{
		#region Constructors/Destructors

		public NullProcessor()
		{
		}

		#endregion

		#region Methods/Operators

		public static SyncProcessDelegate NullMiddlewareMethod(SyncProcessDelegate next)
		{
			SyncProcessDelegate retval;

			Console.WriteLine("{1} (before GetMiddlewareChain): '{0}'", nameof(NullProcessor), nameof(NullMiddlewareMethod));
			retval = SyncProcessorClosure.GetMiddlewareChain(NullProcessorMethod, next);
			Console.WriteLine("{1} (after GetMiddlewareChain): '{0}'", nameof(NullProcessor), nameof(NullMiddlewareMethod));
			return retval;
		}

		private static ISyncChannel NullProcessorMethod(ISyncContext context, RecordConfiguration configuration, ISyncChannel channel, SyncProcessDelegate next)
		{
			ISyncChannel newChannel;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)channel == null)
				throw new ArgumentNullException(nameof(channel));

			Console.WriteLine("{1} (before next) processor: '{0}'", nameof(NullProcessor), nameof(NullProcessorMethod));

			if ((object)next != null)
				newChannel = next(context, configuration, channel);
			else
				newChannel = channel;

			Console.WriteLine("{1} (after next) processor: '{0}'", nameof(NullProcessor), nameof(NullProcessorMethod));

			return newChannel;
		}

		protected override void Create(bool creating)
		{
			Console.WriteLine("{1} processor: '{0}'", nameof(NullProcessor), nameof(this.Create));
			// do nothing
			base.Create(creating);
		}

		protected override void Dispose(bool disposing)
		{
			Console.WriteLine("{1} processor: '{0}'", nameof(NullProcessor), nameof(this.Dispose));
			// do nothing
			base.Dispose(disposing);
		}

		protected override void PostExecuteInternal(ISyncContext context, RecordConfiguration configuration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			Console.WriteLine("{1} processor: '{0}'", nameof(NullProcessor), nameof(this.PostExecuteInternal));
		}

		protected override void PreExecuteInternal(ISyncContext context, RecordConfiguration configuration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			Console.WriteLine("{1} processor: '{0}'", nameof(NullProcessor), nameof(this.PreExecuteInternal));
		}

		protected override ISyncChannel ProcessInternal(ISyncContext context, RecordConfiguration configuration, ISyncChannel channel, SyncProcessDelegate next)
		{
			ISyncChannel newChannel;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)channel == null)
				throw new ArgumentNullException(nameof(channel));

			Console.WriteLine("{1} (before next) processor: '{0}'", nameof(NullProcessor), nameof(this.ProcessInternal));

			if ((object)next != null)
				newChannel = next(context, configuration, channel);
			else
				newChannel = channel;

			Console.WriteLine("{1} (after next) processor: '{0}'", nameof(NullProcessor), nameof(this.ProcessInternal));

			return newChannel;
		}

		#endregion
	}
}
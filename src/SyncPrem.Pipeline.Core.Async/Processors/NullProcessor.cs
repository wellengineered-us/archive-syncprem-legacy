/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Async.Stage.Processor;
using SyncPrem.Pipeline.Abstractions.Configuration;

namespace SyncPrem.Pipeline.Core.Async.Processors
{
	public class NullProcessor : AsyncProcessor<StageSpecificConfiguration>
	{
		#region Constructors/Destructors

		public NullProcessor()
		{
		}

		#endregion

		#region Fields/Constants

		private static readonly TextWriter stdOut = Console.Out;

		#endregion

		#region Properties/Indexers/Events

		private static TextWriter StdOut
		{
			get
			{
				return stdOut;
			}
		}

		#endregion

		#region Methods/Operators

		public static AsyncProcessDelegate NullMiddlewareAsyncMethod(AsyncProcessDelegate asyncNext)
		{
			AsyncProcessDelegate retval;

			Console.WriteLine("{1} (before GetMiddlewareChain): '{0}'", nameof(NullProcessor), nameof(NullMiddlewareAsyncMethod));
			retval = AsyncProcessorClosure.GetMiddlewareChain(NullProcessorAsyncMethod, asyncNext);
			Console.WriteLine("{1} (after GetMiddlewareChain): '{0}'", nameof(NullProcessor), nameof(NullMiddlewareAsyncMethod));
			return retval;
		}

		private static async Task<IAsyncChannel> NullProcessorAsyncMethod(IAsyncContext asyncContext, RecordConfiguration configuration, IAsyncChannel asyncChannel, AsyncProcessDelegate asyncNext, CancellationToken cancellationToken)
		{
			IAsyncChannel newAsyncChannel;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)asyncChannel == null)
				throw new ArgumentNullException(nameof(asyncChannel));

			await StdOut.WriteLineAsync(string.Format("{1} (before next) processor: '{0}'", nameof(NullProcessor), nameof(NullProcessorAsyncMethod)));

			if ((object)asyncNext != null)
				newAsyncChannel = await asyncNext(asyncContext, configuration, asyncChannel, cancellationToken);
			else
				newAsyncChannel = asyncChannel;

			await StdOut.WriteLineAsync(string.Format("{1} (after next) processor: '{0}'", nameof(NullProcessor), nameof(NullProcessorAsyncMethod)));

			return newAsyncChannel;
		}

		protected override async Task CreateAsync(bool creating, CancellationToken cancellationToken)
		{
			await base.CreateAsync(creating, cancellationToken);
			await StdOut.WriteLineAsync(string.Format("{1} processor: '{0}'", nameof(NullProcessor), nameof(this.CreateAsync)));
		}

		protected override async Task DisposeAsync(bool disposing, CancellationToken cancellationToken)
		{
			await StdOut.WriteLineAsync(string.Format("{1} processor: '{0}'", nameof(NullProcessor), nameof(this.DisposeAsync)));
			await base.DisposeAsync(disposing, cancellationToken);
		}

		protected override async Task PostExecuteAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			await StdOut.WriteLineAsync(string.Format("{1} processor: '{0}'", nameof(NullProcessor), nameof(this.PostExecuteAsyncInternal)));
		}

		protected override async Task PreExecuteAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			await StdOut.WriteLineAsync(string.Format("{1} processor: '{0}'", nameof(NullProcessor), nameof(this.PreExecuteAsyncInternal)));
		}

		protected override async Task<IAsyncChannel> ProcessAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, IAsyncChannel asyncChannel, AsyncProcessDelegate asyncNext, CancellationToken cancellationToken)
		{
			IAsyncChannel newAsyncChannel;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)asyncChannel == null)
				throw new ArgumentNullException(nameof(asyncChannel));

			await StdOut.WriteLineAsync(string.Format("{1} (before next) processor: '{0}'", nameof(NullProcessor), nameof(this.ProcessAsyncInternal)));

			if ((object)asyncNext != null)
				newAsyncChannel = await asyncNext(asyncContext, configuration, asyncChannel, cancellationToken);
			else
				newAsyncChannel = asyncChannel;

			await StdOut.WriteLineAsync(string.Format("{1} (after next) processor: '{0}'", nameof(NullProcessor), nameof(this.ProcessAsyncInternal)));

			return newAsyncChannel;
		}

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Configuration;

namespace SyncPrem.Pipeline.Abstractions.Async.Stage.Processor
{
	public sealed class AsyncProcessorClosure
	{
		#region Constructors/Destructors

		private AsyncProcessorClosure(AsyncProcessToNextDelegate asyncProcessToNext, AsyncProcessDelegate asyncNext)
		{
			if ((object)asyncProcessToNext == null)
				throw new ArgumentNullException(nameof(asyncProcessToNext));

			if ((object)asyncNext == null)
				throw new ArgumentNullException(nameof(asyncNext));

			this.asyncProcessToNext = asyncProcessToNext;
			this.asyncNext = asyncNext;
		}

		#endregion

		#region Fields/Constants

		private readonly AsyncProcessDelegate asyncNext;
		private readonly AsyncProcessToNextDelegate asyncProcessToNext;

		#endregion

		#region Properties/Indexers/Events

		private AsyncProcessDelegate AsyncNext
		{
			get
			{
				return this.asyncNext;
			}
		}

		private AsyncProcessToNextDelegate AsyncProcessToNext
		{
			get
			{
				return this.asyncProcessToNext;
			}
		}

		#endregion

		#region Methods/Operators

		public static AsyncProcessDelegate GetMiddlewareChain(AsyncProcessToNextDelegate asyncProcessToNext, AsyncProcessDelegate asyncNext)
		{
			if ((object)asyncProcessToNext == null)
				throw new ArgumentNullException(nameof(asyncProcessToNext));

			if ((object)asyncNext == null)
				throw new ArgumentNullException(nameof(asyncNext));

			return new AsyncProcessorClosure(asyncProcessToNext, asyncNext).TransformAsync;
		}

		private Task<IAsyncChannel> TransformAsync(IAsyncContext asyncContext, RecordConfiguration configuration, IAsyncChannel asyncChannel, CancellationToken cancellationToken)
		{
			Console.WriteLine("voo doo!");
			return this.AsyncProcessToNext(asyncContext, configuration, asyncChannel, this.AsyncNext, cancellationToken);
		}

		#endregion
	}
}
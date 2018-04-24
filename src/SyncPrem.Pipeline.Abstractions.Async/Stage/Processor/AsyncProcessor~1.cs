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
	public abstract class AsyncProcessor<TStageSpecificConfiguration> : AsyncStage<TStageSpecificConfiguration>, IAsyncProcessor
		where TStageSpecificConfiguration : StageSpecificConfiguration, new()
	{
		#region Constructors/Destructors

		protected AsyncProcessor()
		{
		}

		#endregion

		#region Methods/Operators

		public Task<IAsyncChannel> ProcessAsync(IAsyncContext asyncContext, RecordConfiguration configuration, IAsyncChannel asyncChannel, AsyncProcessDelegate asyncNext, CancellationToken cancellationToken)
		{
			Task<IAsyncChannel> newAsyncChannelTask;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)asyncChannel == null)
				throw new ArgumentNullException(nameof(asyncChannel));

			newAsyncChannelTask = this.ProcessAsyncInternal(asyncContext, configuration, asyncChannel, asyncNext, cancellationToken);

			return newAsyncChannelTask;
		}

		protected abstract Task<IAsyncChannel> ProcessAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, IAsyncChannel asyncChannel, AsyncProcessDelegate asyncNext, CancellationToken cancellationToken);

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Configuration;

namespace SyncPrem.Pipeline.Abstractions.Async.Stage.Connector.Destination
{
	public abstract class AsyncDestinationConnector<TStageSpecificConfiguration> : AsyncStage<TStageSpecificConfiguration>, IAsyncDestinationConnector
		where TStageSpecificConfiguration : StageSpecificConfiguration, new()
	{
		#region Constructors/Destructors

		protected AsyncDestinationConnector()
		{
		}

		#endregion

		#region Methods/Operators

		public Task ConsumeAsync(IAsyncContext asyncContext, RecordConfiguration configuration, IAsyncChannel asyncChannel, CancellationToken cancellationToken)
		{
			Task task;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)asyncChannel == null)
				throw new ArgumentNullException(nameof(asyncChannel));

			task = this.ConsumeAsyncInternal(asyncContext, configuration, asyncChannel, cancellationToken);

			return task;
		}

		protected abstract Task ConsumeAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, IAsyncChannel asyncChannel, CancellationToken cancellationToken);

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Configuration;

namespace SyncPrem.Pipeline.Abstractions.Async.Stage.Connector.Source
{
	public abstract class AsyncSourceConnector<TStageSpecificConfiguration> : AsyncStage<TStageSpecificConfiguration>, IAsyncSourceConnector
		where TStageSpecificConfiguration : StageSpecificConfiguration, new()
	{
		#region Constructors/Destructors

		protected AsyncSourceConnector()
		{
		}

		#endregion

		#region Methods/Operators

		public Task<IAsyncChannel> ProduceAsync(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			Task<IAsyncChannel> newChannelTask;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			newChannelTask = this.ProduceAsyncInternal(asyncContext, configuration, cancellationToken);

			return newChannelTask;
		}

		protected abstract Task<IAsyncChannel> ProduceAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken);

		#endregion
	}
}
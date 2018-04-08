/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;

namespace SyncPrem.Pipeline.Abstractions.Stage.Connector.Destination
{
	public abstract class DestinationConnector<TStageSpecificConfiguration> : Stage<TStageSpecificConfiguration>, IDestinationConnector
		where TStageSpecificConfiguration : StageSpecificConfiguration, new()
	{
		#region Constructors/Destructors

		protected DestinationConnector()
		{
		}

		#endregion

		#region Methods/Operators

		public void Consume(IContext context, RecordConfiguration configuration, IChannel channel)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)channel == null)
				throw new ArgumentNullException(nameof(channel));

			this.ConsumeInternal(context, configuration, channel);
		}

		public Task ConsumeAsync(IContext context, RecordConfiguration configuration, IChannel channel, CancellationToken cancellationToken)
		{
			return this.ConsumeAsyncInternal(context, configuration, channel, cancellationToken, null);
		}

		protected abstract Task ConsumeAsyncInternal(IContext context, RecordConfiguration configuration, IChannel channel, CancellationToken cancellationToken, IProgress<int> progress);

		protected abstract void ConsumeInternal(IContext context, RecordConfiguration configuration, IChannel channel);

		#endregion
	}
}
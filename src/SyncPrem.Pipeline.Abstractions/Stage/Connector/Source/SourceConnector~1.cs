/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;

namespace SyncPrem.Pipeline.Abstractions.Stage.Connector.Source
{
	public abstract class SourceConnector<TStageSpecificConfiguration> : Stage<TStageSpecificConfiguration>, ISourceConnector
		where TStageSpecificConfiguration : StageSpecificConfiguration, new()
	{
		#region Constructors/Destructors

		protected SourceConnector()
		{
		}

		#endregion

		#region Methods/Operators

		public IChannel Produce(IContext context, RecordConfiguration configuration)
		{
			IChannel channel;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			channel = this.ProduceInternal(context, configuration);

			return channel;
		}

		public Task<IChannel> ProduceAsync(IContext context, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			return this.ProduceAsyncInternal(context, configuration, cancellationToken, null);
		}

		protected abstract Task<IChannel> ProduceAsyncInternal(IContext context, RecordConfiguration configuration, CancellationToken cancellationToken, IProgress<int> progress);

		protected abstract IChannel ProduceInternal(IContext context, RecordConfiguration configuration);

		#endregion
	}
}
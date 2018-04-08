/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;

namespace SyncPrem.Pipeline.Abstractions.Stage.Processor
{
	public abstract class Processor<TStageSpecificConfiguration> : Stage<TStageSpecificConfiguration>, IProcessor
		where TStageSpecificConfiguration : StageSpecificConfiguration, new()
	{
		#region Constructors/Destructors

		protected Processor()
		{
		}

		#endregion

		#region Methods/Operators

		public IChannel Process(IContext context, RecordConfiguration configuration, IChannel channel, ProcessDelegate next)
		{
			IChannel transformedStream;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)channel == null)
				throw new ArgumentNullException(nameof(channel));

			//if ((object)next == null)
			//throw new ArgumentNullException(nameof(next));

			transformedStream = this.ProcessInternal(context, configuration, channel, next);

			return transformedStream;
		}

		public Task<IChannel> ProcessAsync(IContext context, RecordConfiguration configuration, IChannel channel, ProcessDelegate next, CancellationToken cancellationToken)
		{
			return this.ProcessAsyncInternal(context, configuration, channel, next, cancellationToken, null);
		}

		protected abstract Task<IChannel> ProcessAsyncInternal(IContext context, RecordConfiguration configuration, IChannel channel, ProcessDelegate next, CancellationToken cancellationToken, IProgress<int> progress);

		protected abstract IChannel ProcessInternal(IContext context, RecordConfiguration configuration, IChannel channel, ProcessDelegate next);

		#endregion
	}
}
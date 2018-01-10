/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Channel;
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

		public IChannel Process(IContext context, RecordConfiguration recordConfiguration, IChannel channel, ProcessDelegate next)
		{
			IChannel transformedStream;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			if ((object)channel == null)
				throw new ArgumentNullException(nameof(channel));

			//if ((object)next == null)
			//throw new ArgumentNullException(nameof(next));

			transformedStream = this.ProcessRecord(context, recordConfiguration, channel, next);

			return transformedStream;
		}

		protected abstract IChannel ProcessRecord(IContext context, RecordConfiguration recordConfiguration, IChannel channel, ProcessDelegate next);

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;

namespace SyncPrem.Pipeline.Abstractions.Sync.Stage.Processor
{
	public abstract class SyncProcessor<TStageSpecificConfiguration> : SyncStage<TStageSpecificConfiguration>, ISyncProcessor
		where TStageSpecificConfiguration : StageSpecificConfiguration, new()
	{
		#region Constructors/Destructors

		protected SyncProcessor()
		{
		}

		#endregion

		#region Methods/Operators

		public ISyncChannel Process(ISyncContext context, RecordConfiguration configuration, ISyncChannel channel, SyncProcessDelegate next)
		{
			ISyncChannel newChannel;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)channel == null)
				throw new ArgumentNullException(nameof(channel));

			newChannel = this.ProcessInternal(context, configuration, channel, next);

			return newChannel;
		}

		protected abstract ISyncChannel ProcessInternal(ISyncContext context, RecordConfiguration configuration, ISyncChannel channel, SyncProcessDelegate next);

		#endregion
	}
}
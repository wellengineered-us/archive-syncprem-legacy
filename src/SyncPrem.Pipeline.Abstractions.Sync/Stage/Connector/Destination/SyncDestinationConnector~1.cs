/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;

namespace SyncPrem.Pipeline.Abstractions.Sync.Stage.Connector.Destination
{
	public abstract class SyncDestinationConnector<TStageSpecificConfiguration> : SyncStage<TStageSpecificConfiguration>, ISyncDestinationConnector
		where TStageSpecificConfiguration : StageSpecificConfiguration, new()
	{
		#region Constructors/Destructors

		protected SyncDestinationConnector()
		{
		}

		#endregion

		#region Methods/Operators

		public void Consume(ISyncContext context, RecordConfiguration configuration, ISyncChannel channel)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)channel == null)
				throw new ArgumentNullException(nameof(channel));

			this.ConsumeInternal(context, configuration, channel);
		}

		protected abstract void ConsumeInternal(ISyncContext context, RecordConfiguration configuration, ISyncChannel channel);

		#endregion
	}
}
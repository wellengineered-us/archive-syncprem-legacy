/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;

namespace SyncPrem.Pipeline.Abstractions.Sync.Stage.Connector.Source
{
	public abstract class SyncSourceConnector<TStageSpecificConfiguration> : SyncStage<TStageSpecificConfiguration>, ISyncSourceConnector
		where TStageSpecificConfiguration : StageSpecificConfiguration, new()
	{
		#region Constructors/Destructors

		protected SyncSourceConnector()
		{
		}

		#endregion

		#region Methods/Operators

		public ISyncChannel Produce(ISyncContext context, RecordConfiguration configuration)
		{
			ISyncChannel channel;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			channel = this.ProduceInternal(context, configuration);

			return channel;
		}

		protected abstract ISyncChannel ProduceInternal(ISyncContext context, RecordConfiguration configuration);

		#endregion
	}
}
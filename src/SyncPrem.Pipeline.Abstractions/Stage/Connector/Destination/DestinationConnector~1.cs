/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Payload;
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

		public void Consume(IContext context, RecordConfiguration recordConfiguration, IPipelineMessage pipelineMessage)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			if ((object)pipelineMessage == null)
				throw new ArgumentNullException(nameof(pipelineMessage));

			this.ConsumeRecord(context, recordConfiguration, pipelineMessage);
		}

		protected abstract void ConsumeRecord(IContext context, RecordConfiguration recordConfiguration, IPipelineMessage pipelineMessage);

		#endregion
	}
}
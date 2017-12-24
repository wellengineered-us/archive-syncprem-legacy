/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Payload;
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

		public IPipelineMessage Produce(IContext context, RecordConfiguration recordConfiguration)
		{
			IPipelineMessage pipelineMessage;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			pipelineMessage = this.ProduceRecord(context, recordConfiguration);

			return pipelineMessage;
		}

		protected abstract IPipelineMessage ProduceRecord(IContext context, RecordConfiguration recordConfiguration);

		#endregion
	}
}
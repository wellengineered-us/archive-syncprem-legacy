/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Payload;
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

		public IPipelineMessage Process(IContext context, RecordConfiguration recordConfiguration, IPipelineMessage pipelineMessage, ProcessDelegate next)
		{
			IPipelineMessage transformedPipelineMessage;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			if ((object)pipelineMessage == null)
				throw new ArgumentNullException(nameof(pipelineMessage));

			//if ((object)next == null)
			//throw new ArgumentNullException(nameof(next));

			transformedPipelineMessage = this.ProcessRecord(context, recordConfiguration, pipelineMessage, next);

			return transformedPipelineMessage;
		}

		protected abstract IPipelineMessage ProcessRecord(IContext context, RecordConfiguration recordConfiguration, IPipelineMessage pipelineMessage, ProcessDelegate next);

		#endregion
	}
}
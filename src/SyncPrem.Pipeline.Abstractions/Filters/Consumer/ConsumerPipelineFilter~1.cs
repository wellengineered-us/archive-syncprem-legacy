/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configurations;
using SyncPrem.Pipeline.Abstractions.Messages;

namespace SyncPrem.Pipeline.Abstractions.Filters.Consumer
{
	public abstract class ConsumerPipelineFilter<TFilterSpecificConfiguration> : PipelineFilter<TFilterSpecificConfiguration>, IConsumerPipelineFilter
		where TFilterSpecificConfiguration : FilterSpecificConfiguration, new()
	{
		#region Constructors/Destructors

		protected ConsumerPipelineFilter()
		{
		}

		#endregion

		#region Methods/Operators

		public void Consume(IPipelineContext pipelineContext, TableConfiguration tableConfiguration, IPipelineMessage pipelineMessage)
		{
			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)pipelineMessage == null)
				throw new ArgumentNullException(nameof(pipelineMessage));

			this.ConsumeMessage(pipelineContext, tableConfiguration, pipelineMessage);
		}

		protected abstract void ConsumeMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration, IPipelineMessage pipelineMessage);

		#endregion
	}
}
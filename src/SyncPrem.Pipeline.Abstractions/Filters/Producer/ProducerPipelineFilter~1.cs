/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configurations;
using SyncPrem.Pipeline.Abstractions.Messages;
using SyncPrem.Pipeline.Abstractions.Pipes;

namespace SyncPrem.Pipeline.Abstractions.Filters.Producer
{
	public abstract class ProducerPipelineFilter<TFilterSpecificConfiguration> : PipelineFilter<TFilterSpecificConfiguration>, IProducerPipelineFilter
		where TFilterSpecificConfiguration : FilterSpecificConfiguration, new()
	{
		#region Constructors/Destructors

		protected ProducerPipelineFilter()
		{
		}

		#endregion

		#region Methods/Operators

		public IPipelineMessage Produce(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			IPipelineMessage pipelineMessage;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			pipelineMessage = this.ProduceMessage(pipelineContext, tableConfiguration);

			return pipelineMessage;
		}

		protected abstract IPipelineMessage ProduceMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration);

		public void Write(IPipe pipe)
		{
			if ((object)pipe == null)
				throw new ArgumentNullException(nameof(pipe));
		}

		#endregion
	}
}
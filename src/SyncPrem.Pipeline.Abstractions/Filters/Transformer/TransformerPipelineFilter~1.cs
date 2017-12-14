/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configurations;
using SyncPrem.Pipeline.Abstractions.Messages;

namespace SyncPrem.Pipeline.Abstractions.Filters.Transformer
{
	public delegate IPipelineMessage TransformDelegate(IPipelineContext pipelineContext, TableConfiguration tableConfiguration, IPipelineMessage pipelineMessage);

	public delegate IPipelineMessage TransformWithNextDelegate(IPipelineContext pipelineContext, TableConfiguration tableConfiguration, IPipelineMessage pipelineMessage, TransformDelegate next);

	public abstract class TransformerPipelineFilter<TFilterSpecificConfiguration> : PipelineFilter<TFilterSpecificConfiguration>, ITransformerPipelineFilter
		where TFilterSpecificConfiguration : FilterSpecificConfiguration, new()
	{
		#region Constructors/Destructors

		protected TransformerPipelineFilter()
		{
		}

		#endregion

		#region Methods/Operators

		public IPipelineMessage Transform(IPipelineContext pipelineContext, TableConfiguration tableConfiguration, IPipelineMessage pipelineMessage, TransformDelegate next)
		{
			IPipelineMessage transformedPipelineMessage;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)pipelineMessage == null)
				throw new ArgumentNullException(nameof(pipelineMessage));

			//if ((object)next == null)
			//throw new ArgumentNullException(nameof(next));

			transformedPipelineMessage = this.TransformMessage(pipelineContext, tableConfiguration, pipelineMessage, next);

			return transformedPipelineMessage;
		}

		protected abstract IPipelineMessage TransformMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration, IPipelineMessage pipelineMessage, TransformDelegate next);

		#endregion
	}
}
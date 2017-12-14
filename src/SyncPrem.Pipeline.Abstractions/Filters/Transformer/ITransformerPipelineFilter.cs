/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configurations;
using SyncPrem.Pipeline.Abstractions.Messages;

namespace SyncPrem.Pipeline.Abstractions.Filters.Transformer
{
	public interface ITransformerPipelineFilter : IPipelineFilter
	{
		#region Methods/Operators

		IPipelineMessage Transform(IPipelineContext pipelineContext, TableConfiguration tableConfiguration, IPipelineMessage pipelineMessage, TransformDelegate next);

		#endregion
	}
}
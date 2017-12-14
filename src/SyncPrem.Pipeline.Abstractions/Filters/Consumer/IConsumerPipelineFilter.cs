/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configurations;
using SyncPrem.Pipeline.Abstractions.Messages;

namespace SyncPrem.Pipeline.Abstractions.Filters.Consumer
{
	public interface IConsumerPipelineFilter : IPipelineFilter
	{
		#region Methods/Operators

		void Consume(IPipelineContext pipelineContext, TableConfiguration tableConfiguration, IPipelineMessage pipelineMessage);

		#endregion
	}
}
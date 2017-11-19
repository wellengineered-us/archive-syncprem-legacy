/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configurations;

namespace SyncPrem.Pipeline.Abstractions.Filters
{
	public interface IPipelineFilter : IPipelineComponent
	{
		#region Properties/Indexers/Events

		Type FilterSpecificConfigurationType
		{
			get;
		}

		FilterConfiguration FilterConfiguration
		{
			get;
			set;
		}

		#endregion

		#region Methods/Operators

		void PostProcess(IPipelineContext pipelineContext, TableConfiguration tableConfiguration);

		void PreProcess(IPipelineContext pipelineContext, TableConfiguration tableConfiguration);

		#endregion
	}
}
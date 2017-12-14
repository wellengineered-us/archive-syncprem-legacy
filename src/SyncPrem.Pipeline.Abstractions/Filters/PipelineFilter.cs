/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configurations;

namespace SyncPrem.Pipeline.Abstractions.Filters
{
	public delegate void ProcessDelegate(IPipelineContext pipelineContext, TableConfiguration tableConfiguration);

	public abstract class PipelineFilter : PipelineComponent, IPipelineFilter
	{
		#region Constructors/Destructors

		protected PipelineFilter()
		{
		}

		#endregion

		#region Fields/Constants

		private FilterConfiguration filterConfiguration;

		#endregion

		#region Properties/Indexers/Events

		public abstract Type FilterSpecificConfigurationType
		{
			get;
		}

		public FilterConfiguration FilterConfiguration
		{
			get
			{
				return this.filterConfiguration;
			}
			set
			{
				this.filterConfiguration = value;
			}
		}

		#endregion

		#region Methods/Operators

		public void PostProcess(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			this.PostProcessMessage(pipelineContext, tableConfiguration);
		}

		protected abstract void PostProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration);

		public void PreProcess(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			this.PreProcessMessage(pipelineContext, tableConfiguration);
		}

		protected abstract void PreProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration);

		#endregion
	}
}
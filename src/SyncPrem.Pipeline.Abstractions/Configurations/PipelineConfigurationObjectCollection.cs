/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using SyncPrem.Infrastructure.Configuration;

namespace SyncPrem.Pipeline.Abstractions.Configurations
{
	public class PipelineConfigurationObjectCollection : ConfigurationObjectCollection<PipelineConfigurationObject>, IPipelineConfigurationObjectCollection
	{
		#region Constructors/Destructors

		public PipelineConfigurationObjectCollection(IPipelineConfigurationObject site)
			: base(site)
		{
		}

		#endregion
	}
}
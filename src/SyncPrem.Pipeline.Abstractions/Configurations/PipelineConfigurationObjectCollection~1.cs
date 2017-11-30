/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using SyncPrem.Infrastructure.Configuration;

namespace SyncPrem.Pipeline.Abstractions.Configurations
{
	public class PipelineConfigurationObjectCollection<TConfigurationObject> : ConfigurationObjectCollection<TConfigurationObject>, IPipelineConfigurationObjectCollection<TConfigurationObject>, IPipelineConfigurationObjectCollection
		where TConfigurationObject : IPipelineConfigurationObject
	{
		#region Constructors/Destructors

		public PipelineConfigurationObjectCollection(IPipelineConfigurationObject site)
			: base(site)
		{
		}

		#endregion

		#region Properties/Indexers/Events

		[ConfigurationIgnore]
		public new IPipelineConfigurationObject Site
		{
			get
			{
				return (IPipelineConfigurationObject)base.Site;
			}
		}

		#endregion
	}
}
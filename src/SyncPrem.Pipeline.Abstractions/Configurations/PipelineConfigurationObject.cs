/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Infrastructure.Configuration;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Pipeline.Abstractions.Configurations
{
	public abstract class PipelineConfigurationObject : ConfigurationObject, IPipelineConfigurationObject
	{
		#region Constructors/Destructors

		protected PipelineConfigurationObject()
		{
		}

		#endregion

		#region Properties/Indexers/Events

		[ConfigurationIgnore]
		public new IPipelineConfigurationObject Parent
		{
			get
			{
				return (IPipelineConfigurationObject)base.Parent;
			}
			set
			{
				base.Parent = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected static Type GetTypeFromString(string aqtn)
		{
			Type type;

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(aqtn))
				return null;

			type = Type.GetType(aqtn, false);

			return type;
		}

		#endregion
	}
}
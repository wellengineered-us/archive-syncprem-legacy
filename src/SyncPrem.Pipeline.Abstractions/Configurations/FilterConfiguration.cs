/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using SyncPrem.Infrastructure.Configuration;
using SyncPrem.Pipeline.Abstractions.Filters;

using TextMetal.Middleware.Solder.Extensions;
using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Abstractions.Configurations
{
	public class FilterConfiguration : PipelineConfigurationObject
	{
		#region Constructors/Destructors

		public FilterConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly Dictionary<string, object> filterSpecificConfiguration = new Dictionary<string, object>();
		private string filterAqtn;
		private FilterSpecificConfiguration untypedFilterSpecificConfiguration;

		#endregion

		#region Properties/Indexers/Events

		public Dictionary<string, object> FilterSpecificConfiguration
		{
			get
			{
				return this.filterSpecificConfiguration;
			}
		}

		public string FilterAqtn
		{
			get
			{
				return this.filterAqtn;
			}
			set
			{
				this.filterAqtn = value;
			}
		}

		[ConfigurationIgnore]
		public new IFilterConfigurationDependency Parent
		{
			get
			{
				return (IFilterConfigurationDependency)base.Parent;
			}
			set
			{
				base.Parent = value;
			}
		}

		[ConfigurationIgnore]
		private FilterSpecificConfiguration UntypedFilterSpecificConfiguration
		{
			get
			{
				this.ApplyFilterSpecificConfiguration(); // special case
				return this.untypedFilterSpecificConfiguration;
			}
			set
			{
				this.untypedFilterSpecificConfiguration = value;
			}
		}

		#endregion

		#region Methods/Operators

		public virtual void ApplyFilterSpecificConfiguration()
		{
			this.ApplyFilterSpecificConfiguration(this.GetFilterSpecificConfigurationType());
		}

		private void ApplyFilterSpecificConfiguration(Type type)
		{
			if ((object)this.FilterSpecificConfiguration != null)
			{
				this.UntypedFilterSpecificConfiguration = (FilterSpecificConfiguration)JObject.FromObject(this.FilterSpecificConfiguration).ToObject(type);
			}
		}

		public virtual Type GetFilterSpecificConfigurationType()
		{
			return typeof(FilterSpecificConfiguration);
		}

		public Type GetFilterType()
		{
			return GetTypeFromString(this.FilterAqtn);
		}

		public virtual void ResetFilterSpecificConfiguration()
		{
			this.FilterSpecificConfiguration.Clear();
			this.UntypedFilterSpecificConfiguration = null;
		}

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;
			Type filterType;
			IPipelineFilter pipelineFilter;
			string filterContext;

			filterContext = context as string;
			messages = new List<Message>();

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(this.FilterAqtn))
				messages.Add(NewError(string.Format("{0} filter AQTN is required.", filterContext)));
			else
			{
				filterType = this.GetFilterType();

				if ((object)filterType == null)
					messages.Add(NewError(string.Format("{0} filter failed to load type from AQTN.", filterContext)));
				else if (!typeof(IPipelineFilter).IsAssignableFrom(filterType))
					messages.Add(NewError(string.Format("{0} filter loaded an unrecognized type via AQTN.", filterContext)));
				else
				{
					// new-ing up via default public contructor should be low friction
					pipelineFilter = (IPipelineFilter)Activator.CreateInstance(filterType);

					if ((object)pipelineFilter == null)
						messages.Add(NewError(string.Format("{0} filter failed to instatiate type from AQTN.", filterContext)));
					else
					{
						this.ApplyFilterSpecificConfiguration(pipelineFilter.FilterSpecificConfigurationType);
						messages.AddRange(this.ValidateFilterSpecificConfiguration(filterContext));
					}
				}
			}

			return messages;
		}

		public virtual IEnumerable<Message> ValidateFilterSpecificConfiguration(object context)
		{
			if ((object)this.FilterSpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(this.FilterSpecificConfiguration)));

			if ((object)this.UntypedFilterSpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(this.UntypedFilterSpecificConfiguration)));

			return this.UntypedFilterSpecificConfiguration.Validate(context);
		}

		#endregion
	}
}
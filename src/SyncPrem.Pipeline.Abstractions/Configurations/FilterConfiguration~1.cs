/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Abstractions.Configurations
{
	public class FilterConfiguration<TFilterSpecificConfiguration> : FilterConfiguration
		where TFilterSpecificConfiguration : FilterSpecificConfiguration, new()
	{
		#region Constructors/Destructors

		public FilterConfiguration(FilterConfiguration filterConfiguration)
		{
			if ((object)filterConfiguration == null)
				throw new ArgumentNullException(nameof(filterConfiguration));

			if ((object)base.FilterSpecificConfiguration != null &&
				(object)filterConfiguration.FilterSpecificConfiguration != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair in filterConfiguration.FilterSpecificConfiguration)
					base.FilterSpecificConfiguration.Add(keyValuePair.Key, keyValuePair.Value);
			}

			this.FilterAqtn = filterConfiguration.FilterAqtn;
			this.Parent = filterConfiguration.Parent;
			this.Surround = filterConfiguration.Surround;
		}

		#endregion

		#region Fields/Constants

		private TFilterSpecificConfiguration filterSpecificConfiguration;

		#endregion

		#region Properties/Indexers/Events

		public new TFilterSpecificConfiguration FilterSpecificConfiguration
		{
			get
			{
				this.ApplyFilterSpecificConfiguration(); // special case
				return this.filterSpecificConfiguration;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.filterSpecificConfiguration, value);
				this.filterSpecificConfiguration = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override void ApplyFilterSpecificConfiguration()
		{
			if ((object)base.FilterSpecificConfiguration != null)
			{
				this.FilterSpecificConfiguration = JObject.FromObject(base.FilterSpecificConfiguration).ToObject<TFilterSpecificConfiguration>();
			}
		}

		public override Type GetFilterSpecificConfigurationType()
		{
			return typeof(TFilterSpecificConfiguration);
		}

		public override void ResetFilterSpecificConfiguration()
		{
			base.ResetFilterSpecificConfiguration();
			this.FilterSpecificConfiguration = null;
		}

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;
			string filterContext;

			filterContext = context as string;
			messages = new List<Message>();
			messages.AddRange(base.Validate(filterContext));

			if ((object)this.FilterSpecificConfiguration != null)
				messages.AddRange(this.ValidateFilterSpecificConfiguration(filterContext));

			return messages;
		}

		public override IEnumerable<Message> ValidateFilterSpecificConfiguration(object context)
		{
			if ((object)this.FilterSpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(this.FilterSpecificConfiguration)));

			return this.FilterSpecificConfiguration.Validate(context);
		}

		#endregion
	}
}
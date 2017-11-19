/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Configuration;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Abstractions.Configurations
{
	public class RealPipelineConfiguration : PipelineConfigurationObject, IFilterConfigurationDependency
	{
		#region Constructors/Destructors

		public RealPipelineConfiguration()
		{
			this.transformerFilterConfigurations = new ConfigurationObjectCollection<FilterConfiguration>(this);
		}

		#endregion

		#region Fields/Constants

		private readonly ConfigurationObjectCollection<FilterConfiguration> transformerFilterConfigurations;
		private FilterConfiguration consumerFilterConfiguration;
		private string pipelineAqtn;
		private FilterConfiguration producerFilterConfiguration;

		private TableConfiguration tableConfiguration;

		#endregion

		#region Properties/Indexers/Events

		public ConfigurationObjectCollection<FilterConfiguration> TransformerFilterConfigurations
		{
			get
			{
				return this.transformerFilterConfigurations;
			}
		}

		public FilterConfiguration ConsumerFilterConfiguration
		{
			get
			{
				return this.consumerFilterConfiguration;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.consumerFilterConfiguration, value);
				this.consumerFilterConfiguration = value;
			}
		}

		public string PipelineAqtn
		{
			get
			{
				return this.pipelineAqtn;
			}
			set
			{
				this.pipelineAqtn = value;
			}
		}

		public FilterConfiguration ProducerFilterConfiguration
		{
			get
			{
				return this.producerFilterConfiguration;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.producerFilterConfiguration, value);
				this.producerFilterConfiguration = value;
			}
		}

		public TableConfiguration TableConfiguration
		{
			get
			{
				return this.tableConfiguration;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.tableConfiguration, value);
				this.tableConfiguration = value;
			}
		}

		#endregion

		#region Methods/Operators

		public Type GetPipelineType()
		{
			return GetTypeFromString(this.PipelineAqtn);
		}

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;

			messages = new List<Message>();

			if ((object)this.ProducerFilterConfiguration == null)
				messages.Add(NewError("ProducerFilterConfiguration is required."));
			else
				messages.AddRange(this.ProducerFilterConfiguration.Validate("PRODUCER"));

			if ((object)this.ConsumerFilterConfiguration == null)
				messages.Add(NewError("ConsumerFilterConfiguration is required."));
			else
				messages.AddRange(this.ConsumerFilterConfiguration.Validate("CONSUMER"));

			return messages;
		}

		#endregion
	}
}
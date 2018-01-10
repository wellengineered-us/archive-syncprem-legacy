/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Configuration;

using _Message = TextMetal.Middleware.Solder.Primitives.Message;

namespace SyncPrem.Pipeline.Abstractions.Configuration
{
	public class PipelineConfiguration : ConfigurationObject
	{
		#region Constructors/Destructors

		public PipelineConfiguration()
		{
			this.processorConfigurations = new ConfigurationObjectCollection<StageConfiguration>(this);
		}

		#endregion

		#region Fields/Constants

		private readonly ConfigurationObjectCollection<StageConfiguration> processorConfigurations;
		private StageConfiguration destinationConfiguration;
		private string pipelineAqtn;
		private RecordConfiguration recordConfiguration;
		private StageConfiguration sourceConfiguration;

		#endregion

		#region Properties/Indexers/Events

		public ConfigurationObjectCollection<StageConfiguration> ProcessorConfigurations
		{
			get
			{
				return this.processorConfigurations;
			}
		}

		public StageConfiguration DestinationConfiguration
		{
			get
			{
				return this.destinationConfiguration;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.destinationConfiguration, value);
				this.destinationConfiguration = value;
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

		public RecordConfiguration RecordConfiguration
		{
			get
			{
				return this.recordConfiguration;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.recordConfiguration, value);
				this.recordConfiguration = value;
			}
		}

		public StageConfiguration SourceConfiguration
		{
			get
			{
				return this.sourceConfiguration;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.sourceConfiguration, value);
				this.sourceConfiguration = value;
			}
		}

		#endregion

		#region Methods/Operators

		public Type GetPipelineType()
		{
			return GetTypeFromString(this.PipelineAqtn);
		}

		public override IEnumerable<_Message> Validate(object context)
		{
			List<_Message> messages;

			messages = new List<_Message>();

			if ((object)this.SourceConfiguration == null)
				messages.Add(NewError("SourceConfiguration is required."));
			else
				messages.AddRange(this.SourceConfiguration.Validate("PRODUCER"));

			if ((object)this.DestinationConfiguration == null)
				messages.Add(NewError("DestinationConfiguration is required."));
			else
				messages.AddRange(this.DestinationConfiguration.Validate("CONSUMER"));

			return messages;
		}

		#endregion
	}
}
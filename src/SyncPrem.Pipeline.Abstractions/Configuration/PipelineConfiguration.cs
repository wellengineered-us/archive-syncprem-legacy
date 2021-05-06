/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using SyncPrem.Infrastructure.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;

using TextMetal.Middleware.Solder.Extensions;

using _Message = TextMetal.Middleware.Solder.Primitives.Message;

namespace SyncPrem.Pipeline.Abstractions.Configuration
{
	public class PipelineConfiguration : ComponentConfiguration
	{
		#region Constructors/Destructors

		public PipelineConfiguration()
		{
			this.processorConfigurations = new ConfigurationObjectCollection<StageConfiguration>(this);
		}

		#endregion

		#region Fields/Constants

		private readonly ConfigurationObjectCollection<StageConfiguration> processorConfigurations;

		private string contextAqtn;
		private StageConfiguration destinationConfiguration;
		private bool? isEnabled;
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

		public string ContextAqtn
		{
			get
			{
				return this.contextAqtn;
			}
			set
			{
				this.contextAqtn = value;
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

		public bool? IsEnabled
		{
			get
			{
				return this.isEnabled;
			}
			set
			{
				this.isEnabled = value;
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

		public Type GetContextType(IList<_Message> messages = null)
		{
			return GetTypeFromString(this.ContextAqtn, messages);
		}

		public Type GetPipelineType(IList<_Message> messages = null)
		{
			return GetTypeFromString(this.PipelineAqtn, messages);
		}

		public override IEnumerable<_Message> Validate(object context)
		{
			List<_Message> messages;

			Type contextType;
			Type pipelineType;

			IContext context_;
			IPipeline pipeline;

			messages = new List<_Message>();

			if (string.IsNullOrWhiteSpace(this.ContextAqtn))
				messages.Add(NewError(string.Format("{0} pipeline context is required.", context)));
			else
			{
				contextType = this.GetContextType(messages);

				if ((object)contextType == null)
					messages.Add(NewError(string.Format("{0} pipeline context failed to load type from AQTN.", context)));
				else if (!typeof(IContext).IsAssignableFrom(contextType))
					messages.Add(NewError(string.Format("{0} pipeline context loaded an unrecognized type via AQTN.", context)));
				else
				{
					// new-ing up via default public contructor should be low friction
					context_ = (IContext)Activator.CreateInstance(contextType);

					if ((object)context_ == null)
						messages.Add(NewError(string.Format("{0} pipeline context failed to instatiate type from AQTN.", context)));
				}
			}

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(this.PipelineAqtn))
				messages.Add(NewError(string.Format("{0} adapter serialization strategy is required.", context)));
			else
			{
				pipelineType = this.GetPipelineType(messages);

				if ((object)pipelineType == null)
					messages.Add(NewError(string.Format("{0} adapter serialization strategy failed to load type from AQTN.", context)));
				else if (!typeof(IPipeline).IsAssignableFrom(pipelineType))
					messages.Add(NewError(string.Format("{0} adapter serialization strategy loaded an unrecognized type via AQTN.", context)));
				else
				{
					// new-ing up via default public contructor should be low friction
					pipeline = (IPipeline)Activator.CreateInstance(pipelineType);

					if ((object)pipeline == null)
						messages.Add(NewError(string.Format("{0} adapter serialization strategy failed to instatiate type from AQTN.", context)));
				}
			}

			if ((object)this.SourceConfiguration == null)
				messages.Add(NewError("SourceConfiguration is required."));
			else
				messages.AddRange(this.SourceConfiguration.Validate("SOURCE"));

			messages.AddRange(this.ProcessorConfigurations.SelectMany(pc => pc.Validate("PROCESSOR")));

			if ((object)this.DestinationConfiguration == null)
				messages.Add(NewError("DestinationConfiguration is required."));
			else
				messages.AddRange(this.DestinationConfiguration.Validate("DESTINATION"));

			return messages;
		}

		#endregion
	}
}
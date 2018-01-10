/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using SyncPrem.Infrastructure.Configuration;
using SyncPrem.Pipeline.Abstractions.Stage;

using TextMetal.Middleware.Solder.Extensions;

using _Message = TextMetal.Middleware.Solder.Primitives.Message;

namespace SyncPrem.Pipeline.Abstractions.Configuration
{
	public class StageConfiguration : ConfigurationObject
	{
		#region Constructors/Destructors

		public StageConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly Dictionary<string, object> stageSpecificConfiguration = new Dictionary<string, object>();
		private string stageAqtn;
		private StageSpecificConfiguration untypedStageSpecificConfiguration;

		#endregion

		#region Properties/Indexers/Events

		public Dictionary<string, object> StageSpecificConfiguration
		{
			get
			{
				return this.stageSpecificConfiguration;
			}
		}

		public string StageAqtn
		{
			get
			{
				return this.stageAqtn;
			}
			set
			{
				this.stageAqtn = value;
			}
		}

		[ConfigurationIgnore]
		private StageSpecificConfiguration UntypedStageSpecificConfiguration
		{
			get
			{
				this.ApplyStageSpecificConfiguration(); // special case
				return this.untypedStageSpecificConfiguration;
			}
			set
			{
				this.untypedStageSpecificConfiguration = value;
			}
		}

		#endregion

		#region Methods/Operators

		public virtual void ApplyStageSpecificConfiguration()
		{
			this.ApplyStageSpecificConfiguration(this.GetStageSpecificConfigurationType());
		}

		private void ApplyStageSpecificConfiguration(Type type)
		{
			if ((object)this.StageSpecificConfiguration != null)
			{
				this.UntypedStageSpecificConfiguration = (StageSpecificConfiguration)JObject.FromObject(this.StageSpecificConfiguration).ToObject(type);
			}
		}

		public virtual Type GetStageSpecificConfigurationType()
		{
			return typeof(StageSpecificConfiguration);
		}

		public Type GetStageType()
		{
			return GetTypeFromString(this.StageAqtn);
		}

		public virtual void ResetStageSpecificConfiguration()
		{
			this.StageSpecificConfiguration.Clear();
			this.UntypedStageSpecificConfiguration = null;
		}

		public override IEnumerable<_Message> Validate(object context)
		{
			List<_Message> messages;
			Type stageType;
			IStage stage;
			string stageContext;

			stageContext = context as string;
			messages = new List<_Message>();

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(this.StageAqtn))
				messages.Add(NewError(string.Format("{0} stage AQTN is required.", stageContext)));
			else
			{
				stageType = this.GetStageType();

				if ((object)stageType == null)
					messages.Add(NewError(string.Format("{0} stage failed to load type from AQTN.", stageContext)));
				else if (!typeof(IStage).IsAssignableFrom(stageType))
					messages.Add(NewError(string.Format("{0} stage loaded an unrecognized type via AQTN.", stageContext)));
				else
				{
					// new-ing up via default public contructor should be low friction
					stage = (IStage)Activator.CreateInstance(stageType);

					if ((object)stage == null)
						messages.Add(NewError(string.Format("{0} stage failed to instatiate type from AQTN.", stageContext)));
					else
					{
						this.ApplyStageSpecificConfiguration(stage.StageSpecificConfigurationType);
						messages.AddRange(this.ValidateStageSpecificConfiguration(stageContext));
					}
				}
			}

			return messages;
		}

		public virtual IEnumerable<_Message> ValidateStageSpecificConfiguration(object context)
		{
			if ((object)this.StageSpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(this.StageSpecificConfiguration)));

			if ((object)this.UntypedStageSpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(this.UntypedStageSpecificConfiguration)));

			return this.UntypedStageSpecificConfiguration.Validate(context);
		}

		#endregion
	}
}
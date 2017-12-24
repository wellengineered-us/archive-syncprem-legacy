/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Abstractions.Configuration
{
	public class StageConfiguration<TStageSpecificConfiguration> : StageConfiguration
		where TStageSpecificConfiguration : StageSpecificConfiguration, new()
	{
		#region Constructors/Destructors

		public StageConfiguration(StageConfiguration stageConfiguration)
		{
			if ((object)stageConfiguration == null)
				throw new ArgumentNullException(nameof(stageConfiguration));

			if ((object)base.StageSpecificConfiguration != null &&
				(object)stageConfiguration.StageSpecificConfiguration != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair in stageConfiguration.StageSpecificConfiguration)
					base.StageSpecificConfiguration.Add(keyValuePair.Key, keyValuePair.Value);
			}

			this.StageAqtn = stageConfiguration.StageAqtn;
			this.Parent = stageConfiguration.Parent;
			this.Surround = stageConfiguration.Surround;
		}

		#endregion

		#region Fields/Constants

		private TStageSpecificConfiguration stageSpecificConfiguration;

		#endregion

		#region Properties/Indexers/Events

		public new TStageSpecificConfiguration StageSpecificConfiguration
		{
			get
			{
				this.ApplyStageSpecificConfiguration(); // special case
				return this.stageSpecificConfiguration;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.stageSpecificConfiguration, value);
				this.stageSpecificConfiguration = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override void ApplyStageSpecificConfiguration()
		{
			if ((object)base.StageSpecificConfiguration != null)
			{
				this.StageSpecificConfiguration = JObject.FromObject(base.StageSpecificConfiguration).ToObject<TStageSpecificConfiguration>();
			}
		}

		public override Type GetStageSpecificConfigurationType()
		{
			return typeof(TStageSpecificConfiguration);
		}

		public override void ResetStageSpecificConfiguration()
		{
			base.ResetStageSpecificConfiguration();
			this.StageSpecificConfiguration = null;
		}

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;
			string stageContext;

			stageContext = context as string;
			messages = new List<Message>();
			messages.AddRange(base.Validate(stageContext));

			if ((object)this.StageSpecificConfiguration != null)
				messages.AddRange(this.ValidateStageSpecificConfiguration(stageContext));

			return messages;
		}

		public override IEnumerable<Message> ValidateStageSpecificConfiguration(object context)
		{
			if ((object)this.StageSpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(this.StageSpecificConfiguration)));

			return this.StageSpecificConfiguration.Validate(context);
		}

		#endregion
	}
}
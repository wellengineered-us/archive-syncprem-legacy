/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using _Message = TextMetal.Middleware.Solder.Primitives.Message;

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

		private void ApplyStageSpecificConfiguration()
		{
			if ((object)base.StageSpecificConfiguration != null)
			{
				this.StageSpecificConfiguration = JObject.FromObject(base.StageSpecificConfiguration).ToObject<TStageSpecificConfiguration>();
			}
		}

		public override IEnumerable<_Message> Validate(object context)
		{
			List<_Message> messages;
			string stageContext;

			stageContext = context as string;
			messages = new List<_Message>();
			messages.AddRange(base.Validate(stageContext));

			if ((object)this.StageSpecificConfiguration != null)
				messages.AddRange(this.StageSpecificConfiguration.Validate(stageContext));

			return messages;
		}

		#endregion
	}
}
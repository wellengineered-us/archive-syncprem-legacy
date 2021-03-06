/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions.Stage;

using TextMetal.Middleware.Solder.Extensions;

using _Message = TextMetal.Middleware.Solder.Primitives.Message;

namespace SyncPrem.Pipeline.Abstractions.Configuration
{
	public class StageConfiguration : ComponentConfiguration
	{
		#region Constructors/Destructors

		public StageConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly Dictionary<string, object> stageSpecificConfiguration = new Dictionary<string, object>();
		private string stageAqtn;

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

		#endregion

		#region Methods/Operators

		public Type GetStageType(IList<_Message> messages = null)
		{
			return GetTypeFromString(this.StageAqtn, messages);
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
				stageType = this.GetStageType(messages);

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
						try
						{
							/*using (stage)
							{
								stage.Configuration = this;
								stage.Create();

								messages.AddRange(stage.StageSpecificValidatable.Validate(stageContext));
							}*/
						}
						catch (Exception ex)
						{
							messages.Add(NewError(string.Format("Error during {0} stage validation: {1}.", stageContext, ex)));
						}
					}
				}
			}

			return messages;
		}

		#endregion
	}
}
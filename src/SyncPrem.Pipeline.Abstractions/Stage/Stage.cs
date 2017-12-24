/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;

namespace SyncPrem.Pipeline.Abstractions.Stage
{
	public abstract class Stage : Component, IStage
	{
		#region Constructors/Destructors

		protected Stage()
		{
		}

		#endregion

		#region Fields/Constants

		private StageConfiguration stageConfiguration;

		#endregion

		#region Properties/Indexers/Events

		public abstract Type StageSpecificConfigurationType
		{
			get;
		}

		public StageConfiguration StageConfiguration
		{
			get
			{
				return this.stageConfiguration;
			}
			set
			{
				this.stageConfiguration = value;
			}
		}

		#endregion

		#region Methods/Operators

		public void PostExecute(IContext context, RecordConfiguration recordConfiguration)
		{
			this.PostExecuteRecord(context, recordConfiguration);
		}

		protected abstract void PostExecuteRecord(IContext context, RecordConfiguration recordConfiguration);

		public void PreExecute(IContext context, RecordConfiguration recordConfiguration)
		{
			this.PreExecuteRecord(context, recordConfiguration);
		}

		protected abstract void PreExecuteRecord(IContext context, RecordConfiguration recordConfiguration);

		#endregion
	}
}
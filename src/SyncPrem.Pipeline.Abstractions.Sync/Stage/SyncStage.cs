/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Abstractions.Sync.Stage
{
	public abstract class SyncStage : SyncComponent, ISyncStage
	{
		#region Constructors/Destructors

		protected SyncStage()
		{
		}

		#endregion

		#region Fields/Constants

		private StageConfiguration configuration;

		#endregion

		#region Properties/Indexers/Events

		public abstract Type StageSpecificConfigurationType
		{
			get;
		}

		public abstract IValidatable StageSpecificValidatable
		{
			get;
		}

		public StageConfiguration Configuration
		{
			get
			{
				return this.configuration;
			}
			set
			{
				this.configuration = value;
			}
		}

		#endregion

		#region Methods/Operators

		public void PostExecute(ISyncContext context, RecordConfiguration configuration)
		{
			this.PostExecuteInternal(context, configuration);
		}

		protected abstract void PostExecuteInternal(ISyncContext context, RecordConfiguration configuration);

		public void PreExecute(ISyncContext context, RecordConfiguration configuration)
		{
			this.PreExecuteInternal(context, configuration);
		}

		protected abstract void PreExecuteInternal(ISyncContext context, RecordConfiguration configuration);

		#endregion
	}
}
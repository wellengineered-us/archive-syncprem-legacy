/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;

using TextMetal.Middleware.Solder.Primitives;

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

		public void PostExecute(IContext context, RecordConfiguration configuration)
		{
			this.PostExecuteRecord(context, configuration);
		}

		protected abstract void PostExecuteRecord(IContext context, RecordConfiguration configuration);

		public void PreExecute(IContext context, RecordConfiguration configuration)
		{
			this.PreExecuteRecord(context, configuration);
		}

		protected abstract void PreExecuteRecord(IContext context, RecordConfiguration configuration);

		#endregion
	}
}
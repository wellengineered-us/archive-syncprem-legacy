/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;

namespace SyncPrem.Pipeline.Abstractions.Sync.Runtime
{
	public abstract class SyncPipeline : SyncComponent, ISyncPipeline
	{
		#region Constructors/Destructors

		protected SyncPipeline()
		{
		}

		#endregion

		#region Fields/Constants

		private PipelineConfiguration configuration;

		#endregion

		#region Properties/Indexers/Events

		public PipelineConfiguration Configuration
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

		public ISyncContext CreateContext()
		{
			return this.CreateContextInternal();
		}

		protected abstract ISyncContext CreateContextInternal();

		public long Execute(ISyncContext context)
		{
			return this.ExecuteInternal(context);
		}

		protected abstract long ExecuteInternal(ISyncContext context);

		#endregion
	}
}
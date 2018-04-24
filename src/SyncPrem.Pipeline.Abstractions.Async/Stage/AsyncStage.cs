/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Configuration;

using TextMetal.Middleware.Solder.Primitives.Async;

namespace SyncPrem.Pipeline.Abstractions.Async.Stage
{
	public abstract class AsyncStage : AsyncComponent, IAsyncStage
	{
		#region Constructors/Destructors

		protected AsyncStage()
		{
		}

		#endregion

		#region Fields/Constants

		private StageConfiguration configuration;

		#endregion

		#region Properties/Indexers/Events

		public abstract IAsyncValidatable StageSpecificAsyncValidatable
		{
			get;
		}

		public abstract Type StageSpecificConfigurationType
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

		public Task PostExecuteAsync(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			return this.PostExecuteAsyncInternal(asyncContext, configuration, cancellationToken);
		}

		protected abstract Task PostExecuteAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken);

		public Task PreExecuteAsync(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			return this.PreExecuteAsyncInternal(asyncContext, configuration, cancellationToken);
		}

		protected abstract Task PreExecuteAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken);

		#endregion
	}
}
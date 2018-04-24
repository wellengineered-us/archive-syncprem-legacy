/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Configuration;

namespace SyncPrem.Pipeline.Abstractions.Async.Runtime
{
	public abstract class AsyncPipeline : AsyncComponent, IAsyncPipeline
	{
		#region Constructors/Destructors

		protected AsyncPipeline()
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

		public IAsyncContext CreateContextAsync()
		{
			return this.CreateContextAsyncInternal();
		}

		protected abstract IAsyncContext CreateContextAsyncInternal();

		public Task<long> ExecuteAsync(IAsyncContext asyncContext, CancellationToken cancellationToken)
		{
			return this.ExecuteAsyncInternal(asyncContext, cancellationToken);
		}

		protected abstract Task<long> ExecuteAsyncInternal(IAsyncContext asyncContext, CancellationToken cancellationToken);

		#endregion
	}
}
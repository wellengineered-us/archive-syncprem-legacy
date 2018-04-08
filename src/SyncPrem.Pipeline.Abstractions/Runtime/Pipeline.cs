/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Configuration;

namespace SyncPrem.Pipeline.Abstractions.Runtime
{
	public abstract class Pipeline : Component, IPipeline
	{
		#region Constructors/Destructors

		protected Pipeline()
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

		public IContext CreateContext()
		{
			return this.CreateContextInternal();
		}

		protected abstract IContext CreateContextInternal();

		public int Execute(IContext context)
		{
			return this.ExecuteInternal(context);
		}

		public Task<int> ExecuteAsync(IContext context, CancellationToken cancellationToken)
		{
			return this.ExecuteAsyncInternal(context, cancellationToken, null);
		}

		protected abstract Task<int> ExecuteAsyncInternal(IContext context, CancellationToken cancellationToken, IProgress<int> progress);

		protected abstract int ExecuteInternal(IContext context);

		#endregion
	}
}
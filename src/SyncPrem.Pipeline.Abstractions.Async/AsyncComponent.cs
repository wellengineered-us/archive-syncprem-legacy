/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using TextMetal.Middleware.Solder.Primitives.Async;

namespace SyncPrem.Pipeline.Abstractions.Async
{
	public abstract class AsyncComponent : AsyncLifecycle, IAsyncComponent
	{
		#region Constructors/Destructors

		protected AsyncComponent()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly Guid componentId = Guid.NewGuid();
		private readonly bool isReusable = false;
		private readonly bool supportsAsync = true;

		#endregion

		#region Properties/Indexers/Events

		public Guid ComponentId
		{
			get
			{
				return this.componentId;
			}
		}

		public bool IsReusable
		{
			get
			{
				return this.isReusable;
			}
		}

		public bool SupportsAsync
		{
			get
			{
				return this.supportsAsync;
			}
		}

		#endregion

		#region Methods/Operators

		protected override Task CreateAsync(bool creating, CancellationToken cancellationToken)
		{
			if (creating)
			{
				// do nothing
			}

			return Task.CompletedTask;
		}

		protected override Task DisposeAsync(bool disposing, CancellationToken cancellationToken)
		{
			if (disposing)
			{
				// do nothing
			}

			return Task.CompletedTask;
		}

		#endregion
	}
}
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
	public abstract class AsyncHost : AsyncComponent, IAsyncHost
	{
		#region Constructors/Destructors

		protected AsyncHost()
		{
		}

		#endregion

		#region Fields/Constants

		private HostConfiguration configuration;

		#endregion

		#region Properties/Indexers/Events

		public HostConfiguration Configuration
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

		public Task RunAsync(CancellationToken cancellationToken)
		{
			return this.RunAsyncInternal(cancellationToken);
		}

		protected abstract Task RunAsyncInternal(CancellationToken cancellationToken);

		#endregion
	}
}
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
	public abstract class Host : Component, IHost
	{
		#region Constructors/Destructors

		protected Host()
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
			return this.RunAsyncInternal(cancellationToken, null);
		}

		protected abstract Task RunAsyncInternal(CancellationToken cancellationToken, IProgress<int> progress);

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using SyncPrem.Pipeline.Abstractions.Configuration;

namespace SyncPrem.Pipeline.Abstractions.Sync.Runtime
{
	public abstract class SyncHost : SyncComponent, ISyncHost
	{
		#region Constructors/Destructors

		protected SyncHost()
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

		public void Run()
		{
			this.RunInternal();
		}

		protected abstract void RunInternal();

		#endregion
	}
}
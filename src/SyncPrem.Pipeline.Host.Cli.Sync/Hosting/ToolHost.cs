/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Core.Sync.Runtime;

using TextMetal.Middleware.Solder.Injection;

namespace SyncPrem.Pipeline.Host.Cli.Sync.Hosting
{
	public sealed class ToolHost : DefaultSyncHost, IToolHost
	{
		#region Constructors/Destructors

		[DependencyInjection]
		public ToolHost()
		{
		}

		#endregion
	}
}
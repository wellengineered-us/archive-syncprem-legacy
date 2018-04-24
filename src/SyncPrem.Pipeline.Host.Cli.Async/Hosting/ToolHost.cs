/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Core.Async.Runtime;

using TextMetal.Middleware.Solder.Injection;

namespace SyncPrem.Pipeline.Host.Cli.Async.Hosting
{
	public sealed class ToolHost : DefaultAsyncHost, IToolHost
	{
		#region Constructors/Destructors

		[DependencyInjection]
		public ToolHost()
		{
		}

		#endregion
	}
}
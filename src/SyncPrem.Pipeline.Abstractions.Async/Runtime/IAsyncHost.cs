/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Runtime;

namespace SyncPrem.Pipeline.Abstractions.Async.Runtime
{
	public interface IAsyncHost : IAsyncComponent, IHost
	{
		#region Methods/Operators

		Task RunAsync(CancellationToken cancellationToken);

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Destination;

namespace SyncPrem.Pipeline.Abstractions.Async.Stage.Connector.Destination
{
	public interface IAsyncDestinationConnector : IAsyncConnector, IDestinationConnector
	{
		#region Methods/Operators

		Task ConsumeAsync(IAsyncContext asyncContext, RecordConfiguration configuration, IAsyncChannel asyncChannel, CancellationToken cancellationToken);

		#endregion
	}
}
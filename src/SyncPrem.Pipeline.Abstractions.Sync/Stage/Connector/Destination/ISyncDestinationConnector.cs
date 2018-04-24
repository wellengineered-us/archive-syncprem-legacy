/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Destination;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;

namespace SyncPrem.Pipeline.Abstractions.Sync.Stage.Connector.Destination
{
	public interface ISyncDestinationConnector : ISyncConnector, IDestinationConnector
	{
		#region Methods/Operators

		void Consume(ISyncContext context, RecordConfiguration configuration, ISyncChannel channel);

		#endregion
	}
}
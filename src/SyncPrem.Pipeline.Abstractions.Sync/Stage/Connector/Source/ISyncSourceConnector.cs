/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Source;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;

namespace SyncPrem.Pipeline.Abstractions.Sync.Stage.Connector.Source
{
	public interface ISyncSourceConnector : ISyncConnector, ISourceConnector
	{
		#region Methods/Operators

		ISyncChannel Produce(ISyncContext context, RecordConfiguration configuration);

		#endregion
	}
}
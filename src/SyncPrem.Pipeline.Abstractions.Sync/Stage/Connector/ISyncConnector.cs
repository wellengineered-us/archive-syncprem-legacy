/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using SyncPrem.Pipeline.Abstractions.Stage.Connector;

namespace SyncPrem.Pipeline.Abstractions.Sync.Stage.Connector
{
	public interface ISyncConnector : ISyncStage, IConnector
	{
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;

namespace SyncPrem.Pipeline.Abstractions.Stage.Connector.Source
{
	public interface ISourceConnector : IConnector
	{
		#region Methods/Operators

		IChannel Produce(IContext context, RecordConfiguration configuration);

		#endregion
	}
}
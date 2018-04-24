/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;

namespace SyncPrem.Pipeline.Abstractions.Sync.Stage.Connector
{
	public abstract class SyncConnector<TStageSpecificConfiguration> : SyncStage<TStageSpecificConfiguration>, ISyncConnector
		where TStageSpecificConfiguration : StageSpecificConfiguration, new()
	{
		#region Constructors/Destructors

		protected SyncConnector()
		{
		}

		#endregion
	}
}
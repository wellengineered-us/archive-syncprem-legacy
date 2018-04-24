/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Infrastructure.Configuration;
using SyncPrem.Pipeline.Abstractions.Configuration;

namespace SyncPrem.Pipeline.Abstractions.Stage
{
	public interface IStage : IComponent, IConfigurable<StageConfiguration>
	{
		#region Properties/Indexers/Events

		Type StageSpecificConfigurationType
		{
			get;
		}

		#endregion
	}
}
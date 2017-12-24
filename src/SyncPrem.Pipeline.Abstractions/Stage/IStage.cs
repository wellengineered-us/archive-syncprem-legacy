/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;

namespace SyncPrem.Pipeline.Abstractions.Stage
{
	public interface IStage : IComponent
	{
		#region Properties/Indexers/Events

		Type StageSpecificConfigurationType
		{
			get;
		}

		StageConfiguration StageConfiguration
		{
			get;
			set;
		}

		#endregion

		#region Methods/Operators

		void PostExecute(IContext context, RecordConfiguration recordConfiguration);

		void PreExecute(IContext context, RecordConfiguration recordConfiguration);

		#endregion
	}
}
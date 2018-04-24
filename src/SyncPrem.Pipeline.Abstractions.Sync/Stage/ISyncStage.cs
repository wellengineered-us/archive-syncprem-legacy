/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Stage;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Abstractions.Sync.Stage
{
	public interface ISyncStage : ISyncComponent, IStage
	{
		#region Properties/Indexers/Events

		IValidatable StageSpecificValidatable
		{
			get;
		}

		#endregion

		#region Methods/Operators

		void PostExecute(ISyncContext context, RecordConfiguration configuration);

		void PreExecute(ISyncContext context, RecordConfiguration configuration);

		#endregion
	}
}
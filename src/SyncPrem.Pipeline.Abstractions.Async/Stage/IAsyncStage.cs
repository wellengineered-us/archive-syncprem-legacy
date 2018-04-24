/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Stage;

using TextMetal.Middleware.Solder.Primitives.Async;

namespace SyncPrem.Pipeline.Abstractions.Async.Stage
{
	public interface IAsyncStage : IAsyncComponent, IStage
	{
		#region Properties/Indexers/Events

		IAsyncValidatable StageSpecificAsyncValidatable
		{
			get;
		}

		#endregion

		#region Methods/Operators

		Task PostExecuteAsync(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken);

		Task PreExecuteAsync(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken);

		#endregion
	}
}
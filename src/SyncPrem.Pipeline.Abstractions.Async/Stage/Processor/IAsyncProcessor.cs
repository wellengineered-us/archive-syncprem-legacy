/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Stage.Processor;

namespace SyncPrem.Pipeline.Abstractions.Async.Stage.Processor
{
	public interface IAsyncProcessor : IAsyncStage, IProcessor
	{
		#region Methods/Operators

		Task<IAsyncChannel> ProcessAsync(IAsyncContext asyncContext, RecordConfiguration configuration, IAsyncChannel asyncChannel, AsyncProcessDelegate asyncNext, CancellationToken cancellationToken);

		#endregion
	}
}
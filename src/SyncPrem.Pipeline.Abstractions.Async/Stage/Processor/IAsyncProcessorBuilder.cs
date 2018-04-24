/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Stage.Processor;

namespace SyncPrem.Pipeline.Abstractions.Async.Stage.Processor
{
	public interface IAsyncProcessorBuilder : IProcessorBuilder
	{
		#region Methods/Operators

		AsyncProcessDelegate BuildAsync();

		IAsyncProcessorBuilder NewAsync();

		IAsyncProcessorBuilder UseAsync(Func<AsyncProcessDelegate, AsyncProcessDelegate> asyncMiddleware);

		#endregion
	}
}
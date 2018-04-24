/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions.Runtime;

namespace SyncPrem.Pipeline.Abstractions.Async.Runtime
{
	public interface IAsyncContext : IAsyncComponent, IContext
	{
		#region Methods/Operators

		IAsyncChannel CreateChannelAsync(IAsyncEnumerable<IAsyncRecord> asyncRecords);

		#endregion
	}
}
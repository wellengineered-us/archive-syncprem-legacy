/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

namespace TextMetal.Middleware.Solder.Primitives.Async
{
	public interface IAsyncDisposable
	{
		#region Properties/Indexers/Events

		bool IsDisposed
		{
			get;
		}

		#endregion

		#region Methods/Operators

		Task DisposeAsync(CancellationToken cancellationToken);

		#endregion
	}
}
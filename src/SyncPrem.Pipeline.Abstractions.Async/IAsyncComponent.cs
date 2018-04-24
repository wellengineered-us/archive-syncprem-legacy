/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using TextMetal.Middleware.Solder.Primitives.Async;

namespace SyncPrem.Pipeline.Abstractions.Async
{
	public interface IAsyncComponent : IComponent, IAsyncCreatable, IAsyncDisposable
	{
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Threading;
using System.Threading.Tasks;

namespace TextMetal.Middleware.Solder.Primitives.Async
{
	public interface IAsyncCreatable
	{
		#region Properties/Indexers/Events

		bool IsCreated
		{
			get;
		}

		#endregion

		#region Methods/Operators

		Task CreateAsync(CancellationToken cancellationToken);

		#endregion
	}
}
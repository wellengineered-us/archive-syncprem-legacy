/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace SyncPrem.Pipeline.Abstractions.Async.Runtime
{
	public interface IAsyncContextFactory
	{
		#region Methods/Operators

		IAsyncContext CreateContextAsync();

		#endregion
	}
}
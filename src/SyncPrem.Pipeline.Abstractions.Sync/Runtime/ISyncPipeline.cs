/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using SyncPrem.Pipeline.Abstractions.Runtime;

namespace SyncPrem.Pipeline.Abstractions.Sync.Runtime
{
	public interface ISyncPipeline : ISyncComponent, IPipeline, ISyncContextFactory
	{
		#region Methods/Operators

		long Execute(ISyncContext context);

		#endregion
	}
}
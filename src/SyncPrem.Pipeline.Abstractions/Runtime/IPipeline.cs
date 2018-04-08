/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Infrastructure.Configuration;
using SyncPrem.Pipeline.Abstractions.Configuration;

namespace SyncPrem.Pipeline.Abstractions.Runtime
{
	public interface IPipeline : IComponent, IConfigurable<PipelineConfiguration>
	{
		#region Methods/Operators

		IContext CreateContext();

		int Execute(IContext context);

		Task<int> ExecuteAsync(IContext context, CancellationToken cancellationToken);

		#endregion
	}
}
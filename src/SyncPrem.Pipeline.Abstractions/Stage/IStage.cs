/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Infrastructure.Configuration;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Abstractions.Stage
{
	public interface IStage : IComponent, IConfigurable<StageConfiguration>
	{
		#region Properties/Indexers/Events

		Type StageSpecificConfigurationType
		{
			get;
		}

		IValidatable StageSpecificValidatable
		{
			get;
		}

		#endregion

		#region Methods/Operators

		void PostExecute(IContext context, RecordConfiguration configuration);

		Task PostExecuteAsync(IContext context, RecordConfiguration configuration, CancellationToken cancellationToken);

		void PreExecute(IContext context, RecordConfiguration configuration);

		Task PreExecuteAsync(IContext context, RecordConfiguration configuration, CancellationToken cancellationToken);

		#endregion
	}
}
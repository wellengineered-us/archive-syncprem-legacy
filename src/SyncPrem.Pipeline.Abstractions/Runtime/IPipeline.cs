/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions.Configuration;

namespace SyncPrem.Pipeline.Abstractions.Runtime
{
	public interface IPipeline : IComponent
	{
		#region Properties/Indexers/Events

		PipelineConfiguration PipelineConfiguration
		{
			get;
			set;
		}

		#endregion

		#region Methods/Operators

		IContext CreateContext();

		int Execute(IContext context);

		Type GetPipelineType();

		IReadOnlyCollection<Type> GetStaticStageChain();

		#endregion
	}
}
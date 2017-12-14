/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions.Configurations;

namespace SyncPrem.Pipeline.Abstractions
{
	public interface IRealPipeline : IPipelineComponent
	{
		#region Properties/Indexers/Events

		RealPipelineConfiguration RealPipelineConfiguration
		{
			get;
			set;
		}

		#endregion

		#region Methods/Operators

		IPipelineContext CreateContext();

		int Execute(IPipelineContext pipelineContext);

		Type GetPipelineType();

		IReadOnlyCollection<Type> GetStaticFilterChain();

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Data.Primitives;

namespace SyncPrem.Pipeline.Abstractions.Messages
{
	public interface IPipelineMetadata : IPipelineComponent
	{
		#region Properties/Indexers/Events

		IEnumerable<IField> UpstreamFields
		{
			get;
		}

		#endregion
	}
}
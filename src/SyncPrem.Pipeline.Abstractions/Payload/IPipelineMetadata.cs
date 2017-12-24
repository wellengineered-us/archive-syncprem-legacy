/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.Pipeline.Abstractions.Payload
{
	public interface IPipelineMetadata : IComponent
	{
		#region Properties/Indexers/Events

		IEnumerable<IField> UpstreamFields
		{
			get;
		}

		#endregion
	}
}
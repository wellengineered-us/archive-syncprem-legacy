/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.StreamingIO.Primitives;
using SyncPrem.StreamingIO.ProxyWrappers;

namespace SyncPrem.Pipeline.Abstractions.Payload
{
	public interface IPipelineMessage : IComponent, IApplyWrap<IPipelineMessage, IRecord>
	{
		#region Properties/Indexers/Events

		IEnumerable<IRecord> Records
		{
			get;
		}

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace SyncPrem.Pipeline.Abstractions.Channel
{
	public interface IChannel : IComponent
	{
		#region Properties/Indexers/Events

		IEnumerable<IRecord> Records
		{
			get;
		}

		#endregion
	}
}
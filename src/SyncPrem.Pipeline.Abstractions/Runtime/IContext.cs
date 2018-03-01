/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions.Channel;

namespace SyncPrem.Pipeline.Abstractions.Runtime
{
	public interface IContext : IComponent
	{
		#region Properties/Indexers/Events

		IDictionary<string, object> GlobalState
		{
			get;
		}

		IDictionary<IComponent, IDictionary<string, object>> LocalState
		{
			get;
		}

		#endregion

		#region Methods/Operators

		IChannel CreateChannel(IEnumerable<IRecord> records);

		#endregion
	}
}
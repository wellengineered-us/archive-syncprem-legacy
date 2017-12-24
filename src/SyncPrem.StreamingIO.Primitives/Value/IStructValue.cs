/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SyncPrem.StreamingIO.Primitives.Value
{
	public interface IStructValue : IValue
	{
		#region Properties/Indexers/Events

		IReadOnlyList<object> Values
		{
			get;
		}

		IReadOnlyList<string> Fields
		{
			get;
		}

		#endregion
	}
}
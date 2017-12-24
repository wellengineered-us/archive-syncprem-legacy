/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace SyncPrem.StreamingIO.ProxyWrappers
{
	public interface IApplyWrap<out TThis, TEnumerable>
	{
		#region Methods/Operators

		TThis ApplyWrap(Func<IEnumerable<TEnumerable>, IEnumerable<TEnumerable>> wrapperCallback);

		#endregion
	}
}
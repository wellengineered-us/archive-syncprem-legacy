/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Wrappers;

namespace SyncPrem.Infrastructure.Data.Primitives
{
	public interface IResult : IApplyWrap<IResult, IRecord>
	{
		#region Properties/Indexers/Events

		IEnumerable<IRecord> Records
		{
			get;
		}

		int? RecordsAffected
		{
			get;
		}

		int ResultIndex
		{
			get;
		}

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace SyncPrem.Infrastructure.Data.Primitives
{
	public interface IResult
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

		#region Methods/Operators

		/// <summary>
		/// Feels kind of hack-ish.
		/// </summary>
		/// <param name="wrapperCallback"> </param>
		IResult ApplyWrap(Func<IEnumerable<IRecord>, IEnumerable<IRecord>> wrapperCallback);

		#endregion
	}
}
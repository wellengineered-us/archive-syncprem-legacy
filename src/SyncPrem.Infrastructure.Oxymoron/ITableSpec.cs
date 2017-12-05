/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

namespace SyncPrem.Infrastructure.Oxymoron
{
	public interface ITableSpec
	{
		#region Properties/Indexers/Events

		IEnumerable<IColumnSpec> ColumnSpecs
		{
			get;
			set;
		}

		#endregion
	}
}
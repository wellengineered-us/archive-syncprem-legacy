/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace SyncPrem.StreamingIO.DataMasking
{
	public interface IColumnSpec
	{
		#region Properties/Indexers/Events

		IDictionary<string, object> ObfuscationStrategySpec
		{
			get;
		}

		string ColumnName
		{
			get;
			set;
		}

		Type ObfuscationStrategyType
		{
			get;
			set;
		}

		#endregion
	}
}
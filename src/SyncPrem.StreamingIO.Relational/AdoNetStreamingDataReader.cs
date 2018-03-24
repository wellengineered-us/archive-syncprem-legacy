/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Data.Common;

using SyncPrem.StreamingIO.ProxyWrappers;

namespace SyncPrem.StreamingIO.Relational
{
	public class AdoNetStreamingDataReader : WrappedDbDataReader
	{
		#region Constructors/Destructors

		public AdoNetStreamingDataReader(DbDataReader innerDbDataReader)
			: base(innerDbDataReader)
		{
		}

		#endregion
	}
}
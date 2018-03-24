/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace SyncPrem.StreamingIO.Relational
{
	public sealed class AdoNetStreamingRecord : Dictionary<string, object>, IAdoNetStreamingRecord
	{
		#region Constructors/Destructors

		public AdoNetStreamingRecord(long resultIndex, long recordIndex)
			: base(StringComparer.OrdinalIgnoreCase)
		{
			this.resultIndex = resultIndex;
			this.recordIndex = recordIndex;
		}

		#endregion

		#region Fields/Constants

		private readonly long recordIndex;
		private readonly long resultIndex;

		#endregion

		#region Properties/Indexers/Events

		public long RecordIndex
		{
			get
			{
				return this.recordIndex;
			}
		}

		public long ResultIndex
		{
			get
			{
				return this.resultIndex;
			}
		}

		#endregion
	}
}
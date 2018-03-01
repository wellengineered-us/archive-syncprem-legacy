/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.StreamingIO.Relational
{
	public sealed class AdoNetStreamingResult : IAdoNetStreamingResult
	{
		#region Constructors/Destructors

		public AdoNetStreamingResult(long resultIndex)
		{
			this.resultIndex = resultIndex;
		}

		#endregion

		#region Fields/Constants

		private readonly long resultIndex;
		private IEnumerable<IPayload> records;
		private int recordsAffected;

		#endregion

		#region Properties/Indexers/Events

		public long ResultIndex
		{
			get
			{
				return this.resultIndex;
			}
		}

		public IEnumerable<IPayload> Records
		{
			get
			{
				return this.records;
			}
			set
			{
				this.records = value;
			}
		}

		public int RecordsAffected
		{
			get
			{
				return this.recordsAffected;
			}
			set
			{
				this.recordsAffected = value;
			}
		}

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using __IRecord = System.Collections.Generic.IDictionary<string, object>;
using __Record = System.Collections.Generic.Dictionary<string, object>;

namespace SyncPrem.StreamingIO.AdoNet
{
	public sealed class AdoNetResult : IAdoNetResult
	{
		#region Constructors/Destructors

		public AdoNetResult(long resultIndex)
		{
			this.resultIndex = resultIndex;
		}

		#endregion

		#region Fields/Constants

		private readonly long resultIndex;
		private IEnumerable<__IRecord> records;
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

		public IEnumerable<__IRecord> Records
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

		#region Methods/Operators

		public IAdoNetResult ApplyWrap(Func<IEnumerable<__IRecord>, IEnumerable<__IRecord>> wrapperCallback)
		{
			if ((object)wrapperCallback == null)
				throw new ArgumentNullException(nameof(wrapperCallback));

			this.Records = wrapperCallback(this.Records);

			return this; // fluent API
		}

		#endregion
	}
}
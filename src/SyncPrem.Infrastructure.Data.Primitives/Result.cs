/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace SyncPrem.Infrastructure.Data.Primitives
{
	public sealed class Result : IResult
	{
		#region Constructors/Destructors

		public Result(int resultIndex)
		{
			this.resultIndex = resultIndex;
		}

		#endregion

		#region Fields/Constants

		private readonly int resultIndex;
		private IEnumerable<IRecord> records;
		private int? recordsAffected;

		#endregion

		#region Properties/Indexers/Events

		public int ResultIndex
		{
			get
			{
				return this.resultIndex;
			}
		}

		public IEnumerable<IRecord> Records
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

		public int? RecordsAffected
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

		public IResult ApplyWrap(Func<IEnumerable<IRecord>, IEnumerable<IRecord>> wrapperCallback)
		{
			if ((object)wrapperCallback == null)
				throw new ArgumentNullException(nameof(wrapperCallback));

			this.Records = wrapperCallback(this.Records);

			return this; // fluent API
		}

		#endregion
	}
}
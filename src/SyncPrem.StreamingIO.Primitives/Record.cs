/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace SyncPrem.StreamingIO.Primitives
{
	public sealed class Record : Dictionary<string, object>, IRecord
	{
		#region Constructors/Destructors

		public Record()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		#endregion

		#region Fields/Constants

		private long recordIndex;

		#endregion

		#region Properties/Indexers/Events

		public long RecordIndex
		{
			get
			{
				return this.recordIndex;
			}
			set
			{
				this.recordIndex = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override string ToString()
		{
			return string.Join(", ", this.Select(kv => $"{kv.Key}={kv.Value}"));
		}

		#endregion
	}
}
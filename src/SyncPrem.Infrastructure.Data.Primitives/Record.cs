/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace SyncPrem.Infrastructure.Data.Primitives
{
	public sealed class Record : Dictionary<string, object>, IRecord
	{
		#region Constructors/Destructors

		public Record(int recordIndex)
			: base(StringComparer.OrdinalIgnoreCase)
		{
			this.recordIndex = recordIndex;
		}

		#endregion

		#region Fields/Constants

		private object contextData;
		private readonly int recordIndex;

		#endregion

		#region Properties/Indexers/Events

		public object ContextData
		{
			get
			{
				return this.contextData;
			}
			set
			{
				this.contextData = value;
			}
		}

		public int RecordIndex
		{
			get
			{
				return this.recordIndex;
			}
		}

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections;
using System.Collections.Generic;

namespace SyncPrem.Infrastructure.Data.Primitives
{
	public sealed class FastRecord : IRecord
	{
		#region Constructors/Destructors

		public FastRecord(int recordIndex)
		{
			this.recordIndex = recordIndex;
		}

		#endregion

		#region Fields/Constants

		private readonly int recordIndex;

		private object contextData;
		private int count;
		private bool isReadOnly;
		private string[] keys;
		private object[] values;

		#endregion

		#region Properties/Indexers/Events

		public object this[string key]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public int Count
		{
			get
			{
				return this.count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return this.isReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return this.keys;
			}
		}

		public int RecordIndex
		{
			get
			{
				return this.recordIndex;
			}
		}

		public ICollection<object> Values
		{
			get
			{
				return this.values;
			}
		}

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

		#endregion

		#region Methods/Operators

		public void Add(KeyValuePair<string, object> item)
		{
		}

		public void Add(string key, object value)
		{
		}

		public void Clear()
		{
		}

		public bool Contains(KeyValuePair<string, object> item)
		{
			return false;
		}

		public bool ContainsKey(string key)
		{
			return false;
		}

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			int size = 0;
			for (int fieldIndex = 0; fieldIndex < size; fieldIndex++)
				yield break;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, object> item)
		{
			return false;
		}

		public bool Remove(string key)
		{
			return false;
		}

		public bool TryGetValue(string key, out object value)
		{
			value = null;
			return false;
		}

		#endregion
	}

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

		private readonly int recordIndex;

		private object contextData;

		#endregion

		#region Properties/Indexers/Events

		public int RecordIndex
		{
			get
			{
				return this.recordIndex;
			}
		}

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

		#endregion
	}
}
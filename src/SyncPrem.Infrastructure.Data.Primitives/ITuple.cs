/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SyncPrem.Infrastructure.Data.Primitives
{
	public interface ITuple
	{
		IReadOnlyList<string> Fields
		{
			get;
		}

		IReadOnlyList<object> Values
		{
			get;
		}

		object ContextData
		{
			get;
		}

		int TupleIndex
		{
			get;
		}

		int GetFieldIndex(string fieldName);

		bool TryGetFieldIndex(string fieldName, out int fieldIndex);
	}

	public interface IBatch
	{
		IEnumerable<ITuple> Tuples
		{
			get;
		}

		int BatchIndex
		{
			get;
		}

		int ImpactCount
		{
			get;
		}
	}

	public sealed class Tuple : ITuple
	{
		public Tuple(int tupleIndex, string[] fields, object[] values)
		{
			if((object)fields == null)
				throw new ArgumentNullException(nameof(fields));

			if ((object)values == null)
				throw new ArgumentNullException(nameof(values));

			if(fields.Length == 0 || values.Length == 0 || fields.Length != values.Length)
				throw new InvalidOperationException(string.Format("Tuple size issue."));

			var tempIdx = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

			// only index fields for performance purposes
			for(int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
				tempIdx.Add(fields[fieldIndex], fieldIndex);

			this.fields = fields;
			this.values = values;
			this.tupleIndex = tupleIndex;
			this.fieldIndexMap = new ReadOnlyDictionary<string, int>(tempIdx);
		}

		private readonly string[] fields;
		private readonly object[] values;
		private readonly IReadOnlyDictionary<string, int> fieldIndexMap;
		private object contextData;
		private readonly int tupleIndex;

		public int TupleIndex
		{
			get
			{
				return this.tupleIndex;
			}
		}

		public bool TryGetFieldIndex(string fieldName, out int fieldIndex)
		{
			return this.FieldIndexMap.TryGetValue(fieldName, out fieldIndex);
		}

		public int GetFieldIndex(string fieldName)
		{
			int value;

			if(!this.TryGetFieldIndex(fieldName, out value))
				throw new InvalidOperationException(string.Format("FieldIndexMap???"));

			return value;
		}

		public IReadOnlyList<string> Fields
		{
			get
			{
				return this.fields;
			}
		}

		public IReadOnlyList<object> Values
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

		private IReadOnlyDictionary<string, int> FieldIndexMap
		{
			get
			{
				return this.fieldIndexMap;
			}
		}
	}
}
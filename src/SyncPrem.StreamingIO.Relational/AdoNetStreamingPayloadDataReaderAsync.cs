/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.StreamingIO.Relational
{
	public class AdoNetStreamingPayloadDataReaderAsync : DbDataReader
	{
		#region Constructors/Destructors

		private AdoNetStreamingPayloadDataReaderAsync(IEnumerable<IField> fieldMetadata, IAsyncEnumerable<IBatch> targetBatchEnumerable)
		{
			if ((object)fieldMetadata == null)
				throw new ArgumentNullException(nameof(fieldMetadata));

			if ((object)targetBatchEnumerable == null)
				throw new ArgumentNullException(nameof(targetBatchEnumerable));

			this.targetBatchEnumerable = targetBatchEnumerable;
			this.targetBatchEnumerator = targetBatchEnumerable.GetEnumerator();

			if ((object)this.TargetBatchEnumerator == null)
				throw new InvalidOperationException(nameof(this.TargetBatchEnumerator));

			// if we are going to enumerate the enumerable anyways and incur possible side effects,
			// we can first force to array, stash it in member, then project OLC...
			this.ordinalLookupCache = (this.fieldMetadata = fieldMetadata.ToArray()).Select((mc, i) => new
																										{
																											Index = i,
																											Name = mc.FieldName
																										}).ToDictionary(
				p => p.Name,
				p => p.Index,
				StringComparer.CurrentCultureIgnoreCase);

			if (!this.AdvanceResultAsync())
				throw new InvalidOperationException(nameof(this.AdvanceResultAsync));
		}

		public AdoNetStreamingPayloadDataReaderAsync(IEnumerable<IField> fieldMetadata, IAsyncEnumerable<IPayload> targetPayloadEnumerable)
			: this(fieldMetadata, GetBatch(targetPayloadEnumerable))
		{
		}

		#endregion

		#region Fields/Constants

		private readonly IEnumerable<IField> fieldMetadata;
		private readonly Dictionary<string, int> ordinalLookupCache;
		private readonly IAsyncEnumerable<IBatch> targetBatchEnumerable;
		private readonly IAsyncEnumerator<IBatch> targetBatchEnumerator;
		private string[] currentKeys;
		private object[] currentValues;
		private bool? isBatchEnumerableClosed;
		private bool? isPayloadEnumerableClosed;
		private IAsyncEnumerable<IPayload> targetPayloadEnumerable;
		private IAsyncEnumerator<IPayload> targetPayloadEnumerator;
		private int visibleFieldCount = default(int);

		#endregion

		#region Properties/Indexers/Events

		public override object this[string name]
		{
			get
			{
				return this.HasRecord ? this.TargetPayloadEnumerator.Current[name] : null;
			}
		}

		public override object this[int i]
		{
			get
			{
				return this.HasRecord ? this.CurrentValues[i] : null;
			}
		}

		public override int Depth
		{
			get
			{
				return 1;
			}
		}

		public override int FieldCount
		{
			get
			{
				return this.HasRecord ? this.TargetPayloadEnumerator.Current.Keys.Count() : 0;
			}
		}

		private IEnumerable<IField> FieldMetadata
		{
			get
			{
				return this.fieldMetadata;
			}
		}

		private bool HasRecord
		{
			get
			{
				return this.HasResult && (object)this.TargetPayloadEnumerator.Current != null;
			}
		}

		private bool HasResult
		{
			get
			{
				return (object)this.TargetBatchEnumerator.Current != null;
			}
		}

		public override bool HasRows
		{
			get
			{
				return this.HasRecord;
			}
		}

		public override bool IsClosed
		{
			get
			{
				return this.IsPayloadEnumerableClosed ?? false;
			}
		}

		private Dictionary<string, int> OrdinalLookupCache
		{
			get
			{
				return this.ordinalLookupCache;
			}
		}

		public override int RecordsAffected
		{
			get
			{
				return -1;
			}
		}

		private IAsyncEnumerable<IBatch> TargetBatchEnumerable
		{
			get
			{
				return this.targetBatchEnumerable;
			}
		}

		private IAsyncEnumerator<IBatch> TargetBatchEnumerator
		{
			get
			{
				return this.targetBatchEnumerator;
			}
		}

		public override int VisibleFieldCount
		{
			get
			{
				return this.visibleFieldCount;
			}
		}

		private string[] CurrentKeys
		{
			get
			{
				return this.currentKeys;
			}
			set
			{
				this.currentKeys = value;
			}
		}

		private object[] CurrentValues
		{
			get
			{
				return this.currentValues;
			}
			set
			{
				this.currentValues = value;
			}
		}

		protected bool? IsBatchEnumerableClosed
		{
			get
			{
				return this.isBatchEnumerableClosed;
			}
			set
			{
				this.isBatchEnumerableClosed = value;
			}
		}

		protected bool? IsPayloadEnumerableClosed
		{
			get
			{
				return this.isPayloadEnumerableClosed;
			}
			set
			{
				this.isPayloadEnumerableClosed = value;
			}
		}

		private IAsyncEnumerable<IPayload> TargetPayloadEnumerable
		{
			get
			{
				return this.targetPayloadEnumerable;
			}
			set
			{
				this.targetPayloadEnumerable = value;
			}
		}

		private IAsyncEnumerator<IPayload> TargetPayloadEnumerator
		{
			get
			{
				return this.targetPayloadEnumerator;
			}
			set
			{
				this.targetPayloadEnumerator = value;
			}
		}

		#endregion

		#region Methods/Operators

		private static IAsyncEnumerable<IBatch> GetBatch(IAsyncEnumerable<IPayload> payloads)
		{
			if ((object)payloads == null)
				throw new ArgumentNullException(nameof(payloads));

			return null;
		}

		private async Task<bool> AdvanceResultAsync()
		{
			if (!(this.IsBatchEnumerableClosed ?? false))
			{
				bool retval = !(bool)(this.IsBatchEnumerableClosed = !await this.TargetBatchEnumerator.MoveNext());

				if (retval && this.HasResult)
				{
					IBatch batch;

					batch = this.TargetBatchEnumerator.Current;

					if ((object)batch == null)
						throw new ArgumentNullException(nameof(batch));

					this.TargetPayloadEnumerable = batch.AsyncPayloads;

					if ((object)this.TargetPayloadEnumerable == null)
						throw new ArgumentNullException(nameof(this.TargetPayloadEnumerable));

					this.TargetPayloadEnumerator = this.TargetPayloadEnumerable.GetEnumerator();

					if ((object)this.TargetPayloadEnumerator == null)
						throw new InvalidOperationException(nameof(this.TargetPayloadEnumerator));
				}

				return retval;
			}
			else
				return true;
		}

		public override void Close()
		{
			IDisposable disposable;

			disposable = this.TargetBatchEnumerator;

			if ((object)disposable != null)
				disposable.Dispose();

			disposable = this.TargetBatchEnumerable as IDisposable;

			if ((object)disposable != null)
				disposable.Dispose();

			disposable = this.TargetPayloadEnumerator;

			if ((object)disposable != null)
				disposable.Dispose();

			disposable = this.TargetPayloadEnumerable as IDisposable;

			if ((object)disposable != null)
				disposable.Dispose();

			disposable = this.FieldMetadata as IDisposable;

			if ((object)disposable != null)
				disposable.Dispose();
		}

		public override bool GetBoolean(int i)
		{
			return this.HasRecord ? (Boolean)this.CurrentValues[i] : default(Boolean);
		}

		public override byte GetByte(int i)
		{
			return this.HasRecord ? (Byte)this.CurrentValues[i] : default(Byte);
		}

		public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			return 0;
		}

		public override char GetChar(int i)
		{
			return this.HasRecord ? (Char)this.CurrentValues[i] : default(Char);
		}

		public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			return 0;
		}

		//public override DbDataReader GetData(int i)
		//{
		//	return null;
		//}

		public override string GetDataTypeName(int i)
		{
			return null;
		}

		public override DateTime GetDateTime(int i)
		{
			return this.HasRecord ? (DateTime)this.CurrentValues[i] : default(DateTime);
		}

		public override decimal GetDecimal(int i)
		{
			return this.HasRecord ? (Decimal)this.CurrentValues[i] : default(Decimal);
		}

		public override double GetDouble(int i)
		{
			return this.HasRecord ? (Double)this.CurrentValues[i] : default(Double);
		}

		public override IEnumerator GetEnumerator()
		{
			return this.TargetBatchEnumerable?.ToEnumerable()?.GetEnumerator();
		}

		public override Type GetFieldType(int i)
		{
			return this.HasRecord && (object)this.CurrentValues[i] != null ? this.CurrentValues[i].GetType() : null;
		}

		public override T GetFieldValue<T>(int ordinal)
		{
			return base.GetFieldValue<T>(ordinal);
		}

		public override Task<T> GetFieldValueAsync<T>(int ordinal, CancellationToken cancellationToken)
		{
			return base.GetFieldValueAsync<T>(ordinal, cancellationToken);
		}

		public override float GetFloat(int i)
		{
			return this.HasRecord ? (Single)this.CurrentValues[i] : default(Single);
		}

		public override Guid GetGuid(int i)
		{
			return this.HasRecord ? (Guid)this.CurrentValues[i] : default(Guid);
		}

		public override short GetInt16(int i)
		{
			return this.HasRecord ? (Int16)this.CurrentValues[i] : default(Int16);
		}

		public override int GetInt32(int i)
		{
			return this.HasRecord ? (Int32)this.CurrentValues[i] : default(Int32);
		}

		public override long GetInt64(int i)
		{
			return this.HasRecord ? (Int64)this.CurrentValues[i] : default(Int64);
		}

		public override string GetName(int i)
		{
			return this.HasRecord ? this.CurrentKeys[i] : null;
		}

		public override int GetOrdinal(string name)
		{
			int value;

			if (this.OrdinalLookupCache.TryGetValue(name, out value))
				return value;

			return -1;
		}

		public override Type GetProviderSpecificFieldType(int ordinal)
		{
			return base.GetProviderSpecificFieldType(ordinal);
		}

		public override object GetProviderSpecificValue(int ordinal)
		{
			return base.GetProviderSpecificValue(ordinal);
		}

		public override int GetProviderSpecificValues(object[] values)
		{
			return base.GetProviderSpecificValues(values);
		}

		public override DataTable GetSchemaTable()
		{
			return base.GetSchemaTable();
		}

		public override Stream GetStream(int ordinal)
		{
			return base.GetStream(ordinal);
		}

		public override string GetString(int i)
		{
			return this.HasRecord ? (String)this.CurrentValues[i] : default(String);
		}

		public override TextReader GetTextReader(int ordinal)
		{
			return base.GetTextReader(ordinal);
		}

		public override object GetValue(int i)
		{
			return this.HasRecord ? (Object)this.CurrentValues[i] : default(Object);
		}

		public override int GetValues(object[] values)
		{
			return 0;
		}

		public override bool IsDBNull(int i)
		{
			return this.HasRecord ? (object)this.CurrentValues[i] == null : true;
		}

		public override Task<bool> IsDBNullAsync(int ordinal, CancellationToken cancellationToken)
		{
			return base.IsDBNullAsync(ordinal, cancellationToken);
		}

		public override bool NextResult()
		{
			return this.AdvanceResult().GetAwaiter().GetResult();
		}

		public override Task<bool> NextResultAsync(CancellationToken cancellationToken)
		{
			return base.NextResultAsync(cancellationToken);
		}

		public override bool Read()
		{
			if (!(this.IsPayloadEnumerableClosed ?? false))
			{
				bool retval = !(bool)(this.IsPayloadEnumerableClosed = !this.TargetPayloadEnumerator.MoveNext().GetAwaiter().GetResult());

				if (retval && this.HasRecord)
				{
					this.CurrentKeys = this.TargetPayloadEnumerator.Current.Keys.ToArray();
					this.CurrentValues = this.TargetPayloadEnumerator.Current.Values.ToArray();
				}

				return retval;
			}
			else
				return true;
		}

		public override Task<bool> ReadAsync(CancellationToken cancellationToken)
		{
			return base.ReadAsync(cancellationToken);
		}

		#endregion
	}
}
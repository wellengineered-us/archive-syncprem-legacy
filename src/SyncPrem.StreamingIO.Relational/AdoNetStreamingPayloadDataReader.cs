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
	public class AdoNetStreamingPayloadDataReader : DbDataReader
	{
		#region Constructors/Destructors

		private AdoNetStreamingPayloadDataReader(ISchema schema, IEnumerable<IAdoNetStreamingResult> results)
		{
			IEnumerator<IAdoNetStreamingResult> resultz;

			if ((object)schema == null)
				throw new ArgumentNullException(nameof(schema));

			if ((object)results == null)
				throw new ArgumentNullException(nameof(results));

			resultz = results.GetEnumerator();

			if ((object)resultz == null)
				throw new InvalidOperationException(nameof(resultz));

			this.schema = schema;
			this.results = results;
			this.resultz = resultz;

			this.fieldCache = schema.Fields.Select((f, i) => new
																	{
																		FieldName = f.Key,
																		FieldOrdinal = i
																	}).ToDictionary(p => p.FieldName, p => p.FieldOrdinal, StringComparer.CurrentCultureIgnoreCase);
		}

		private readonly IDictionary<string, int> fieldCache;

		#endregion

		#region Fields/Constants

		private readonly ISchema schema;
		private IEnumerable<IAdoNetStreamingResult> results;
		private IEnumerator<IAdoNetStreamingResult> resultz;
		private IEnumerable<IAdoNetStreamingRecord> records;
		private IEnumerator<IAdoNetStreamingRecord> recordz;

		private bool? isBatchEnumerableClosed;
		private bool? isPayloadEnumerableClosed;

		#endregion

		#region Properties/Indexers/Events

		public override object this[string name]
		{
			get
			{
				return this.HasRecord ? this.Recordz.Current[name] : null;
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
				return this.HasRecord ? this.Recordz.Current.Keys.Count() : 0;
			}
		}

		private ISchema Schema
		{
			get
			{
				return this.schema;
			}
		}

		private bool HasRecord
		{
			get
			{
				return this.HasResult && (object)this.Recordz.Current != null;
			}
		}

		private bool HasResult
		{
			get
			{
				return (object)this.Resultz.Current != null;
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

		public override int RecordsAffected
		{
			get
			{
				return -1;
			}
		}

		private IEnumerable<IAdoNetStreamingResult> Results
		{
			get
			{
				return this.results;
			}
		}

		private IEnumerator<IAdoNetStreamingResult> Resultz
		{
			get
			{
				return this.resultz;
			}
		}

		public override int VisibleFieldCount
		{
			get
			{
				return 0;
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

		private IEnumerable<IAdoNetStreamingRecord> Records
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

		private IEnumerator<IAdoNetStreamingRecord> Recordz
		{
			get
			{
				return this.recordz;
			}
			set
			{
				this.recordz = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override void Close()
		{
			IDisposable disposable;

			disposable = this.Recordz;

			if ((object)disposable != null)
				disposable.Dispose();

			disposable = this.Records as IDisposable;

			if ((object)disposable != null)
				disposable.Dispose();

			disposable = this.Resultz;

			if ((object)disposable != null)
				disposable.Dispose();

			disposable = this.Results as IDisposable;

			if ((object)disposable != null)
				disposable.Dispose();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		protected override DbDataReader GetDbDataReader(int ordinal)
		{
			return base.GetDbDataReader(ordinal);
		}

		public override bool GetBoolean(int i)
		{
			this.Schema.Fields
			return (bool)this.Recordz.Current;
		}

		public override byte GetByte(int i)
		{
			return base.GetByte(i);
		}

		public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferOffset, int length)
		{
			return base.GetBytes(i, fieldOffset, buffer, bufferOffset, length);
		}

		public override char GetChar(int i)
		{
			return base.GetChar(i);
		}

		public override long GetChars(int i, long fieldOffset, char[] buffer, int bufferOffset, int length)
		{
			return base.GetChars(i, fieldOffset, buffer, bufferOffset, length);
		}

		public override string GetDataTypeName(int i)
		{
			return base.GetDataTypeName(i);
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
			return this.Recordz;
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
			if (this.Schema.Fields.TryGetValue(name, out IField field))
				return (int)field.FieldIndex;

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
			if (!(this.IsBatchEnumerableClosed ?? false))
			{
				bool retval = !(bool)(this.IsBatchEnumerableClosed = !this.TargetBatchEnumerator.MoveNext());

				if (retval && this.HasResult)
				{
					IBatch batch;

					batch = this.TargetBatchEnumerator.Current;

					if ((object)batch == null)
						throw new ArgumentNullException(nameof(batch));

					this.TargetPayloadEnumerable = batch.Payloads;

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

		public override Task<bool> NextResultAsync(CancellationToken cancellationToken)
		{
			return base.NextResultAsync(cancellationToken);
		}

		public override bool Read()
		{
			if (!(this.IsPayloadEnumerableClosed ?? false))
			{
				bool retval = !(bool)(this.IsPayloadEnumerableClosed = !this.TargetPayloadEnumerator.MoveNext());

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
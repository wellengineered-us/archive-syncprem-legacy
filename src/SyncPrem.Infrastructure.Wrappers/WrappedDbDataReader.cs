/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SyncPrem.Infrastructure.Wrappers
{
	public class WrappedDbDataReader : DbDataReader, IDbColumnSchemaGenerator
	{
		#region Constructors/Destructors

		public WrappedDbDataReader(DbDataReader innerDbDataReader)
		{
			if ((object)innerDbDataReader == null)
				throw new ArgumentNullException(nameof(innerDbDataReader));

			this.innerDbDataReader = innerDbDataReader;
		}

		#endregion

		#region Fields/Constants

		private readonly DbDataReader innerDbDataReader;

		#endregion

		#region Properties/Indexers/Events

		public override object this[string name]
		{
			get
			{
				return this.InnerDbDataReader[name];
			}
		}

		public override object this[int i]
		{
			get
			{
				return this.InnerDbDataReader[i];
			}
		}

		public override int Depth
		{
			get
			{
				return this.InnerDbDataReader.Depth;
			}
		}

		public override int FieldCount
		{
			get
			{
				return this.InnerDbDataReader.FieldCount;
			}
		}

		public override bool HasRows
		{
			get
			{
				return this.InnerDbDataReader.HasRows;
			}
		}

		protected DbDataReader InnerDbDataReader
		{
			get
			{
				return this.innerDbDataReader;
			}
		}

		public override bool IsClosed
		{
			get
			{
				return this.InnerDbDataReader.IsClosed;
			}
		}

		public override int RecordsAffected
		{
			get
			{
				return this.InnerDbDataReader.RecordsAffected;
			}
		}

		public override int VisibleFieldCount
		{
			get
			{
				return this.InnerDbDataReader.VisibleFieldCount;
			}
		}

		#endregion

		#region Methods/Operators

		public override void Close()
		{
			this.InnerDbDataReader.Close();
			GC.SuppressFinalize(this);
		}

		protected override void Dispose(bool disposing)
		{
			// may not need this in .NET Core v2.0
			base.Dispose(disposing);
		}

		public override bool GetBoolean(int i)
		{
			return this.InnerDbDataReader.GetBoolean(i);
		}

		public override byte GetByte(int i)
		{
			return this.InnerDbDataReader.GetByte(i);
		}

		public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			return this.InnerDbDataReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
		}

		public override char GetChar(int i)
		{
			return this.InnerDbDataReader.GetChar(i);
		}

		public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			return this.InnerDbDataReader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
		}

		public ReadOnlyCollection<DbColumn> GetColumnSchema()
		{
			IDbColumnSchemaGenerator dbColumnSchemaGenerator;

			dbColumnSchemaGenerator = this.InnerDbDataReader as IDbColumnSchemaGenerator;

			if ((object)dbColumnSchemaGenerator != null)
				return dbColumnSchemaGenerator.GetColumnSchema();

			return null;
		}

		public override string GetDataTypeName(int i)
		{
			return this.InnerDbDataReader.GetDataTypeName(i);
		}

		public override DateTime GetDateTime(int i)
		{
			return this.InnerDbDataReader.GetDateTime(i);
		}

		public override decimal GetDecimal(int i)
		{
			return this.InnerDbDataReader.GetDecimal(i);
		}

		public override double GetDouble(int i)
		{
			return this.InnerDbDataReader.GetDouble(i);
		}

		public override IEnumerator GetEnumerator()
		{
			return this.InnerDbDataReader.GetEnumerator();
		}

		public override Type GetFieldType(int i)
		{
			Type retval;

			retval = this.InnerDbDataReader.GetFieldType(i);

			return retval;
		}

		public override T GetFieldValue<T>(int ordinal)
		{
			return this.InnerDbDataReader.GetFieldValue<T>(ordinal);
		}

		public override Task<T> GetFieldValueAsync<T>(int ordinal, CancellationToken cancellationToken)
		{
			return this.InnerDbDataReader.GetFieldValueAsync<T>(ordinal, cancellationToken);
		}

		public override float GetFloat(int i)
		{
			return this.InnerDbDataReader.GetFloat(i);
		}

		public override Guid GetGuid(int i)
		{
			return this.InnerDbDataReader.GetGuid(i);
		}

		public override short GetInt16(int i)
		{
			return this.InnerDbDataReader.GetInt16(i);
		}

		public override int GetInt32(int i)
		{
			return this.InnerDbDataReader.GetInt32(i);
		}

		public override long GetInt64(int i)
		{
			return this.InnerDbDataReader.GetInt64(i);
		}

		public override string GetName(int i)
		{
			string retval;

			retval = this.InnerDbDataReader.GetName(i);

			return retval;
		}

		public override int GetOrdinal(string name)
		{
			int retval;

			retval = this.InnerDbDataReader.GetOrdinal(name);

			return retval;
		}

		public override Type GetProviderSpecificFieldType(int ordinal)
		{
			return this.InnerDbDataReader.GetProviderSpecificFieldType(ordinal);
		}

		public override object GetProviderSpecificValue(int ordinal)
		{
			return this.InnerDbDataReader.GetProviderSpecificValue(ordinal);
		}

		public override int GetProviderSpecificValues(object[] values)
		{
			return this.InnerDbDataReader.GetProviderSpecificValues(values);
		}

		public override DataTable GetSchemaTable()
		{
			return this.InnerDbDataReader.GetSchemaTable();
		}

		public override Stream GetStream(int ordinal)
		{
			return this.InnerDbDataReader.GetStream(ordinal);
		}

		public override string GetString(int i)
		{
			return this.InnerDbDataReader.GetString(i);
		}

		public override TextReader GetTextReader(int ordinal)
		{
			return this.InnerDbDataReader.GetTextReader(ordinal);
		}

		public override object GetValue(int i)
		{
			object retval;

			retval = this.InnerDbDataReader.GetValue(i);

			return retval;
		}

		public override int GetValues(object[] values)
		{
			return this.InnerDbDataReader.GetValues(values);
		}

		public override bool IsDBNull(int i)
		{
			return this.InnerDbDataReader.IsDBNull(i);
		}

		public override Task<bool> IsDBNullAsync(int ordinal, CancellationToken cancellationToken)
		{
			return this.InnerDbDataReader.IsDBNullAsync(ordinal, cancellationToken);
		}

		public override bool NextResult()
		{
			bool retval;

			retval = this.InnerDbDataReader.NextResult();

			return retval;
		}

		public override Task<bool> NextResultAsync(CancellationToken cancellationToken)
		{
			return this.InnerDbDataReader.NextResultAsync(cancellationToken);
		}

		public override bool Read()
		{
			bool retval;

			retval = this.InnerDbDataReader.Read();

			return retval;
		}

		public override Task<bool> ReadAsync(CancellationToken cancellationToken)
		{
			return this.InnerDbDataReader.ReadAsync(cancellationToken);
		}

		#endregion
	}
}
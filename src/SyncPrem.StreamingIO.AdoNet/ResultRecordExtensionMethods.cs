/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using __IRecord = System.Collections.Generic.IDictionary<string, object>;
using __Record = System.Collections.Generic.Dictionary<string, object>;

namespace SyncPrem.StreamingIO.AdoNet
{
	public static class ResultRecordExtensionMethods
	{
		#region Methods/Operators

		public static __IRecord ToRecord(this IEnumerable<DbParameter> dbParameters)
		{
			__IRecord record;

			if ((object)dbParameters == null)
				throw new ArgumentNullException(nameof(dbParameters));

			record = new __Record(StringComparer.OrdinalIgnoreCase);

			foreach (DbParameter dbParameter in dbParameters)
			{
				if (dbParameter.Direction != ParameterDirection.InputOutput &&
					dbParameter.Direction != ParameterDirection.Output &&
					dbParameter.Direction != ParameterDirection.ReturnValue)
					continue;

				record.Add(dbParameter.ParameterName, dbParameter.Value);
			}

			return record;
		}

		public static IEnumerable<IAdoNetResult> ToResults(this IEnumerable<__IRecord> records)
		{
			IEnumerable<IAdoNetResult> results;

			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			results = new IAdoNetResult[]
					{
						new AdoNetResult(0) { Records = records, RecordsAffected = -1 }
					};

			return results;
		}

		#endregion
	}
}
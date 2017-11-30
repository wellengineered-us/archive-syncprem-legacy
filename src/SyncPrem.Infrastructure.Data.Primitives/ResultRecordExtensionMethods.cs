/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace SyncPrem.Infrastructure.Data.Primitives
{
	public static class ResultRecordExtensionMethods
	{
		#region Methods/Operators

		public static IRecord ToRecord(this IEnumerable<DbParameter> dbParameters)
		{
			IRecord record;

			if ((object)dbParameters == null)
				throw new ArgumentNullException(nameof(dbParameters));

			record = new Record(int.MaxValue);

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

		public static IEnumerable<IResult> ToResults(this IEnumerable<IRecord> records)
		{
			IEnumerable<IResult> results;

			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			results = new IResult[]
					{
						new Result(0) { Records = records, RecordsAffected = null }
					};

			return results;
		}

		#endregion
	}
}
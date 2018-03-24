/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.StreamingIO.Relational
{
	public static class AdoNetStreamingExtensionMethods
	{
		#region Methods/Operators

		public static IPayload ToRecord(this IEnumerable<DbParameter> dbParameters)
		{
			Payload record;

			if ((object)dbParameters == null)
				throw new ArgumentNullException(nameof(dbParameters));

			record = new Payload();

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

		#endregion
	}
}
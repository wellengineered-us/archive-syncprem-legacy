/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using __IRow = System.Collections.Generic.IDictionary<string, object>;
using __Row = System.Collections.Generic.Dictionary<string, object>;

namespace TextMetal.Middleware.Solder.Utilities
{
	public interface IAdoNetBufferingFascade
	{
		#region Methods/Operators

		DbParameter CreateParameter(Type connectionType, string sourceColumn, ParameterDirection parameterDirection, DbType parameterDbType, int parameterSize, byte parameterPrecision, byte parameterScale, bool parameterNullable, string parameterName, object parameterValue);

		IEnumerable<__IRow> ExecuteRecords(bool schemaOnly, Type connectionType, string connectionString, bool transactional, IsolationLevel isolationLevel, CommandType commandType, string commandText, IEnumerable<DbParameter> commandParameters, Action<int> resultCallback = null);

		#endregion
	}
}
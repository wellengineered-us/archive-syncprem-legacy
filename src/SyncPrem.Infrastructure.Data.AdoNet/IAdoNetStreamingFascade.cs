/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using SyncPrem.Infrastructure.Data.Primitives;

namespace SyncPrem.Infrastructure.Data.AdoNet
{
	internal interface IAdoNetStreamingFascade
	{
		#region Methods/Operators

		/// <summary>
		/// Create a new data parameter from the data source.
		/// This method DOES NOT DISPOSE OF CONNECTION/TRANSACTION - UP TO THE CALLER.
		/// </summary>
		/// <param name="dbConnection"> The database connection. </param>
		/// <param name="dbTransaction"> An optional local database transaction. </param>
		/// <param name="sourceColumn"> Specifies the source column. </param>
		/// <param name="parameterDirection"> Specifies the parameter direction. </param>
		/// <param name="parameterDbType"> Specifies the parameter provider-(in)dependent type. </param>
		/// <param name="parameterSize"> Specifies the parameter size. </param>
		/// <param name="parameterPrecision"> Specifies the parameter precision. </param>
		/// <param name="parameterScale"> Specifies the parameter scale. </param>
		/// <param name="parameterNullable"> Specifies the parameter nullable-ness. </param>
		/// <param name="parameterName"> Specifies the parameter name. </param>
		/// <param name="parameterValue"> Specifies the parameter value. </param>
		/// <returns> The data parameter with the specified properties set. </returns>
		DbParameter CreateParameter(DbConnection dbConnection, DbTransaction dbTransaction, string sourceColumn, ParameterDirection parameterDirection, DbType parameterDbType, int parameterSize, byte parameterPrecision, byte parameterScale, bool parameterNullable, string parameterName, object parameterValue);

		/// <summary>
		/// Executes a command, returning a data reader, against a data source.
		/// This method DOES NOT DISPOSE OF CONNECTION/TRANSACTION - UP TO THE CALLER.
		/// This method DOES NOT DISPOSE OF DATA READER - UP TO THE CALLER.
		/// </summary>
		/// <param name="dbConnection"> The database connection. </param>
		/// <param name="dbTransaction"> An optional local database transaction. </param>
		/// <param name="commandType"> The type of the command. </param>
		/// <param name="commandText"> The SQL text or stored procedure name. </param>
		/// <param name="commandParameters"> The parameters to use during the operation. </param>
		/// <param name="commandBehavior"> The reader behavior. </param>
		/// <param name="commandTimeout"> The command timeout (use null for default). </param>
		/// <param name="commandPrepare"> Whether to prepare the command at the data source. </param>
		/// <returns> The data reader result. </returns>
		DbDataReader ExecuteReader(DbConnection dbConnection, DbTransaction dbTransaction, CommandType commandType, string commandText, IEnumerable<DbParameter> commandParameters, CommandBehavior commandBehavior, int? commandTimeout, bool commandPrepare);

		/// <summary>
		/// Execute a command against a data source, mapping the data reader to an enumerable of record dictionaries.
		/// This method perfoms LAZY LOADING/DEFERRED EXECUTION.
		/// This method DOES NOT DISPOSE OF CONNECTION/TRANSACTION - UP TO THE CALLER.
		/// </summary>
		/// <param name="dbConnection"> The database connection. </param>
		/// <param name="dbTransaction"> An optional local database transaction. </param>
		/// <param name="commandType"> The type of the command. </param>
		/// <param name="commandText"> The SQL text or stored procedure name. </param>
		/// <param name="commandParameters"> The parameters to use during the operation. </param>
		/// <param name="recordsAffectedCallback"> Executed when the output count of records affected is available to return (post enumeration). </param>
		/// <returns> An enumerable of result instances, each containing an enumerable of dictionaries with record key/value pairs of schema metadata. </returns>
		IEnumerable<IRecord> ExecuteRecords(DbConnection dbConnection, DbTransaction dbTransaction, CommandType commandType, string commandText, IEnumerable<DbParameter> commandParameters, Action<int> recordsAffectedCallback);

		/// <summary>
		/// Execute a command against a data source, mapping the data reader to an enumerable of results, each with an enumerable of record dictionaries.
		/// This method perfoms LAZY LOADING/DEFERRED EXECUTION.
		/// This method DOES NOT DISPOSE OF CONNECTION/TRANSACTION - UP TO THE CALLER.
		/// </summary>
		/// <param name="dbConnection"> The database connection. </param>
		/// <param name="dbTransaction"> An optional local database transaction. </param>
		/// <param name="commandType"> The type of the command. </param>
		/// <param name="commandText"> The SQL text or stored procedure name. </param>
		/// <param name="commandParameters"> The parameters to use during the operation. </param>
		/// <returns> An enumerable of result instances, each containing an enumerable of dictionaries with record key/value pairs of data. </returns>
		IEnumerable<IResult> ExecuteResults(DbConnection dbConnection, DbTransaction dbTransaction, CommandType commandType, string commandText, IEnumerable<DbParameter> commandParameters);

		/// <summary>
		/// Execute a command against a data source, mapping the data reader GetSchemaTable() result to an enumerable of enumerable of record dictionaries.
		/// This method perfoms LAZY LOADING/DEFERRED EXECUTION.
		/// This method DOES NOT DISPOSE OF CONNECTION/TRANSACTION - UP TO THE CALLER.
		/// </summary>
		/// <param name="dbConnection"> The database connection. </param>
		/// <param name="dbTransaction"> An optional local database transaction. </param>
		/// <param name="commandType"> The type of the command. </param>
		/// <param name="commandText"> The SQL text or stored procedure name. </param>
		/// <param name="commandParameters"> The parameters to use during the operation. </param>
		/// <param name="recordsAffectedCallback"> Executed when the output count of records affected is available to return (post enumeration). </param>
		/// <returns> An enumerable of result instances, each containing an enumerable of dictionaries with record key/value pairs of schema metadata. </returns>
		IEnumerable<IRecord> ExecuteSchemaRecords(DbConnection dbConnection, DbTransaction dbTransaction, CommandType commandType, string commandText, IEnumerable<DbParameter> commandParameters, Action<int> recordsAffectedCallback);

		/// <summary>
		/// Execute a command against a data source, mapping the data reader GetSchemaTable() result to an results, each with an enumerable of record dictionaries.
		/// This method perfoms LAZY LOADING/DEFERRED EXECUTION.
		/// This method DOES NOT DISPOSE OF CONNECTION/TRANSACTION - UP TO THE CALLER.
		/// </summary>
		/// <param name="dbConnection"> The database connection. </param>
		/// <param name="dbTransaction"> An optional local database transaction. </param>
		/// <param name="commandType"> The type of the command. </param>
		/// <param name="commandText"> The SQL text or stored procedure name. </param>
		/// <param name="commandParameters"> The parameters to use during the operation. </param>
		/// <returns> An enumerable of result instances, each containing an enumerable of dictionaries with record key/value pairs of schema metadata. </returns>
		IEnumerable<IResult> ExecuteSchemaResults(DbConnection dbConnection, DbTransaction dbTransaction, CommandType commandType, string commandText, IEnumerable<DbParameter> commandParameters);

		/// <summary>
		/// Execute a command against a data source, mapping the data reader to an enumerable of record dictionaries.
		/// This method perfoms LAZY LOADING/DEFERRED EXECUTION.
		/// Note that THE DATA READER WILL NOT BE DISPOSED UPON ENUMERATION OR FOREACH BRANCH OUT.
		/// </summary>
		/// <param name="dbDataReader"> The target data reader. </param>
		/// <param name="recordsAffectedCallback"> Executed when the output count of records affected is available to return (post enumeration). </param>
		/// <returns> An enumerable of record dictionary instances, containing key/value pairs of data. </returns>
		IEnumerable<IRecord> GetRecordsFromReader(DbDataReader dbDataReader, Action<int> recordsAffectedCallback);

		/// <summary>
		/// Execute a command against a data source, mapping the data reader to an enumerable of results, each with an enumerable of records.
		/// This method perfoms LAZY LOADING/DEFERRED EXECUTION.
		/// Note that THE DATA READER WILL NOT BE DISPOSED UPON ENUMERATION OR FOREACH BRANCH OUT.
		/// </summary>
		/// <param name="dbDataReader"> The target data reader. </param>
		/// <returns> An enumerable of result instances, each containing an enumerable of dictionaries with record key/value pairs of data. </returns>
		IEnumerable<IResult> GetResultsFromReader(DbDataReader dbDataReader);

		/// <summary>
		/// Execute a command against a data source, mapping the data reader GetSchemaTable() result to an enumerable of record dictionaries.
		/// This method perfoms LAZY LOADING/DEFERRED EXECUTION.
		/// Note that THE DATA READER WILL NOT BE DISPOSED UPON ENUMERATION OR FOREACH BRANCH OUT.
		/// </summary>
		/// <param name="dbDataReader"> The target data reader. </param>
		/// <param name="recordsAffectedCallback"> Executed when the output count of records affected is available to return (post enumeration). </param>
		/// <returns> An enumerable of record dictionary instances, containing key/value pairs of schema metadata. </returns>
		IEnumerable<IRecord> GetSchemaRecordsFromReader(DbDataReader dbDataReader, Action<int> recordsAffectedCallback);

		/// <summary>
		/// Execute a command against a data source, mapping the data reader GetSchemaTable() result to an enumerable of results, each with an enumerable of records.
		/// This method perfoms LAZY LOADING/DEFERRED EXECUTION.
		/// Note that THE DATA READER WILL NOT BE DISPOSED UPON ENUMERATION OR FOREACH BRANCH OUT.
		/// </summary>
		/// <param name="dbDataReader"> The target data reader. </param>
		/// <returns> An enumerable of result instances, each containing an enumerable of dictionaries with record key/value pairs of schema metadata. </returns>
		IEnumerable<IResult> GetSchemaResultsFromReader(DbDataReader dbDataReader);

		#endregion
	}
}
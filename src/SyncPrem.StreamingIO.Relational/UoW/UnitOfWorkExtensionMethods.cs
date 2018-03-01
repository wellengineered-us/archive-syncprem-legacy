/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using SyncPrem.StreamingIO.Primitives;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.StreamingIO.Relational.UoW
{
	/// <summary>
	/// Provides extension methods for unit of work instances.
	/// </summary>
	public static class UnitOfWorkExtensionMethods
	{
		#region Fields/Constants

		private static readonly Lazy<IAdoNetStreamingFascade> adoNetStreamingFascadeFactory = new Lazy<IAdoNetStreamingFascade>(() => new AdoNetStreamingFascade());

		#endregion

		#region Properties/Indexers/Events

		private static IAdoNetStreamingFascade AdoNetStreamingFascade
		{
			get
			{
				return AdoNetStreamingFascadeFactory.Value;
			}
		}

		private static Lazy<IAdoNetStreamingFascade> AdoNetStreamingFascadeFactory
		{
			get
			{
				return adoNetStreamingFascadeFactory;
			}
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// An extension method to create a new data parameter from the data source.
		/// </summary>
		/// <param name="unitOfWork"> The target unit of work. </param>
		/// <param name="columnSource"> Specifies the column source. </param>
		/// <param name="parameterDirection"> Specifies the parameter direction. </param>
		/// <param name="dbType"> Specifies the parameter provider-(in)dependent type. </param>
		/// <param name="parameterSize"> Specifies the parameter size. </param>
		/// <param name="parameterPrecision"> Specifies the parameter precision. </param>
		/// <param name="parameterScale"> Specifies the parameter scale. </param>
		/// <param name="parameterNullable"> Specifies the parameter nullable-ness. </param>
		/// <param name="parameterName"> Specifies the parameter name. </param>
		/// <param name="parameterValue"> Specifies the parameter value. </param>
		/// <returns> The data parameter with the specified properties set. </returns>
		public static DbParameter CreateParameter(this IUnitOfWork unitOfWork, string columnSource, ParameterDirection parameterDirection, DbType dbType, int parameterSize, byte parameterPrecision, byte parameterScale, bool parameterNullable, string parameterName, object parameterValue)
		{
			DbParameter dbParameter;

			if ((object)unitOfWork == null)
				throw new ArgumentNullException(nameof(unitOfWork));

			dbParameter = AdoNetStreamingFascade.CreateParameter(unitOfWork.Connection, unitOfWork.Transaction, columnSource, parameterDirection, dbType, parameterSize, parameterPrecision, parameterScale, parameterNullable, parameterName, parameterValue);

			return dbParameter;
		}

		public static IEnumerable<IPayload> ExecuteRecords(this IUnitOfWork unitOfWork, CommandType commandType, string commandText, IEnumerable<DbParameter> commandParameters, Action<int> rowsAffectedCallback)
		{
			IEnumerable<IPayload> records;

			if ((object)unitOfWork == null)
				throw new ArgumentNullException(nameof(unitOfWork));

			// DO NOT DISPOSE OF DATA READER HERE - THE YIELD STATE MACHINE BELOW WILL DO THIS
			records = AdoNetStreamingFascade.ExecuteRecords(unitOfWork.Connection, unitOfWork.Transaction, commandType, commandText, commandParameters, rowsAffectedCallback);

			return records;
		}

		/// <summary>
		/// An extension method to execute a result query operation against a target unit of work.
		/// DO NOT DISPOSE OF UNIT OF WORK CONTEXT - UP TO THE CALLER.
		/// </summary>
		/// <param name="unitOfWork"> The target unit of work. </param>
		/// <param name="commandType"> The type of the command. </param>
		/// <param name="commandText"> The SQL text or stored procedure name. </param>
		/// <param name="commandParameters"> The parameters to use during the operation. </param>
		/// <returns> An enumerable of result instances, each containing an enumerable of records with (key/value pairs of data). </returns>
		public static IEnumerable<IAdoNetStreamingResult> ExecuteResults(this IUnitOfWork unitOfWork, CommandType commandType, string commandText, IEnumerable<DbParameter> commandParameters)
		{
			IEnumerable<IAdoNetStreamingResult> results;

			if ((object)unitOfWork == null)
				throw new ArgumentNullException(nameof(unitOfWork));

			// DO NOT DISPOSE OF DATA READER HERE - THE YIELD STATE MACHINE BELOW WILL DO THIS
			results = AdoNetStreamingFascade.ExecuteResults(unitOfWork.Connection, unitOfWork.Transaction, commandType, commandText, commandParameters);

			return results;
		}

		public static TValue ExecuteScalar<TValue>(this IUnitOfWork unitOfWork, CommandType commandType, string commandText, IEnumerable<DbParameter> commandParameters)
		{
			IEnumerable<IPayload> records;
			IPayload payload;

			object dbValue;

			if ((object)unitOfWork == null)
				throw new ArgumentNullException(nameof(unitOfWork));

			records = unitOfWork.ExecuteRecords(commandType, commandText, commandParameters, null);

			if ((object)records == null)
				return default(TValue);

			payload = records.SingleOrDefault();

			if ((object)payload == null)
				return default(TValue);

			if (payload.Count != 1)
				return default(TValue);

			var key = payload.Keys.FirstOrDefault();

			if ((object)key == null)
				return default(TValue);

			dbValue = payload[key];

			return dbValue.ChangeType<TValue>();
		}

		public static IEnumerable<IPayload> ExecuteSchemaRecords(this IUnitOfWork unitOfWork, CommandType commandType, string commandText, IEnumerable<DbParameter> commandParameters, Action<int> rowsAffectedCallback)
		{
			IEnumerable<IPayload> records;

			if ((object)unitOfWork == null)
				throw new ArgumentNullException(nameof(unitOfWork));

			// DO NOT DISPOSE OF DATA READER HERE - THE YIELD STATE MACHINE BELOW WILL DO THIS
			records = AdoNetStreamingFascade.ExecuteSchemaRecords(unitOfWork.Connection, unitOfWork.Transaction, commandType, commandText, commandParameters, rowsAffectedCallback);

			return records;
		}

		/// <summary>
		/// An extension method to execute a result query operation against a target unit of work.
		/// DO NOT DISPOSE OF UNIT OF WORK CONTEXT - UP TO THE CALLER.
		/// </summary>
		/// <param name="unitOfWork"> The target unit of work. </param>
		/// <param name="commandType"> The type of the command. </param>
		/// <param name="commandText"> The SQL text or stored procedure name. </param>
		/// <param name="commandParameters"> The parameters to use during the operation. </param>
		/// <returns> An enumerable of result instances, each containing an enumerable of records with (key/value pairs of schema metadata). </returns>
		public static IEnumerable<IAdoNetStreamingResult> ExecuteSchemaResults(this IUnitOfWork unitOfWork, CommandType commandType, string commandText, IEnumerable<DbParameter> commandParameters)
		{
			IEnumerable<IAdoNetStreamingResult> results;

			if ((object)unitOfWork == null)
				throw new ArgumentNullException(nameof(unitOfWork));

			results = AdoNetStreamingFascade.ExecuteSchemaResults(unitOfWork.Connection, unitOfWork.Transaction, commandType, commandText, commandParameters);

			return results;
		}

		#endregion
	}
}
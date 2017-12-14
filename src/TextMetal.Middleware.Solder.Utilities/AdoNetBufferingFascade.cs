/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

using __IRow = System.Collections.Generic.IDictionary<string, object>;
using __Row = System.Collections.Generic.Dictionary<string, object>;

namespace TextMetal.Middleware.Solder.Utilities
{
	public class AdoNetBufferingFascade : IAdoNetBufferingFascade
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the AdoNetBufferingFascade class.
		/// </summary>
		/// <param name="dataTypeFascade"> The data type fascade instance to use. </param>
		public AdoNetBufferingFascade(IDataTypeFascade dataTypeFascade)
		{
			if ((object)dataTypeFascade == null)
				throw new ArgumentNullException(nameof(dataTypeFascade));

			this.dataTypeFascade = dataTypeFascade;
		}

		#endregion

		#region Fields/Constants

		private readonly IDataTypeFascade dataTypeFascade;

		#endregion

		#region Properties/Indexers/Events

		protected IDataTypeFascade DataTypeFascade
		{
			get
			{
				return this.dataTypeFascade;
			}
		}

		#endregion

		#region Methods/Operators

		public DbParameter CreateParameter(Type connectionType, string sourceColumn, ParameterDirection parameterDirection, DbType parameterDbType, int parameterSize, byte parameterPrecision, byte parameterScale, bool parameterNullable, string parameterName, object parameterValue)
		{
			DbParameter dbParameter;

			if ((object)connectionType == null)
				throw new ArgumentNullException(nameof(connectionType));

			using (DbConnection dbConnection = (DbConnection)Activator.CreateInstance(connectionType))
			{
				using (DbCommand dbCommand = dbConnection.CreateCommand())
					dbParameter = dbCommand.CreateParameter();

				dbParameter.ParameterName = parameterName;
				dbParameter.Size = parameterSize;
				dbParameter.Value = parameterValue;
				dbParameter.Direction = parameterDirection;
				dbParameter.DbType = parameterDbType;
				dbParameter.IsNullable = parameterNullable;
				dbParameter.Precision = parameterPrecision;
				dbParameter.Scale = parameterScale;
				dbParameter.SourceColumn = sourceColumn;

				return dbParameter;
			}
		}

		public IEnumerable<__IRow> ExecuteRecords(bool schemaOnly, Type connectionType, string connectionString, bool transactional, IsolationLevel isolationLevel, CommandType commandType, string commandText, IEnumerable<DbParameter> commandParameters, Action<long> resultCallback = null)
		{
			DbTransaction dbTransaction;
			const bool OPEN = true;

			IList<__Row> rows;

			// force no preparation
			const bool COMMAND_PREPARE = false;

			// force provider default timeout
			object COMMAND_TIMEOUT = null; /*int?*/

			CommandBehavior commandBehavior;
			long resultIndex = 0;

			ReadOnlyCollection<DbColumn> dbColumns;
			DbColumn dbColumn;
			PropertyInfo[] propertyInfos;
			PropertyInfo propertyInfo;

			if ((object)connectionType == null)
				throw new ArgumentNullException(nameof(connectionType));

			if ((object)connectionString == null)
				throw new ArgumentNullException(nameof(connectionString));

			using (DbConnection dbConnection = (DbConnection)Activator.CreateInstance(connectionType))
			{
				if (OPEN)
				{
					dbConnection.ConnectionString = connectionString;
					dbConnection.Open();

					if (transactional)
						dbTransaction = dbConnection.BeginTransaction(isolationLevel);
					else
						dbTransaction = null;
				}

				using (DbCommand dbCommand = dbConnection.CreateCommand())
				{
					dbCommand.Transaction = dbTransaction;
					dbCommand.CommandType = commandType;
					dbCommand.CommandText = commandText;

					if ((object)COMMAND_TIMEOUT != null)
						dbCommand.CommandTimeout = (int)COMMAND_TIMEOUT;

					// add parameters
					if ((object)commandParameters != null)
					{
						foreach (DbParameter commandParameter in commandParameters)
						{
							if ((object)commandParameter.Value == null)
								commandParameter.Value = DBNull.Value;

							dbCommand.Parameters.Add(commandParameter);
						}
					}

					if (COMMAND_PREPARE)
						dbCommand.Prepare();

					rows = new List<__Row>();

					commandBehavior = schemaOnly ? CommandBehavior.SchemaOnly : CommandBehavior.Default;

					using (DbDataReader dbDataReader = (dbCommand.ExecuteReader(commandBehavior)))
					{
						__Row row;
						string key;
						object value;

						if (!schemaOnly)
						{
							do
							{
								if ((object)resultCallback != null)
									resultCallback(resultIndex++);

								while (dbDataReader.Read())
								{
									row = new __Row();

									for (int fieldIndex = 0; fieldIndex < dbDataReader.FieldCount; fieldIndex++)
									{
										key = dbDataReader.GetName(fieldIndex);
										value = dbDataReader.GetValue(fieldIndex);
										value = this.DataTypeFascade.ChangeType<object>(value);

										if (row.ContainsKey(key) || (key ?? string.Empty).Length == 0)
											key = string.Format("Field_{0:0000}", fieldIndex);

										row.Add(key, value);
									}

									rows.Add(row);
								}
							}
							while (dbDataReader.NextResult());
						}
						else
						{
							if (!dbDataReader.CanGetColumnSchema())
								throw new NotSupportedException(string.Format("The connection command type '{0}' does not support schema access.", dbDataReader.GetType().FullName));

							dbColumns = dbDataReader.GetColumnSchema();
							{
								if ((object)dbColumns != null)
								{
									for (long recordIndex = 0; recordIndex < dbColumns.Count; recordIndex++)
									{
										dbColumn = dbColumns[(int)recordIndex];

										propertyInfos = dbColumn.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

										row = new __Row();
										row.Add(string.Empty, dbColumn);

										if ((object)propertyInfos != null)
										{
											for (int fieldIndex = 0; fieldIndex < propertyInfos.Length; fieldIndex++)
											{
												propertyInfo = propertyInfos[fieldIndex];

												if (propertyInfo.GetIndexParameters().Any())
													continue;

												key = propertyInfo.Name;
												value = propertyInfo.GetValue(dbColumn);
												value = this.DataTypeFascade.ChangeType<object>(value);

												row.Add(key, value);
											}
										}
									}
								}
							}
						}
					}

					return rows;
				}
			}
		}

		#endregion
	}
}
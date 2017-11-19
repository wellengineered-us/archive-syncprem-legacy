/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Data;
using System.Data.Common;

namespace TextMetal.Middleware.UnitTests.TestingInfrastructure
{
	public class MockConnection : DbConnection
	{
		#region Constructors/Destructors

		public MockConnection()
		{
		}

		#endregion

		#region Properties/Indexers/Events

		/// <summary>
		/// Gets the name of the current database after a connection is opened, or the database name specified in the connection string before the connection is opened.
		/// </summary>
		/// <returns>
		/// The name of the current database or the name of the database to be used after a connection is opened. The default value is an empty string.
		/// </returns>
		public override string Database
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Gets the name of the database server to which to connect.
		/// </summary>
		/// <returns>
		/// The name of the database server to which to connect. The default value is an empty string.
		/// </returns>
		public override string DataSource
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Gets a string that represents the version of the server to which the object is connected.
		/// </summary>
		/// <returns>
		/// The version of the database. The format of the string returned depends on the specific type of connection you are using.
		/// </returns>
		/// <exception cref="T:System.InvalidOperationException"> <see cref="P:System.Data.Common.DbConnection.ServerVersion" /> was called while the returned Task was not completed and the connection was not opened after a call to <see cref="DbConnection.OpenAsync" />. </exception>
		public override string ServerVersion
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Gets a string that describes the state of the connection.
		/// </summary>
		/// <returns>
		/// The state of the connection. The format of the string returned depends on the specific type of connection you are using.
		/// </returns>
		public override ConnectionState State
		{
			get
			{
				return ConnectionState.Closed;
			}
		}

		/// <summary>
		/// Gets or sets the string used to open the connection.
		/// </summary>
		/// <returns>
		/// The connection string used to establish the initial connection. The exact contents of the connection string depend on the specific data source for this connection. The default value is an empty string.
		/// </returns>
		public override string ConnectionString
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// Starts a database transaction.
		/// </summary>
		/// <returns>
		/// An object representing the new transaction.
		/// </returns>
		/// <param name="isolationLevel"> Specifies the isolation level for the transaction. </param>
		protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
		{
			return null;
		}

		/// <summary>
		/// Changes the current database for an open connection.
		/// </summary>
		/// <param name="databaseName"> Specifies the name of the database for the connection to use. </param>
		public override void ChangeDatabase(string databaseName)
		{
		}

		/// <summary>
		/// Closes the connection to the database. This is the preferred method of closing any open connection.
		/// </summary>
		/// <exception cref="T:System.Data.Common.DbException"> The connection-level error that occurred while opening the connection. </exception>
		public override void Close()
		{
		}

		/// <summary>
		/// Creates and returns a <see cref="T:System.Data.Common.DbCommand" /> object associated with the current connection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Data.Common.DbCommand" /> object.
		/// </returns>
		protected override DbCommand CreateDbCommand()
		{
			return null;
		}

		/// <summary>
		/// Opens a database connection with the settings specified by the <see cref="P:System.Data.Common.DbConnection.ConnectionString" />.
		/// </summary>
		public override void Open()
		{
		}

		#endregion
	}
}
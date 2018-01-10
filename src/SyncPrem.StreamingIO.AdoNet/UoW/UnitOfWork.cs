/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Data;
using System.Data.Common;

namespace SyncPrem.StreamingIO.AdoNet.UoW
{
	/// <summary>
	/// Represents an atomic set of data operations on a single connection/transaction.
	/// </summary>
	public sealed class UnitOfWork : IUnitOfWork
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the unitOfWork class.
		/// </summary>
		public UnitOfWork(DbConnection connection, DbTransaction transaction)
		{
			this.connection = connection;
			this.transaction = transaction;
		}

		#endregion

		#region Fields/Constants

		private readonly DbConnection connection;
		private readonly DbTransaction transaction;
		private bool completed;
		private IDisposable context;
		private bool disposed;
		private bool diverged;

		#endregion

		#region Properties/Indexers/Events

		/// <summary>
		/// Gets the underlying ADO.NET connection.
		/// </summary>
		public DbConnection Connection
		{
			get
			{
				if (this.Disposed)
					throw new ObjectDisposedException(typeof(UnitOfWork).FullName);

				return this.connection;
			}
		}

		/// <summary>
		/// Gets the underlying ADO.NET transaction.
		/// </summary>
		public DbTransaction Transaction
		{
			get
			{
				if (this.Disposed)
					throw new ObjectDisposedException(typeof(UnitOfWork).FullName);

				return this.transaction;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the current instance has been completed.
		/// </summary>
		public bool Completed
		{
			get
			{
				return this.completed;
			}
			private set
			{
				this.completed = value;
			}
		}

		/// <summary>
		/// Gets the context object.
		/// </summary>
		public IDisposable Context
		{
			get
			{
				if (this.Disposed)
					throw new ObjectDisposedException(typeof(UnitOfWork).FullName);

				return this.context;
			}
			set
			{
				this.context = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the current instance has been disposed.
		/// </summary>
		public bool Disposed
		{
			get
			{
				return this.disposed;
			}
			private set
			{
				this.disposed = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the current instance has been diverged.
		/// </summary>
		public bool Diverged
		{
			get
			{
				return this.diverged;
			}
			private set
			{
				this.diverged = value;
			}
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// Creates a new unit of work (and opens the underlying connection) for the given connection type and connection string with an optional transaction started.
		/// </summary>
		/// <param name="connectionType"> The run-time type of the connection to use. </param>
		/// <param name="connectionString"> The ADO.NET provider connection string to use. </param>
		/// <param name="transactional"> A value indicating whether a new local data source transaction isstarted on the connection. </param>
		/// <param name="isolationLevel"> A value indicating the transaction isolation level. </param>
		/// <returns> An instance of teh unitOfWork ready for execution of operations. This should be wrapped in a using(...){} block for an optimal usage scenario. </returns>
		public static IUnitOfWork Create(Type connectionType, string connectionString, bool transactional, IsolationLevel isolationLevel = IsolationLevel.Unspecified)
		{
			UnitOfWork unitOfWork;
			DbConnection dbConnection;
			DbTransaction dbTransaction;
			const bool OPEN = true;

			if ((object)connectionType == null)
				throw new ArgumentNullException(nameof(connectionType));

			if ((object)connectionString == null)
				throw new ArgumentNullException(nameof(connectionString));

			dbConnection = (DbConnection)Activator.CreateInstance(connectionType);

			if (OPEN)
			{
				dbConnection.ConnectionString = connectionString;
				dbConnection.Open();

				if (transactional)
					dbTransaction = dbConnection.BeginTransaction(isolationLevel);
				else
					dbTransaction = null;
			}

			unitOfWork = new UnitOfWork(dbConnection, dbTransaction);

			return unitOfWork;
		}

		public static IUnitOfWork From(DbConnection dbConnection, DbTransaction dbTransaction)
		{
			UnitOfWork unitOfWork;

			if ((object)dbConnection == null)
				throw new ArgumentNullException(nameof(dbConnection));

			unitOfWork = new UnitOfWork(dbConnection, dbTransaction);

			return unitOfWork;
		}

		/// <summary>
		/// Contains the logic to 'adjudicate' or realize a transaction based on state of the current unit of work instance.
		/// </summary>
		private void Adjudicate()
		{
			try
			{
				if ((object)this.Transaction != null)
				{
					if (this.Completed && !this.Diverged)
						this.Transaction.Commit();
					else
						this.Transaction.Rollback();
				}
			}
			finally
			{
				// destroy and tear-down the context
				if ((object)this.Context != null)
					this.Context.Dispose();

				// destroy and tear-down the transaction
				if ((object)this.Transaction != null)
					this.Transaction.Dispose();

				// destroy and tear-down the connection
				if ((object)this.Connection != null)
					this.Connection.Dispose();
			}
		}

		public /*virtual*/ void Close()
		{
			if (this.Disposed)
				return;

			this.Dispose(true);
			GC.SuppressFinalize(this);

			this.Disposed = true;
		}

		/// <summary>
		/// Indicates that all operations within the unit of work have completed successfully. This method should only be called once.
		/// </summary>
		public void Complete()
		{
			if (this.Disposed)
				throw new ObjectDisposedException(typeof(UnitOfWork).FullName);

			if (this.Completed)
				throw new InvalidOperationException(string.Format("The current unit of work is already complete. You should dispose of the unit of work."));

			this.Completed = true;
		}

		private /*protected virtual*/ void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Adjudicate();
			}
		}

		/// <summary>
		/// Dispose of the unit of work.
		/// </summary>
		public void Dispose()
		{
			this.Close();
		}

		/// <summary>
		/// Indicates that at least one operation within the unit of work cause a failure in data concurrency or nullipotency. This forces the entire unit of work to yield an incomplete status. This method can be called any number of times.
		/// </summary>
		public void Divergent()
		{
			if (this.Disposed)
				throw new ObjectDisposedException(typeof(UnitOfWork).FullName);

			this.Diverged = true;
		}

		#endregion
	}
}
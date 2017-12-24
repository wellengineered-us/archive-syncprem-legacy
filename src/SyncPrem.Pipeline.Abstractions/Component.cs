/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace SyncPrem.Pipeline.Abstractions
{
	public abstract class Component : IComponent
	{
		#region Constructors/Destructors

		protected Component()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly Guid componentId = new Guid();
		private bool created;
		private bool disposed;

		#endregion

		#region Properties/Indexers/Events

		public Guid ComponentId
		{
			get
			{
				return this.componentId;
			}
		}

		public bool Created
		{
			get
			{
				return this.created;
			}
			private set
			{
				this.created = value;
			}
		}

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

		#endregion

		#region Methods/Operators

		public void Create()
		{
			this.Initialize();
		}

		protected virtual void Create(bool creating)
		{
			if (creating)
			{
				// do nothing
			}
		}

		public void Dispose()
		{
			this.Terminate();
		}

		/// <summary>
		/// Note: Never change this to call other virtual methods on this type
		/// like Donkey(), since the state on subclasses has already been
		/// torn down.  This is the last code to run on cleanup for this type.
		/// </summary>
		/// <param name="disposing"> </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// do nothing
			}
		}

		public virtual void Initialize()
		{
			if (this.Created)
				return;

			//GC.ReRegisterForFinalize(this);
			this.Create(true);

			this.Created = true;
		}

		public virtual void Terminate()
		{
			if (this.Disposed)
				return;

			this.Dispose(true);
			GC.SuppressFinalize(this);

			this.Disposed = true;
		}

		#endregion
	}
}
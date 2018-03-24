/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace TextMetal.Middleware.Solder.Primitives
{
	public abstract class Lifecycle : ICreatable, IDisposableEx
	{
		#region Constructors/Destructors

		protected Lifecycle()
		{
		}

		#endregion

		#region Fields/Constants

		private bool isCreated;
		private bool isDisposed;

		#endregion

		#region Properties/Indexers/Events

		public bool IsCreated
		{
			get
			{
				return this.isCreated;
			}
			private set
			{
				this.isCreated = value;
			}
		}

		public bool IsDisposed
		{
			get
			{
				return this.isDisposed;
			}
			private set
			{
				this.isDisposed = value;
			}
		}

		#endregion

		#region Methods/Operators

		public void Create()
		{
			this.Initialize();
		}

		protected abstract void Create(bool creating);

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
		protected abstract void Dispose(bool disposing);

		public void Initialize()
		{
			if (this.IsCreated)
				return;

			//GC.ReRegisterForFinalize(this);
			this.Create(true);
			this.IsCreated = true;
		}

		public void Terminate()
		{
			if (this.IsDisposed)
				return;

			this.Dispose(true);
			GC.SuppressFinalize(this);
			this.IsDisposed = true;
		}

		#endregion
	}
}
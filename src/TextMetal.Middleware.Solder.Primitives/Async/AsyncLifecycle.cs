/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;
using System.Threading.Tasks;

namespace TextMetal.Middleware.Solder.Primitives.Async
{
	public abstract class AsyncLifecycle : IAsyncCreatable, IAsyncDisposable
	{
		#region Constructors/Destructors

		protected AsyncLifecycle()
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

		public async Task CreateAsync(CancellationToken cancellationToken)
		{
			await this.InitializeAsync(cancellationToken);
		}

		protected abstract Task CreateAsync(bool creating, CancellationToken cancellationToken);

		public async Task DisposeAsync(CancellationToken cancellationToken)
		{
			await this.TerminateAsync(cancellationToken);
		}

		/// <summary>
		/// Note: Never change this to call other virtual methods on this type
		/// like Donkey(), since the state on subclasses has already been
		/// torn down.  This is the last code to run on cleanup for this type.
		/// </summary>
		/// <param name="disposing"> </param>
		/// <param name="cancellationToken"> </param>
		protected abstract Task DisposeAsync(bool disposing, CancellationToken cancellationToken);

		protected void ExplicitSetIsCreated()
		{
			//GC.ReRegisterForFinalize(this);
			this.IsCreated = true;
		}

		protected void ExplicitSetIsDisposed()
		{
			this.IsDisposed = true;
			GC.SuppressFinalize(this);
		}

		public async Task InitializeAsync(CancellationToken cancellationToken)
		{
			if (this.IsCreated)
				return;

			await this.CreateAsync(true, cancellationToken);
			this.MaybeSetIsCreated();
		}

		protected virtual void MaybeSetIsCreated()
		{
			this.ExplicitSetIsCreated();
		}

		protected virtual void MaybeSetIsDisposed()
		{
			this.ExplicitSetIsDisposed();
		}

		public async Task TerminateAsync(CancellationToken cancellationToken)
		{
			if (this.IsDisposed)
				return;

			await this.DisposeAsync(true, cancellationToken);
			this.MaybeSetIsDisposed();
		}

		#endregion
	}
}
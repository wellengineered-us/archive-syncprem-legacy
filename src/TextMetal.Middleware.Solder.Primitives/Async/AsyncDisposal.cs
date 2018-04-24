/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace TextMetal.Middleware.Solder.Primitives.Async
{
	public sealed class AsyncDisposal : IDisposable
	{
		#region Constructors/Destructors

		private AsyncDisposal(IAsyncDisposable disposable, CancellationToken cancellationToken)
		{
			this.disposable = disposable;
			this.cancellationToken = cancellationToken;
		}

		#endregion

		#region Fields/Constants

		private static readonly TextWriter stdOut = Console.Out;
		private readonly IAsyncDisposable disposable;
		private readonly CancellationToken cancellationToken;

		#endregion

		#region Properties/Indexers/Events

		private static TextWriter StdOut
		{
			get
			{
				return stdOut;
			}
		}

		private IAsyncDisposable Disposable
		{
			get
			{
				return this.disposable;
			}
		}

		private CancellationToken CancellationToken
		{
			get
			{
				return this.cancellationToken;
			}
		}

		#endregion

		#region Methods/Operators

		public static IDisposable Await(IAsyncDisposable disposable, CancellationToken cancellationToken)
		{
			return new AsyncDisposal(disposable, cancellationToken);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if ((object)this.Disposable != null)
					this.DisposeAsync().GetAwaiter().GetResult();
			}
		}

		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private async Task DisposeAsync()
		{
			await StdOut.WriteLineAsync(string.Format("{0}::{1}", nameof(AsyncDisposal), nameof(this.DisposeAsync)));
			await this.Disposable.DisposeAsync(this.CancellationToken);
		}

		#endregion
	}
}
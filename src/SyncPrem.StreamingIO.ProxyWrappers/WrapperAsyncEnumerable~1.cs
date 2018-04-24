using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncPrem.StreamingIO.ProxyWrappers
{
	public class WrapperAsyncEnumerable<T> : IAsyncEnumerable<T>
	{
		#region Constructors/Destructors

		public WrapperAsyncEnumerable(string sourceLabel,
			Func<long, T, Task<T>> asyncItemCallback,
			Func<string, long, bool, double, Task> asyncProcessingCallback,
			IAsyncEnumerable<T> innerAsyncEnumerable)
		{
			if ((object)innerAsyncEnumerable == null)
				throw new ArgumentNullException(nameof(innerAsyncEnumerable));

			this.sourceLabel = sourceLabel;
			this.asyncItemCallback = asyncItemCallback;
			this.asyncProcessingCallback = asyncProcessingCallback;
			this.innerAsyncEnumerable = innerAsyncEnumerable;

			this.__state = false; // maps to yield state -2: before the first call to GetEnumerator() from the creating thread
			this.__initialThreadId = Environment.CurrentManagedThreadId;
		}

		#endregion

		#region Fields/Constants

		private readonly Func<long, T, Task<T>> asyncItemCallback;
		private readonly Func<string, long, bool, double, Task> asyncProcessingCallback;

		private readonly IAsyncEnumerable<T> innerAsyncEnumerable;
		private readonly string sourceLabel;
		private int __initialThreadId;
		private bool __state;

		#endregion

		#region Properties/Indexers/Events

		private IAsyncEnumerable<T> InnerAsyncEnumerable
		{
			get
			{
				return this.innerAsyncEnumerable;
			}
		}

		#endregion

		#region Methods/Operators

		public IAsyncEnumerator<T> GetEnumerator()
		{
			IAsyncEnumerator<T> asyncEnumerator;

			if (this.__state || this.__initialThreadId != Environment.CurrentManagedThreadId)
				throw new InvalidOperationException(string.Format("Invalid enumerable state or execution thread ID."));

			this.__state = true; // maps to yield state 0: "Before" - MoveNext() hasn't been called yet on enumerator instance

			asyncEnumerator = this.InnerAsyncEnumerable.GetEnumerator();
			asyncEnumerator = new WrapperAsyncEnumerator<T>(this.__initialThreadId,
				this.sourceLabel, this.asyncItemCallback, this.asyncProcessingCallback,
				asyncEnumerator);

			return asyncEnumerator;
		}

		#endregion
	}
}
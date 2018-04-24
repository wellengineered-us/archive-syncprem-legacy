using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncPrem.StreamingIO.ProxyWrappers
{
	public class WrapperAsyncEnumerator<T> : IAsyncEnumerator<T>
	{
		#region Constructors/Destructors

		public WrapperAsyncEnumerator(int __initialThreadId,
			string sourceLabel,
			Func<long, T, Task<T>> asyncItemCallback,
			Func<string, long, bool, double, Task> asyncProcessingCallback,
			IAsyncEnumerator<T> innerAsyncEnumerator)
		{
			if ((object)innerAsyncEnumerator == null)
				throw new ArgumentNullException(nameof(innerAsyncEnumerator));

			this.__initialThreadId = __initialThreadId;
			this.sourceLabel = sourceLabel;
			this.asyncItemCallback = asyncItemCallback;
			this.asyncProcessingCallback = asyncProcessingCallback;
			this.innerAsyncEnumerator = innerAsyncEnumerator;
			this.__state = 0; // maps to yield state 0: "Before" - MoveNext() hasn't been called yet on enumerator instance
		}

		#endregion

		#region Fields/Constants

		private const int PROCESSING_CALLBACK_WINDOW_SIZE = 1000;
		private readonly int __initialThreadId;
		private readonly Func<long, T, Task<T>> asyncItemCallback;
		private readonly Func<string, long, bool, double, Task> asyncProcessingCallback;
		private readonly IAsyncEnumerator<T> innerAsyncEnumerator;
		private readonly string sourceLabel;
		private int __state;
		private long itemIndex;
		private DateTime startUtc;

		#endregion

		#region Properties/Indexers/Events

		public T Current
		{
			get
			{
				return this.InnerAsyncEnumerator.Current;
			}
		}

		private IAsyncEnumerator<T> InnerAsyncEnumerator
		{
			get
			{
				return this.innerAsyncEnumerator;
			}
		}

		#endregion

		#region Methods/Operators

		public void Dispose()
		{
			this.InnerAsyncEnumerator.Dispose();
		}

		public async Task<bool> MoveNext(CancellationToken cancellationToken)
		{
			bool hasNext;
			T current;

			//if (this.__initialThreadId != Environment.CurrentManagedThreadId)
				//throw new InvalidOperationException(string.Format("Invalid enumerator execution thread ID."));

			while (!cancellationToken.IsCancellationRequested &&
					(this.__state == 0 || this.__state == 1))
			{
				switch (this.__state)
				{
					case 0:
						this.__state = -1;
						this.itemIndex = 0;
						this.startUtc = DateTime.UtcNow;

						// call pre-proc
						if ((object)this.asyncProcessingCallback != null)
							await this.asyncProcessingCallback(this.sourceLabel, -1, false, (DateTime.UtcNow - this.startUtc).TotalSeconds);

						this.__state = 1;
						break;
					case 1:
						this.__state = -1;

						hasNext = await this.InnerAsyncEnumerator.MoveNext(cancellationToken);

						if (hasNext)
						{
							current = this.InnerAsyncEnumerator.Current;

							// call on item
							if ((this.itemIndex % PROCESSING_CALLBACK_WINDOW_SIZE) == 0)
							{
								if ((object)this.asyncProcessingCallback != null)
									await this.asyncProcessingCallback(this.sourceLabel, this.itemIndex, false, (DateTime.UtcNow - this.startUtc).TotalSeconds);
							}

							if ((object)this.asyncItemCallback != null)
								current = await this.asyncItemCallback(this.itemIndex, current);

							this.itemIndex++;

							this.__state = 1;
							return true;
						}
						else
						{
							this.__state = -1;

							// post-proc
							if ((object)this.asyncProcessingCallback != null)
								await this.asyncProcessingCallback(this.sourceLabel, this.itemIndex, true, (DateTime.UtcNow - this.startUtc).TotalSeconds);

							return false;
						}
				}
			}

			return false;
		}

		#endregion
	}
}
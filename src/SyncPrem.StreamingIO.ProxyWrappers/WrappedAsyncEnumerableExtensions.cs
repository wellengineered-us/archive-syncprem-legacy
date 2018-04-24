/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncPrem.StreamingIO.ProxyWrappers
{
	public static class WrappedAsyncEnumerableExtensions
	{
		#region Fields/Constants

		private const int PROCESSING_CALLBACK_WINDOW_SIZE = 1000;

		#endregion

		#region Methods/Operators

		public static async Task ForceAsyncEnumeration<T>(this IAsyncEnumerable<T> asyncEnumerable, CancellationToken cancellationToken)
		{
			IAsyncEnumerator<T> asyncEnumerator;

			if ((object)asyncEnumerable == null)
				throw new ArgumentNullException(nameof(asyncEnumerable));

			asyncEnumerator = asyncEnumerable.GetEnumerator();

			if ((object)asyncEnumerator == null)
				throw new InvalidOperationException(nameof(asyncEnumerator));

			while (await asyncEnumerator.MoveNext(cancellationToken))
			{
				// do nothing
			}
		}

		public static IAsyncEnumerable<TItem> GetWrappedAsyncEnumerable<TItem>(this IAsyncEnumerable<TItem> asyncEnumerable, string sourceLabel, Func<long, TItem, Task<TItem>> asyncItemCallback, Func<string, long, bool, double, Task> asyncProcessingCallback)
		{
			if ((object)asyncEnumerable == null)
				throw new ArgumentNullException(nameof(asyncEnumerable));

			return new WrapperAsyncEnumerable<TItem>(sourceLabel, asyncItemCallback, asyncProcessingCallback, asyncEnumerable);
		}

		public static IAsyncEnumerable<T> GetWrappedAsyncEnumerable<T>(this IAsyncEnumerable<T> asyncEnumerable, Func<long, T, Task<T>> asyncItemCallback)
		{
			return asyncEnumerable.GetWrappedAsyncEnumerable(null, asyncItemCallback, null);
		}

		#endregion
	}
}
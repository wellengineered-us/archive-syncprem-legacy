/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace SyncPrem.Infrastructure.Wrappers
{
	public static class WrappedEnumerableExtensions
	{
		#region Fields/Constants

		private const int PROCESSING_CALLBACK_WINDOW_SIZE = 5;

		#endregion

		#region Methods/Operators

		public static IEnumerable<T> GetMetricsWrappedEnumerable<T>(this IEnumerable<T> enumerable, string source, Func<T, T> itemCallback, Action<string, ulong, bool, double> processingCallback)
		{
			ulong itemCount = 0;
			DateTime startUtc;

			startUtc = DateTime.UtcNow;

			if ((object)enumerable == null)
				throw new ArgumentNullException(nameof(enumerable));

			foreach (T item in enumerable)
			{
				itemCount++;

				if ((itemCount % PROCESSING_CALLBACK_WINDOW_SIZE) == 0)
				{
					if ((object)processingCallback != null)
						; //processingCallback(source, itemCount, false, (DateTime.UtcNow - startUtc).TotalSeconds);
				}

				if ((object)itemCallback != null)
					yield return itemCallback(item);
				else
					yield return item;
			}

			if ((object)processingCallback != null)
				processingCallback(source, itemCount, true, (DateTime.UtcNow - startUtc).TotalSeconds);
		}

		public static IEnumerable<T> GetWrappedEnumerable<T>(this IEnumerable<T> enumerable, Func<T, T> itemCallback)
		{
			if ((object)enumerable == null)
				throw new ArgumentNullException(nameof(enumerable));

			foreach (T item in enumerable)
			{
				if ((object)itemCallback != null)
					yield return itemCallback(item);
				else
					yield return item;
			}
		}

		#endregion
	}
}
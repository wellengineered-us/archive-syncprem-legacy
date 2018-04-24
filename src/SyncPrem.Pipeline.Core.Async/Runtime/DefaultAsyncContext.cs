/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Async;
using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.StreamingIO.ProxyWrappers;

namespace SyncPrem.Pipeline.Core.Async.Runtime
{
	public sealed class DefaultAsyncContext : AsyncComponent, IAsyncContext
	{
		#region Constructors/Destructors

		public DefaultAsyncContext()
		{
		}

		#endregion

		#region Fields/Constants

		private static readonly TextWriter stdOut = Console.Out;

		private readonly IDictionary<string, object> globalState = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);
		private readonly IDictionary<IComponent, IDictionary<string, object>> localState = new ConcurrentDictionary<IComponent, IDictionary<string, object>>();

		#endregion

		#region Properties/Indexers/Events

		private static TextWriter StdOut
		{
			get
			{
				return stdOut;
			}
		}

		public IDictionary<string, object> GlobalState
		{
			get
			{
				return this.globalState;
			}
		}

		public IDictionary<IComponent, IDictionary<string, object>> LocalState
		{
			get
			{
				return this.localState;
			}
		}

		#endregion

		#region Methods/Operators

		private static Task<IAsyncRecord> ApplyRecordIndexAsync(long recordIndex, IAsyncRecord asyncRecord)
		{
			if ((object)asyncRecord == null)
				throw new ArgumentNullException(nameof(asyncRecord));

			asyncRecord.Index = recordIndex;

			return Task.FromResult(asyncRecord);
		}

		private static Task<T> LogItem<T>(long itemIndex, T item)
		{
			//Console.WriteLine(item.SafeToString(null, "<null>"));
			return Task.FromResult(item);
		}

		public IAsyncChannel CreateChannelAsync(IAsyncEnumerable<IAsyncRecord> asyncRecords)
		{
			if ((object)asyncRecords == null)
				throw new ArgumentNullException(nameof(asyncRecords));

			asyncRecords = asyncRecords.GetWrappedAsyncEnumerable(ApplyRecordIndexAsync);
			asyncRecords = asyncRecords.GetWrappedAsyncEnumerable("records", LogItem, this.LogMetrics);
			return new DefaultAsyncChannel(asyncRecords);
		}

		private async Task LogMetrics(string sourceLabel, long itemIndex, bool isCompleted, double rollingTiming)
		{
			if (itemIndex == -1 || isCompleted)
				await StdOut.WriteLineAsync(string.Format("{0}@{4}-{5:N}: itemIndex = {1}, isCompleted = {2}, rollingTiming = {3}", sourceLabel, itemIndex, isCompleted, rollingTiming, Environment.CurrentManagedThreadId, this.ComponentId));

			if (isCompleted && itemIndex == 0)
				throw new InvalidOperationException();
		}

		#endregion
	}
}
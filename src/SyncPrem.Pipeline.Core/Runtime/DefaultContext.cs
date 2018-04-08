﻿/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.StreamingIO.ProxyWrappers;

namespace SyncPrem.Pipeline.Core.Runtime
{
	public sealed class DefaultContext : Component, IContext
	{
		#region Constructors/Destructors

		public DefaultContext()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly IDictionary<string, object> globalState = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);
		private readonly IDictionary<IComponent, IDictionary<string, object>> localState = new ConcurrentDictionary<IComponent, IDictionary<string, object>>();

		#endregion

		#region Properties/Indexers/Events

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

		private static IRecord ApplyRecordIndex(long recordIndex, IRecord record)
		{
			if ((object)record == null)
				throw new ArgumentNullException(nameof(record));

			record.Index = recordIndex;
			return record;
		}

		private static T LogItem<T>(long itemIndex, T item)
		{
			//Console.WriteLine(item.SafeToString(null, "<null>"));
			return item;
		}

		private static void LogMetrics(string sourceLabel, long itemIndex, bool isCompleted, double rollingTiming)
		{
			if (itemIndex == -1 || isCompleted)
				Console.WriteLine("{0}@{4}: itemIndex = {1}, isCompleted = {2}, rollingTiming = {3}", sourceLabel, itemIndex, isCompleted, rollingTiming, Environment.CurrentManagedThreadId);
		}

		public IChannel CreateChannel(IEnumerable<IRecord> records)
		{
			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			records = records.GetWrappedEnumerable(ApplyRecordIndex);
			records = records.GetWrappedEnumerable("records", LogItem, LogMetrics);
			return new DefaultChannel(records);
		}

		#endregion
	}
}
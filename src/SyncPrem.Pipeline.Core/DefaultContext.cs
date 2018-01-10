/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Channel;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Core.Channels;
using SyncPrem.StreamingIO.Primitives;
using SyncPrem.StreamingIO.ProxyWrappers;

namespace SyncPrem.Pipeline.Core
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

		private static T LogItem<T>(T item)
		{
			//Console.WriteLine(item.SafeToString(null, "<null>"));
			return item;
		}

		private static void LogMetrics(string label, ulong count, bool done, double timing)
		{
			Console.WriteLine("{0}: count = {1}, done = {2}, timing = {3}", label, count, done, timing);
		}

		public IChannel CreateChannel(ISchema schema, IEnumerable<IRecord> records)
		{
			if ((object)schema == null)
				throw new ArgumentNullException(nameof(schema));

			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			return new Channel(schema, records.GetMetricsWrappedEnumerable("records", LogItem, LogMetrics)); // TODO DI/IoC
		}

		#endregion
	}
}
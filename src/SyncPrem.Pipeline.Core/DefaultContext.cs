/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Payload;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Core.Payloads;
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

		private readonly Stack<IPipelineMetadata> metadataChain = new Stack<IPipelineMetadata>();

		#endregion

		#region Properties/Indexers/Events

		public Stack<IPipelineMetadata> MetadataChain
		{
			get
			{
				return this.metadataChain;
			}
		}

		#endregion

		#region Methods/Operators

		private static void LogMetrics(string label, ulong count, bool done, double timing)
		{
			Console.WriteLine("{0}: count = {1}, done = {2}, timing = {3}", label, count, done, timing);
		}

		public IPipelineMessage CreateMessage(IEnumerable<IRecord> records)
		{
			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			return new DefaultPipelineMessage(records.GetMetricsWrappedEnumerable("records", null, LogMetrics)); // TODO DI/IoC
		}

		public IPipelineMetadata CreateMetadata(IEnumerable<IField> fields)
		{
			if ((object)fields == null)
				throw new ArgumentNullException(nameof(fields));

			return new DefaultPipelineMetadata(fields.GetMetricsWrappedEnumerable("fields", null, LogMetrics)); // TODO DI/IoC
		}

		#endregion
	}
}
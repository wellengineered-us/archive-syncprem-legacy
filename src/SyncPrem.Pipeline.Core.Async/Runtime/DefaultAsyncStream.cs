using System;
using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions.Async;
using SyncPrem.Pipeline.Abstractions.Async.Runtime;

namespace SyncPrem.Pipeline.Core.Async.Runtime
{
	public sealed class DefaultAsyncStream : AsyncComponent, IAsyncStream
	{
		#region Constructors/Destructors

		public DefaultAsyncStream(IAsyncEnumerable<IAsyncRecord> records)
		{
			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			this.records = records;
		}

		#endregion

		#region Fields/Constants

		private readonly IAsyncEnumerable<IAsyncRecord> records;

		#endregion

		#region Properties/Indexers/Events

		public IAsyncEnumerable<IAsyncRecord> Records
		{
			get
			{
				return this.records;
			}
		}

		#endregion

		#region Methods/Operators

		public IAsyncEnumerator<IAsyncRecord> GetEnumerator()
		{
			return this.Records.GetEnumerator();
		}

		#endregion
	}
}
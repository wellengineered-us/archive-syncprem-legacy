/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace SyncPrem.Pipeline.Abstractions.Async.Runtime
{
	public sealed class AsyncStream : AsyncComponent, IAsyncStream
	{
		#region Constructors/Destructors

		public AsyncStream(IAsyncEnumerable<IAsyncRecord> innerEnumerable)
		{
			if ((object)innerEnumerable == null)
				throw new ArgumentNullException(nameof(innerEnumerable));

			this.innerEnumerable = innerEnumerable;
		}

		#endregion

		#region Fields/Constants

		private readonly IAsyncEnumerable<IAsyncRecord> innerEnumerable;

		#endregion

		#region Properties/Indexers/Events

		private IAsyncEnumerable<IAsyncRecord> InnerEnumerable
		{
			get
			{
				return this.innerEnumerable;
			}
		}

		#endregion

		#region Methods/Operators

		public IAsyncEnumerator<IAsyncRecord> GetEnumerator()
		{
			return this.InnerEnumerable.GetEnumerator();
		}

		#endregion
	}
}
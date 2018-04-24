/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections;
using System.Collections.Generic;

namespace SyncPrem.Pipeline.Abstractions.Sync.Runtime
{
	public sealed class SyncStream : SyncComponent, ISyncStream
	{
		#region Constructors/Destructors

		public SyncStream(IEnumerable<ISyncRecord> innerEnumerable)
		{
			if ((object)innerEnumerable == null)
				throw new ArgumentNullException(nameof(innerEnumerable));

			this.innerEnumerable = innerEnumerable;
		}

		#endregion

		#region Fields/Constants

		private readonly IEnumerable<ISyncRecord> innerEnumerable;

		#endregion

		#region Properties/Indexers/Events

		private IEnumerable<ISyncRecord> InnerEnumerable
		{
			get
			{
				return this.innerEnumerable;
			}
		}

		#endregion

		#region Methods/Operators

		public IEnumerator<ISyncRecord> GetEnumerator()
		{
			return this.InnerEnumerable.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
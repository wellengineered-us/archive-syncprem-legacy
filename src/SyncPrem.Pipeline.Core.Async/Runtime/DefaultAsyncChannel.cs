﻿/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions.Async;
using SyncPrem.Pipeline.Abstractions.Async.Runtime;

namespace SyncPrem.Pipeline.Core.Async.Runtime
{
	public sealed class DefaultAsyncChannel : AsyncComponent, IAsyncChannel
	{
		#region Constructors/Destructors

		public DefaultAsyncChannel(IAsyncEnumerable<IAsyncRecord> records)
		{
			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			this.records = new DefaultAsyncStream(records);
		}

		#endregion

		#region Fields/Constants

		private readonly IAsyncStream records;

		#endregion

		#region Properties/Indexers/Events

		public IAsyncStream Records
		{
			get
			{
				return this.records;
			}
		}

		#endregion
	}
}
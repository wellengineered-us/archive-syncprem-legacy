/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Runtime;

namespace SyncPrem.Pipeline.Core.Runtime
{
	public sealed class DefaultChannel : Component, IChannel
	{
		#region Constructors/Destructors

		public DefaultChannel(IEnumerable<IRecord> records)
		{
			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			this.records = records;
		}

		#endregion

		#region Fields/Constants

		private readonly IEnumerable<IRecord> records;

		#endregion

		#region Properties/Indexers/Events

		public IEnumerable<IRecord> Records
		{
			get
			{
				return this.records;
			}
		}

		#endregion
	}
}
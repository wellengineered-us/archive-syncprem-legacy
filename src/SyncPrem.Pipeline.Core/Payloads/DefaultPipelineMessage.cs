/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Payload;
using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.Pipeline.Core.Payloads
{
	public sealed class DefaultPipelineMessage : Component, IPipelineMessage
	{
		#region Constructors/Destructors

		public DefaultPipelineMessage(IEnumerable<IRecord> records)
		{
			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			this.records = records;
		}

		#endregion

		#region Fields/Constants

		private IEnumerable<IRecord> records;

		#endregion

		#region Properties/Indexers/Events

		public IEnumerable<IRecord> Records
		{
			get
			{
				return this.records;
			}
			private set
			{
				this.records = value;
			}
		}

		#endregion

		#region Methods/Operators

		public IPipelineMessage ApplyWrap(Func<IEnumerable<IRecord>, IEnumerable<IRecord>> wrapperCallback)
		{
			if ((object)wrapperCallback == null)
				throw new ArgumentNullException(nameof(wrapperCallback));

			this.Records = wrapperCallback(this.Records);

			return this; // fluent API
		}

		#endregion
	}
}
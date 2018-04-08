/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Destination;
using SyncPrem.StreamingIO.Primitives;
using SyncPrem.StreamingIO.ProxyWrappers;

namespace SyncPrem.Pipeline.Core.Connectors
{
	public class NullDestinationConnector : DestinationConnector<StageSpecificConfiguration>
	{
		#region Constructors/Destructors

		public NullDestinationConnector()
		{
		}

		#endregion

		#region Methods/Operators

		protected override Task ConsumeAsyncInternal(IContext context, RecordConfiguration configuration, IChannel channel, CancellationToken cancellationToken, IProgress<int> progress)
		{
			throw new NotImplementedException();
		}

		protected override void ConsumeInternal(IContext context, RecordConfiguration configuration, IChannel channel)
		{
			IEnumerable<IRecord> records;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)channel == null)
				throw new ArgumentNullException(nameof(channel));

			this.AssertValidConfiguration();

			records = channel.Records;

			if ((object)records == null)
				throw new SyncPremException(nameof(records));

			records.ForceEnumeration(); // force execution
		}

		protected override void Create(bool creating)
		{
			// do nothing
			base.Create(creating);
		}

		protected override void Dispose(bool disposing)
		{
			// do nothing
			base.Dispose(disposing);
		}

		protected override Task PostExecuteAsyncInternal(IContext context, RecordConfiguration configuration, CancellationToken cancellationToken, IProgress<int> progress)
		{
			throw new NotImplementedException();
		}

		protected override void PostExecuteInternal(IContext context, RecordConfiguration configuration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();
		}

		protected override Task PreExecuteAsyncInternal(IContext context, RecordConfiguration configuration, CancellationToken cancellationToken, IProgress<int> progress)
		{
			throw new NotImplementedException();
		}

		protected override void PreExecuteInternal(IContext context, RecordConfiguration configuration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();
		}

		#endregion
	}
}
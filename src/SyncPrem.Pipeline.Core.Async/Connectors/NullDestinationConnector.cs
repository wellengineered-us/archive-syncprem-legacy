/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Async.Stage.Connector.Destination;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.StreamingIO.Primitives;
using SyncPrem.StreamingIO.ProxyWrappers;

namespace SyncPrem.Pipeline.Core.Async.Connectors
{
	public class NullDestinationConnector : AsyncDestinationConnector<StageSpecificConfiguration>
	{
		#region Constructors/Destructors

		public NullDestinationConnector()
		{
		}

		#endregion

		#region Methods/Operators

		protected override async Task ConsumeAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, IAsyncChannel asyncChannel, CancellationToken cancellationToken)
		{
			IAsyncEnumerable<IAsyncRecord> asyncRecords;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if ((object)asyncChannel == null)
				throw new ArgumentNullException(nameof(asyncChannel));

			this.AssertValidConfiguration();

			asyncRecords = asyncChannel.Records;

			if ((object)asyncRecords == null)
				throw new SyncPremException(nameof(asyncRecords));

			await asyncRecords.ForceAsyncEnumeration(cancellationToken); // force execution
		}

		protected override async Task CreateAsync(bool creating, CancellationToken cancellationToken)
		{
			// do nothing
			await base.CreateAsync(creating, cancellationToken);
		}

		protected override async Task DisposeAsync(bool disposing, CancellationToken cancellationToken)
		{
			// do nothing
			await base.DisposeAsync(disposing, cancellationToken);
		}

		protected override Task PostExecuteAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			return Task.CompletedTask;
		}

		protected override Task PreExecuteAsyncInternal(IAsyncContext asyncContext, RecordConfiguration configuration, CancellationToken cancellationToken)
		{
			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			this.AssertValidConfiguration();

			return Task.CompletedTask;
		}

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Linq;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Payload;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Destination;
using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.Pipeline.Core.Connectors.Null
{
	public class NullDestinationConnector : DestinationConnector<StageSpecificConfiguration>
	{
		#region Constructors/Destructors

		public NullDestinationConnector()
		{
		}

		#endregion

		#region Methods/Operators

		protected override void ConsumeRecord(IContext context, RecordConfiguration recordConfiguration, IPipelineMessage pipelineMessage)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			if ((object)pipelineMessage == null)
				throw new ArgumentNullException(nameof(pipelineMessage));

			// force full enumeration
			pipelineMessage.Records.ToArray();
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

		protected override void PostExecuteRecord(IContext context, RecordConfiguration recordConfiguration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));
		}

		protected override void PreExecuteRecord(IContext context, RecordConfiguration recordConfiguration)
		{
			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));
		}

		#endregion
	}
}
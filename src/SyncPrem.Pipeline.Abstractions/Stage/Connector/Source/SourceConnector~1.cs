/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Channel;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;

namespace SyncPrem.Pipeline.Abstractions.Stage.Connector.Source
{
	public abstract class SourceConnector<TStageSpecificConfiguration> : Stage<TStageSpecificConfiguration>, ISourceConnector
		where TStageSpecificConfiguration : StageSpecificConfiguration, new()
	{
		#region Constructors/Destructors

		protected SourceConnector()
		{
		}

		#endregion

		#region Methods/Operators

		public IChannel Produce(IContext context, RecordConfiguration recordConfiguration)
		{
			IChannel channel;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)recordConfiguration == null)
				throw new ArgumentNullException(nameof(recordConfiguration));

			channel = this.ProduceRecord(context, recordConfiguration);

			return channel;
		}

		protected abstract IChannel ProduceRecord(IContext context, RecordConfiguration recordConfiguration);

		#endregion
	}
}
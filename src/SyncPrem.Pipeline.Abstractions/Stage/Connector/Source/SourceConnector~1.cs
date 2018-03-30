/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

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

		public IChannel Produce(IContext context, RecordConfiguration configuration)
		{
			IChannel channel;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			channel = this.ProduceRecord(context, configuration);

			return channel;
		}

		protected abstract IChannel ProduceRecord(IContext context, RecordConfiguration configuration);

		#endregion
	}
}
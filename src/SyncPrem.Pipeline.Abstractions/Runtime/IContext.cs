/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions.Payload;
using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.Pipeline.Abstractions.Runtime
{
	public interface IContext : IComponent
	{
		#region Properties/Indexers/Events

		Stack<IPipelineMetadata> MetadataChain
		{
			get;
		}

		#endregion

		#region Methods/Operators

		IPipelineMessage CreateMessage(IEnumerable<IRecord> records);

		IPipelineMetadata CreateMetadata(IEnumerable<IField> fields);

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Data.Primitives;
using SyncPrem.Pipeline.Abstractions.Messages;

namespace SyncPrem.Pipeline.Abstractions
{
	public interface IPipelineContext : IPipelineComponent
	{
		#region Properties/Indexers/Events

		Stack<IPipelineMetadata> MetadataChain
		{
			get;
		}

		#endregion

		#region Methods/Operators

		IPipelineMessage CreateMessage(IEnumerable<IResult> results);

		IPipelineMetadata CreateMetadata(IEnumerable<IField> fields);

		#endregion
	}
}
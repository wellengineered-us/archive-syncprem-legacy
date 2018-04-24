/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Stage.Processor;

namespace SyncPrem.Pipeline.Abstractions.Sync.Stage.Processor
{
	public interface ISyncProcessorBuilder : IProcessorBuilder
	{
		#region Methods/Operators

		SyncProcessDelegate Build();

		ISyncProcessorBuilder New();

		ISyncProcessorBuilder Use(Func<SyncProcessDelegate, SyncProcessDelegate> middleware);

		#endregion
	}
}
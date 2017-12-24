/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace SyncPrem.Pipeline.Abstractions.Stage.Processor
{
	public interface IProcessorBuilder
	{
		#region Methods/Operators

		ProcessDelegate Build();

		IProcessorBuilder New();

		IProcessorBuilder Use(Func<ProcessDelegate, ProcessDelegate> middleware);

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Channel;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;

namespace SyncPrem.Pipeline.Abstractions.Stage.Processor
{
	public sealed class ProcessorClosure
	{
		#region Constructors/Destructors

		private ProcessorClosure(ProcessToNextDelegate processToNext, ProcessDelegate nextProcess)
		{
			if ((object)processToNext == null)
				throw new ArgumentNullException(nameof(processToNext));

			if ((object)nextProcess == null)
				throw new ArgumentNullException(nameof(nextProcess));

			this.processToNext = processToNext;
			this.nextProcess = nextProcess;
		}

		#endregion

		#region Fields/Constants

		private readonly ProcessDelegate nextProcess;
		private readonly ProcessToNextDelegate processToNext;

		#endregion

		#region Methods/Operators

		public static ProcessDelegate GetMiddlewareChain(ProcessToNextDelegate processToNext, ProcessDelegate nextProcess)
		{
			if ((object)processToNext == null)
				throw new ArgumentNullException(nameof(processToNext));

			if ((object)nextProcess == null)
				throw new ArgumentNullException(nameof(nextProcess));

			return new ProcessorClosure(processToNext, nextProcess).Transform;
		}

		private IChannel Transform(IContext context, RecordConfiguration configuration, IChannel channel)
		{
			Console.WriteLine("voo doo!");
			return this.processToNext(context, configuration, channel, this.nextProcess);
		}

		#endregion
	}
}
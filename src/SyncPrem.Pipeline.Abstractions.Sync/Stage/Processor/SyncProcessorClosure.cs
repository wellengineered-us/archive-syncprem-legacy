/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;

namespace SyncPrem.Pipeline.Abstractions.Sync.Stage.Processor
{
	public sealed class SyncProcessorClosure
	{
		#region Constructors/Destructors

		private SyncProcessorClosure(SyncProcessToNextDelegate processToNext, SyncProcessDelegate next)
		{
			if ((object)processToNext == null)
				throw new ArgumentNullException(nameof(processToNext));

			if ((object)next == null)
				throw new ArgumentNullException(nameof(next));

			this.processToNext = processToNext;
			this.next = next;
		}

		#endregion

		#region Fields/Constants

		private readonly SyncProcessDelegate next;
		private readonly SyncProcessToNextDelegate processToNext;

		#endregion

		#region Properties/Indexers/Events

		private SyncProcessDelegate SyncNext
		{
			get
			{
				return this.next;
			}
		}

		private SyncProcessToNextDelegate SyncProcessToNext
		{
			get
			{
				return this.processToNext;
			}
		}

		#endregion

		#region Methods/Operators

		public static SyncProcessDelegate GetMiddlewareChain(SyncProcessToNextDelegate processToNext, SyncProcessDelegate next)
		{
			if ((object)processToNext == null)
				throw new ArgumentNullException(nameof(processToNext));

			if ((object)next == null)
				throw new ArgumentNullException(nameof(next));

			return new SyncProcessorClosure(processToNext, next).Transform;
		}

		private ISyncChannel Transform(ISyncContext context, RecordConfiguration configuration, ISyncChannel channel)
		{
			Console.WriteLine("voo doo!");
			return this.SyncProcessToNext(context, configuration, channel, this.SyncNext);
		}

		#endregion
	}
}
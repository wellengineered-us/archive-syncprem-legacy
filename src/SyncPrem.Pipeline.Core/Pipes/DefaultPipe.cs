/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Pipes;

namespace SyncPrem.Pipeline.Core.Pipes
{
	public sealed class DefaultPipe : PipelineComponent, IPipe
	{
		#region Constructors/Destructors

		public DefaultPipe()
		{
			this.correlationId = Guid.NewGuid();
		}

		#endregion

		#region Fields/Constants

		private readonly Guid correlationId;

		#endregion

		#region Properties/Indexers/Events

		public Guid CorrelationId
		{
			get
			{
				return this.correlationId;
			}
		}

		#endregion
	}
}
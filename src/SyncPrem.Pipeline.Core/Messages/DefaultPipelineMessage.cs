﻿/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Data.Primitives;
using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Messages;

namespace SyncPrem.Pipeline.Core.Messages
{
	public sealed class DefaultPipelineMessage : PipelineComponent, IPipelineMessage
	{
		#region Constructors/Destructors

		public DefaultPipelineMessage(IEnumerable<IResult> results)
		{
			if ((object)results == null)
				throw new ArgumentNullException(nameof(results));

			this.results = results;
		}

		#endregion

		#region Fields/Constants

		private readonly IEnumerable<IResult> results;

		#endregion

		#region Properties/Indexers/Events

		public IEnumerable<IResult> Results
		{
			get
			{
				return this.results;
			}
		}

		#endregion
	}
}
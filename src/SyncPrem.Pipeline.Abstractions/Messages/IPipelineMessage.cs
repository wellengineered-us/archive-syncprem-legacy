﻿/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Data.Primitives;
using SyncPrem.Infrastructure.Wrappers;

namespace SyncPrem.Pipeline.Abstractions.Messages
{
	public interface IPipelineMessage : IPipelineComponent, IApplyWrap<IPipelineMessage, IResult>
	{
		#region Properties/Indexers/Events

		IEnumerable<IResult> Results
		{
			get;
		}

		#endregion
	}
}
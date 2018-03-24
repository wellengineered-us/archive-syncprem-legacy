﻿/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;

namespace SyncPrem.Pipeline.Abstractions
{
	public interface ISpecifiable<TSpecification>
		where TSpecification : StageSpecificConfiguration
	{
		#region Properties/Indexers/Events

		Type SpecificationType
		{
			get;
		}

		TSpecification Specification
		{
			get;
			set;
		}

		#endregion
	}
}
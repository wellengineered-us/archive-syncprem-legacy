/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace SyncPrem.Pipeline.Abstractions
{
	public interface IComponent
	{
		#region Properties/Indexers/Events

		Guid ComponentId
		{
			get;
		}

		bool IsReusable
		{
			get;
		}

		bool SupportsAsync
		{
			get;
		}

		#endregion
	}
}
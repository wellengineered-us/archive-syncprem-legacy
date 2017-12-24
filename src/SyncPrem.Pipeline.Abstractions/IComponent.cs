/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Abstractions
{
	public interface IComponent : ICreatable, IDisposable
	{
		#region Properties/Indexers/Events

		Guid ComponentId
		{
			get;
		}

		#endregion
	}
}
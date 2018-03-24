/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace TextMetal.Middleware.Solder.Primitives
{
	public interface IDisposableEx : IDisposable
	{
		#region Properties/Indexers/Events

		bool IsDisposed
		{
			get;
		}

		#endregion
	}
}
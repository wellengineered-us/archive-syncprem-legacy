/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;

namespace SyncPrem.StreamingIO.ProxyWrappers.Strategies
{
	public interface IStreamWrapperStrategy
	{
		#region Methods/Operators

		Stream ApplyStreamWrap(Stream stream);

		#endregion
	}
}
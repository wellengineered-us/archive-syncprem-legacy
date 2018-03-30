/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;

namespace SyncPrem.StreamingIO.ProxyWrappers.Strategies
{
	public sealed class NoneCompressionStrategy : ICompressionStrategy
	{
		#region Constructors/Destructors

		public NoneCompressionStrategy()
		{
		}

		#endregion

		#region Methods/Operators

		public Stream ApplyStreamWrap(Stream stream)
		{
			return stream;
		}

		#endregion
	}
}
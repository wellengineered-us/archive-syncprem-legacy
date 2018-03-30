/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;
using System.IO.Compression;

using SyncPrem.StreamingIO.ProxyWrappers.Internal;

namespace SyncPrem.StreamingIO.ProxyWrappers.Strategies
{
	public sealed class GzipCompressionStrategy : ICompressionStrategy
	{
		#region Constructors/Destructors

		public GzipCompressionStrategy()
		{
		}

		#endregion

		#region Methods/Operators

		public Stream ApplyStreamWrap(Stream stream)
		{
			stream = new ProgressWrappedStream(stream); // compressesed
			stream = new GZipStream(stream, CompressionLevel.Optimal);
			return stream;
		}

		#endregion
	}
}
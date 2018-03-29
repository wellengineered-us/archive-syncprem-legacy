/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SyncPrem.StreamingIO.ProxyWrappers.Internal
{
	public class ProgressWrappedStream : WrappedStream
	{
		#region Constructors/Destructors

		public ProgressWrappedStream(Stream innerStream)
			: base(innerStream)
		{
		}

		#endregion

		#region Fields/Constants

		private long total = 0;

		#endregion

		#region Methods/Operators

		public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			Console.WriteLine("COPY_ASYNCH");
			return base.CopyToAsync((destination), bufferSize, cancellationToken);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			int retval;

			retval = base.Read(buffer, offset, count);

			this.total += retval;
			Console.WriteLine("READ: offset={0}, count={1}; retval={2}; total={3}", offset, count, retval, this.total);

			return retval;
		}

		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			Task<int> retval = base.ReadAsync(buffer, offset, count, cancellationToken);

			Console.WriteLine("READ: offset={0}, count={1}; retval={2}; total={3}", offset, count, retval, this.total);

			return retval;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			int retval;

			base.Write(buffer, offset, count);
			retval = count;

			this.total += retval;
			Console.WriteLine("WRITE: offset={0}, count={1}; retval={2}; total={3}", offset, count, retval, this.total);
		}

		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			Task retval = base.WriteAsync(buffer, offset, count, cancellationToken);

			Console.WriteLine("WRITE: offset={0}, count={1}; retval={2}; total={3}", offset, count, retval, this.total);

			return retval;
		}

		#endregion

		/*public override void CopyTo(Stream destination, int bufferSize)
		{
			// disconnect between .NET Core and .NET Standard APIs
			Console.WriteLine("COPY");
			base.CopyTo(destination, bufferSize);
		}*/
	}
}
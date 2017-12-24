/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SyncPrem.StreamingIO.ProxyWrappers
{
	public class WrappedStream : Stream
	{
		#region Constructors/Destructors

		public WrappedStream(Stream innerStream)
		{
			if ((object)innerStream == null)
				throw new ArgumentNullException(nameof(innerStream));

			this.innerStream = innerStream;
		}

		#endregion

		#region Fields/Constants

		private readonly Stream innerStream;

		#endregion

		#region Properties/Indexers/Events

		public override bool CanRead
		{
			get
			{
				return this.InnerStream.CanRead;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return this.InnerStream.CanSeek;
			}
		}

		public override bool CanTimeout
		{
			get
			{
				return this.InnerStream.CanTimeout;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return this.InnerStream.CanWrite;
			}
		}

		private Stream InnerStream
		{
			get
			{
				return this.innerStream;
			}
		}

		public override long Length
		{
			get
			{
				return this.InnerStream.Length;
			}
		}

		public override long Position
		{
			get
			{
				return this.InnerStream.Position;
			}
			set
			{
				this.InnerStream.Position = value;
			}
		}

		public override int ReadTimeout
		{
			get
			{
				return this.InnerStream.ReadTimeout;
			}
			set
			{
				this.InnerStream.ReadTimeout = value;
			}
		}

		public override int WriteTimeout
		{
			get
			{
				return this.InnerStream.WriteTimeout;
			}
			set
			{
				this.InnerStream.WriteTimeout = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.InnerStream.BeginRead(buffer, offset, count, callback, state);
		}

		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.InnerStream.BeginWrite(buffer, offset, count, callback, state);
		}

		public override void Close()
		{
			this.InnerStream.Close();
			GC.SuppressFinalize(this);
		}

#if NETCOREAPP2_0
		public override void CopyTo(Stream destination, int bufferSize)
		{
			/* disconnect between .NET Core and .NET Standard APIs */
			this.InnerStream.CopyTo(destination, bufferSize);
		}
#endif

		public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			return this.InnerStream.CopyToAsync(destination, bufferSize, cancellationToken);
		}

		protected override void Dispose(bool disposing)
		{
			// may not need this in .NET Core v2.0
			base.Dispose(disposing);
		}

		public override int EndRead(IAsyncResult asyncResult)
		{
			return this.InnerStream.EndRead(asyncResult);
		}

		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.InnerStream.EndWrite(asyncResult);
		}

		public override void Flush()
		{
			this.InnerStream.Flush();
		}

		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			return this.InnerStream.FlushAsync(cancellationToken);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.InnerStream.Read(buffer, offset, count);
		}

		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return this.InnerStream.ReadAsync(buffer, offset, count, cancellationToken);
		}

		public override int ReadByte()
		{
			return this.InnerStream.ReadByte();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.InnerStream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			this.InnerStream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			this.InnerStream.Write(buffer, offset, count);
		}

		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return this.InnerStream.WriteAsync(buffer, offset, count, cancellationToken);
		}

		public override void WriteByte(byte value)
		{
			this.InnerStream.WriteByte(value);
		}

		#endregion
	}
}
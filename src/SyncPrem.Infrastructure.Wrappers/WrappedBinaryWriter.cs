/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;

namespace SyncPrem.Infrastructure.Wrappers
{
	public class WrappedBinaryWriter : BinaryWriter
	{
		#region Constructors/Destructors

		public WrappedBinaryWriter(BinaryWriter innerBinaryWriter)
			: base(Stream.Null)
		{
			if ((object)innerBinaryWriter == null)
				throw new ArgumentNullException(nameof(innerBinaryWriter));

			this.innerBinaryWriter = innerBinaryWriter;
		}

		#endregion

		#region Fields/Constants

		private readonly BinaryWriter innerBinaryWriter;

		#endregion

		#region Properties/Indexers/Events

		public override Stream BaseStream
		{
			get
			{
				return this.InnerBinaryWriter.BaseStream;
			}
		}

		protected BinaryWriter InnerBinaryWriter
		{
			get
			{
				return this.innerBinaryWriter;
			}
		}

		#endregion

		#region Methods/Operators

		public override void Close()
		{
			this.InnerBinaryWriter.Close();
			GC.SuppressFinalize(this);
		}

		protected override void Dispose(bool disposing)
		{
			// may not need this in .NET Core v2.0
			base.Dispose(disposing);
		}

		public override void Flush()
		{
			this.InnerBinaryWriter.Flush();
		}

		public override long Seek(int offset, SeekOrigin origin)
		{
			return this.InnerBinaryWriter.Seek(offset, origin);
		}

		public override void Write(bool value)
		{
			this.InnerBinaryWriter.Write(value);
		}

		public override void Write(byte value)
		{
			this.InnerBinaryWriter.Write(value);
		}

		public override void Write(byte[] buffer)
		{
			this.InnerBinaryWriter.Write(buffer);
		}

		public override void Write(byte[] buffer, int index, int count)
		{
			this.InnerBinaryWriter.Write(buffer, index, count);
		}

		public override void Write(char ch)
		{
			this.InnerBinaryWriter.Write(ch);
		}

		public override void Write(char[] chars)
		{
			this.InnerBinaryWriter.Write(chars);
		}

		public override void Write(char[] chars, int index, int count)
		{
			this.InnerBinaryWriter.Write(chars, index, count);
		}

		public override void Write(decimal value)
		{
			this.InnerBinaryWriter.Write(value);
		}

		public override void Write(double value)
		{
			this.InnerBinaryWriter.Write(value);
		}

		public override void Write(short value)
		{
			this.InnerBinaryWriter.Write(value);
		}

		public override void Write(int value)
		{
			this.InnerBinaryWriter.Write(value);
		}

		public override void Write(long value)
		{
			this.InnerBinaryWriter.Write(value);
		}

		public override void Write(sbyte value)
		{
			this.InnerBinaryWriter.Write(value);
		}

		public override void Write(float value)
		{
			this.InnerBinaryWriter.Write(value);
		}

		public override void Write(string value)
		{
			this.InnerBinaryWriter.Write(value);
		}

		public override void Write(ushort value)
		{
			this.InnerBinaryWriter.Write(value);
		}

		public override void Write(uint value)
		{
			this.InnerBinaryWriter.Write(value);
		}

		public override void Write(ulong value)
		{
			this.InnerBinaryWriter.Write(value);
		}

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;

namespace SyncPrem.StreamingIO.ProxyWrappers
{
	public class WrappedBinaryReader : BinaryReader
	{
		#region Constructors/Destructors

		public WrappedBinaryReader(BinaryReader innerBinaryReader)
			: base(Stream.Null)
		{
			if ((object)innerBinaryReader == null)
				throw new ArgumentNullException(nameof(innerBinaryReader));

			this.innerBinaryReader = innerBinaryReader;
		}

		#endregion

		#region Fields/Constants

		private readonly BinaryReader innerBinaryReader;

		#endregion

		#region Properties/Indexers/Events

		public override Stream BaseStream
		{
			get
			{
				return this.InnerBinaryReader.BaseStream;
			}
		}

		protected BinaryReader InnerBinaryReader
		{
			get
			{
				return this.innerBinaryReader;
			}
		}

		#endregion

		#region Methods/Operators

		public override void Close()
		{
			this.InnerBinaryReader.Close();
			GC.SuppressFinalize(this);
		}

		protected override void Dispose(bool disposing)
		{
			// may not need this in .NET Core v2.0
			base.Dispose(disposing);
		}

		public override int PeekChar()
		{
			return this.InnerBinaryReader.PeekChar();
		}

		public override int Read()
		{
			return this.InnerBinaryReader.Read();
		}

		public override int Read(byte[] buffer, int index, int count)
		{
			return this.InnerBinaryReader.Read(buffer, index, count);
		}

		public override int Read(char[] buffer, int index, int count)
		{
			return this.InnerBinaryReader.Read(buffer, index, count);
		}

		public override bool ReadBoolean()
		{
			return this.InnerBinaryReader.ReadBoolean();
		}

		public override byte ReadByte()
		{
			return this.InnerBinaryReader.ReadByte();
		}

		public override byte[] ReadBytes(int count)
		{
			return this.InnerBinaryReader.ReadBytes(count);
		}

		public override char ReadChar()
		{
			return this.InnerBinaryReader.ReadChar();
		}

		public override char[] ReadChars(int count)
		{
			return this.InnerBinaryReader.ReadChars(count);
		}

		public override decimal ReadDecimal()
		{
			return this.InnerBinaryReader.ReadDecimal();
		}

		public override double ReadDouble()
		{
			return this.InnerBinaryReader.ReadDouble();
		}

		public override short ReadInt16()
		{
			return this.InnerBinaryReader.ReadInt16();
		}

		public override int ReadInt32()
		{
			return this.InnerBinaryReader.ReadInt32();
		}

		public override long ReadInt64()
		{
			return this.InnerBinaryReader.ReadInt64();
		}

		public override sbyte ReadSByte()
		{
			return this.InnerBinaryReader.ReadSByte();
		}

		public override float ReadSingle()
		{
			return this.InnerBinaryReader.ReadSingle();
		}

		public override string ReadString()
		{
			return this.InnerBinaryReader.ReadString();
		}

		public override ushort ReadUInt16()
		{
			return this.InnerBinaryReader.ReadUInt16();
		}

		public override uint ReadUInt32()
		{
			return this.InnerBinaryReader.ReadUInt32();
		}

		public override ulong ReadUInt64()
		{
			return this.InnerBinaryReader.ReadUInt64();
		}

		#endregion
	}
}
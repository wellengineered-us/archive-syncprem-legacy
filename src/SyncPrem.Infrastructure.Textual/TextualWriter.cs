﻿/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;

using SyncPrem.Infrastructure.Data.Primitives;

namespace SyncPrem.Infrastructure.Textual
{
	public abstract class TextualWriter : MarshalByRefObject, IDisposable
	{
		#region Constructors/Destructors

		protected TextualWriter(TextWriter baseTextWriter)
		{
			if ((object)baseTextWriter == null)
				throw new ArgumentNullException(nameof(baseTextWriter));

			this.baseTextWriter = baseTextWriter;
		}

		#endregion

		#region Fields/Constants

		private readonly TextWriter baseTextWriter;

		#endregion

		#region Properties/Indexers/Events

		protected TextWriter BaseTextWriter
		{
			get
			{
				return this.baseTextWriter;
			}
		}

		#endregion

		#region Methods/Operators

		public virtual void Close()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Dispose()
		{
			this.Close();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
				this.BaseTextWriter.Dispose();
		}

		public void Flush()
		{
			this.BaseTextWriter.Flush();
		}

		public abstract void WriteHeaders(IEnumerable<IField> headers);

		public abstract void WriteRecords(IEnumerable<IRecord> records);

		public abstract void WriteResults(IEnumerable<IResult> results);

		#endregion
	}
}
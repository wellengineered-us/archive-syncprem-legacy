/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;

using SyncPrem.StreamingIO.Primitives;
using SyncPrem.StreamingIO.Textual.Delimited;

namespace SyncPrem.StreamingIO.Textual
{
	public abstract class TextualReader : MarshalByRefObject, IDisposable
	{
		#region Constructors/Destructors

		protected TextualReader(TextReader baseTextReader)
		{
			if ((object)baseTextReader == null)
				throw new ArgumentNullException(nameof(baseTextReader));

			this.baseTextReader = baseTextReader;
		}

		#endregion

		#region Fields/Constants

		private readonly TextReader baseTextReader;

		#endregion

		#region Properties/Indexers/Events

		protected TextReader BaseTextReader
		{
			get
			{
				return this.baseTextReader;
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
				this.BaseTextReader.Dispose();
		}

		public abstract IEnumerable<IPayload> ReadFooterRecords(IEnumerable<IDelimitedTextualFieldSpec> headers);

		public abstract IEnumerable<IDelimitedTextualFieldSpec> ReadHeaderFields();

		public abstract IEnumerable<IPayload> ReadRecords();

		#endregion
	}
}
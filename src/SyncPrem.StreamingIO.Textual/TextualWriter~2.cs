/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;

using SyncPrem.StreamingIO.Primitives;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.StreamingIO.Textual
{
	public abstract class TextualWriter<TTextualFieldSpec, TTextualSpec> : Lifecycle, ITextualWriter<TTextualFieldSpec, TTextualSpec>
		where TTextualFieldSpec : ITextualFieldSpec
		where TTextualSpec : ITextualSpec<TTextualFieldSpec>
	{
		#region Constructors/Destructors

		protected TextualWriter(TextWriter baseTextWriter, TTextualSpec textualSpec)
		{
			if ((object)baseTextWriter == null)
				throw new ArgumentNullException(nameof(baseTextWriter));

			if ((object)textualSpec == null)
				throw new ArgumentNullException(nameof(textualSpec));

			this.baseTextWriter = baseTextWriter;
			this.textualSpec = textualSpec;
		}

		#endregion

		#region Fields/Constants

		private readonly TextWriter baseTextWriter;

		private readonly TTextualSpec textualSpec;

		#endregion

		#region Properties/Indexers/Events

		public TextWriter BaseTextWriter
		{
			get
			{
				return this.baseTextWriter;
			}
		}

		public TTextualSpec TextualSpec
		{
			get
			{
				return this.textualSpec;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void Create(bool creating)
		{
			// do nothing
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				this.BaseTextWriter.Dispose();
		}

		public void Flush()
		{
			this.BaseTextWriter.Flush();
		}

		public abstract void WriteFooterRecords(IEnumerable<TTextualFieldSpec> footers, IEnumerable<ITextualStreamingRecord> records);

		public abstract void WriteHeaderFields(IEnumerable<TTextualFieldSpec> headers);

		public abstract void WriteRecords(IEnumerable<IPayload> records);

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.StreamingIO.Textual
{
	public abstract class TextualReader<TTextualFieldSpec, TTextualSpec> : Lifecycle, ITextualReader<TTextualFieldSpec, TTextualSpec>
		where TTextualFieldSpec : ITextualFieldSpec
		where TTextualSpec : ITextualSpec<TTextualFieldSpec>
	{
		#region Constructors/Destructors

		protected TextualReader(TextReader baseTextReader, TTextualSpec textualSpec)
		{
			if ((object)baseTextReader == null)
				throw new ArgumentNullException(nameof(baseTextReader));

			if ((object)textualSpec == null)
				throw new ArgumentNullException(nameof(textualSpec));

			this.baseTextReader = baseTextReader;
			this.textualSpec = textualSpec;
		}

		#endregion

		#region Fields/Constants

		private readonly TextReader baseTextReader;
		private readonly TTextualSpec textualSpec;

		#endregion

		#region Properties/Indexers/Events

		public TextReader BaseTextReader
		{
			get
			{
				return this.baseTextReader;
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
				this.BaseTextReader.Dispose();
		}

		public abstract IEnumerable<ITextualStreamingRecord> ReadFooterRecords(IEnumerable<TTextualFieldSpec> footers);

		public abstract IEnumerable<TTextualFieldSpec> ReadHeaderFields();

		public abstract IEnumerable<ITextualStreamingRecord> ReadRecords();

		#endregion
	}
}
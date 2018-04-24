/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.StreamingIO.Textual.Lined
{
	public class LinedTextualReader : TextualReader<ILinedTextualFieldSpec, ILinedTextualSpec>
	{
		#region Constructors/Destructors

		public LinedTextualReader(TextReader baseTextReader, ILinedTextualSpec linedTextualSpec)
			: base(baseTextReader, linedTextualSpec)
		{
			this.ResetParserState();
		}

		#endregion

		#region Fields/Constants

		private readonly _ParserState parserState = new _ParserState();

		#endregion

		#region Properties/Indexers/Events

		private _ParserState ParserState
		{
			get
			{
				return this.parserState;
			}
		}

		#endregion

		#region Methods/Operators

		public override IEnumerable<ITextualStreamingRecord> ReadFooterRecords(IEnumerable<ILinedTextualFieldSpec> footers)
		{
			throw new NotSupportedException();
		}

		public override IAsyncEnumerable<ITextualStreamingRecord> ReadFooterRecordsAsync(IEnumerable<ILinedTextualFieldSpec> footers, CancellationToken cancellationToken)
		{
			throw new NotSupportedException(string.Format("Cannot read footer records (from fields) in this version."));
		}

		public override IEnumerable<ILinedTextualFieldSpec> ReadHeaderFields()
		{
			return new ILinedTextualFieldSpec[] { };
		}

		public override IAsyncEnumerable<ILinedTextualFieldSpec> ReadHeaderFieldsAsync(CancellationToken cancellationToken)
		{
			return AsyncEnumerable.CreateEnumerable<ILinedTextualFieldSpec>(() => AsyncEnumerable.CreateEnumerator(MoveNextAsync, Current, Dispose));

			Task<bool> MoveNextAsync(CancellationToken cancellationToken_)
			{
				return Task.FromResult(false);
			}

			ILinedTextualFieldSpec Current()
			{
				return null;
			}

			void Dispose()
			{
				// do nothing
			}
		}

		public override IEnumerable<ITextualStreamingRecord> ReadRecords()
		{
			string line;
			ITextualStreamingRecord record;

			while (true)
			{
				line = this.BaseTextReader.ReadLine();

				if (SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(line))
					yield break;

				// TODO fix counts
				record = new TextualStreamingRecord(this.ParserState.lineIndex, this.ParserState.lineIndex, this.ParserState.characterIndex);
				record.Add(string.Empty, line);

				yield return record;
			}
		}

		public override IAsyncEnumerable<ITextualStreamingRecord> ReadRecordsAsync(CancellationToken cancellationToken)
		{
			return AsyncEnumerable.CreateEnumerable<ITextualStreamingRecord>(() => AsyncEnumerable.CreateEnumerator(MoveNextAsync, Current, Dispose));

			async Task<bool> MoveNextAsync(CancellationToken cancellationToken_)
			{
				bool hasNext;
				string line;
				ITextualStreamingRecord record;

				line = await this.BaseTextReader.ReadLineAsync();

				if (SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(line))
				{
					record = null;
					hasNext = false;
				}
				else
				{
					// TODO fix counts
					record = new TextualStreamingRecord(this.ParserState.lineIndex, this.ParserState.lineIndex, this.ParserState.characterIndex);
					record.Add(string.Empty, line);
					hasNext = true;
				}

				this.ParserState.record = record;
				return hasNext;
			}

			ITextualStreamingRecord Current()
			{
				return this.parserState.record;
			}

			void Dispose()
			{
				// do nothing
			}
		}

		private void ResetParserState()
		{
			const long DEFAULT_INDEX = 0;

			this.ParserState.record = new TextualStreamingRecord(0, 0, 0);
			this.ParserState.characterIndex = DEFAULT_INDEX;
			this.ParserState.lineIndex = DEFAULT_INDEX;

			this.TextualSpec.AssertValid();
		}

		#endregion

		#region Classes/Structs/Interfaces/Enums/Delegates

		private sealed class _ParserState
		{
			#region Fields/Constants

			public long characterIndex;
			public long lineIndex;
			public ITextualStreamingRecord record;

			#endregion
		}

		#endregion
	}
}
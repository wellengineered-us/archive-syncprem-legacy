/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.StreamingIO.Primitives;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.StreamingIO.Textual.Lined
{
	public class LinedTextualWriter : TextualWriter<ILinedTextualFieldSpec, ILinedTextualSpec>
	{
		#region Constructors/Destructors

		public LinedTextualWriter(TextWriter baseTextWriter, ILinedTextualSpec linedTextualSpec)
			: base(baseTextWriter, linedTextualSpec)
		{
		}

		#endregion

		#region Fields/Constants

		private bool footerRecordWritten;
		private bool headerRecordWritten;

		#endregion

		#region Properties/Indexers/Events

		private bool FooterRecordWritten
		{
			get
			{
				return this.footerRecordWritten;
			}
			set
			{
				this.footerRecordWritten = value;
			}
		}

		private bool HeaderRecordWritten
		{
			get
			{
				return this.headerRecordWritten;
			}
			set
			{
				this.headerRecordWritten = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override void WriteFooterRecords(IEnumerable<ILinedTextualFieldSpec> footers, IEnumerable<ITextualStreamingRecord> records)
		{
			// do nothing
		}

		public override Task WriteFooterRecordsAsync(IAsyncEnumerable<ILinedTextualFieldSpec> specs, IAsyncEnumerable<ITextualStreamingRecord> footers, CancellationToken cancellationToken)
		{
			// do nothing
			return Task.CompletedTask;
		}

		public override void WriteHeaderFields(IEnumerable<ILinedTextualFieldSpec> headers)
		{
			// do nothing
		}

		public override Task WriteHeaderFieldsAsync(IAsyncEnumerable<ILinedTextualFieldSpec> headers, CancellationToken cancellationToken)
		{
			// do nothing
			return Task.CompletedTask;
		}

		public override void WriteRecords(IEnumerable<IPayload> records)
		{
			long recordIndex = 0;
			foreach (IPayload record in records)
			{
				long fieldIndex = 0;
				if (record.TryGetValue(string.Empty, out object rawFieldValue))
				{
					string safeFieldValue;

					safeFieldValue = rawFieldValue.SafeToString(null, string.Empty);

					this.BaseTextWriter.Write(safeFieldValue);

					fieldIndex++;
				}

				if (!SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.TextualSpec.RecordDelimiter))
					this.BaseTextWriter.Write(this.TextualSpec.RecordDelimiter);

				recordIndex++;
			}
		}

		public override async Task WriteRecordsAsync(IAsyncEnumerable<IPayload> records, CancellationToken cancellationToken)
		{
			IAsyncEnumerator<IPayload> recordz;

			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			if (!this.HeaderRecordWritten)
				await this.WriteHeaderFieldsAsync(null, cancellationToken); // force fields if not explicitly called in advance
			
			recordz = records.GetEnumerator();

			long recordIndex = 0;
			while (await recordz.MoveNext(cancellationToken))
			{
				IPayload record = recordz.Current;
				
				long fieldIndex = 0;
				if (record.TryGetValue(string.Empty, out object rawFieldValue))
				{
					string safeFieldValue;

					safeFieldValue = rawFieldValue.SafeToString(null, string.Empty);

					await this.BaseTextWriter.WriteAsync(safeFieldValue /*, cancellationToken*/); // TODO: no CT overload in 2.0 - SMH

					fieldIndex++;
				}

				if (!SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.TextualSpec.RecordDelimiter))
					await this.BaseTextWriter.WriteAsync(this.TextualSpec.RecordDelimiter /*, cancellationToken*/); // TODO: no CT overload in 2.0 - SMH

				recordIndex++;
			}
		}

		#endregion
	}
}
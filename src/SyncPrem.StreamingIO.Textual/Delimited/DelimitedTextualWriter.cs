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

namespace SyncPrem.StreamingIO.Textual.Delimited
{
	public class DelimitedTextualWriter : TextualWriter<IDelimitedTextualFieldSpec, IDelimitedTextualSpec>
	{
		#region Constructors/Destructors

		public DelimitedTextualWriter(TextWriter baseTextWriter, IDelimitedTextualSpec delimitedTextualSpec)
			: base(baseTextWriter, delimitedTextualSpec)
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

		protected static string FormatFieldTitle(string fieldTitle)
		{
			string value;

			if ((object)fieldTitle == null)
				throw new ArgumentNullException(nameof(fieldTitle));

			value = fieldTitle; // TODO: escape bad chars

			return value;
		}

		protected string FormatFieldValue(long fieldIndex, string fieldTitle, object fieldValue)
		{
			IDelimitedTextualFieldSpec header = null;
			string value;
			string safeFieldValue;

			if ((object)fieldTitle == null)
				throw new ArgumentNullException(nameof(fieldTitle));

			// TODO: do not assume order is corrcetly aligned to index
			if (fieldIndex < this.TextualSpec.TextualHeaderSpecs.Count)
				header = this.TextualSpec.TextualHeaderSpecs[(int)fieldIndex];

			safeFieldValue = fieldValue?.ToString() ?? string.Empty;

			if ((object)header != null && !SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(header.FieldFormat))
				value = string.Format("{0:" + header.FieldFormat + "}", safeFieldValue);
			else
				value = safeFieldValue;

			return value;
		}

		private void WriteField(bool firstFieldInRecord, string fieldValue)
		{
			if (!firstFieldInRecord && !SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.TextualSpec.FieldDelimiter))
				this.BaseTextWriter.Write(this.TextualSpec.FieldDelimiter);

			if (!SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.TextualSpec.OpenQuoteValue))
				this.BaseTextWriter.Write(this.TextualSpec.OpenQuoteValue);

			this.BaseTextWriter.Write(fieldValue);

			if (!SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.TextualSpec.CloseQuoteValue))
				this.BaseTextWriter.Write(this.TextualSpec.CloseQuoteValue);
		}

		private async Task WriteFieldAsync(bool firstFieldInRecord, string fieldValue, CancellationToken cancellationToken)
		{
			if (!firstFieldInRecord && !SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.TextualSpec.FieldDelimiter))
				await this.BaseTextWriter.WriteAsync(this.TextualSpec.FieldDelimiter);

			if (!SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.TextualSpec.OpenQuoteValue))
				await this.BaseTextWriter.WriteAsync(this.TextualSpec.OpenQuoteValue);

			await this.BaseTextWriter.WriteAsync(fieldValue);

			if (!SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.TextualSpec.CloseQuoteValue))
				await this.BaseTextWriter.WriteAsync(this.TextualSpec.CloseQuoteValue);
		}

		public override void WriteFooterRecords(IEnumerable<IDelimitedTextualFieldSpec> footers, IEnumerable<ITextualStreamingRecord> records)
		{
			//bool firstFieldInRecord;

			throw new NotSupportedException(string.Format("Cannot write footer records (via fields) in this version."));

			// fields != null IF AND ONLY IF caller wishes to override DelimitedTextualSpec.DelimitedTextHeaderSpecs
			/*fields = fields ?? this.DelimitedTextualSpec.DelimitedTextHeaderSpecs;

			if ((object)fields != null &&
				(this.DelimitedTextualSpec.IsLastRecordFooter ?? false))
			{
				firstFieldInRecord = true;
				foreach (IField field in fields)
				{
					this.WriteField(firstFieldInRecord, field.FieldTitle);

					if (firstFieldInRecord)
						firstFieldInRecord = false;
				}

				if (!SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.DelimitedTextualSpec.RecordDelimiter))
					this.BaseTextWriter.Write(this.DelimitedTextualSpec.RecordDelimiter);

				this.HeaderRecordWritten = true;
			}*/
		}

		public override Task WriteFooterRecordsAsync(IAsyncEnumerable<IDelimitedTextualFieldSpec> specs, IAsyncEnumerable<ITextualStreamingRecord> footers, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public override void WriteHeaderFields(IEnumerable<IDelimitedTextualFieldSpec> headers)
		{
			if (this.HeaderRecordWritten)
				throw new InvalidOperationException(string.Format("Header record (fields) has (have) alredy been written."));

			// fields != null IF AND ONLY IF caller wishes to override DelimitedTextualSpec.DelimitedTextHeaderSpecs
			headers = headers ?? this.TextualSpec.TextualHeaderSpecs;

			if ((object)headers != null &&
				this.TextualSpec.IsFirstRecordHeader)
			{
				long fieldIndex = 0;
				foreach (IDelimitedTextualFieldSpec header in headers)
				{
					this.WriteField(fieldIndex == 0, FormatFieldTitle(header.FieldTitle));

					fieldIndex++;
				}

				if (!SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.TextualSpec.RecordDelimiter))
					this.BaseTextWriter.Write(this.TextualSpec.RecordDelimiter);

				this.HeaderRecordWritten = true;
			}
		}

		public override async Task WriteHeaderFieldsAsync(IAsyncEnumerable<IDelimitedTextualFieldSpec> headers, CancellationToken cancellationToken)
		{
			if (this.HeaderRecordWritten)
				throw new InvalidOperationException(string.Format("Header record (fields) has (have) alredy been written."));

			// fields != null IF AND ONLY IF caller wishes to override DelimitedTextualSpec.DelimitedTextHeaderSpecs
			//headers = headers ?? this.TextualSpec.TextualHeaderSpecs;

			if ((object)headers != null &&
				this.TextualSpec.IsFirstRecordHeader)
			{
				IAsyncEnumerator<IDelimitedTextualFieldSpec> headerz;

				headerz = headers.GetEnumerator();

				long fieldIndex = 0;
				while (await headerz.MoveNext(cancellationToken))
				{
					IDelimitedTextualFieldSpec header = headerz.Current;

					await this.WriteFieldAsync(fieldIndex == 0, FormatFieldTitle(header.FieldTitle), cancellationToken);

					fieldIndex++;
				}

				if (!SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.TextualSpec.RecordDelimiter))
					await this.BaseTextWriter.WriteAsync(this.TextualSpec.RecordDelimiter /*, cancellationToken*/); // TODO: no CT overload in 2.0 - SMH

				this.HeaderRecordWritten = true;
			}
		}

		public override void WriteRecords(IEnumerable<IPayload> records)
		{
			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			if (!this.HeaderRecordWritten)
				this.WriteHeaderFields(null); // force fields if not explicitly called in advance

			long recordIndex = 0;
			foreach (IPayload record in records)
			{
				long fieldIndex = 0;
				foreach (KeyValuePair<string, object> item in record)
				{
					this.WriteField(fieldIndex == 0, this.FormatFieldValue(fieldIndex, item.Key, item.Value));

					fieldIndex++;
				}

				if (!SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.TextualSpec.RecordDelimiter))
					this.BaseTextWriter.Write(this.TextualSpec.RecordDelimiter);

				recordIndex++;
			}
		}

		public override async Task WriteRecordsAsync(IAsyncEnumerable<IPayload> records, CancellationToken cancellationToken)
		{
			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			if (!this.HeaderRecordWritten)
				await this.WriteHeaderFieldsAsync(null, cancellationToken); // force fields if not explicitly called in advance

			long recordIndex = 0;
			IAsyncEnumerator<IPayload> recordz;

			recordz = records.GetEnumerator();

			while (await recordz.MoveNext(cancellationToken))
			{
				IPayload record = recordz.Current;

				long fieldIndex = 0;
				foreach (KeyValuePair<string, object> item in record)
				{
					await this.WriteFieldAsync(fieldIndex == 0, this.FormatFieldValue(fieldIndex, item.Key, item.Value), cancellationToken);

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
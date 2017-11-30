/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;

using SyncPrem.Infrastructure.Data.Primitives;

namespace SyncPrem.Infrastructure.Textual.Delimited
{
	public class DelimitedTextWriter : TextualWriter
	{
		#region Constructors/Destructors

		public DelimitedTextWriter(TextWriter baseTextWriter, IDelimitedTextSpec delimitedTextSpec)
			: base(baseTextWriter)
		{
			if ((object)delimitedTextSpec == null)
				throw new ArgumentNullException(nameof(delimitedTextSpec));

			this.delimitedTextSpec = delimitedTextSpec;
		}

		#endregion

		#region Fields/Constants

		private readonly IDelimitedTextSpec delimitedTextSpec;
		private bool footerRecordWritten;
		private bool headerRecordWritten;

		#endregion

		#region Properties/Indexers/Events

		public IDelimitedTextSpec DelimitedTextSpec
		{
			get
			{
				return this.delimitedTextSpec;
			}
		}

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

		private void WriteField(bool firstFieldInRecord, object fieldValue)
		{
			if (!firstFieldInRecord && !string.IsNullOrEmpty(this.DelimitedTextSpec.FieldDelimiter))
				this.BaseTextWriter.Write(this.DelimitedTextSpec.FieldDelimiter);

			if (!string.IsNullOrEmpty(this.DelimitedTextSpec.OpenQuoteValue))
				this.BaseTextWriter.Write(this.DelimitedTextSpec.OpenQuoteValue);

			this.BaseTextWriter.Write(fieldValue);

			if (!string.IsNullOrEmpty(this.DelimitedTextSpec.CloseQuoteValue))
				this.BaseTextWriter.Write(this.DelimitedTextSpec.CloseQuoteValue);
		}

		public override void WriteFooterRecords(IEnumerable<IField> fields, IEnumerable<IRecord> records)
		{
			//bool firstFieldInRecord;

			throw new NotSupportedException(string.Format("Cannot write footer records (via fields) in this version."));

			// fields != null IF AND ONLY IF caller wishes to override DelimitedTextSpec.DelimitedTextFieldSpecs
			/*fields = fields ?? this.DelimitedTextSpec.DelimitedTextFieldSpecs;

			if ((object)fields != null &&
				(this.DelimitedTextSpec.LastRecordIsFooter ?? false))
			{
				firstFieldInRecord = true;
				foreach (IField field in fields)
				{
					this.WriteField(firstFieldInRecord, field.FieldName);

					if (firstFieldInRecord)
						firstFieldInRecord = false;
				}

				if (!string.IsNullOrEmpty(this.DelimitedTextSpec.RecordDelimiter))
					this.BaseTextWriter.Write(this.DelimitedTextSpec.RecordDelimiter);

				this.HeaderRecordWritten = true;
			}*/
		}

		public override void WriteHeaderFields(IEnumerable<IField> fields)
		{
			bool firstFieldInRecord;

			if (this.HeaderRecordWritten)
				throw new InvalidOperationException(string.Format("Header record (fields) has (have) alredy been written."));

			// fields != null IF AND ONLY IF caller wishes to override DelimitedTextSpec.DelimitedTextFieldSpecs
			fields = fields ?? this.DelimitedTextSpec.DelimitedTextFieldSpecs;

			if ((object)fields != null &&
				(this.DelimitedTextSpec.FirstRecordIsHeader ?? false))
			{
				firstFieldInRecord = true;
				foreach (IDelimitedTextFieldSpec field in fields)
				{
					this.WriteField(firstFieldInRecord, field.FieldName);

					if (firstFieldInRecord)
						firstFieldInRecord = false;
				}

				if (!string.IsNullOrEmpty(this.DelimitedTextSpec.RecordDelimiter))
					this.BaseTextWriter.Write(this.DelimitedTextSpec.RecordDelimiter);

				this.HeaderRecordWritten = true;
			}
		}

		public override void WriteRecords(IEnumerable<IRecord> records)
		{
			bool firstFieldInRecord;

			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			if (!this.HeaderRecordWritten)
				this.WriteHeaderFields(null); // force fields if not explicitly called in advance

			foreach (IRecord record in records)
			{
				firstFieldInRecord = true;
				foreach (KeyValuePair<string, object> item in record)
				{
					this.WriteField(firstFieldInRecord, item.Value);

					if (firstFieldInRecord)
						firstFieldInRecord = false;
				}

				if (!string.IsNullOrEmpty(this.DelimitedTextSpec.RecordDelimiter))
					this.BaseTextWriter.Write(this.DelimitedTextSpec.RecordDelimiter);
			}
		}

		public override void WriteResults(IEnumerable<IResult> results)
		{
			if ((object)results == null)
				throw new ArgumentNullException(nameof(results));

			foreach (IResult result in results)
			{
				// club together all results' records into same output
				this.WriteRecords(result.Records);
			}
		}

		#endregion
	}
}
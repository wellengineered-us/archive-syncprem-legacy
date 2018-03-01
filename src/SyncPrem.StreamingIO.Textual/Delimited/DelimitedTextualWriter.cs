/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;

using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.StreamingIO.Textual.Delimited
{
	public class DelimitedTextualWriter : TextualWriter
	{
		#region Constructors/Destructors

		public DelimitedTextualWriter(TextWriter baseTextWriter, IDelimitedTextualSpec delimitedTextualSpec)
			: base(baseTextWriter)
		{
			if ((object)delimitedTextualSpec == null)
				throw new ArgumentNullException(nameof(delimitedTextualSpec));

			this.delimitedTextualSpec = delimitedTextualSpec;
		}

		#endregion

		#region Fields/Constants

		private readonly IDelimitedTextualSpec delimitedTextualSpec;
		private bool footerRecordWritten;
		private bool headerRecordWritten;

		#endregion

		#region Properties/Indexers/Events

		public IDelimitedTextualSpec DelimitedTextualSpec
		{
			get
			{
				return this.delimitedTextualSpec;
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
			if (!firstFieldInRecord && !string.IsNullOrEmpty(this.DelimitedTextualSpec.FieldDelimiter))
				this.BaseTextWriter.Write(this.DelimitedTextualSpec.FieldDelimiter);

			if (!string.IsNullOrEmpty(this.DelimitedTextualSpec.OpenQuoteValue))
				this.BaseTextWriter.Write(this.DelimitedTextualSpec.OpenQuoteValue);

			this.BaseTextWriter.Write(fieldValue);

			if (!string.IsNullOrEmpty(this.DelimitedTextualSpec.CloseQuoteValue))
				this.BaseTextWriter.Write(this.DelimitedTextualSpec.CloseQuoteValue);
		}

		public override void WriteFooterRecords(IEnumerable<IDelimitedTextualFieldSpec> specs, IEnumerable<IPayload> footers)
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

				if (!string.IsNullOrEmpty(this.DelimitedTextualSpec.RecordDelimiter))
					this.BaseTextWriter.Write(this.DelimitedTextualSpec.RecordDelimiter);

				this.HeaderRecordWritten = true;
			}*/
		}

		public override void WriteHeaderFields(IEnumerable<IDelimitedTextualFieldSpec> specs)
		{
			bool firstFieldInRecord;

			if (this.HeaderRecordWritten)
				throw new InvalidOperationException(string.Format("Header record (fields) has (have) alredy been written."));

			// fields != null IF AND ONLY IF caller wishes to override DelimitedTextualSpec.DelimitedTextHeaderSpecs
			specs = specs ?? this.DelimitedTextualSpec.TextualHeaderSpecs;

			if ((object)specs != null &&
				this.DelimitedTextualSpec.IsFirstRecordHeader)
			{
				firstFieldInRecord = true;
				foreach (IDelimitedTextualFieldSpec spec in specs)
				{
					this.WriteField(firstFieldInRecord, spec.FieldTitle);

					if (firstFieldInRecord)
						firstFieldInRecord = false;
				}

				if (!string.IsNullOrEmpty(this.DelimitedTextualSpec.RecordDelimiter))
					this.BaseTextWriter.Write(this.DelimitedTextualSpec.RecordDelimiter);

				this.HeaderRecordWritten = true;
			}
		}

		public override void WriteRecords(IEnumerable<IPayload> records)
		{
			bool firstFieldInRecord;

			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			if (!this.HeaderRecordWritten)
				this.WriteHeaderFields(null); // force fields if not explicitly called in advance

			foreach (IPayload record in records)
			{
				firstFieldInRecord = true;

				foreach (KeyValuePair<string, object> item in record)
				{
					this.WriteField(firstFieldInRecord, item.Value);

					if (firstFieldInRecord)
						firstFieldInRecord = false;
				}

				if (!string.IsNullOrEmpty(this.DelimitedTextualSpec.RecordDelimiter))
					this.BaseTextWriter.Write(this.DelimitedTextualSpec.RecordDelimiter);
			}
		}

		#endregion
	}
}
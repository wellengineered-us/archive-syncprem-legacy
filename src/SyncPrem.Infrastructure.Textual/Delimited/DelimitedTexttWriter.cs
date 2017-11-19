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
	public class DelimitedTexttWriter : TextualWriter
	{
		#region Constructors/Destructors

		public DelimitedTexttWriter(TextWriter baseTextWriter, IDelimitedTextSpec delimitedTextSpec)
			: base(baseTextWriter)
		{
			if ((object)delimitedTextSpec == null)
				throw new ArgumentNullException(nameof(delimitedTextSpec));

			this.delimitedTextSpec = delimitedTextSpec;
		}

		#endregion

		#region Fields/Constants

		private readonly IDelimitedTextSpec delimitedTextSpec;
		private bool headersWritten = false;

		#endregion

		#region Properties/Indexers/Events

		public IDelimitedTextSpec DelimitedTextSpec
		{
			get
			{
				return this.delimitedTextSpec;
			}
		}

		private bool HeadersWritten
		{
			get
			{
				return this.headersWritten;
			}
			set
			{
				this.headersWritten = value;
			}
		}

		#endregion

		#region Methods/Operators

		private void WriteField(bool first, object fieldValue)
		{
			if (!first && !string.IsNullOrEmpty(this.DelimitedTextSpec.FieldDelimiter))
				this.BaseTextWriter.Write(this.DelimitedTextSpec.FieldDelimiter);

			if (!string.IsNullOrEmpty(this.DelimitedTextSpec.QuoteValue))
				this.BaseTextWriter.Write(this.DelimitedTextSpec.QuoteValue);

			this.BaseTextWriter.Write(fieldValue);

			if (!string.IsNullOrEmpty(this.DelimitedTextSpec.QuoteValue))
				this.BaseTextWriter.Write(this.DelimitedTextSpec.QuoteValue);
		}

		public override void WriteHeaders(IEnumerable<IField> headers)
		{
			bool first;
			IEnumerable<IField> effectiveHeaders;

			if (this.HeadersWritten)
				throw new InvalidOperationException(string.Format("Headers have alredy been written."));

			// headers != null IF AND ONLY IF caller wishes to override DelimitedTextSpec.HeaderSpecs
			effectiveHeaders = headers ?? this.DelimitedTextSpec.HeaderSpecs;

			if ((object)effectiveHeaders != null &&
				(this.DelimitedTextSpec.FirstRecordIsHeader ?? false))
			{
				first = true;
				foreach (IField header in effectiveHeaders)
				{
					this.WriteField(first, header.FieldName);

					if (first)
						first = false;
				}

				if (!string.IsNullOrEmpty(this.DelimitedTextSpec.RecordDelimiter))
					this.BaseTextWriter.Write(this.DelimitedTextSpec.RecordDelimiter);

				this.HeadersWritten = true;
			}
		}

		public override void WriteRecords(IEnumerable<IRecord> records)
		{
			bool first;

			if ((object)records == null)
				throw new ArgumentNullException(nameof(records));

			if (!this.HeadersWritten)
				this.WriteHeaders(null); // force headers if not explicitly called in advance

			foreach (IDictionary<string, object> record in records)
			{
				first = true;
				foreach (KeyValuePair<string, object> item in record)
				{
					this.WriteField(first, item.Value);

					if (first)
						first = false;
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
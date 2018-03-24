/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.StreamingIO.Textual;
using SyncPrem.StreamingIO.Textual.Delimited;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Core.Configurations
{
	public class DelimitedTextualConfiguration : TextualConfiguration<DelimitedTextualFieldConfiguration, IDelimitedTextualFieldSpec, IDelimitedTextualSpec>
	{
		#region Constructors/Destructors

		public DelimitedTextualConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private string closeQuoteValue;
		private string fieldDelimiter;
		private string openQuoteValue;

		#endregion

		#region Properties/Indexers/Events

		public string CloseQuoteValue
		{
			get
			{
				return this.closeQuoteValue;
			}
			set
			{
				this.closeQuoteValue = value;
			}
		}

		public string FieldDelimiter
		{
			get
			{
				return this.fieldDelimiter;
			}
			set
			{
				this.fieldDelimiter = value;
			}
		}

		public string OpenQuoteValue
		{
			get
			{
				return this.openQuoteValue;
			}
			set
			{
				this.openQuoteValue = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override IDelimitedTextualSpec MapToSpec()
		{
			DelimitedTextualSpec delimitedTextualSpec;

			delimitedTextualSpec = new DelimitedTextualSpec()
									{
										CloseQuoteValue = this.CloseQuoteValue,
										FieldDelimiter = this.FieldDelimiter,
										IsFirstRecordHeader = this.FirstRecordIsHeader ?? false,
										IsLastRecordFooter = this.LastRecordIsFooter ?? false,
										OpenQuoteValue = this.OpenQuoteValue,
										RecordDelimiter = this.RecordDelimiter
									};

			foreach (DelimitedTextualFieldConfiguration delimitedTextHeaderFieldConfiguration in this.TextualHeaderFieldConfigurations)
			{
				DelimitedTextualFieldSpec delimitedTextualFieldSpec;

				delimitedTextualFieldSpec = new DelimitedTextualFieldSpec()
											{
												FieldTitle = delimitedTextHeaderFieldConfiguration.FieldTitle,
												FieldFormat = delimitedTextHeaderFieldConfiguration.FieldFormat,
												IsFieldIdentity = delimitedTextHeaderFieldConfiguration.IsFieldIdentity ?? false,
												FieldType = delimitedTextHeaderFieldConfiguration.FieldType ?? TextualFieldType.Text,
												IsFieldRequired = delimitedTextHeaderFieldConfiguration.IsFieldRequired ?? false,
												FieldOrdinal = delimitedTextHeaderFieldConfiguration.FieldOrdinal ?? delimitedTextualSpec.TextualHeaderSpecs.Count
											};

				delimitedTextualSpec.TextualHeaderSpecs.Add(delimitedTextualFieldSpec);
			}

			return delimitedTextualSpec;
		}

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;

			messages = new List<Message>();

			return messages;
		}

		#endregion
	}
}
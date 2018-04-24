/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.StreamingIO.Textual;
using SyncPrem.StreamingIO.Textual.Lined;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Core.Configurations
{
	public class LinedTextualConfiguration : TextualConfiguration<LinedTextualFieldConfiguration, ILinedTextualFieldSpec, ILinedTextualSpec>
	{
		#region Constructors/Destructors

		public LinedTextualConfiguration()
		{
		}

		#endregion

		#region Methods/Operators

		public override ILinedTextualSpec MapToSpec()
		{
			LinedTextualSpec linedTextualSpec;

			linedTextualSpec = new LinedTextualSpec()
								{
									IsFirstRecordHeader = this.FirstRecordIsHeader ?? false,
									IsLastRecordFooter = this.LastRecordIsFooter ?? false,
									RecordDelimiter = this.RecordDelimiter
								};

			foreach (LinedTextualFieldConfiguration linedTextHeaderFieldConfiguration in this.TextualHeaderFieldConfigurations)
			{
				LinedTextualFieldSpec linedTextualFieldSpec;

				linedTextualFieldSpec = new LinedTextualFieldSpec()
										{
											FieldTitle = linedTextHeaderFieldConfiguration.FieldTitle,
											FieldFormat = linedTextHeaderFieldConfiguration.FieldFormat,
											IsFieldIdentity = linedTextHeaderFieldConfiguration.IsFieldIdentity ?? false,
											FieldType = linedTextHeaderFieldConfiguration.FieldType ?? TextualFieldType.Text,
											IsFieldRequired = linedTextHeaderFieldConfiguration.IsFieldRequired ?? false,
											FieldOrdinal = linedTextHeaderFieldConfiguration.FieldOrdinal ?? linedTextualSpec.TextualHeaderSpecs.Count
										};

				linedTextualSpec.TextualHeaderSpecs.Add(linedTextualFieldSpec);
			}

			return linedTextualSpec;
		}

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;

			messages = new List<Message>();

			messages.AddRange(base.Validate(context));

			return messages;
		}

		#endregion
	}
}
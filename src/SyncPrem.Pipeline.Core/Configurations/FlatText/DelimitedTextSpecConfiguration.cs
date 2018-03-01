/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Configuration;
using SyncPrem.StreamingIO.Textual.Delimited;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Core.Configurations.FlatText
{
	public class DelimitedTextSpecConfiguration : ConfigurationObject
	{
		#region Constructors/Destructors

		public DelimitedTextSpecConfiguration()
		{
			this.delimitedTextHeaderFieldConfigurations = new ConfigurationObjectCollection<DelimitedTextFieldConfiguration>(this);
		}

		#endregion

		#region Fields/Constants

		private readonly ConfigurationObjectCollection<DelimitedTextFieldConfiguration> delimitedTextHeaderFieldConfigurations;
		private string closeQuoteValue;
		private string fieldDelimiter;
		private bool? firstRecordIsHeader;
		private bool? lastRecordIsFooter;
		private string openQuoteValue;
		private string recordDelimiter;

		#endregion

		#region Properties/Indexers/Events

		public ConfigurationObjectCollection<DelimitedTextFieldConfiguration> DelimitedTextHeaderFieldConfigurations
		{
			get
			{
				return this.delimitedTextHeaderFieldConfigurations;
			}
		}

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

		public bool? FirstRecordIsHeader
		{
			get
			{
				return this.firstRecordIsHeader;
			}
			set
			{
				this.firstRecordIsHeader = value;
			}
		}

		public bool? LastRecordIsFooter
		{
			get
			{
				return this.lastRecordIsFooter;
			}
			set
			{
				this.lastRecordIsFooter = value;
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

		[ConfigurationIgnore]
		public new DelimitedTextConnectorSpecificConfiguration Parent
		{
			get
			{
				return (DelimitedTextConnectorSpecificConfiguration)base.Parent;
			}
			set
			{
				base.Parent = value;
			}
		}

		public string RecordDelimiter
		{
			get
			{
				return this.recordDelimiter;
			}
			set
			{
				this.recordDelimiter = value;
			}
		}

		#endregion

		#region Methods/Operators

		public static DelimitedTextualSpec ToSpec(DelimitedTextSpecConfiguration delimitedTextSpecConfiguration)
		{
			IList<IDelimitedTextualFieldSpec> delimitedTextFieldSpecs;
			DelimitedTextualSpec delimitedTextualSpec;

			if ((object)delimitedTextSpecConfiguration == null)
				throw new ArgumentNullException(nameof(delimitedTextSpecConfiguration));

			delimitedTextualSpec = new DelimitedTextualSpec()
									{
										CloseQuoteValue = delimitedTextSpecConfiguration.CloseQuoteValue,
										FieldDelimiter = delimitedTextSpecConfiguration.FieldDelimiter,
										IsFirstRecordHeader = delimitedTextSpecConfiguration.FirstRecordIsHeader ?? false,
										IsLastRecordFooter = delimitedTextSpecConfiguration.LastRecordIsFooter ?? false,
										OpenQuoteValue = delimitedTextSpecConfiguration.OpenQuoteValue,
										RecordDelimiter = delimitedTextSpecConfiguration.RecordDelimiter
									};

			delimitedTextFieldSpecs = new List<IDelimitedTextualFieldSpec>();

			foreach (DelimitedTextFieldConfiguration delimitedTextHeaderFieldConfiguration in delimitedTextSpecConfiguration.DelimitedTextHeaderFieldConfigurations)
			{
				DelimitedTextualFieldSpec delimitedTextualFieldSpec;

				delimitedTextualFieldSpec = new DelimitedTextualFieldSpec()
											{
												FieldTitle = delimitedTextHeaderFieldConfiguration.FieldName,
												IsFieldIdentity = delimitedTextHeaderFieldConfiguration.IsKeyComponent ?? false,
												FieldType = delimitedTextHeaderFieldConfiguration.GetFieldType(),
												IsFieldRequired = delimitedTextHeaderFieldConfiguration.IsOptional ?? false,
												FieldOrdinal = delimitedTextFieldSpecs.Count
											};

				delimitedTextFieldSpecs.Add(delimitedTextualFieldSpec);
			}

			delimitedTextualSpec.DelimitedTextHeaderSpecs = delimitedTextFieldSpecs;

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
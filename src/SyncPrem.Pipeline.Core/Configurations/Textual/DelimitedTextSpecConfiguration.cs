/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using SyncPrem.Infrastructure.Configuration;
using SyncPrem.Infrastructure.Textual;
using SyncPrem.Infrastructure.Textual.Delimited;
using SyncPrem.Pipeline.Abstractions.Configurations;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Core.Configurations.Textual
{
	public class DelimitedTextSpecConfiguration : PipelineConfigurationObject, IDelimitedTextSpec
	{
		#region Constructors/Destructors

		public DelimitedTextSpecConfiguration()
		{
			this.delimitedTextFieldConfigurations = new PipelineConfigurationObjectCollection<DelimitedTextFieldConfiguration>(this);
		}

		#endregion

		#region Fields/Constants

		private readonly PipelineConfigurationObjectCollection<DelimitedTextFieldConfiguration> delimitedTextFieldConfigurations;
		private string closeQuoteValue;
		private string fieldDelimiter;
		private bool? firstRecordIsHeader;
		private bool? lastRecordIsFooter;
		private string openQuoteValue;
		private string recordDelimiter;

		#endregion

		#region Properties/Indexers/Events

		public PipelineConfigurationObjectCollection<DelimitedTextFieldConfiguration> DelimitedTextFieldConfigurations
		{
			get
			{
				return this.delimitedTextFieldConfigurations;
			}
		}

		IEnumerable<IFlatTextFieldSpec> IFlatTextSpec.FlatTextFieldSpecs
		{
			get
			{
				return this.DelimitedTextFieldConfigurations;
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

		IEnumerable<IDelimitedTextFieldSpec> IDelimitedTextSpec.DelimitedTextFieldSpecs
		{
			get
			{
				return this.DelimitedTextFieldConfigurations;
			}
			set
			{
				this.DelimitedTextFieldConfigurations.Clear();

				if ((object)value != null)
					this.DelimitedTextFieldConfigurations.AddRange(value.Select(f => new DelimitedTextFieldConfiguration()
																					{
																						FieldName = f.FieldName,
																						FieldTypeAqtn = f.FieldType.AssemblyQualifiedName,
																						IsFieldOptional = f.IsFieldOptional,
																						IsFieldKeyComponent = f.IsFieldKeyComponent,
																					}));
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
		public new DelimitedTextFilterSpecificConfiguration Parent
		{
			get
			{
				return (DelimitedTextFilterSpecificConfiguration)base.Parent;
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

		void IFlatTextSpec.AssertValid()
		{
			if (this.Validate().Any())
				throw new InvalidOperationException(string.Format("TODO"));
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
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Configuration;
using SyncPrem.StreamingIO.Textual;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Core.Configurations
{
	public abstract class TextualConfiguration<TTextualFieldConfiguration, TTextualFieldSpec, TTextualSpec> : ConfigurationObject
		where TTextualFieldConfiguration : TextualFieldConfiguration
		where TTextualFieldSpec : ITextualFieldSpec
		where TTextualSpec : ITextualSpec<TTextualFieldSpec>
	{
		#region Constructors/Destructors

		protected TextualConfiguration(ConfigurationObjectCollection<TTextualFieldConfiguration> textualHeaderFieldConfigurations, ConfigurationObjectCollection<TTextualFieldConfiguration> textualFooterFieldConfigurations)
		{
			if ((object)textualHeaderFieldConfigurations == null)
				throw new ArgumentNullException(nameof(textualHeaderFieldConfigurations));

			if ((object)textualFooterFieldConfigurations == null)
				throw new ArgumentNullException(nameof(textualFooterFieldConfigurations));

			this.textualHeaderFieldConfigurations = textualHeaderFieldConfigurations;
			this.textualFooterFieldConfigurations = textualFooterFieldConfigurations;
		}

		protected TextualConfiguration()
		{
			this.textualHeaderFieldConfigurations = new ConfigurationObjectCollection<TTextualFieldConfiguration>(this);
			this.textualFooterFieldConfigurations = new ConfigurationObjectCollection<TTextualFieldConfiguration>(this);
		}

		#endregion

		#region Fields/Constants

		private readonly ConfigurationObjectCollection<TTextualFieldConfiguration> textualFooterFieldConfigurations;
		private readonly ConfigurationObjectCollection<TTextualFieldConfiguration> textualHeaderFieldConfigurations;
		private bool? firstRecordIsHeader;
		private bool? lastRecordIsFooter;
		private string recordDelimiter;

		#endregion

		#region Properties/Indexers/Events

		public ConfigurationObjectCollection<TTextualFieldConfiguration> TextualFooterFieldConfigurations
		{
			get
			{
				return this.textualFooterFieldConfigurations;
			}
		}

		public ConfigurationObjectCollection<TTextualFieldConfiguration> TextualHeaderFieldConfigurations
		{
			get
			{
				return this.textualHeaderFieldConfigurations;
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

		public abstract TTextualSpec MapToSpec();

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;

			messages = new List<Message>();

			return messages;
		}

		#endregion
	}
}
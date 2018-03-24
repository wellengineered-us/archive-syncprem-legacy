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
	public abstract class TextualFieldConfiguration : ConfigurationObject
	{
		#region Constructors/Destructors

		protected TextualFieldConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private string fieldFormat;
		private long? fieldOrdinal;
		private string fieldTitle;
		private TextualFieldType? fieldType;
		private bool? isFieldIdentity;
		private bool? isFieldRequired;

		#endregion

		#region Properties/Indexers/Events

		public long? FieldOrdinal
		{
			get
			{
				return this.fieldOrdinal;
			}
			set
			{
				this.fieldOrdinal = value;
			}
		}

		public string FieldFormat
		{
			get
			{
				return this.fieldFormat;
			}
			set
			{
				this.fieldFormat = value;
			}
		}

		public string FieldTitle
		{
			get
			{
				return this.fieldTitle;
			}
			set
			{
				this.fieldTitle = value;
			}
		}

		public TextualFieldType? FieldType
		{
			get
			{
				return this.fieldType;
			}
			set
			{
				this.fieldType = value;
			}
		}

		public bool? IsFieldIdentity
		{
			get
			{
				return this.isFieldIdentity;
			}
			set
			{
				this.isFieldIdentity = value;
			}
		}

		public bool? IsFieldRequired
		{
			get
			{
				return this.isFieldRequired;
			}
			set
			{
				this.isFieldRequired = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;

			messages = new List<Message>();

			return messages;
		}

		#endregion
	}
}
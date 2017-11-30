/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Configuration;
using SyncPrem.Infrastructure.Data.Primitives;
using SyncPrem.Infrastructure.Textual.Delimited;
using SyncPrem.Pipeline.Abstractions.Configurations;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Core.Configurations.Textual
{
	public class DelimitedTextFieldConfiguration : PipelineConfigurationObject, IDelimitedTextFieldSpec
	{
		#region Constructors/Destructors

		public DelimitedTextFieldConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private string fieldName;
		private string fieldTypeAqtn;
		private bool? isFieldKeyComponent;
		private bool? isFieldOptional;

		#endregion

		#region Properties/Indexers/Events

		object IField.ContextData
		{
			get
			{
				return null;
			}
		}

		int IField.FieldIndex
		{
			get
			{
				return 0;
			}
		}

		Type IField.FieldType
		{
			get
			{
				return this.GetFieldType();
			}
		}

		bool IField.IsFieldKeyComponent
		{
			get
			{
				return this.IsFieldKeyComponent ?? false;
			}
		}

		bool IField.IsFieldOptional
		{
			get
			{
				return this.IsFieldOptional ?? true;
			}
		}

		public string FieldName
		{
			get
			{
				return this.fieldName;
			}
			set
			{
				this.fieldName = value;
			}
		}

		public string FieldTypeAqtn
		{
			get
			{
				return this.fieldTypeAqtn;
			}
			set
			{
				this.fieldTypeAqtn = value;
			}
		}

		public bool? IsFieldKeyComponent
		{
			get
			{
				return this.isFieldKeyComponent;
			}
			set
			{
				this.isFieldKeyComponent = value;
			}
		}

		public bool? IsFieldOptional
		{
			get
			{
				return this.isFieldOptional;
			}
			set
			{
				this.isFieldOptional = value;
			}
		}

		[ConfigurationIgnore]
		public new DelimitedTextSpecConfiguration Parent
		{
			get
			{
				return (DelimitedTextSpecConfiguration)base.Parent;
			}
			set
			{
				base.Parent = value;
			}
		}

		#endregion

		#region Methods/Operators

		public Type GetFieldType()
		{
			return GetTypeFromString(this.FieldTypeAqtn);
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
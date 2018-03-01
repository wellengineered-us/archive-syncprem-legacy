/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Configuration;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Core.Configurations.FlatText
{
	public class DelimitedTextFieldConfiguration : ConfigurationObject
	{
		#region Constructors/Destructors

		public DelimitedTextFieldConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private string fieldName;
		private string fieldTypeAqtn;
		private bool? isKeyComponent;
		private bool? isOptional;

		#endregion

		#region Properties/Indexers/Events

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

		public bool? IsKeyComponent
		{
			get
			{
				return this.isKeyComponent;
			}
			set
			{
				this.isKeyComponent = value;
			}
		}

		public bool? IsOptional
		{
			get
			{
				return this.isOptional;
			}
			set
			{
				this.isOptional = value;
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
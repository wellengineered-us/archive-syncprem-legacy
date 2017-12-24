/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Configuration;

using TextMetal.Middleware.Solder.Extensions;
using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Abstractions.Configuration
{
	public class FieldConfiguration : ConfigurationObject
	{
		#region Constructors/Destructors

		public FieldConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private string fieldName;

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

		[ConfigurationIgnore]
		public new RecordConfiguration Parent
		{
			get
			{
				return (RecordConfiguration)base.Parent;
			}
			set
			{
				base.Parent = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override bool Equals(object obj)
		{
			FieldConfiguration other;

			other = obj as FieldConfiguration;

			if ((object)other != null)
				return other.FieldName.SafeToString().ToLower() == this.FieldName.SafeToString().ToLower();

			return false;
		}

		public override int GetHashCode()
		{
			return this.FieldName.SafeToString().ToLower().GetHashCode();
		}

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;
			int? columnIndex;

			columnIndex = context as int?;
			messages = new List<Message>();

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(this.FieldName))
				messages.Add(NewError(string.Format("Field[{0}] name is required.", columnIndex)));

			return messages;
		}

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Configuration;

using TextMetal.Middleware.Solder.Extensions;
using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Abstractions.Configurations
{
	public class ColumnConfiguration : PipelineConfigurationObject
	{
		#region Constructors/Destructors

		public ColumnConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private string columnName;

		#endregion

		#region Properties/Indexers/Events

		public string ColumnName
		{
			get
			{
				return this.columnName;
			}
			set
			{
				this.columnName = value;
			}
		}

		[ConfigurationIgnore]
		public new TableConfiguration Parent
		{
			get
			{
				return (TableConfiguration)base.Parent;
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
			ColumnConfiguration other;

			other = obj as ColumnConfiguration;

			if ((object)other != null)
				return other.ColumnName.SafeToString().ToLower() == this.ColumnName.SafeToString().ToLower();

			return false;
		}

		public override int GetHashCode()
		{
			return this.ColumnName.SafeToString().ToLower().GetHashCode();
		}

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;
			int? columnIndex;

			columnIndex = context as int?;
			messages = new List<Message>();

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(this.ColumnName))
				messages.Add(NewError(string.Format("Column[{0}] name is required.", columnIndex)));

			return messages;
		}

		#endregion
	}
}
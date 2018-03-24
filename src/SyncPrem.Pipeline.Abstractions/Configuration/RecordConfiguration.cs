/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using SyncPrem.Infrastructure.Configuration;

using _Message = TextMetal.Middleware.Solder.Primitives.Message;

namespace SyncPrem.Pipeline.Abstractions.Configuration
{
	public class RecordConfiguration : ComponentConfiguration
	{
		#region Constructors/Destructors

		public RecordConfiguration()
		{
			this.columnConfigurations = new ConfigurationObjectCollection<FieldConfiguration>(this);
		}

		#endregion

		#region Fields/Constants

		private readonly ConfigurationObjectCollection<FieldConfiguration> columnConfigurations;

		#endregion

		#region Properties/Indexers/Events

		public ConfigurationObjectCollection<FieldConfiguration> ColumnConfigurations
		{
			get
			{
				return this.columnConfigurations;
			}
		}

		[ConfigurationIgnore]
		public new PipelineConfiguration Parent
		{
			get
			{
				return (PipelineConfiguration)base.Parent;
			}
			set
			{
				base.Parent = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override IEnumerable<_Message> Validate(object context)
		{
			List<_Message> messages;
			int index;

			messages = new List<_Message>();

			// check for duplicate columns
			var columnNameSums = this.ColumnConfigurations.GroupBy(c => c.FieldName)
				.Select(cl => new
							{
								ColumnName = cl.First().FieldName,
								Count = cl.Count()
							}).Where(cl => cl.Count > 1);

			if (columnNameSums.Any())
				messages.AddRange(columnNameSums.Select(c => NewError(string.Format("Table configuration with duplicate column configuration found: '{0}'.", c.ColumnName))).ToArray());

			index = 0;
			foreach (FieldConfiguration columnConfiguration in this.ColumnConfigurations)
				messages.AddRange(columnConfiguration.Validate(index++));

			return messages;
		}

		#endregion
	}
}
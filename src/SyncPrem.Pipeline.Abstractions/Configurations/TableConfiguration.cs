/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using SyncPrem.Infrastructure.Configuration;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Abstractions.Configurations
{
	public class TableConfiguration : PipelineConfigurationObject
	{
		#region Constructors/Destructors

		public TableConfiguration()
		{
			this.columnConfigurations = new ConfigurationObjectCollection<ColumnConfiguration>(this);
		}

		#endregion

		#region Fields/Constants

		private readonly ConfigurationObjectCollection<ColumnConfiguration> columnConfigurations;

		#endregion

		#region Properties/Indexers/Events

		public ConfigurationObjectCollection<ColumnConfiguration> ColumnConfigurations
		{
			get
			{
				return this.columnConfigurations;
			}
		}

		[ConfigurationIgnore]
		public new RealPipelineConfiguration Parent
		{
			get
			{
				return (RealPipelineConfiguration)base.Parent;
			}
			set
			{
				base.Parent = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;
			int index;

			messages = new List<Message>();

			// check for duplicate columns
			var columnNameSums = this.ColumnConfigurations.GroupBy(c => c.ColumnName)
				.Select(cl => new
							{
								ColumnName = cl.First().ColumnName,
								Count = cl.Count()
							}).Where(cl => cl.Count > 1);

			if (columnNameSums.Any())
				messages.AddRange(columnNameSums.Select(c => NewError(string.Format("Table configuration with duplicate column configuration found: '{0}'.", c.ColumnName))).ToArray());

			index = 0;
			foreach (ColumnConfiguration columnConfiguration in this.ColumnConfigurations)
				messages.AddRange(columnConfiguration.Validate(index++));

			return messages;
		}

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using SyncPrem.Infrastructure.Configuration;
using SyncPrem.Infrastructure.Data.AdoNet.UoW;
using SyncPrem.Pipeline.Abstractions.Configurations;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Core.Configurations.AdoNet
{
	public class AdoNetCommandConfiguration : PipelineConfigurationObject
	{
		#region Constructors/Destructors

		public AdoNetCommandConfiguration()
		{
			this.adoNetParameterConfigurations = new ConfigurationObjectCollection<AdoNetParameterConfiguration>(this);
		}

		#endregion

		#region Fields/Constants

		private readonly ConfigurationObjectCollection<AdoNetParameterConfiguration> adoNetParameterConfigurations;
		private CommandBehavior commandBehavior = CommandBehavior.Default;
		private bool commandPrepare;
		private string commandText;
		private int? commandTimeout;
		private CommandType? commandType;

		#endregion

		#region Properties/Indexers/Events

		public ConfigurationObjectCollection<AdoNetParameterConfiguration> AdoNetParameterConfigurations
		{
			get
			{
				return this.adoNetParameterConfigurations;
			}
		}

		public CommandBehavior CommandBehavior
		{
			get
			{
				return this.commandBehavior;
			}
			set
			{
				this.commandBehavior = value;
			}
		}

		public bool CommandPrepare
		{
			get
			{
				return this.commandPrepare;
			}
			set
			{
				this.commandPrepare = value;
			}
		}

		public string CommandText
		{
			get
			{
				return this.commandText;
			}
			set
			{
				this.commandText = value;
			}
		}

		public int? CommandTimeout
		{
			get
			{
				return this.commandTimeout;
			}
			set
			{
				this.commandTimeout = value;
			}
		}

		public CommandType? CommandType
		{
			get
			{
				return this.commandType;
			}
			set
			{
				this.commandType = value;
			}
		}

		[ConfigurationIgnore]
		public new AdoNetFilterSpecificConfiguration Parent
		{
			get
			{
				return (AdoNetFilterSpecificConfiguration)base.Parent;
			}
			set
			{
				base.Parent = value;
			}
		}

		#endregion

		#region Methods/Operators

		public IEnumerable<DbParameter> GetDbDataParameters(IUnitOfWork unitOfWork)
		{
			List<DbParameter> dbParameters;
			DbParameter dbDataParameter;

			if ((object)unitOfWork == null)
				throw new ArgumentNullException(nameof(unitOfWork));

			dbParameters = new List<DbParameter>();

			foreach (AdoNetParameterConfiguration adoNetParameterConfiguration in this.AdoNetParameterConfigurations)
			{
				dbDataParameter = unitOfWork.CreateParameter(adoNetParameterConfiguration.SourceColumn, adoNetParameterConfiguration.ParameterDirection, adoNetParameterConfiguration.ParameterDbType, adoNetParameterConfiguration.ParameterSize, adoNetParameterConfiguration.ParameterPrecision, adoNetParameterConfiguration.ParameterScale, adoNetParameterConfiguration.ParameterNullable, adoNetParameterConfiguration.ParameterName, adoNetParameterConfiguration.ParameterValue);
				dbParameters.Add(dbDataParameter);
			}

			return dbParameters.ToArray();
		}

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;
			int index;

			messages = new List<Message>();

			// check for duplicate columns
			var columnNameSums = this.AdoNetParameterConfigurations.GroupBy(c => c.ParameterName)
				.Select(cl => new
							{
								ParameterName = cl.First().ParameterName,
								Count = cl.Count()
							}).Where(cl => cl.Count > 1);

			if (columnNameSums.Any())
				messages.AddRange(columnNameSums.Select(c => NewError(string.Format("ADO.NET command configuration with duplicate ADO.NET parameter configuration found: '{0}'.", c.ParameterName))).ToArray());

			index = 0;
			foreach (AdoNetParameterConfiguration adoNetParameterConfiguration in this.AdoNetParameterConfigurations)
				messages.AddRange(adoNetParameterConfiguration.Validate(index++));

			return messages;
		}

		#endregion
	}
}
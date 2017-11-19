/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Data;

using SyncPrem.Infrastructure.Configuration;

using TextMetal.Middleware.Solder.Extensions;
using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Core.Configurations.Relational
{
	public class AdoNetParameterConfiguration : ConfigurationObject
	{
		#region Constructors/Destructors

		public AdoNetParameterConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private DbType parameterDbType;
		private ParameterDirection parameterDirection;
		private string parameterName;
		private bool parameterNullable;
		private byte parameterPrecision;
		private byte parameterScale;
		private int parameterSize;
		private object parameterValue;
		private string sourceColumn;

		#endregion

		#region Properties/Indexers/Events

		public DbType ParameterDbType
		{
			get
			{
				return this.parameterDbType;
			}
			set
			{
				this.parameterDbType = value;
			}
		}

		public ParameterDirection ParameterDirection
		{
			get
			{
				return this.parameterDirection;
			}
			set
			{
				this.parameterDirection = value;
			}
		}

		public string ParameterName
		{
			get
			{
				return this.parameterName;
			}
			set
			{
				this.parameterName = value;
			}
		}

		public bool ParameterNullable
		{
			get
			{
				return this.parameterNullable;
			}
			set
			{
				this.parameterNullable = value;
			}
		}

		public byte ParameterPrecision
		{
			get
			{
				return this.parameterPrecision;
			}
			set
			{
				this.parameterPrecision = value;
			}
		}

		public byte ParameterScale
		{
			get
			{
				return this.parameterScale;
			}
			set
			{
				this.parameterScale = value;
			}
		}

		public int ParameterSize
		{
			get
			{
				return this.parameterSize;
			}
			set
			{
				this.parameterSize = value;
			}
		}

		public object ParameterValue
		{
			get
			{
				return this.parameterValue;
			}
			set
			{
				this.parameterValue = value;
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

		public string SourceColumn
		{
			get
			{
				return this.sourceColumn;
			}
			set
			{
				this.sourceColumn = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;
			int? parameterIndex;

			parameterIndex = context as int?;
			messages = new List<Message>();

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(this.ParameterName))
				messages.Add(NewError(string.Format("Parameter[{0}] name is required.", parameterIndex)));

			return messages;
		}

		#endregion
	}
}
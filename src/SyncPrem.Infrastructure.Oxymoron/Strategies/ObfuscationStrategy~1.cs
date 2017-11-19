/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Data.Primitives;
using SyncPrem.Infrastructure.Oxymoron.Configuration;
using SyncPrem.Infrastructure.Oxymoron.Strategies.Configuration;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Infrastructure.Oxymoron.Strategies
{
	public abstract class ObfuscationStrategy<TObfuscationStrategyConfiguration> : IObfuscationStrategy
		where TObfuscationStrategyConfiguration : ObfuscationStrategyConfiguration, new()
	{
		#region Constructors/Destructors

		protected ObfuscationStrategy()
		{
		}

		#endregion

		#region Methods/Operators

		protected abstract object CoreGetObfuscatedValue(IObfuscationContext obfuscationContext, ColumnConfiguration<TObfuscationStrategyConfiguration> columnConfiguration, IField field, object columnValue);

		public object GetObfuscatedValue(IObfuscationContext obfuscationContext, ColumnConfiguration columnConfiguration, IField field, object originalFieldValue)
		{
			ColumnConfiguration<TObfuscationStrategyConfiguration> _columnConfiguration;
			object value;

			if ((object)obfuscationContext == null)
				throw new ArgumentNullException(nameof(obfuscationContext));

			if ((object)columnConfiguration == null)
				throw new ArgumentNullException(nameof(columnConfiguration));

			if ((object)field == null)
				throw new ArgumentNullException(nameof(field));

			if ((object)originalFieldValue == DBNull.Value)
				originalFieldValue = null;

			if ((object)columnConfiguration.ObfuscationStrategySpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(columnConfiguration.ObfuscationStrategySpecificConfiguration)));

			_columnConfiguration = new ColumnConfiguration<TObfuscationStrategyConfiguration>(columnConfiguration);

			if ((object)_columnConfiguration.ObfuscationStrategySpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(_columnConfiguration.ObfuscationStrategySpecificConfiguration)));

			value = this.CoreGetObfuscatedValue(obfuscationContext, _columnConfiguration, field, originalFieldValue);

			return value;
		}

		public Type GetObfuscationStrategySpecificConfigurationType()
		{
			return typeof(TObfuscationStrategyConfiguration);
		}

		public IEnumerable<Message> ValidateObfuscationStrategySpecificConfiguration(ColumnConfiguration columnConfiguration, int? colummIndex)
		{
			ColumnConfiguration<TObfuscationStrategyConfiguration> _columnConfiguration;

			if ((object)columnConfiguration == null)
				throw new ArgumentNullException(nameof(columnConfiguration));

			if ((object)columnConfiguration.ObfuscationStrategySpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(columnConfiguration.ObfuscationStrategySpecificConfiguration)));

			_columnConfiguration = new ColumnConfiguration<TObfuscationStrategyConfiguration>(columnConfiguration);

			if ((object)_columnConfiguration.ObfuscationStrategySpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(_columnConfiguration.ObfuscationStrategySpecificConfiguration)));

			return _columnConfiguration.ObfuscationStrategySpecificConfiguration.Validate(colummIndex);
		}

		#endregion
	}
}
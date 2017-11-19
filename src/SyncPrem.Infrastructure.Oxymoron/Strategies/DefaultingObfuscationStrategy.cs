/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Infrastructure.Data.Primitives;
using SyncPrem.Infrastructure.Oxymoron.Configuration;
using SyncPrem.Infrastructure.Oxymoron.Strategies.Configuration;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Infrastructure.Oxymoron.Strategies
{
	/// <summary>
	/// Returns an alternate value that is always null if NULL or the default value if NOT NULL.
	/// DATA TYPE: any
	/// </summary>
	public sealed class DefaultingObfuscationStrategy : ObfuscationStrategy<DefaultingObfuscationStrategyConfiguration>
	{
		#region Constructors/Destructors

		public DefaultingObfuscationStrategy()
		{
		}

		#endregion

		#region Methods/Operators

		private static object GetDefault(bool isNullable, Type valueType)
		{
			if ((object)valueType == null)
				throw new ArgumentNullException(nameof(valueType));

			if (valueType == typeof(String))
				return isNullable ? null : string.Empty;

			if (isNullable)
				valueType = SolderFascadeAccessor.ReflectionFascade.MakeNullableType(valueType);

			return SolderFascadeAccessor.DataTypeFascade.DefaultValue(valueType);
		}

		protected override object CoreGetObfuscatedValue(IObfuscationContext obfuscationContext, ColumnConfiguration<DefaultingObfuscationStrategyConfiguration> columnConfiguration, IField field, object columnValue)
		{
			object value;

			if ((object)obfuscationContext == null)
				throw new ArgumentNullException(nameof(obfuscationContext));

			if ((object)columnConfiguration == null)
				throw new ArgumentNullException(nameof(columnConfiguration));

			if ((object)field == null)
				throw new ArgumentNullException(nameof(field));

			if ((object)columnConfiguration.ObfuscationStrategySpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(columnConfiguration.ObfuscationStrategySpecificConfiguration)));

			value = GetDefault(field.IsFieldOptional && (columnConfiguration.ObfuscationStrategySpecificConfiguration.DefaultingCanBeNull ?? false), field.FieldType);

			return value;
		}

		#endregion
	}
}
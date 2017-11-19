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
	/// Returns an alternate value using a hashed lookup into a dictionary.
	/// DATA TYPE: string
	/// </summary>
	public sealed class SubstitutionObfuscationStrategy : ObfuscationStrategy<SubstitutionObfuscationStrategyConfiguration>
	{
		#region Constructors/Destructors

		public SubstitutionObfuscationStrategy()
		{
		}

		#endregion

		#region Methods/Operators

		protected override object CoreGetObfuscatedValue(IObfuscationContext obfuscationContext, ColumnConfiguration<SubstitutionObfuscationStrategyConfiguration> columnConfiguration, IField field, object columnValue)
		{
			Type valueType;
			string _columnValue;

			long surrogateKey;
			object surrogateValue;

			DictionaryConfiguration dictionaryConfiguration;

			if ((object)obfuscationContext == null)
				throw new ArgumentNullException(nameof(obfuscationContext));

			if ((object)columnConfiguration == null)
				throw new ArgumentNullException(nameof(columnConfiguration));

			if ((object)field == null)
				throw new ArgumentNullException(nameof(field));

			if ((object)columnConfiguration.ObfuscationStrategySpecificConfiguration == null)
				throw new InvalidOperationException(string.Format("Configuration missing: '{0}'.", nameof(columnConfiguration.ObfuscationStrategySpecificConfiguration)));

			if ((object)columnValue == null)
				return null;

			valueType = columnValue.GetType();

			if (valueType != typeof(string))
				return null;

			_columnValue = (string)columnValue;
			_columnValue = _columnValue.Trim();

			if (SolderFascadeAccessor.DataTypeFascade.IsWhiteSpace(_columnValue))
				return _columnValue;

			dictionaryConfiguration = columnConfiguration.ObfuscationStrategySpecificConfiguration.DictionaryConfiguration;

			if ((object)dictionaryConfiguration == null)
				throw new InvalidOperationException(string.Format("Failed to obtain dictionary."));

			if ((dictionaryConfiguration.RecordCount ?? 0L) <= 0L)
				return null;

			surrogateKey = obfuscationContext.GetValueHash(dictionaryConfiguration.RecordCount, columnValue);

			if (!obfuscationContext.TryGetSurrogateValue(dictionaryConfiguration, surrogateKey, out surrogateValue))
				throw new InvalidOperationException(string.Format("Failed to obtain surrogate value."));

			return surrogateValue;
		}

		#endregion
	}
}
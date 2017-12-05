/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Infrastructure.Data.Primitives;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.Infrastructure.Oxymoron.Strategies
{
	/// <summary>
	/// Returns an alternate value using a hashed lookup into a dictionary.
	/// DATA TYPE: string
	/// </summary>
	public sealed class SubstitutionObfuscationStrategy : ObfuscationStrategy<SubstitutionObfuscationStrategy.Spec>
	{
		#region Constructors/Destructors

		public SubstitutionObfuscationStrategy()
		{
		}

		#endregion

		#region Methods/Operators

		protected override object CoreGetObfuscatedValue(IObfuscationContext obfuscationContext, IColumnSpec<Spec> columnSpec, IField field, object columnValue)
		{
			Type valueType;
			string _columnValue;

			long surrogateKey;
			object surrogateValue;

			IDictionarySpec dictionarySpec;

			if ((object)obfuscationContext == null)
				throw new ArgumentNullException(nameof(obfuscationContext));

			if ((object)columnSpec == null)
				throw new ArgumentNullException(nameof(columnSpec));

			if ((object)field == null)
				throw new ArgumentNullException(nameof(field));

			if ((object)columnSpec.ObfuscationStrategySpec == null)
				throw new InvalidOperationException(string.Format("Specification missing: '{0}'.", nameof(columnSpec.ObfuscationStrategySpec)));

			if ((object)columnValue == null)
				return null;

			valueType = columnValue.GetType();

			if (valueType != typeof(string))
				return null;

			_columnValue = (string)columnValue;
			_columnValue = _columnValue.Trim();

			if (SolderFascadeAccessor.DataTypeFascade.IsWhiteSpace(_columnValue))
				return _columnValue;

			dictionarySpec = columnSpec.ObfuscationStrategySpec.DictionarySpec;

			if ((object)dictionarySpec == null)
				throw new InvalidOperationException(string.Format("Failed to obtain dictionary."));

			if ((dictionarySpec.RecordCount ?? 0L) <= 0L)
				return null;

			surrogateKey = obfuscationContext.GetValueHash(dictionarySpec.RecordCount, columnValue);

			if (!obfuscationContext.TryGetSurrogateValue(dictionarySpec, surrogateKey, out surrogateValue))
				throw new InvalidOperationException(string.Format("Failed to obtain surrogate value."));

			return surrogateValue;
		}

		#endregion

		#region Classes/Structs/Interfaces/Enums/Delegates

		public sealed class Spec : IObfuscationStrategySpec
		{
			#region Constructors/Destructors

			public Spec()
			{
			}

			#endregion

			#region Fields/Constants

			private IDictionarySpec dictionarySpec;

			#endregion

			#region Properties/Indexers/Events

			public IDictionarySpec DictionarySpec
			{
				get
				{
					return this.dictionarySpec;
				}
				set
				{
					this.dictionarySpec = value;
				}
			}

			#endregion
		}

		#endregion
	}
}
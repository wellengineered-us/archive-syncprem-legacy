/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.StreamingIO.DataMasking.Strategies
{
	public abstract class ObfuscationStrategy<TObfuscationStrategySpec> : IObfuscationStrategy
		where TObfuscationStrategySpec : class, IObfuscationStrategySpec
	{
		#region Constructors/Destructors

		protected ObfuscationStrategy()
		{
		}

		#endregion

		#region Methods/Operators

		protected abstract object CoreGetObfuscatedValue(IObfuscationContext obfuscationContext, IColumnSpec<TObfuscationStrategySpec> columnSpec, IField field, object columnValue);

		public object GetObfuscatedValue(IObfuscationContext obfuscationContext, IColumnSpec columnSpec, IField field, object originalFieldValue)
		{
			IColumnSpec<TObfuscationStrategySpec> _columnSpec;
			object value;

			if ((object)obfuscationContext == null)
				throw new ArgumentNullException(nameof(obfuscationContext));

			if ((object)columnSpec == null)
				throw new ArgumentNullException(nameof(columnSpec));

			if ((object)field == null)
				throw new ArgumentNullException(nameof(field));

			if ((object)originalFieldValue == DBNull.Value)
				originalFieldValue = null;

			if ((object)columnSpec.ObfuscationStrategySpec == null)
				throw new InvalidOperationException(string.Format("Specification missing: '{0}'.", nameof(columnSpec.ObfuscationStrategySpec)));

			_columnSpec = new ColumnSpec<TObfuscationStrategySpec>(columnSpec);

			if ((object)_columnSpec.ObfuscationStrategySpec == null)
				throw new InvalidOperationException(string.Format("Specification missing: '{0}'.", nameof(_columnSpec.ObfuscationStrategySpec)));

			value = this.CoreGetObfuscatedValue(obfuscationContext, _columnSpec, field, originalFieldValue);

			return value;
		}

		public Type GetObfuscationStrategySpecificSpecType()
		{
			return typeof(TObfuscationStrategySpec);
		}

		#endregion
	}
}
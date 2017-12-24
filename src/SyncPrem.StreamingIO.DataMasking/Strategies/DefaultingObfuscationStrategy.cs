/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.StreamingIO.Primitives;

using TextMetal.Middleware.Solder.Extensions;

namespace SyncPrem.StreamingIO.DataMasking.Strategies
{
	/// <summary>
	/// Returns an alternate value that is always null if NULL or the default value if NOT NULL.
	/// DATA TYPE: any
	/// </summary>
	public sealed class DefaultingObfuscationStrategy : ObfuscationStrategy<DefaultingObfuscationStrategy.Spec>
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

		protected override object CoreGetObfuscatedValue(IObfuscationContext obfuscationContext, IColumnSpec<Spec> columnSpec, IField field, object columnValue)
		{
			object value;

			if ((object)obfuscationContext == null)
				throw new ArgumentNullException(nameof(obfuscationContext));

			if ((object)columnSpec == null)
				throw new ArgumentNullException(nameof(columnSpec));

			if ((object)field == null)
				throw new ArgumentNullException(nameof(field));

			if ((object)columnSpec.ObfuscationStrategySpec == null)
				throw new InvalidOperationException(string.Format("Specification missing: '{0}'.", nameof(columnSpec.ObfuscationStrategySpec)));

			value = GetDefault(field.IsFieldOptional && (columnSpec.ObfuscationStrategySpec.DefaultingCanBeNull ?? false), field.FieldType);

			return value;
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

			private bool? defaultingCanBeNull;

			#endregion

			#region Properties/Indexers/Events

			public bool? DefaultingCanBeNull
			{
				get
				{
					return this.defaultingCanBeNull;
				}
				set
				{
					this.defaultingCanBeNull = value;
				}
			}

			#endregion
		}

		#endregion
	}
}
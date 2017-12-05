/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Infrastructure.Data.Primitives;

namespace SyncPrem.Infrastructure.Oxymoron.Strategies
{
	/// <summary>
	/// Returns an alternate value within +/- (x%) of the original value.
	/// DATA TYPE: numeric
	/// Returns an alternate value within +/- (x%:365.25d) of the original value.
	/// DATA TYPE: temporal
	/// </summary>
	public sealed class VarianceObfuscationStrategy : ObfuscationStrategy<VarianceObfuscationStrategy.Spec>
	{
		#region Constructors/Destructors

		public VarianceObfuscationStrategy()
		{
		}

		#endregion

		#region Methods/Operators

		private static object GetVariance(double varianceFactor, object value)
		{
			Type valueType;
			Type openNullableType;
			object originalValue;

			if ((object)value == null)
				return null;

			originalValue = value;
			valueType = value.GetType();
			openNullableType = typeof(Nullable<>);

			if (valueType.IsGenericType &&
				!valueType.IsGenericTypeDefinition &&
				valueType.GetGenericTypeDefinition().Equals(openNullableType))
				valueType = valueType.GetGenericArguments()[0];

			if (valueType == typeof(Boolean))
				value = (Math.Sign(varianceFactor) >= 0) ? true : false;
			else if (valueType == typeof(SByte))
				value = (SByte)value + (SByte)(varianceFactor * (Double)(SByte)value);
			else if (valueType == typeof(Int16))
				value = (Int16)value + (Int16)(varianceFactor * (Double)(Int16)value);
			else if (valueType == typeof(Int32))
				value = (Int32)value + (Int32)(varianceFactor * (Double)(Int32)value);
			else if (valueType == typeof(Int64))
				value = (Int64)value + (Int64)(varianceFactor * (Double)(Int64)value);
			else if (valueType == typeof(Byte))
				value = (Byte)value + (Byte)(varianceFactor * (Double)(Byte)value);
			else if (valueType == typeof(UInt16))
				value = (UInt16)value + (UInt16)(varianceFactor * (Double)(UInt16)value);
			else if (valueType == typeof(Int32))
				value = (UInt32)value + (UInt32)(varianceFactor * (Double)(UInt32)value);
			else if (valueType == typeof(UInt64))
				value = (UInt64)value + (UInt64)(varianceFactor * (Double)(UInt64)value);
			else if (valueType == typeof(Decimal))
				value = (Decimal)value + ((Decimal)varianceFactor * (Decimal)value);
			else if (valueType == typeof(Single))
				value = (Single)value + (Single)(varianceFactor * (Double)(Single)value);
			else if (valueType == typeof(Double))
				value = (Double)value + (Double)(varianceFactor * (Double)value);
			else if (valueType == typeof(Char))
				value = (Char)value + (Char)(varianceFactor * (Char)value);
			else if (valueType == typeof(DateTime))
				value = ((DateTime)value).AddDays((Double)(varianceFactor * 365.25));
			else if (valueType == typeof(DateTimeOffset))
				value = ((DateTimeOffset)value).AddDays((Double)(varianceFactor * 365.25));
			else if (valueType == typeof(TimeSpan))
				value = ((TimeSpan)value).Add(TimeSpan.FromDays((Double)(varianceFactor * 365.25)));
			else // unsupported type
				value = "#VALUE";

			// roll a recursive doubler call until a new value is achieved
			//if (DataType.ObjectsEqualValueSemantics(originalValue, value))
			//return GetVariance(varianceFactor * 2.0, value);

			return value;
		}

		protected override object CoreGetObfuscatedValue(IObfuscationContext obfuscationContext, IColumnSpec<Spec> columnSpec, IField field, object columnValue)
		{
			long signHash, valueHash;
			object value;
			double varianceFactor;

			if ((object)obfuscationContext == null)
				throw new ArgumentNullException(nameof(obfuscationContext));

			if ((object)columnSpec == null)
				throw new ArgumentNullException(nameof(columnSpec));

			if ((object)field == null)
				throw new ArgumentNullException(nameof(field));

			if ((object)columnSpec.ObfuscationStrategySpec == null)
				throw new InvalidOperationException(string.Format("Specification missing: '{0}'.", nameof(columnSpec.ObfuscationStrategySpec)));

			signHash = obfuscationContext.GetSignHash(columnValue);
			valueHash = obfuscationContext.GetValueHash(columnSpec.ObfuscationStrategySpec.VariancePercentValue, columnValue);
			varianceFactor = ((((valueHash <= 0 ? 1 : valueHash)) * ((signHash % 2 == 0 ? 1.0 : -1.0))) / 100.0);

			value = GetVariance(varianceFactor, columnValue);

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

			private long? variancePercentValue;

			#endregion

			#region Properties/Indexers/Events

			public long? VariancePercentValue
			{
				get
				{
					return this.variancePercentValue;
				}
				set
				{
					this.variancePercentValue = value;
				}
			}

			#endregion
		}

		#endregion
	}
}
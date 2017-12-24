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
	/// Returns un-obfuscated, original value.
	/// DATA TYPE: any
	/// </summary>
	public sealed class SurrogateKeyObfuscationStrategy : ObfuscationStrategy<SurrogateKeyObfuscationStrategy.Spec>
	{
		#region Constructors/Destructors

		public SurrogateKeyObfuscationStrategy()
		{
		}

		#endregion

		#region Methods/Operators

		private static object GetSurrogateKey(long randomSeed, object value)
		{
			Random random;
			Op op;
			int val;

			Type valueType;
			Int64 _value;

			if ((object)value == null)
				return null;

			valueType = value.GetType();

			if (!typeof(Int64).IsAssignableFrom(valueType))
				return null;

			_value = value.ChangeType<Int64>();

			random = new Random((int)randomSeed);
			int max = random.Next(0, 99);

			// TODO - use lighweight dynmamic method here?
			for (int i = 0; i < max; i++)
			{
				op = (Op)random.Next(1, 4);

				val = random.Next(); // unbounded

				switch (op)
				{
					case Op.Add:
						_value += val;
						break;
					case Op.Sub:
						_value -= val;
						break;
					case Op.Mul:
						_value *= val;
						break;
					case Op.Div:
						if (val != 0)
							_value /= val;
						break;
					case Op.Mod:
						_value %= val;
						break;
					default:
						break;
				}
			}

			value = _value.ChangeType(valueType);
			return value;
		}

		protected override object CoreGetObfuscatedValue(IObfuscationContext obfuscationContext, IColumnSpec<Spec> columnSpec, IField field, object columnValue)
		{
			long valueHash;
			object value;
			long randomSeed;

			if ((object)obfuscationContext == null)
				throw new ArgumentNullException(nameof(obfuscationContext));

			if ((object)columnSpec == null)
				throw new ArgumentNullException(nameof(columnSpec));

			if ((object)field == null)
				throw new ArgumentNullException(nameof(field));

			if ((object)columnSpec.ObfuscationStrategySpec == null)
				throw new InvalidOperationException(string.Format("Specification missing: '{0}'.", nameof(columnSpec.ObfuscationStrategySpec)));

			valueHash = obfuscationContext.GetValueHash(null, field.FieldName);
			randomSeed = valueHash;

			// create a new repeatable yet random-ish math funcion using the random seed, then executes for column value 
			value = GetSurrogateKey(randomSeed, columnValue);

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
		}

		private enum Op
		{
			Add = 1,
			Sub,
			Mul,
			Div,
			Mod
		}

		#endregion
	}
}
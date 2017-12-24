/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.StreamingIO.Primitives;

namespace SyncPrem.StreamingIO.DataMasking.Strategies
{
	/// <summary>
	/// Returns un-obfuscated, original value.
	/// DATA TYPE: any
	/// </summary>
	public sealed class NoneObfuscationStrategy : ObfuscationStrategy<NoneObfuscationStrategy.Spec>
	{
		#region Constructors/Destructors

		public NoneObfuscationStrategy()
		{
		}

		#endregion

		#region Methods/Operators

		protected override object CoreGetObfuscatedValue(IObfuscationContext obfuscationContext, IColumnSpec<Spec> columnSpec, IField field, object columnValue)
		{
			if ((object)obfuscationContext == null)
				throw new ArgumentNullException(nameof(obfuscationContext));

			if ((object)columnSpec == null)
				throw new ArgumentNullException(nameof(columnSpec));

			if ((object)field == null)
				throw new ArgumentNullException(nameof(field));

			if ((object)columnSpec.ObfuscationStrategySpec == null)
				throw new InvalidOperationException(string.Format("Specification missing: '{0}'.", nameof(columnSpec.ObfuscationStrategySpec)));

			return columnValue;
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

		#endregion
	}
}
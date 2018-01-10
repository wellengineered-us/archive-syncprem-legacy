/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace SyncPrem.StreamingIO.DataMasking
{
	public class ColumnSpec<TObfuscationStrategySpec> : ColumnSpec, IColumnSpec<TObfuscationStrategySpec>
		where TObfuscationStrategySpec : class, IObfuscationStrategySpec
	{
		#region Constructors/Destructors

		public ColumnSpec(IColumnSpec columnSpec)
		{
			if ((object)columnSpec == null)
				throw new ArgumentNullException(nameof(columnSpec));

			if ((object)base.ObfuscationStrategySpec != null &&
				(object)columnSpec.ObfuscationStrategySpec != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair in columnSpec.ObfuscationStrategySpec)
					base.ObfuscationStrategySpec.Add(keyValuePair.Key, keyValuePair.Value);
			}

			this.ColumnName = columnSpec.ColumnName;
			this.ObfuscationStrategyType = columnSpec.ObfuscationStrategyType;
		}

		#endregion

		#region Fields/Constants

		private TObfuscationStrategySpec obfuscationStrategySpec;

		#endregion

		#region Properties/Indexers/Events

		public new TObfuscationStrategySpec ObfuscationStrategySpec
		{
			get
			{
				this.ApplyObfuscationStrategySpec(); // special case
				return this.obfuscationStrategySpec;
			}
			set
			{
				this.obfuscationStrategySpec = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override void ApplyObfuscationStrategySpec()
		{
			if ((object)base.ObfuscationStrategySpec != null)
			{
				this.ObfuscationStrategySpec = JObject.FromObject(base.ObfuscationStrategySpec).ToObject<TObfuscationStrategySpec>();
			}
		}

		public override void ResetObfuscationStrategySpec()
		{
			base.ResetObfuscationStrategySpec();
			this.ObfuscationStrategySpec = null;
		}

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace SyncPrem.StreamingIO.DataMasking
{
	public class ColumnSpec : IColumnSpec
	{
		#region Constructors/Destructors

		public ColumnSpec()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly Dictionary<string, object> obfuscationStrategySpec = new Dictionary<string, object>();

		private string columnName;
		private Type obfuscationStrategyType;

		private IObfuscationStrategySpec untypedObfuscationStrategySpec;

		#endregion

		#region Properties/Indexers/Events

		public IDictionary<string, object> ObfuscationStrategySpec
		{
			get
			{
				return this.obfuscationStrategySpec;
			}
		}

		public string ColumnName
		{
			get
			{
				return this.columnName;
			}
			set
			{
				this.columnName = value;
			}
		}

		public Type ObfuscationStrategyType
		{
			get
			{
				return this.obfuscationStrategyType;
			}
			set
			{
				this.obfuscationStrategyType = value;
			}
		}

		private IObfuscationStrategySpec UntypedObfuscationStrategySpec
		{
			get
			{
				this.ApplyObfuscationStrategySpec(); // special case
				return this.untypedObfuscationStrategySpec;
			}
			set
			{
				this.untypedObfuscationStrategySpec = value;
			}
		}

		#endregion

		#region Methods/Operators

		public virtual void ApplyObfuscationStrategySpec()
		{
			this.ApplyObfuscationStrategySpec(this.ObfuscationStrategyType);
		}

		private void ApplyObfuscationStrategySpec(Type type)
		{
			if ((object)this.ObfuscationStrategySpec != null)
			{
				this.UntypedObfuscationStrategySpec = (IObfuscationStrategySpec)JObject.FromObject(this.ObfuscationStrategySpec).ToObject(type);
			}
		}

		public virtual void ResetObfuscationStrategySpec()
		{
			this.ObfuscationStrategySpec.Clear();
			this.UntypedObfuscationStrategySpec = null;
		}

		#endregion
	}
}
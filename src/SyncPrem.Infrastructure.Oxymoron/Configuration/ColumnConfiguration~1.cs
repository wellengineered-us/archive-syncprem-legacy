/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using SyncPrem.Infrastructure.Oxymoron.Strategies.Configuration;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Infrastructure.Oxymoron.Configuration
{
	public sealed class ColumnConfiguration<TObfuscationStrategyConfiguration> : ColumnConfiguration
		where TObfuscationStrategyConfiguration : ObfuscationStrategyConfiguration, new()
	{
		#region Constructors/Destructors

		public ColumnConfiguration(ColumnConfiguration columnConfiguration)
		{
			if ((object)columnConfiguration == null)
				throw new ArgumentNullException(nameof(columnConfiguration));

			if ((object)base.ObfuscationStrategySpecificConfiguration != null &&
				(object)columnConfiguration.ObfuscationStrategySpecificConfiguration != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair in columnConfiguration.ObfuscationStrategySpecificConfiguration)
					base.ObfuscationStrategySpecificConfiguration.Add(keyValuePair.Key, keyValuePair.Value);
			}

			this.ColumnName = columnConfiguration.ColumnName;
			this.ObfuscationStrategyAqtn = columnConfiguration.ObfuscationStrategyAqtn;
			this.Parent = columnConfiguration.Parent;
			this.Surround = columnConfiguration.Surround;
		}

		#endregion

		#region Fields/Constants

		private bool frozen;
		private TObfuscationStrategyConfiguration obfuscationStrategySpecificConfiguration;

		#endregion

		#region Properties/Indexers/Events

		public bool Frozen
		{
			get
			{
				return this.frozen;
			}
			set
			{
				this.frozen = value;
			}
		}

		public new TObfuscationStrategyConfiguration ObfuscationStrategySpecificConfiguration
		{
			get
			{
				this.ApplyObfuscationStrategySpecificConfiguration(); // special case
				return this.obfuscationStrategySpecificConfiguration;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.obfuscationStrategySpecificConfiguration, value);
				this.obfuscationStrategySpecificConfiguration = value;
			}
		}

		#endregion

		#region Methods/Operators

		public void ApplyObfuscationStrategySpecificConfiguration()
		{
			if ((object)base.ObfuscationStrategySpecificConfiguration != null && !this.Frozen)
			{
				this.ObfuscationStrategySpecificConfiguration = JObject.FromObject(base.ObfuscationStrategySpecificConfiguration).ToObject<TObfuscationStrategyConfiguration>();
				this.Frozen = true;
			}
		}

		public override void ResetObfuscationStrategySpecificConfiguration()
		{
			base.ResetObfuscationStrategySpecificConfiguration();
			this.Frozen = false;
			this.ObfuscationStrategySpecificConfiguration = null;
		}

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;
			int? columnIndex;

			columnIndex = context as int?;
			messages = new List<Message>();
			messages.AddRange(base.Validate(columnIndex));

			if ((object)this.ObfuscationStrategySpecificConfiguration != null)
				messages.AddRange(this.ObfuscationStrategySpecificConfiguration.Validate(columnIndex));

			return messages;
		}

		#endregion
	}
}
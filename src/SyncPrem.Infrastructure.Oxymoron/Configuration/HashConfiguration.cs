/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Configuration;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Infrastructure.Oxymoron.Configuration
{
	public class HashConfiguration : ConfigurationObject
	{
		#region Constructors/Destructors

		public HashConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private long? multiplier;
		private long? seed;

		#endregion

		#region Properties/Indexers/Events

		public long? Multiplier
		{
			get
			{
				return this.multiplier;
			}
			set
			{
				this.multiplier = value;
			}
		}

		public long? Seed
		{
			get
			{
				return this.seed;
			}
			set
			{
				this.seed = value;
			}
		}

		#endregion

		#region Methods/Operators

		public HashConfiguration Clone()
		{
			return new HashConfiguration() { Multiplier = this.Multiplier, Seed = this.Seed };
		}

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;

			messages = new List<Message>();

			if ((object)this.Multiplier == null)
				messages.Add(NewError(string.Format("Hash[{0}] multiplier is required.", context)));

			if ((object)this.Seed == null)
				messages.Add(NewError(string.Format("Hash[{0}] seed is required.", context)));

			return messages;
		}

		#endregion
	}
}
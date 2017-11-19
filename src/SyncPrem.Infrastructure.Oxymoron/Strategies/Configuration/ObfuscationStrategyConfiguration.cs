/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

using SyncPrem.Infrastructure.Configuration;
using SyncPrem.Infrastructure.Oxymoron.Configuration;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Infrastructure.Oxymoron.Strategies.Configuration
{
	public class ObfuscationStrategyConfiguration : ConfigurationObject
	{
		#region Constructors/Destructors

		public ObfuscationStrategyConfiguration()
		{
		}

		#endregion

		#region Properties/Indexers/Events

		[ConfigurationIgnore]
		public new ColumnConfiguration Parent
		{
			get
			{
				return (ColumnConfiguration)base.Parent;
			}
			set
			{
				base.Parent = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override IEnumerable<Message> Validate(object context)
		{
			return new Message[] { };
		}

		#endregion
	}
}
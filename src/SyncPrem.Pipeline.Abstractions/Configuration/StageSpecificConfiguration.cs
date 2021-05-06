/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

using SyncPrem.Infrastructure.Configuration;

using _Message = TextMetal.Middleware.Solder.Primitives.Message;

namespace SyncPrem.Pipeline.Abstractions.Configuration
{
	public class StageSpecificConfiguration : ComponentConfiguration
	{
		#region Constructors/Destructors

		public StageSpecificConfiguration()
		{
		}

		#endregion

		#region Properties/Indexers/Events

		[ConfigurationIgnore]
		public new StageConfiguration Parent
		{
			get
			{
				return (StageConfiguration)base.Parent;
			}
			set
			{
				base.Parent = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override IEnumerable<_Message> Validate(object context)
		{
			return new _Message[] { };
		}

		#endregion
	}
}
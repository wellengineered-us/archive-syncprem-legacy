/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

using SyncPrem.StreamingIO.Textual.Lined;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Core.Configurations
{
	public class LinedTextConnectorSpecificConfiguration : TextualConnectorSpecificConfiguration<LinedTextualFieldConfiguration, LinedTextualConfiguration, ILinedTextualFieldSpec, ILinedTextualSpec>
	{
		#region Constructors/Destructors

		public LinedTextConnectorSpecificConfiguration()
		{
		}

		#endregion

		#region Methods/Operators

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;

			messages = new List<Message>();

			messages.AddRange(base.Validate(context));

			return messages;
		}

		#endregion
	}
}
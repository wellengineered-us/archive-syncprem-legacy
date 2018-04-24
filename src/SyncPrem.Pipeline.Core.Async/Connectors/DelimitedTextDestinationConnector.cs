/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;

using SyncPrem.Pipeline.Core.Configurations;
using SyncPrem.StreamingIO.Textual.Delimited;

namespace SyncPrem.Pipeline.Core.Async.Connectors
{
	public class DelimitedTextDestinationConnector : TextualDestinationConnector
	<DelimitedTextualFieldConfiguration, DelimitedTextualConfiguration,
		IDelimitedTextualFieldSpec, IDelimitedTextualSpec,
		DelimitedTextConnectorSpecificConfiguration, DelimitedTextualWriter>
	{
		#region Constructors/Destructors

		public DelimitedTextDestinationConnector()
		{
		}

		#endregion

		#region Methods/Operators

		protected override DelimitedTextualWriter CreateTextualWriter(StreamWriter streamWriter, IDelimitedTextualSpec textualSpec)
		{
			if ((object)streamWriter == null)
				throw new ArgumentNullException(nameof(streamWriter));

			if ((object)textualSpec == null)
				throw new ArgumentNullException(nameof(textualSpec));

			return new DelimitedTextualWriter(streamWriter, textualSpec);
		}

		#endregion
	}
}
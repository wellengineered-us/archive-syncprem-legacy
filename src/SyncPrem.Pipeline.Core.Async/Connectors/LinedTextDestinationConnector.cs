/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;

using SyncPrem.Pipeline.Core.Configurations;
using SyncPrem.StreamingIO.Textual.Lined;

namespace SyncPrem.Pipeline.Core.Async.Connectors
{
	public class LinedTextDestinationConnector : TextualDestinationConnector
	<LinedTextualFieldConfiguration, LinedTextualConfiguration,
		ILinedTextualFieldSpec, ILinedTextualSpec,
		LinedTextConnectorSpecificConfiguration, LinedTextualWriter>
	{
		#region Constructors/Destructors

		public LinedTextDestinationConnector()
		{
		}

		#endregion

		#region Methods/Operators

		protected override LinedTextualWriter CreateTextualWriter(StreamWriter streamWriter, ILinedTextualSpec textualSpec)
		{
			if ((object)streamWriter == null)
				throw new ArgumentNullException(nameof(streamWriter));

			if ((object)textualSpec == null)
				throw new ArgumentNullException(nameof(textualSpec));

			return new LinedTextualWriter(streamWriter, textualSpec);
		}

		#endregion
	}
}
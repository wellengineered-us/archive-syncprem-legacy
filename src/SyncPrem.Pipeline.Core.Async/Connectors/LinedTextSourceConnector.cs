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
	public class LinedTextSourceConnector : TextualSourceConnector
	<LinedTextualFieldConfiguration, LinedTextualConfiguration,
		ILinedTextualFieldSpec, ILinedTextualSpec,
		LinedTextConnectorSpecificConfiguration, LinedTextualReader>
	{
		#region Constructors/Destructors

		public LinedTextSourceConnector()
		{
		}

		#endregion

		#region Methods/Operators

		protected override LinedTextualReader CreateTextualReader(StreamReader streamReader, ILinedTextualSpec textualSpec)
		{
			if ((object)streamReader == null)
				throw new ArgumentNullException(nameof(streamReader));

			if ((object)textualSpec == null)
				throw new ArgumentNullException(nameof(textualSpec));

			return new LinedTextualReader(streamReader, textualSpec);
		}

		#endregion
	}
}
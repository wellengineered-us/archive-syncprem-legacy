/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;

using SyncPrem.Pipeline.Core.Configurations;
using SyncPrem.StreamingIO.Textual.Delimited;

namespace SyncPrem.Pipeline.Core.Connectors
{
	public class DelimitedTextSourceConnector : TextualSourceConnector
	<DelimitedTextualFieldConfiguration, DelimitedTextualConfiguration,
		IDelimitedTextualFieldSpec, IDelimitedTextualSpec,
		DelimitedTextConnectorSpecificConfiguration, DelimitedTextualReader>
	{
		#region Constructors/Destructors

		public DelimitedTextSourceConnector()
		{
		}

		#endregion

		#region Methods/Operators

		protected override DelimitedTextualReader CreateTextualReader(StreamReader streamReader, IDelimitedTextualSpec textualSpec)
		{
			if ((object)streamReader == null)
				throw new ArgumentNullException(nameof(streamReader));

			if ((object)textualSpec == null)
				throw new ArgumentNullException(nameof(textualSpec));

			return new DelimitedTextualReader(streamReader, textualSpec);
		}

		#endregion
	}
}
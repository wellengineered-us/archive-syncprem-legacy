/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.StreamingIO.Textual.Delimited;

using TextMetal.Middleware.Solder.Extensions;
using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Core.Configurations
{
	public class DelimitedTextConnectorSpecificConfiguration : TextualConnectorSpecificConfiguration<DelimitedTextualFieldConfiguration, DelimitedTextualConfiguration, IDelimitedTextualFieldSpec, IDelimitedTextualSpec>
	{
		#region Constructors/Destructors

		public DelimitedTextConnectorSpecificConfiguration()
		{
		}

		#endregion

		#region Methods/Operators

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;
			string adapterContext;

			adapterContext = context as string;
			messages = new List<Message>();

			messages.AddRange(base.Validate(context));

			if ((object)this.TextualConfiguration != null)
			{
				if (SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.TextualConfiguration.RecordDelimiter))
					messages.Add(NewError(string.Format("{0} adapter textual (delimited) record delimiter is required.", adapterContext)));

				if (SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.TextualConfiguration.FieldDelimiter))
					messages.Add(NewError(string.Format("{0} adapter textual (delimited) field delimiter is required.", adapterContext)));
			}

			return messages;
		}

		#endregion
	}
}
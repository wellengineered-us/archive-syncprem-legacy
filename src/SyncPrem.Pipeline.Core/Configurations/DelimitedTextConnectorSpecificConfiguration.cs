/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions.Configuration;

using TextMetal.Middleware.Solder.Extensions;
using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Core.Configurations
{
	public class DelimitedTextConnectorSpecificConfiguration : StageSpecificConfiguration
	{
		#region Constructors/Destructors

		public DelimitedTextConnectorSpecificConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private string delimitedTextFilePath;
		private DelimitedTextualConfiguration delimitedTextualConfiguration;

		#endregion

		#region Properties/Indexers/Events

		public string DelimitedTextFilePath
		{
			get
			{
				return this.delimitedTextFilePath;
			}
			set
			{
				this.delimitedTextFilePath = value;
			}
		}

		public DelimitedTextualConfiguration DelimitedTextualConfiguration
		{
			get
			{
				return this.delimitedTextualConfiguration;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.delimitedTextualConfiguration, value);
				this.delimitedTextualConfiguration = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;
			string adapterContext;

			adapterContext = context as string;
			messages = new List<Message>();

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(this.DelimitedTextFilePath))
				messages.Add(NewError(string.Format("{0} adapter delimited text file path is required.", adapterContext)));

			if ((object)this.DelimitedTextualConfiguration == null)
				messages.Add(NewError(string.Format("{0} adapter delimited text specification is required.", adapterContext)));
			else
			{
				//if (SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.DelimitedTextualConfiguration.QuoteValue))
				//	messages.Add(NewError(string.Format("{0} adapter delimited text quote value is required.", adapterContext)));

				if (SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.DelimitedTextualConfiguration.RecordDelimiter))
					messages.Add(NewError(string.Format("{0} adapter delimited text record delimiter is required.", adapterContext)));

				if (SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.DelimitedTextualConfiguration.FieldDelimiter))
					messages.Add(NewError(string.Format("{0} adapter delimited text field delimiter is required.", adapterContext)));
			}

			return messages;
		}

		#endregion
	}
}
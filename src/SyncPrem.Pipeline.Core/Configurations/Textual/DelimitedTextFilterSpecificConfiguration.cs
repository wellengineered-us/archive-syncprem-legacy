/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

using SyncPrem.Infrastructure.Textual.Delimited;
using SyncPrem.Pipeline.Abstractions.Configurations;

using TextMetal.Middleware.Solder.Extensions;
using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Core.Configurations.Textual
{
	public class DelimitedTextFilterSpecificConfiguration : FilterSpecificConfiguration
	{
		#region Constructors/Destructors

		public DelimitedTextFilterSpecificConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private string delimitedTextFilePath;
		private DelimitedTextSpec delimitedTextSpec;

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

		public DelimitedTextSpec DelimitedTextSpec
		{
			get
			{
				return this.delimitedTextSpec;
			}
			set
			{
				this.delimitedTextSpec = value;
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

			if ((object)this.DelimitedTextSpec == null)
				messages.Add(NewError(string.Format("{0} adapter delimited text specification is required.", adapterContext)));
			else
			{
				//if (SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.DelimitedTextSpec.QuoteValue))
				//	messages.Add(NewError(string.Format("{0} adapter delimited text quote value is required.", adapterContext)));

				if (SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.DelimitedTextSpec.RecordDelimiter))
					messages.Add(NewError(string.Format("{0} adapter delimited text record delimiter is required.", adapterContext)));

				if (SolderFascadeAccessor.DataTypeFascade.IsNullOrEmpty(this.DelimitedTextSpec.FieldDelimiter))
					messages.Add(NewError(string.Format("{0} adapter delimited text field delimiter is required.", adapterContext)));
			}

			return messages;
		}

		#endregion
	}
}
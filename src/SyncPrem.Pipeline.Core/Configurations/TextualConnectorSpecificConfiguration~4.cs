/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.StreamingIO.Textual;

using TextMetal.Middleware.Solder.Extensions;
using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Core.Configurations
{
	public abstract class TextualConnectorSpecificConfiguration<TTextualFieldConfiguration, TTextualConfiguration, TTextualFieldSpec, TTextualSpec> : StageSpecificConfiguration
		where TTextualFieldConfiguration : TextualFieldConfiguration
		where TTextualConfiguration : TextualConfiguration<TTextualFieldConfiguration, TTextualFieldSpec, TTextualSpec>
		where TTextualFieldSpec : ITextualFieldSpec
		where TTextualSpec : ITextualSpec<TTextualFieldSpec>
	{
		#region Constructors/Destructors

		protected TextualConnectorSpecificConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private string textualFilePath;
		private TTextualConfiguration textualConfiguration;

		#endregion

		#region Properties/Indexers/Events

		public string TextualFilePath
		{
			get
			{
				return this.textualFilePath;
			}
			set
			{
				this.textualFilePath = value;
			}
		}

		public TTextualConfiguration TextualConfiguration
		{
			get
			{
				return this.textualConfiguration;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.textualConfiguration, value);
				this.textualConfiguration = value;
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

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(this.TextualFilePath))
				messages.Add(NewError(string.Format("{0} adapter textual file path is required.", adapterContext)));

			if ((object)this.TextualConfiguration == null)
				messages.Add(NewError(string.Format("{0} adapter textual specification is required.", adapterContext)));
			
			return messages;
		}

		#endregion
	}
}
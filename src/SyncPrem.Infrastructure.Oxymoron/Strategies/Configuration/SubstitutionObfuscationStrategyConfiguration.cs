/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;
using System.Linq;

using SyncPrem.Infrastructure.Oxymoron.Configuration;

using TextMetal.Middleware.Solder.Extensions;
using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Infrastructure.Oxymoron.Strategies.Configuration
{
	public class SubstitutionObfuscationStrategyConfiguration : ObfuscationStrategyConfiguration
	{
		#region Constructors/Destructors

		public SubstitutionObfuscationStrategyConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private string dictionaryReference;

		#endregion

		#region Properties/Indexers/Events

		public DictionaryConfiguration DictionaryConfiguration
		{
			get
			{
				return this.Parent.Parent.Parent.DictionaryConfigurations.SingleOrDefault(d => d.DictionaryId.SafeToString().Trim().ToLower() == this.DictionaryReference.SafeToString().Trim().ToLower());
			}
		}

		public string DictionaryReference
		{
			get
			{
				return this.dictionaryReference;
			}
			set
			{
				this.dictionaryReference = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;
			int? columnIndex;

			columnIndex = context as int?;
			messages = new List<Message>();

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(this.DictionaryReference))
				messages.Add(NewError(string.Format("Column[{0}/{1}] dictionary reference is required.", columnIndex, this.Parent.ColumnName)));

			return messages;
		}

		#endregion
	}
}
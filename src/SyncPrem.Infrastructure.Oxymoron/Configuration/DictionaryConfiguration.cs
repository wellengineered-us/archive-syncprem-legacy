/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using SyncPrem.Infrastructure.Configuration;

using TextMetal.Middleware.Solder.Extensions;
using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Infrastructure.Oxymoron.Configuration
{
	public class DictionaryConfiguration : ConfigurationObject
	{
		#region Constructors/Destructors

		public DictionaryConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private string dictionaryId;
		private bool preloadEnabled;
		private long? recordCount;

		#endregion

		#region Properties/Indexers/Events

		public string DictionaryId
		{
			get
			{
				return this.dictionaryId;
			}
			set
			{
				this.dictionaryId = value;
			}
		}

		[ConfigurationIgnore]
		public new ObfuscationConfiguration Parent
		{
			get
			{
				return (ObfuscationConfiguration)base.Parent;
			}
			set
			{
				base.Parent = value;
			}
		}

		public bool PreloadEnabled
		{
			get
			{
				return this.preloadEnabled;
			}
			set
			{
				this.preloadEnabled = value;
			}
		}

		public long? RecordCount
		{
			get
			{
				return this.recordCount;
			}
			set
			{
				this.recordCount = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;
			const string CONTEXT = "Dictionary";
			int? dictionaryIndex;

			dictionaryIndex = context as int?;
			messages = new List<Message>();

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(this.DictionaryId))
				messages.Add(NewError(string.Format("Dictionary[{0}] ID is required.", dictionaryIndex)));

			return messages;
		}

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using SyncPrem.Infrastructure.Configuration;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Infrastructure.Oxymoron.Configuration
{
	public class ObfuscationConfiguration : ConfigurationObject
	{
		#region Constructors/Destructors

		public ObfuscationConfiguration()
		{
			this.dictionaryConfigurations = new ConfigurationObjectCollection<DictionaryConfiguration>(this);
		}

		#endregion

		#region Fields/Constants

		private readonly ConfigurationObjectCollection<DictionaryConfiguration> dictionaryConfigurations;
		private bool? disableEngineCaches;
		private bool? enablePassThru;
		private HashConfiguration hashConfiguration;
		private TableConfiguration tableConfiguration;

		#endregion

		#region Properties/Indexers/Events

		public ConfigurationObjectCollection<DictionaryConfiguration> DictionaryConfigurations
		{
			get
			{
				return this.dictionaryConfigurations;
			}
		}

		public bool? DisableEngineCaches
		{
			get
			{
				return this.disableEngineCaches;
			}
			set
			{
				this.disableEngineCaches = value;
			}
		}

		public bool? EnablePassThru
		{
			get
			{
				return this.enablePassThru;
			}
			set
			{
				this.enablePassThru = value;
			}
		}

		public HashConfiguration HashConfiguration
		{
			get
			{
				return this.hashConfiguration;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.hashConfiguration, value);
				this.hashConfiguration = value;
			}
		}

		[ConfigurationIgnore]
		public new ObfuscationConfiguration Parent
		{
			get
			{
				return this;
			}
			set
			{
			}
		}

		public TableConfiguration TableConfiguration
		{
			get
			{
				return this.tableConfiguration;
			}
			set
			{
				this.EnsureParentOnPropertySet(this.tableConfiguration, value);
				this.tableConfiguration = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override IEnumerable<Message> Validate(object context)
		{
			List<Message> messages;
			int index;
			const string SRC_CONTEXT = "Source";
			const string DST_CONTEXT = "Destination";

			messages = new List<Message>();

			if ((object)this.TableConfiguration == null)
				messages.Add(NewError("Table configuration is required."));
			else
				messages.AddRange(this.TableConfiguration.Validate());

			if ((object)this.HashConfiguration == null)
				messages.Add(NewError("Hash configuration is required."));
			else
				messages.AddRange(this.HashConfiguration.Validate());

			// check for duplicate dictionaries
			var dictionaryIdSums = this.DictionaryConfigurations.GroupBy(d => d.DictionaryId)
				.Select(dl => new
							{
								DictionaryId = dl.First().DictionaryId,
								Count = dl.Count()
							}).Where(dl => dl.Count > 1);

			if (dictionaryIdSums.Any())
				messages.AddRange(dictionaryIdSums.Select(d => NewError(string.Format("Duplicate dictionary configuration found: '{0}'.", d.DictionaryId))).ToArray());

			index = 0;
			foreach (DictionaryConfiguration dictionaryConfiguration in this.DictionaryConfigurations)
				messages.AddRange(dictionaryConfiguration.Validate(index++));

			return messages;
		}

		#endregion
	}
}
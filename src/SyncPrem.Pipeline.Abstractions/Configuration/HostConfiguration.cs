/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using SyncPrem.Infrastructure.Configuration;

using _Message = TextMetal.Middleware.Solder.Primitives.Message;

namespace SyncPrem.Pipeline.Abstractions.Configuration
{
	public class HostConfiguration : ComponentConfiguration
	{
		#region Constructors/Destructors

		public HostConfiguration()
		{
			this.pipelineConfigurations = new ConfigurationObjectCollection<PipelineConfiguration>(this);
		}

		#endregion

		#region Fields/Constants

		private readonly ConfigurationObjectCollection<PipelineConfiguration> pipelineConfigurations;

		private string __;

		private bool? enableDispatchLoop;

		#endregion

		#region Properties/Indexers/Events

		public ConfigurationObjectCollection<PipelineConfiguration> PipelineConfigurations
		{
			get
			{
				return this.pipelineConfigurations;
			}
		}

		public string _
		{
			get
			{
				return this.__;
			}
			set
			{
				this.__ = value;
			}
		}

		public bool? EnableDispatchLoop
		{
			get
			{
				return this.enableDispatchLoop;
			}
			set
			{
				this.enableDispatchLoop = value;
			}
		}

		#endregion

		#region Methods/Operators

		public override IEnumerable<_Message> Validate(object context)
		{
			List<_Message> messages;

			messages = new List<_Message>();

			if (this._ != nameof(this._))
				messages.Add(NewError(string.Format("Not a valid host configuration (magic property missing).")));
			else
			{
				messages.AddRange(this.PipelineConfigurations.SelectMany(pc => pc.Validate("PIPELINE")));
			}

			return messages;
		}

		#endregion
	}
}
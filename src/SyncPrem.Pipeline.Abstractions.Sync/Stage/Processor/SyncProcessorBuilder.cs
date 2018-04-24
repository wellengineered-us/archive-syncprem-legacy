/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace SyncPrem.Pipeline.Abstractions.Sync.Stage.Processor
{
	public class SyncProcessorBuilder : ISyncProcessorBuilder
	{
		#region Constructors/Destructors

		public SyncProcessorBuilder()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly IList<Func<SyncProcessDelegate, SyncProcessDelegate>> components = new List<Func<SyncProcessDelegate, SyncProcessDelegate>>();

		#endregion

		#region Properties/Indexers/Events

		private IList<Func<SyncProcessDelegate, SyncProcessDelegate>> Components
		{
			get
			{
				return this.components;
			}
		}

		#endregion

		#region Methods/Operators

		public SyncProcessDelegate Build()
		{
			SyncProcessDelegate process = (context, configuration, channel) => channel; // simply return original channel unmodified

			foreach (Func<SyncProcessDelegate, SyncProcessDelegate> component in this.Components.Reverse())
			{
				process = component(process);
			}

			return process;
		}

		public ISyncProcessorBuilder New()
		{
			return new SyncProcessorBuilder();
		}

		public ISyncProcessorBuilder Use(Func<SyncProcessDelegate, SyncProcessDelegate> middleware)
		{
			this.Components.Add(middleware);
			return this;
		}

		#endregion
	}
}
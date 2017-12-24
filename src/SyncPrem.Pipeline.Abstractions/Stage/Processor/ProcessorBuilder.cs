/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace SyncPrem.Pipeline.Abstractions.Stage.Processor
{
	public class ProcessorBuilder : IProcessorBuilder
	{
		#region Constructors/Destructors

		public ProcessorBuilder()
		{
		}

		private ProcessorBuilder(ProcessorBuilder processorBuilder)
		{
		}

		#endregion

		#region Fields/Constants

		private readonly IList<Func<ProcessDelegate, ProcessDelegate>> components = new List<Func<ProcessDelegate, ProcessDelegate>>();

		#endregion

		#region Properties/Indexers/Events

		private IList<Func<ProcessDelegate, ProcessDelegate>> Components
		{
			get
			{
				return this.components;
			}
		}

		#endregion

		#region Methods/Operators

		public ProcessDelegate Build()
		{
			ProcessDelegate process = (ctx, cfg, msg) => msg; // simply return original message unmodified

			foreach (Func<ProcessDelegate, ProcessDelegate> component in this.Components.Reverse())
			{
				process = component(process);
			}

			return process;
		}

		public IProcessorBuilder New()
		{
			return new ProcessorBuilder(this);
		}

		public IProcessorBuilder Use(Func<ProcessDelegate, ProcessDelegate> middleware)
		{
			this.Components.Add(middleware);
			return this;
		}

		#endregion
	}
}
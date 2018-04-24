/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyncPrem.Pipeline.Abstractions.Async.Stage.Processor
{
	public class AsyncProcessorBuilder : IAsyncProcessorBuilder
	{
		#region Constructors/Destructors

		public AsyncProcessorBuilder()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly IList<Func<AsyncProcessDelegate, AsyncProcessDelegate>> components = new List<Func<AsyncProcessDelegate, AsyncProcessDelegate>>();

		#endregion

		#region Properties/Indexers/Events

		private IList<Func<AsyncProcessDelegate, AsyncProcessDelegate>> Components
		{
			get
			{
				return this.components;
			}
		}

		#endregion

		#region Methods/Operators

		public AsyncProcessDelegate BuildAsync()
		{
			AsyncProcessDelegate process = async (asyncContext, configuration, asyncChannel, cancellationToken) => await Task.FromResult(asyncChannel); // simply return original channel unmodified

			foreach (Func<AsyncProcessDelegate, AsyncProcessDelegate> component in this.Components.Reverse())
			{
				process = component(process);
			}

			return process;
		}

		public IAsyncProcessorBuilder NewAsync()
		{
			return new AsyncProcessorBuilder();
		}

		public IAsyncProcessorBuilder UseAsync(Func<AsyncProcessDelegate, AsyncProcessDelegate> middleware)
		{
			this.Components.Add(middleware);
			return this;
		}

		#endregion
	}
}
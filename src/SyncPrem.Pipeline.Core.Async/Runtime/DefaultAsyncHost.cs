/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Async.Runtime;
using SyncPrem.Pipeline.Abstractions.Configuration;

using TextMetal.Middleware.Solder.Injection;
using TextMetal.Middleware.Solder.Primitives;
using TextMetal.Middleware.Solder.Primitives.Async;

namespace SyncPrem.Pipeline.Core.Async.Runtime
{
	public class DefaultAsyncHost : AsyncHost
	{
		#region Constructors/Destructors

		[DependencyInjection]
		public DefaultAsyncHost()
		{
		}

		#endregion

		#region Methods/Operators

		private static async Task<long> ExecutePipelineAsync(Type pipelineType, PipelineConfiguration configuration, CancellationToken cancellationToken)
		{
			IAsyncPipeline asyncPipeline;

			if ((object)pipelineType == null)
				throw new ArgumentNullException(nameof(pipelineType));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			asyncPipeline = (IAsyncPipeline)Activator.CreateInstance(pipelineType);

			if ((object)asyncPipeline == null)
				throw new InvalidOperationException(nameof(asyncPipeline));

			using (AsyncDisposal.Await(asyncPipeline, cancellationToken))
			{
				asyncPipeline.Configuration = configuration;
				await asyncPipeline.CreateAsync(cancellationToken);

				IAsyncContext asyncContext;
				using (AsyncDisposal.Await(asyncContext = asyncPipeline.CreateContextAsync(), cancellationToken))
				{
					await asyncContext.CreateAsync(cancellationToken);

					return await asyncPipeline.ExecuteAsync(asyncContext, cancellationToken);
				}
			}
		}

		private static async Task<bool> WasCanceled(Task task)
		{
			try
			{
				await task;
			}
			catch (TaskCanceledException ex)
			{
				return true;
			}

			return false;
		}

		protected override async Task RunAsyncInternal(CancellationToken cancellationToken)
		{
			Type hostType;
			Message[] messages;
			PipelineConfiguration[] pipelineConfigurations;

			List<Task> tasks;

			if ((object)this.Configuration == null)
				throw new InvalidOperationException(string.Format("Host configuration is required"));

			messages = this.Configuration.Validate().ToArray();

			if (messages.Length > 0)
				throw new InvalidOperationException(string.Format("Host configuration validation failed:\r\n{0}", string.Join("\r\n", messages.Select(m => m.Description).ToArray())));

			hostType = this.GetType();
			Console.WriteLine("poll(): {0}", hostType.FullName);

			pipelineConfigurations = this.Configuration.PipelineConfigurations.Where(pc => (object)pc != null && (pc.IsEnabled ?? false)).ToArray();

			tasks = new List<Task>();

			do
			{
				Console.WriteLine("poll() begin");

				foreach (PipelineConfiguration pipelineConfiguration in pipelineConfigurations)
				{
					Task task;
					Type pipelineType;

					pipelineType = pipelineConfiguration.GetPipelineType();

					if ((object)pipelineType == null)
						throw new InvalidOperationException(nameof(pipelineType));

					task = ExecutePipelineAsync(pipelineType, pipelineConfiguration, cancellationToken);
					//task = Task.Run(async () => await ExecutePipelineAsync(pipelineType, pipelineConfiguration, cancellationToken), cancellationToken);
					tasks.Add(task);
				}

				Console.WriteLine("poll() wait...");
				await Task.WhenAll(tasks);

				Console.WriteLine("poll() delay...");
				if (await WasCanceled(Task.Delay(TimeSpan.FromMilliseconds(5000), cancellationToken)))
					break;

				Console.WriteLine("poll() end");
				tasks.Clear();
			}
			while (this.Configuration.EnableDispatchLoop ?? false);
		}

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Linq;
using System.Threading;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;

using TextMetal.Middleware.Solder.Injection;
using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Core.Sync.Runtime
{
	public class DefaultSyncHost : SyncHost
	{
		#region Constructors/Destructors

		[DependencyInjection]
		public DefaultSyncHost()
		{
		}

		#endregion

		#region Methods/Operators

		private static void ExecutePipeline(SemaphoreSlim semaphoreSlim, Type pipelineType, PipelineConfiguration configuration)
		{
			ISyncPipeline pipeline;

			try
			{
				if ((object)semaphoreSlim == null)
					throw new ArgumentNullException(nameof(semaphoreSlim));

				if ((object)pipelineType == null)
					throw new ArgumentNullException(nameof(pipelineType));

				if ((object)configuration == null)
					throw new ArgumentNullException(nameof(configuration));

				pipeline = (ISyncPipeline)Activator.CreateInstance(pipelineType);

				if ((object)pipeline == null)
					throw new InvalidOperationException(nameof(pipeline));

				using (pipeline)
				{
					pipeline.Configuration = configuration;
					pipeline.Create();

					using (ISyncContext context = pipeline.CreateContext())
					{
						context.Create();

						pipeline.Execute(context);
					}
				}
			}
			finally
			{
				Thread.Sleep(10000);

				if ((object)semaphoreSlim != null)
					semaphoreSlim.Release();
			}
		}

		private static void ExecutePipelineThreadProc(object stateInfo)
		{
			Tuple<SemaphoreSlim, Type, PipelineConfiguration> tuple;

			tuple = (Tuple<SemaphoreSlim, Type, PipelineConfiguration>)stateInfo;

			ExecutePipeline(tuple.Item1, tuple.Item2, tuple.Item3);
		}

		protected override void RunInternal()
		{
			Type hostType;
			Message[] messages;
			PipelineConfiguration[] pipelineConfigurations;

			if ((object)this.Configuration == null)
				throw new InvalidOperationException(string.Format("Host configuration is required"));

			messages = this.Configuration.Validate().ToArray();

			if (messages.Length > 0)
				throw new InvalidOperationException(string.Format("Host configuration validation failed:\r\n{0}", string.Join("\r\n", messages.Select(m => m.Description).ToArray())));

			hostType = this.GetType();
			Console.WriteLine("poll(): {0}", hostType.FullName);

			pipelineConfigurations = this.Configuration.PipelineConfigurations.Where(pc => (object)pc != null && (pc.IsEnabled ?? false)).ToArray();

			using (SemaphoreSlim semaphoreSlim = new SemaphoreSlim(0, pipelineConfigurations.Length))
			{
				do
				{
					Console.WriteLine("poll() begin");

					foreach (PipelineConfiguration pipelineConfiguration in pipelineConfigurations)
					{
						Type pipelineType;

						pipelineType = pipelineConfiguration.GetPipelineType();

						if ((object)pipelineType == null)
							throw new InvalidOperationException(nameof(pipelineType));

						ThreadPool.QueueUserWorkItem(ExecutePipelineThreadProc, Tuple.Create(semaphoreSlim, pipelineType, pipelineConfiguration));
					}

					Console.WriteLine("poll() wait...");
					semaphoreSlim.Wait();

					Console.WriteLine("poll() sleep...");
					Thread.Sleep(5000);

					Console.WriteLine("poll() end");
				}
				while (this.Configuration.EnableDispatchLoop ?? false);
			}
		}

		#endregion
	}
}
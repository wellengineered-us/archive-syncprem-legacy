/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;

using TextMetal.Middleware.Solder.Injection;
using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Host.Cli.Hosting
{
	public sealed class ToolHost : Abstractions.Runtime.Host, IToolHost
	{
		#region Constructors/Destructors

		[DependencyInjection]
		public ToolHost()
		{
		}

		#endregion

		#region Methods/Operators

		private static async Task<int> ExecutePipelineAsync(Type pipelineType, PipelineConfiguration configuration, CancellationToken cancellationToken)
		{
			IPipeline pipeline;

			if ((object)pipelineType == null)
				throw new ArgumentNullException(nameof(pipelineType));

			if ((object)configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			pipeline = (IPipeline)Activator.CreateInstance(pipelineType);

			if ((object)pipeline == null)
				throw new InvalidOperationException(nameof(pipeline));

			using (pipeline)
			{
				pipeline.Configuration = configuration;
				pipeline.Create();

				using (IContext context = pipeline.CreateContext())
				{
					context.Create();

					return await pipeline.ExecuteAsync(context, cancellationToken);
				}
			}
		}

		protected override async Task RunAsyncInternal(CancellationToken cancellationToken, IProgress<int> progress)
		{
			Type hostType;
			Type pipelineType;
			Message[] messages;

			if ((object)this.Configuration == null)
				throw new InvalidOperationException(string.Format("Host configuration is required"));

			messages = this.Configuration.Validate().ToArray();

			if (messages.Length > 0)
				throw new InvalidOperationException(string.Format("Host configuration validation failed:\r\n{0}", string.Join("\r\n", messages.Select(m => m.Description).ToArray())));

			hostType = this.GetType();

			List<Task> tasks = new List<Task>();

			while (true)
			{
				Console.WriteLine("poll() begin");

				for (int i = 0; i < 10; i++)
					foreach (PipelineConfiguration pipelineConfiguration in this.Configuration.PipelineConfigurations)
					{
						Task task;

						if ((object)pipelineConfiguration == null)
							throw new InvalidOperationException(nameof(pipelineConfiguration));

						if (!(pipelineConfiguration.IsEnabled ?? false))
							continue;

						pipelineType = pipelineConfiguration.GetPipelineType();

						if ((object)pipelineType == null)
							throw new InvalidOperationException(nameof(pipelineType));

						task = ExecutePipelineAsync(pipelineType, pipelineConfiguration, cancellationToken);
						tasks.Add(task);
					}

				Console.WriteLine("poll() await...");
				await Task.WhenAll(tasks);

				Console.WriteLine("poll() delay...");
				await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
				Console.WriteLine("poll() end");
			}
		}

		#endregion
	}
}
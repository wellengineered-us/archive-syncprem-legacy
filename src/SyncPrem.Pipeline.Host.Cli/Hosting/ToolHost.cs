/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;
using System.Linq;

using SyncPrem.Infrastructure.Configuration;
using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;

using TextMetal.Middleware.Solder.Injection;
using TextMetal.Middleware.Solder.Primitives;
using TextMetal.Middleware.Solder.Serialization;

namespace SyncPrem.Pipeline.Host.Cli.Hosting
{
	public sealed class ToolHost : Component, IToolHost
	{
		#region Constructors/Destructors

		[DependencyInjection]
		public ToolHost()
		{
		}

		#endregion

		#region Fields/Constants

		private PipelineConfiguration configuration;

		#endregion

		#region Properties/Indexers/Events

		public PipelineConfiguration Configuration
		{
			get
			{
				return this.configuration;
			}
			set
			{
				this.configuration = value;
			}
		}

		#endregion

		#region Methods/Operators

		private static void ExecutePipelineThreadProc(object stateInfo)
		{
			IPipeline pipeline;
			Tuple<Type, PipelineConfiguration> tuple;

			if ((object)stateInfo == null)
				throw new ArgumentNullException(nameof(stateInfo));

			tuple = (Tuple<Type, PipelineConfiguration>)stateInfo;
			pipeline = (IPipeline)Activator.CreateInstance(tuple.Item1);

			if ((object)pipeline == null)
				throw new InvalidOperationException(nameof(pipeline));

			using (pipeline)
			{
				pipeline.Configuration = tuple.Item2;
				pipeline.Create();

				using (IContext context = pipeline.CreateContext())
				{
					context.Create();

					pipeline.Execute(context);
				}
			}
		}

		private static TConfiguration FromJsonFile<TConfiguration>(string jsonFilePath)
			where TConfiguration : class, IConfigurationObject, new()
		{
			TConfiguration configuration;

			configuration = JsonSerializationStrategy.Instance.GetObjectFromFile<TConfiguration>(jsonFilePath);

			return configuration;
		}

		public void Host(string sourceFilePath)
		{
			PipelineConfiguration pipelineConfiguration;

			sourceFilePath = Path.GetFullPath(sourceFilePath);
			pipelineConfiguration = FromJsonFile<PipelineConfiguration>(sourceFilePath);

			this.Configuration = pipelineConfiguration;
			this.Create();
			this.RunHost();
		}

		private void RunHost()
		{
			Type pipelineType;
			Message[] messages;

			if ((object)this.Configuration == null)
				throw new InvalidOperationException(string.Format("Pipeline configuration is required"));

			messages = this.Configuration.Validate().ToArray();

			if (messages.Length > 0)
				throw new InvalidOperationException(string.Format("PipelineConfiguration validation failed:\r\n{0}", string.Join("\r\n", messages.Select(m => m.Description).ToArray())));

			pipelineType = this.Configuration.GetPipelineType();

			if ((object)pipelineType == null)
				throw new InvalidOperationException(nameof(pipelineType));

			//while (true)
			{
				//var t = new Thread(ExecutePipelineThreadProc);
				//t.Start(Tuple.Create(pipelineType, this.Configuration));
				//t.Join();
			}

			ExecutePipelineThreadProc(Tuple.Create(pipelineType, this.Configuration));

			//ThreadPool.QueueUserWorkItem(ExecutePipelineThreadProc, Tuple.Create(pipelineType, this.Configuration));
			//Thread.Sleep(int.MaxValue);
		}

		#endregion
	}
}
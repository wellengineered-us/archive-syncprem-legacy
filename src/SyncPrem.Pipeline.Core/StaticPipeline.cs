/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Payload;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Stage;
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Destination;
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Source;
using SyncPrem.Pipeline.Abstractions.Stage.Processor;
using SyncPrem.Pipeline.Core.Processors;

using TextMetal.Middleware.Solder.Injection;
using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Core
{
	public sealed class StaticPipeline : Component, IPipeline
	{
		#region Constructors/Destructors

		[DependencyInjection]
		public StaticPipeline()
		{
		}

		#endregion

		#region Fields/Constants

		private PipelineConfiguration pipelineConfiguration;

		#endregion

		#region Properties/Indexers/Events

		public PipelineConfiguration PipelineConfiguration
		{
			get
			{
				return this.pipelineConfiguration;
			}
			set
			{
				this.pipelineConfiguration = value;
			}
		}

		#endregion

		#region Methods/Operators

		public IContext CreateContext()
		{
			return new DefaultContext(); // TODO DI/IoC
		}

		public int Execute(IContext context)
		{
			IPipelineMessage pipelineMessage;

			ISourceConnector sourceConnector;
			IDestinationConnector destinationConnector;

			Type sourceConnectorType;
			Type destinationConnectorType;
			IDictionary<StageConfiguration, Type> processorTypeConfigMappings;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			sourceConnectorType = this.PipelineConfiguration.SourceConfiguration.GetStageType();
			destinationConnectorType = this.PipelineConfiguration.DestinationConfiguration.GetStageType();
			processorTypeConfigMappings = this.PipelineConfiguration.ProcessorConfigurations.ToDictionary(c => c, c => c.GetStageType());

			if ((object)sourceConnectorType == null)
				throw new InvalidOperationException(nameof(sourceConnectorType));

			sourceConnector = (ISourceConnector)Activator.CreateInstance(sourceConnectorType);

			if ((object)sourceConnector == null)
				throw new InvalidOperationException(nameof(sourceConnector));

			using (sourceConnector)
			{
				sourceConnector.StageConfiguration = this.PipelineConfiguration.SourceConfiguration;
				sourceConnector.Create();

				if ((object)destinationConnectorType == null)
					throw new InvalidOperationException(nameof(destinationConnectorType));

				destinationConnector = (IDestinationConnector)Activator.CreateInstance(destinationConnectorType);

				if ((object)destinationConnector == null)
					throw new InvalidOperationException(nameof(destinationConnector));

				using (destinationConnector)
				{
					RecordConfiguration recordConfiguration;

					ProcessDelegate process;
					IProcessorBuilder processorBuilder;

					destinationConnector.StageConfiguration = this.PipelineConfiguration.DestinationConfiguration;
					destinationConnector.Create();

					recordConfiguration = this.PipelineConfiguration.RecordConfiguration ?? new RecordConfiguration();
					sourceConnector.PreExecute(context, recordConfiguration);
					destinationConnector.PreExecute(context, recordConfiguration);

					// --
					processorBuilder = new ProcessorBuilder();

					if (false)
					{
						// regular methods
						processorBuilder.Use(NullProcessor.NullMiddlewareMethod);

						// local functions
						ProcessDelegate _middleware(ProcessDelegate next)
						{
							IPipelineMessage _processor(IContext ctx, RecordConfiguration cfg, IPipelineMessage msg)
							{
								Console.WriteLine("processor_zero");
								return next(ctx, cfg, msg);
							}

							return _processor;
						}

						processorBuilder.Use(_middleware);

						// lambda expressions
						processorBuilder.Use(next =>
											{
												return (ctx, cfg, msg) =>
														{
															Console.WriteLine("processor_first");
															return next(ctx, cfg, msg);
														};
											});
					}

					foreach (KeyValuePair<StageConfiguration, Type> processorTypeConfigMapping in processorTypeConfigMappings)
					{
						if ((object)processorTypeConfigMapping.Key == null)
							throw new InvalidOperationException(nameof(processorTypeConfigMapping.Key));

						if ((object)processorTypeConfigMapping.Value == null)
							throw new InvalidOperationException(nameof(processorTypeConfigMapping.Value));

						processorBuilder.UseMiddleware(processorTypeConfigMapping.Value, processorTypeConfigMapping.Key);
					}

					process = processorBuilder.Build();

					// --

					pipelineMessage = sourceConnector.Produce(context, recordConfiguration);

					pipelineMessage = process(context, recordConfiguration, pipelineMessage);

					destinationConnector.Consume(context, recordConfiguration, pipelineMessage);

					destinationConnector.PostExecute(context, recordConfiguration);
					sourceConnector.PostExecute(context, recordConfiguration);

					this.__check();
				}
			}

			return 0;
		}

		public Type GetPipelineType()
		{
			return typeof(StaticPipeline);
		}

		public IReadOnlyCollection<Type> GetStaticStageChain()
		{
			return null;
		}

		#endregion
	}

	public interface IProcessBuilder
	{
		#region Methods/Operators

		ExecuteDelegate Build(bool reverse);

		IProcessBuilder New();

		IProcessBuilder Use(ExecuteDelegate middleware);

		#endregion
	}

	public class ProcessBuilder : IProcessBuilder
	{
		#region Constructors/Destructors

		public ProcessBuilder()
		{
		}

		private ProcessBuilder(ProcessBuilder processBuilder)
		{
		}

		#endregion

		#region Fields/Constants

		private readonly IList<ExecuteDelegate> components = new List<ExecuteDelegate>();

		#endregion

		#region Properties/Indexers/Events

		private IList<ExecuteDelegate> Components
		{
			get
			{
				return this.components;
			}
		}

		#endregion

		#region Methods/Operators

		public ExecuteDelegate Build(bool reverse)
		{
			void _process(IContext ctx, RecordConfiguration cfg)
			{
				foreach (ExecuteDelegate component in (reverse ? this.Components.Reverse() : this.Components))
				{
					component(ctx, cfg);
				}
			}

			return _process;
		}

		public IProcessBuilder New()
		{
			return new ProcessBuilder(this);
		}

		public IProcessBuilder Use(ExecuteDelegate middleware)
		{
			this.Components.Add(middleware);
			return this;
		}

		#endregion
	}

	public static class ProcessBuilderExtensions
	{
		#region Methods/Operators

		public static IProcessBuilder UseMiddleware(this IProcessBuilder processBuilder, Type processorType, StageConfiguration stageConfiguration)
		{
			if ((object)processBuilder == null)
				throw new ArgumentNullException(nameof(processBuilder));

			if ((object)processorType == null)
				throw new ArgumentNullException(nameof(processorType));

			if ((object)stageConfiguration == null)
				throw new ArgumentNullException(nameof(stageConfiguration));

			return processBuilder.Use((ctx, cfg) =>
									{
										Type _processorType = processorType; // prevent closure bug
										IProcessor processor;

										processor = (IProcessor)Activator.CreateInstance(_processorType);

										if ((object)processor == null)
											throw new InvalidOperationException(nameof(processor));

										using (processor)
										{
											processor.StageConfiguration = stageConfiguration;
											processor.Create();

											processor.PreExecute(ctx, cfg);
										}
									});
		}

		#endregion
	}
}
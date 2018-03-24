/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Channel;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Runtime;
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Destination;
using SyncPrem.Pipeline.Abstractions.Stage.Connector.Source;
using SyncPrem.Pipeline.Abstractions.Stage.Processor;

using TextMetal.Middleware.Solder.Injection;

using static TextMetal.Middleware.Solder.Primitives.OnlyWhen;

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

		public IContext CreateContext()
		{
			return new DefaultContext(); // TODO DI/IoC
		}

		public int Execute(IContext context)
		{
			IChannel channel;

			ISourceConnector sourceConnector;
			IDestinationConnector destinationConnector;

			Type sourceConnectorType;
			Type destinationConnectorType;
			IDictionary<StageConfiguration, Type> processorTypeConfigMappings;

			if ((object)context == null)
				throw new ArgumentNullException(nameof(context));

			sourceConnectorType = this.Configuration.SourceConfiguration.GetStageType();
			destinationConnectorType = this.Configuration.DestinationConfiguration.GetStageType();
			processorTypeConfigMappings = this.Configuration.ProcessorConfigurations.ToDictionary(c => c, c => c.GetStageType());

			if ((object)sourceConnectorType == null)
				throw new InvalidOperationException(nameof(sourceConnectorType));

			sourceConnector = (ISourceConnector)Activator.CreateInstance(sourceConnectorType);

			if ((object)sourceConnector == null)
				throw new InvalidOperationException(nameof(sourceConnector));

			using (sourceConnector)
			{
				sourceConnector.Configuration = this.Configuration.SourceConfiguration;
				sourceConnector.Create();

				if ((object)destinationConnectorType == null)
					throw new InvalidOperationException(nameof(destinationConnectorType));

				destinationConnector = (IDestinationConnector)Activator.CreateInstance(destinationConnectorType);

				if ((object)destinationConnector == null)
					throw new InvalidOperationException(nameof(destinationConnector));

				using (destinationConnector)
				{
					RecordConfiguration configuration;

					ProcessDelegate process;
					IProcessorBuilder processorBuilder;

					destinationConnector.Configuration = this.Configuration.DestinationConfiguration;
					destinationConnector.Create();

					configuration = this.Configuration.RecordConfiguration ?? new RecordConfiguration();
					sourceConnector.PreExecute(context, configuration);
					destinationConnector.PreExecute(context, configuration);

					// --
					processorBuilder = new ProcessorBuilder();

					/*if (true)
					{
						// regular methods
						processorBuilder.Use(NullProcessor.NullMiddlewareMethod);

						// local functions
						ProcessDelegate _middleware(ProcessDelegate _next)
						{
							IChannel _processor(IContext _context, RecordConfiguration _configuration, IChannel _channel)
							{
								Console.WriteLine("processor_zero");
								return _next(_context, _configuration, _channel);
							}

							return _processor;
						}

						processorBuilder.Use(_middleware);

						// lambda expressions
						processorBuilder.Use(next =>
											{
												return (_context, _configuration, _channel) =>
														{
															Console.WriteLine("processor_first");
															return next(_context, _configuration, _channel);
														};
											});
					}*/

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

					channel = sourceConnector.Produce(context, configuration);

					channel = process(context, configuration, channel);

					destinationConnector.Consume(context, configuration, channel);

					destinationConnector.PostExecute(context, configuration);
					sourceConnector.PostExecute(context, configuration);

					this.__check();
				}
			}

			return 0;
		}

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Sync.Runtime;
using SyncPrem.Pipeline.Abstractions.Sync.Stage.Connector.Destination;
using SyncPrem.Pipeline.Abstractions.Sync.Stage.Connector.Source;
using SyncPrem.Pipeline.Abstractions.Sync.Stage.Processor;
using SyncPrem.Pipeline.Core.Sync.Processors;

using TextMetal.Middleware.Solder.Injection;
using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Core.Sync.Runtime
{
	public sealed class DefaultSyncPipeline : SyncPipeline
	{
		#region Constructors/Destructors

		[DependencyInjection]
		public DefaultSyncPipeline()
		{
		}

		#endregion

		#region Methods/Operators

		protected override ISyncContext CreateContextInternal()
		{
			ISyncContext context;
			Type contextType;

			contextType = this.Configuration.GetContextType() ?? typeof(DefaultSyncContext); // TODO DI/IoC

			context = (ISyncContext)Activator.CreateInstance(contextType);

			if ((object)context == null)
				throw new InvalidOperationException(nameof(context));

			return context;
		}

		protected override long ExecuteInternal(ISyncContext context)
		{
			ISyncChannel channel;

			ISyncSourceConnector sourceConnector;
			ISyncDestinationConnector destinationConnector;

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

			sourceConnector = (ISyncSourceConnector)Activator.CreateInstance(sourceConnectorType);

			if ((object)sourceConnector == null)
				throw new InvalidOperationException(nameof(sourceConnector));

			using (sourceConnector)
			{
				sourceConnector.Configuration = this.Configuration.SourceConfiguration;
				sourceConnector.Create();

				if ((object)destinationConnectorType == null)
					throw new InvalidOperationException(nameof(destinationConnectorType));

				destinationConnector = (ISyncDestinationConnector)Activator.CreateInstance(destinationConnectorType);

				if ((object)destinationConnector == null)
					throw new InvalidOperationException(nameof(destinationConnector));

				using (destinationConnector)
				{
					RecordConfiguration configuration;

					SyncProcessDelegate process;
					ISyncProcessorBuilder processorBuilder;

					destinationConnector.Configuration = this.Configuration.DestinationConfiguration;
					destinationConnector.Create();

					configuration = this.Configuration.RecordConfiguration ?? new RecordConfiguration();

					sourceConnector.PreExecute(context, configuration);
					destinationConnector.PreExecute(context, configuration);

					// --
					processorBuilder = new SyncProcessorBuilder();

					if (false)
					{
						// regular methods
						processorBuilder.Use(NullProcessor.NullMiddlewareMethod);

						// local functions
						SyncProcessDelegate _middleware(SyncProcessDelegate _next)
						{
							ISyncChannel _processor(ISyncContext _context, RecordConfiguration _configuration, ISyncChannel _channel)
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
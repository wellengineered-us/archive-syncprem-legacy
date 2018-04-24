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
using SyncPrem.Pipeline.Abstractions.Async.Stage.Connector.Destination;
using SyncPrem.Pipeline.Abstractions.Async.Stage.Connector.Source;
using SyncPrem.Pipeline.Abstractions.Async.Stage.Processor;
using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Core.Async.Processors;

using TextMetal.Middleware.Solder.Injection;
using TextMetal.Middleware.Solder.Primitives;
using TextMetal.Middleware.Solder.Primitives.Async;

namespace SyncPrem.Pipeline.Core.Async.Runtime
{
	public sealed class DefaultAsyncPipeline : AsyncPipeline
	{
		#region Constructors/Destructors

		[DependencyInjection]
		public DefaultAsyncPipeline()
		{
		}

		#endregion

		#region Methods/Operators

		protected override IAsyncContext CreateContextAsyncInternal()
		{
			IAsyncContext context;
			Type contextType;

			contextType = this.Configuration.GetContextType() ?? typeof(DefaultAsyncContext); // TODO DI/IoC

			context = (IAsyncContext)Activator.CreateInstance(contextType);

			if ((object)context == null)
				throw new InvalidOperationException(nameof(context));

			return context;
		}

		protected override async Task<long> ExecuteAsyncInternal(IAsyncContext asyncContext, CancellationToken cancellationToken)
		{
			IAsyncChannel asyncChannel;

			IAsyncSourceConnector sourceConnector;
			IAsyncDestinationConnector destinationConnector;

			Type sourceConnectorType;
			Type destinationConnectorType;
			IDictionary<StageConfiguration, Type> processorTypeConfigMappings;

			if ((object)asyncContext == null)
				throw new ArgumentNullException(nameof(asyncContext));

			sourceConnectorType = this.Configuration.SourceConfiguration.GetStageType();
			destinationConnectorType = this.Configuration.DestinationConfiguration.GetStageType();
			processorTypeConfigMappings = this.Configuration.ProcessorConfigurations.ToDictionary(c => c, c => c.GetStageType());

			if ((object)sourceConnectorType == null)
				throw new InvalidOperationException(nameof(sourceConnectorType));

			sourceConnector = (IAsyncSourceConnector)Activator.CreateInstance(sourceConnectorType);

			if ((object)sourceConnector == null)
				throw new InvalidOperationException(nameof(sourceConnector));

			using (AsyncDisposal.Await(sourceConnector, cancellationToken))
			{
				sourceConnector.Configuration = this.Configuration.SourceConfiguration;
				await sourceConnector.CreateAsync(cancellationToken);

				if ((object)destinationConnectorType == null)
					throw new InvalidOperationException(nameof(destinationConnectorType));

				destinationConnector = (IAsyncDestinationConnector)Activator.CreateInstance(destinationConnectorType);

				if ((object)destinationConnector == null)
					throw new InvalidOperationException(nameof(destinationConnector));

				using (AsyncDisposal.Await(destinationConnector, cancellationToken))
				{
					RecordConfiguration configuration;

					AsyncProcessDelegate asyncProcess;
					IAsyncProcessorBuilder asyncProcessorBuilder;

					destinationConnector.Configuration = this.Configuration.DestinationConfiguration;
					await destinationConnector.CreateAsync(cancellationToken);

					configuration = this.Configuration.RecordConfiguration ?? new RecordConfiguration();

					await sourceConnector.PreExecuteAsync(asyncContext, configuration, cancellationToken);
					await destinationConnector.PreExecuteAsync(asyncContext, configuration, cancellationToken);

					// --
					asyncProcessorBuilder = new AsyncProcessorBuilder();

					if (true)
					{
						// regular methods
						asyncProcessorBuilder.UseAsync(NullProcessor.NullMiddlewareAsyncMethod);

						// local functions
						AsyncProcessDelegate _asyncMiddleware(AsyncProcessDelegate _asyncNext)
						{
							async Task<IAsyncChannel> _asyncProcessor(IAsyncContext _asyncContext, RecordConfiguration _configuration, IAsyncChannel _asyncChannel, CancellationToken _cancellationToken)
							{
								Console.WriteLine("processor_zero");
								return await _asyncNext(_asyncContext, _configuration, _asyncChannel, _cancellationToken);
							}

							return _asyncProcessor;
						}

						asyncProcessorBuilder.UseAsync(_asyncMiddleware);

						// lambda expressions
						asyncProcessorBuilder.UseAsync(asyncNext =>
														{
															return (_asyncContext, _configuration, _asyncChannel, _cancelationToken) =>
																	{
																		Console.WriteLine("processor_first");
																		return asyncNext(_asyncContext, _configuration, _asyncChannel, _cancelationToken);
																	};
														});
					}

					foreach (KeyValuePair<StageConfiguration, Type> processorTypeConfigMapping in processorTypeConfigMappings)
					{
						if ((object)processorTypeConfigMapping.Key == null)
							throw new InvalidOperationException(nameof(processorTypeConfigMapping.Key));

						if ((object)processorTypeConfigMapping.Value == null)
							throw new InvalidOperationException(nameof(processorTypeConfigMapping.Value));

						asyncProcessorBuilder.UseMiddlewareAsync(processorTypeConfigMapping.Value, processorTypeConfigMapping.Key);
					}

					asyncProcess = asyncProcessorBuilder.BuildAsync();

					// --

					asyncChannel = await sourceConnector.ProduceAsync(asyncContext, configuration, cancellationToken);

					asyncChannel = await asyncProcess(asyncContext, configuration, asyncChannel, cancellationToken);

					await destinationConnector.ConsumeAsync(asyncContext, configuration, asyncChannel, cancellationToken);

					await destinationConnector.PostExecuteAsync(asyncContext, configuration, cancellationToken);
					await sourceConnector.PostExecuteAsync(asyncContext, configuration, cancellationToken);

					this.__check();
				}
			}

			return 0;
		}

		#endregion
	}
}
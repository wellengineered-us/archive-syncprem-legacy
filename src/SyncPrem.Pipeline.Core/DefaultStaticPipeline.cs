/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configurations;
using SyncPrem.Pipeline.Abstractions.Filters;
using SyncPrem.Pipeline.Abstractions.Filters.Consumer;
using SyncPrem.Pipeline.Abstractions.Filters.Producer;
using SyncPrem.Pipeline.Abstractions.Filters.Transformer;
using SyncPrem.Pipeline.Abstractions.Messages;
using SyncPrem.Pipeline.Core.Filters.Null;

using TextMetal.Middleware.Solder.Injection;
using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Core
{
	public sealed class DefaultStaticPipeline : PipelineComponent, IRealPipeline
	{
		#region Constructors/Destructors

		[DependencyInjection]
		public DefaultStaticPipeline()
		{
		}

		#endregion

		#region Fields/Constants

		private RealPipelineConfiguration realPipelineConfiguration;

		#endregion

		#region Properties/Indexers/Events

		public RealPipelineConfiguration RealPipelineConfiguration
		{
			get
			{
				return this.realPipelineConfiguration;
			}
			set
			{
				this.realPipelineConfiguration = value;
			}
		}

		#endregion

		#region Methods/Operators

		public IPipelineContext CreateContext()
		{
			return new DefaultPipelineContext(); // TODO DI/IoC
		}

		public int Execute(IPipelineContext pipelineContext)
		{
			IPipelineMessage pipelineMessage;

			IProducerPipelineFilter producerPipelineFilter;
			IConsumerPipelineFilter consumerPipelineFilter;

			Type producerPipelineFilterType;
			Type consumerPipelineFilterType;
			IDictionary<FilterConfiguration, Type> transformerPipelineFilterTypeConfigMappings;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			producerPipelineFilterType = this.RealPipelineConfiguration.ProducerFilterConfiguration.GetFilterType();
			consumerPipelineFilterType = this.RealPipelineConfiguration.ConsumerFilterConfiguration.GetFilterType();
			transformerPipelineFilterTypeConfigMappings = this.RealPipelineConfiguration.TransformerFilterConfigurations.ToDictionary(c => c, c => c.GetFilterType());

			if ((object)producerPipelineFilterType == null)
				throw new InvalidOperationException(nameof(producerPipelineFilterType));

			producerPipelineFilter = (IProducerPipelineFilter)Activator.CreateInstance(producerPipelineFilterType);

			if ((object)producerPipelineFilter == null)
				throw new InvalidOperationException(nameof(producerPipelineFilter));

			using (producerPipelineFilter)
			{
				producerPipelineFilter.FilterConfiguration = this.RealPipelineConfiguration.ProducerFilterConfiguration;
				producerPipelineFilter.Create();

				if ((object)consumerPipelineFilterType == null)
					throw new InvalidOperationException(nameof(consumerPipelineFilterType));

				consumerPipelineFilter = (IConsumerPipelineFilter)Activator.CreateInstance(consumerPipelineFilterType);

				if ((object)consumerPipelineFilter == null)
					throw new InvalidOperationException(nameof(consumerPipelineFilter));

				using (consumerPipelineFilter)
				{
					TableConfiguration tableConfiguration;

					TransformDelegate transform;
					ITransformBuilder transformBuilder;

					consumerPipelineFilter.FilterConfiguration = this.RealPipelineConfiguration.ConsumerFilterConfiguration;
					consumerPipelineFilter.Create();

					tableConfiguration = this.RealPipelineConfiguration.TableConfiguration ?? new TableConfiguration();
					producerPipelineFilter.PreProcess(pipelineContext, tableConfiguration);
					consumerPipelineFilter.PreProcess(pipelineContext, tableConfiguration);

					// --
					transformBuilder = new TransformBuilder();

					if (false)
					{
						// regular methods
						transformBuilder.Use(NullTransformerPipelineFilter.NullMiddlewareMethod);

						// local functions
						TransformDelegate _middleware(TransformDelegate next)
						{
							IPipelineMessage _filter(IPipelineContext ctx, TableConfiguration cfg, IPipelineMessage msg)
							{
								Console.WriteLine("filter_zero");
								return next(ctx, cfg, msg);
							}

							return _filter;
						}

						transformBuilder.Use(_middleware);

						// lambda expressions
						transformBuilder.Use(next =>
											{
												return (ctx, cfg, msg) =>
														{
															Console.WriteLine("filter_first");
															return next(ctx, cfg, msg);
														};
											});
					}

					foreach (KeyValuePair<FilterConfiguration, Type> transformerPipelineFilterTypeConfigMapping in transformerPipelineFilterTypeConfigMappings)
					{
						if ((object)transformerPipelineFilterTypeConfigMapping.Key == null)
							throw new InvalidOperationException(nameof(transformerPipelineFilterTypeConfigMapping.Key));

						if ((object)transformerPipelineFilterTypeConfigMapping.Value == null)
							throw new InvalidOperationException(nameof(transformerPipelineFilterTypeConfigMapping.Value));

						transformBuilder.UseMiddleware(transformerPipelineFilterTypeConfigMapping.Value, transformerPipelineFilterTypeConfigMapping.Key);
					}

					transform = transformBuilder.Build();

					// --

					pipelineMessage = producerPipelineFilter.Produce(pipelineContext, tableConfiguration);

					pipelineMessage = transform(pipelineContext, tableConfiguration, pipelineMessage);

					consumerPipelineFilter.Consume(pipelineContext, tableConfiguration, pipelineMessage);

					consumerPipelineFilter.PostProcess(pipelineContext, tableConfiguration);
					producerPipelineFilter.PostProcess(pipelineContext, tableConfiguration);

					this.__check();
				}
			}

			return 0;
		}

		public Type GetPipelineType()
		{
			return typeof(DefaultStaticPipeline);
		}

		public IReadOnlyCollection<Type> GetStaticFilterChain()
		{
			return null;
		}

		#endregion
	}

	public interface IProcessBuilder
	{
		#region Methods/Operators

		ProcessDelegate Build(bool reverse);

		IProcessBuilder New();

		IProcessBuilder Use(ProcessDelegate middleware);

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

		private readonly IList<ProcessDelegate> components = new List<ProcessDelegate>();

		#endregion

		#region Properties/Indexers/Events

		private IList<ProcessDelegate> Components
		{
			get
			{
				return this.components;
			}
		}

		#endregion

		#region Methods/Operators

		public ProcessDelegate Build(bool reverse)
		{
			void _process(IPipelineContext ctx, TableConfiguration cfg)
			{
				foreach (ProcessDelegate component in (reverse ? this.Components.Reverse() : this.Components))
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

		public IProcessBuilder Use(ProcessDelegate middleware)
		{
			this.Components.Add(middleware);
			return this;
		}

		#endregion
	}

	public static class ProcessBuilderExtensions
	{
		#region Methods/Operators

		public static IProcessBuilder UseMiddleware(this IProcessBuilder processBuilder, Type transformerPipelineFilterType, FilterConfiguration filterConfiguration)
		{
			if ((object)processBuilder == null)
				throw new ArgumentNullException(nameof(processBuilder));

			if ((object)transformerPipelineFilterType == null)
				throw new ArgumentNullException(nameof(transformerPipelineFilterType));

			if ((object)filterConfiguration == null)
				throw new ArgumentNullException(nameof(filterConfiguration));

			return processBuilder.Use((ctx, cfg) =>
									{
										ITransformerPipelineFilter transformerPipelineFilter;

										transformerPipelineFilter = (ITransformerPipelineFilter)Activator.CreateInstance(transformerPipelineFilterType);

										if ((object)transformerPipelineFilter == null)
											throw new InvalidOperationException(nameof(transformerPipelineFilter));

										using (transformerPipelineFilter)
										{
											transformerPipelineFilter.FilterConfiguration = filterConfiguration;
											transformerPipelineFilter.Create();

											transformerPipelineFilter.PreProcess(ctx, cfg);
										}
									});
		}

		#endregion
	}
}
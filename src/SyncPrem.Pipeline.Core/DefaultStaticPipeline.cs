/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configurations;
using SyncPrem.Pipeline.Abstractions.Filters.Consumer;
using SyncPrem.Pipeline.Abstractions.Filters.Producer;
using SyncPrem.Pipeline.Abstractions.Filters.Transformer;
using SyncPrem.Pipeline.Abstractions.Messages;
using SyncPrem.Pipeline.Abstractions.Pipes;
using SyncPrem.Pipeline.Core.Pipes;

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
			IDictionary<Type, FilterConfiguration> transformerPipelineFilterTypeConfigMappings;

			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			producerPipelineFilterType = this.RealPipelineConfiguration.ProducerFilterConfiguration.GetFilterType();
			consumerPipelineFilterType = this.RealPipelineConfiguration.ConsumerFilterConfiguration.GetFilterType();
			transformerPipelineFilterTypeConfigMappings = this.RealPipelineConfiguration.TransformerFilterConfigurations.ToDictionary(c => c.GetFilterType(), c => c);

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
					consumerPipelineFilter.FilterConfiguration = this.RealPipelineConfiguration.ConsumerFilterConfiguration;
					consumerPipelineFilter.Create();

					TableConfiguration tc = this.RealPipelineConfiguration.TableConfiguration ?? new TableConfiguration();
					producerPipelineFilter.PreProcess(pipelineContext, tc);
					consumerPipelineFilter.PreProcess(pipelineContext, tc);

					pipelineMessage = producerPipelineFilter.Produce(pipelineContext, tc);

					//

					TransformDelegate transform;
					ITransformBuilder transformBuilder;

					transformBuilder = new TransformBuilder();

					transformBuilder.Use(next =>
										{
											return (ctx, cfg, msg) =>
													{
														Console.WriteLine("filter1");
														return next(ctx, cfg, msg);
													};
										});

					foreach (KeyValuePair<Type, FilterConfiguration> transformerPipelineFilterType in transformerPipelineFilterTypeConfigMappings)
					{
						transformBuilder.UseMiddleware(transformerPipelineFilterType.Key, transformerPipelineFilterType.Value);
					}

					transformBuilder.Use(next =>
										{
											return (ctx, cfg, msg) =>
													{
														Console.WriteLine("filter2");
														return msg;
													};
										});

					transform = transformBuilder.Build();

					pipelineMessage = transform(pipelineContext, tc, pipelineMessage);

					//

					consumerPipelineFilter.Consume(pipelineContext, tc, pipelineMessage);

					consumerPipelineFilter.PostProcess(pipelineContext, tc);
					producerPipelineFilter.PostProcess(pipelineContext, tc);

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

		public void ReleasePipe(IPipe pipe)
		{
			if ((object)pipe == null)
				throw new ArgumentNullException(nameof(pipe));

			// TODO DI/IoC
			using (pipe)
				;
		}

		public IPipe ReservePipe()
		{
			return new DefaultPipe(); // TODO DI/IoC
		}

		#endregion
	}

	public interface ITransformBuilder
	{
		#region Methods/Operators

		TransformDelegate Build();

		ITransformBuilder New();

		ITransformBuilder Use(Func<TransformDelegate, TransformDelegate> middleware);

		#endregion
	}

	public class TransformBuilder : ITransformBuilder
	{
		#region Constructors/Destructors

		public TransformBuilder()
		{
		}

		private TransformBuilder(TransformBuilder transformBuilder)
		{
		}

		#endregion

		#region Fields/Constants

		private readonly IList<Func<TransformDelegate, TransformDelegate>> components = new List<Func<TransformDelegate, TransformDelegate>>();

		#endregion

		#region Properties/Indexers/Events

		private IList<Func<TransformDelegate, TransformDelegate>> Components
		{
			get
			{
				return this.components;
			}
		}

		#endregion

		#region Methods/Operators

		public TransformDelegate Build()
		{
			TransformDelegate transform = (ctx, cfg, msg) => msg; // simply return original message unmodified

			foreach (Func<TransformDelegate, TransformDelegate> component in this.Components.Reverse())
			{
				transform = component(transform);
			}

			return transform;
		}

		public ITransformBuilder New()
		{
			return new TransformBuilder(this);
		}

		public ITransformBuilder Use(Func<TransformDelegate, TransformDelegate> middleware)
		{
			this.Components.Add(middleware);
			return this;
		}

		#endregion
	}

	public static class TransformBuilderExtensions
	{
		#region Methods/Operators

		public static ITransformBuilder UseMiddleware(this ITransformBuilder transformBuilder, Type transformerPipelineFilterType, FilterConfiguration filterConfiguration)
		{
			if ((object)transformBuilder == null)
				throw new ArgumentNullException(nameof(transformBuilder));

			if ((object)transformerPipelineFilterType == null)
				throw new ArgumentNullException(nameof(transformerPipelineFilterType));

			if ((object)filterConfiguration == null)
				throw new ArgumentNullException(nameof(filterConfiguration));

			return transformBuilder.Use(next =>
										{
											return (ctx, cfg, msg) =>
													{
														ITransformerPipelineFilter transformerPipelineFilter;

														transformerPipelineFilter = (ITransformerPipelineFilter)Activator.CreateInstance(transformerPipelineFilterType);

														if ((object)transformerPipelineFilter == null)
															throw new InvalidOperationException(nameof(transformerPipelineFilter));

														using (transformerPipelineFilter)
														{
															transformerPipelineFilter.FilterConfiguration = filterConfiguration;
															transformerPipelineFilter.Create();

															return transformerPipelineFilter.Transform(ctx, cfg, msg, next);
														}
													};
										});
		}

		#endregion
	}
}
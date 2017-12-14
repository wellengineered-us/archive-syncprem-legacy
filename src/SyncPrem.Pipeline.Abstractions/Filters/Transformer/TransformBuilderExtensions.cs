/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configurations;
using SyncPrem.Pipeline.Abstractions.Messages;

namespace SyncPrem.Pipeline.Abstractions.Filters.Transformer
{
	public static class TransformBuilderExtensions
	{
		#region Methods/Operators

		public static ITransformBuilder UseMiddleware(this ITransformBuilder transformBuilder, ITransformerPipelineFilter transformerPipelineFilter, FilterConfiguration filterConfiguration)
		{
			if ((object)transformBuilder == null)
				throw new ArgumentNullException(nameof(transformBuilder));

			if ((object)transformerPipelineFilter == null)
				throw new ArgumentNullException(nameof(transformerPipelineFilter));

			if ((object)filterConfiguration == null)
				throw new ArgumentNullException(nameof(filterConfiguration));

			return transformBuilder.Use(next =>
										{
											return (ctx, cfg, msg) =>
													{
														ITransformerPipelineFilter _transformerPipelineFilter = transformerPipelineFilter; // prevent closure bug
														IPipelineMessage pipelineMessage;

														using (_transformerPipelineFilter)
														{
															_transformerPipelineFilter.FilterConfiguration = filterConfiguration;
															_transformerPipelineFilter.Create();

															_transformerPipelineFilter.PreProcess(ctx, cfg);
															pipelineMessage = _transformerPipelineFilter.Transform(ctx, cfg, msg, next);
															_transformerPipelineFilter.PostProcess(ctx, cfg);

															return pipelineMessage;
														}
													};
										});
		}

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
														IPipelineMessage pipelineMessage;

														transformerPipelineFilter = (ITransformerPipelineFilter)Activator.CreateInstance(transformerPipelineFilterType);

														if ((object)transformerPipelineFilter == null)
															throw new InvalidOperationException(nameof(transformerPipelineFilter));

														using (transformerPipelineFilter)
														{
															transformerPipelineFilter.FilterConfiguration = filterConfiguration;
															transformerPipelineFilter.Create();

															transformerPipelineFilter.PreProcess(ctx, cfg);
															pipelineMessage = transformerPipelineFilter.Transform(ctx, cfg, msg, next);
															transformerPipelineFilter.PostProcess(ctx, cfg);

															return pipelineMessage;
														}
													};
										});
		}

		#endregion
	}
}
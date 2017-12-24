/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configuration;
using SyncPrem.Pipeline.Abstractions.Payload;

namespace SyncPrem.Pipeline.Abstractions.Stage.Processor
{
	public static class ProcessorBuilderExtensions
	{
		#region Methods/Operators

		public static IProcessorBuilder UseMiddleware(this IProcessorBuilder processorBuilder, IProcessor processor, StageConfiguration stageConfiguration)
		{
			if ((object)processorBuilder == null)
				throw new ArgumentNullException(nameof(processorBuilder));

			if ((object)processor == null)
				throw new ArgumentNullException(nameof(processor));

			if ((object)stageConfiguration == null)
				throw new ArgumentNullException(nameof(stageConfiguration));

			return processorBuilder.Use(next =>
										{
											return (ctx, cfg, msg) =>
													{
														IProcessor _processor = processor; // prevent closure bug
														IPipelineMessage pipelineMessage;

														using (_processor)
														{
															_processor.StageConfiguration = stageConfiguration;
															_processor.Create();

															_processor.PreExecute(ctx, cfg);
															pipelineMessage = _processor.Process(ctx, cfg, msg, next);
															_processor.PostExecute(ctx, cfg);

															return pipelineMessage;
														}
													};
										});
		}

		public static IProcessorBuilder UseMiddleware(this IProcessorBuilder processorBuilder, Type processorType, StageConfiguration stageConfiguration)
		{
			if ((object)processorBuilder == null)
				throw new ArgumentNullException(nameof(processorBuilder));

			if ((object)processorType == null)
				throw new ArgumentNullException(nameof(processorType));

			if ((object)stageConfiguration == null)
				throw new ArgumentNullException(nameof(stageConfiguration));

			return processorBuilder.Use(next =>
										{
											return (ctx, cfg, msg) =>
													{
														IProcessor processor;
														IPipelineMessage pipelineMessage;

														processor = (IProcessor)Activator.CreateInstance(processorType);

														if ((object)processor == null)
															throw new InvalidOperationException(nameof(processor));

														using (processor)
														{
															processor.StageConfiguration = stageConfiguration;
															processor.Create();

															processor.PreExecute(ctx, cfg);
															pipelineMessage = processor.Process(ctx, cfg, msg, next);
															processor.PostExecute(ctx, cfg);

															return pipelineMessage;
														}
													};
										});
		}

		#endregion
	}
}
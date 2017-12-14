/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions;
using SyncPrem.Pipeline.Abstractions.Configurations;
using SyncPrem.Pipeline.Abstractions.Filters.Transformer;
using SyncPrem.Pipeline.Abstractions.Messages;

namespace SyncPrem.Pipeline.Core.Filters.Null
{
	public class NullTransformerPipelineFilter : TransformerPipelineFilter<FilterSpecificConfiguration>
	{
		#region Constructors/Destructors

		public NullTransformerPipelineFilter()
		{
		}

		#endregion

		#region Methods/Operators

		public static TransformDelegate NullMiddlewareMethod(TransformDelegate next)
		{
			TransformDelegate retval;

			System.Console.WriteLine("NullMiddlewareMethod (before GetMiddlewareChain): '{0}'", nameof(NullTransformerPipelineFilter));
			retval = TransformClosure.GetMiddlewareChain(NullTransformFilterMethod, next);
			System.Console.WriteLine("NullMiddlewareMethod (after GetMiddlewareChain): '{0}'", nameof(NullTransformerPipelineFilter));
			return retval;
		}

		private static IPipelineMessage NullTransformFilterMethod(IPipelineContext ctx, TableConfiguration cfg, IPipelineMessage msg, TransformDelegate next)
		{
			if ((object)ctx == null)
				throw new ArgumentNullException(nameof(ctx));

			if ((object)cfg == null)
				throw new ArgumentNullException(nameof(cfg));

			if ((object)msg == null)
				throw new ArgumentNullException(nameof(msg));

			System.Console.WriteLine("NullTransformFilterMethod (before next) transform: '{0}'", nameof(NullTransformerPipelineFilter));

			msg = next(ctx, cfg, msg);

			System.Console.WriteLine("NullTransformFilterMethod (after next) transform: '{0}'", nameof(NullTransformerPipelineFilter));

			return msg;
		}

		protected override void Create(bool creating)
		{
			System.Console.WriteLine("Creating transform: '{0}'", nameof(NullTransformerPipelineFilter));
			// do nothing
			base.Create(creating);
		}

		protected override void Dispose(bool disposing)
		{
			System.Console.WriteLine("Disposing transform: '{0}'", nameof(NullTransformerPipelineFilter));
			// do nothing
			base.Dispose(disposing);
		}

		protected override void PostProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			System.Console.WriteLine("PostProcessMessage transform: '{0}'", nameof(NullTransformerPipelineFilter));
		}

		protected override void PreProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			System.Console.WriteLine("PreProcessMessage transform: '{0}'", nameof(NullTransformerPipelineFilter));
		}

		protected override IPipelineMessage TransformMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration, IPipelineMessage pipelineMessage, TransformDelegate next)
		{
			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)pipelineMessage == null)
				throw new ArgumentNullException(nameof(pipelineMessage));

			System.Console.WriteLine("TransformMessage (before next) transform: '{0}'", nameof(NullTransformerPipelineFilter));

			pipelineMessage = next(pipelineContext, tableConfiguration, pipelineMessage);

			System.Console.WriteLine("TransformMessage (after next) transform: '{0}'", nameof(NullTransformerPipelineFilter));

			return pipelineMessage;
		}

		#endregion
	}
}
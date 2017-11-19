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
		}

		protected override void PreProcessMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration)
		{
			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));
		}

		protected override IPipelineMessage TransformMessage(IPipelineContext pipelineContext, TableConfiguration tableConfiguration, IPipelineMessage pipelineMessage, TransformDelegate next)
		{
			if ((object)pipelineContext == null)
				throw new ArgumentNullException(nameof(pipelineContext));

			if ((object)tableConfiguration == null)
				throw new ArgumentNullException(nameof(tableConfiguration));

			if ((object)pipelineMessage == null)
				throw new ArgumentNullException(nameof(pipelineMessage));

			return next(pipelineContext, tableConfiguration, pipelineMessage);
		}

		#endregion
	}
}
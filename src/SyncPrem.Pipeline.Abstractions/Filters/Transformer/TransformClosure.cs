/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Pipeline.Abstractions.Configurations;
using SyncPrem.Pipeline.Abstractions.Messages;

namespace SyncPrem.Pipeline.Abstractions.Filters.Transformer
{
	public sealed class TransformClosure
	{
		#region Constructors/Destructors

		private TransformClosure(TransformWithNextDelegate transformWithNext, TransformDelegate nextTransform)
		{
			if ((object)transformWithNext == null)
				throw new ArgumentNullException(nameof(transformWithNext));

			if ((object)nextTransform == null)
				throw new ArgumentNullException(nameof(nextTransform));

			this.transformWithNext = transformWithNext;
			this.nextTransform = nextTransform;
		}

		#endregion

		#region Fields/Constants

		private readonly TransformDelegate nextTransform;
		private readonly TransformWithNextDelegate transformWithNext;

		#endregion

		#region Methods/Operators

		public static TransformDelegate GetMiddlewareChain(TransformWithNextDelegate transformWithNext, TransformDelegate nextTransform)
		{
			if ((object)transformWithNext == null)
				throw new ArgumentNullException(nameof(transformWithNext));

			if ((object)nextTransform == null)
				throw new ArgumentNullException(nameof(nextTransform));

			return new TransformClosure(transformWithNext, nextTransform).Transform;
		}

		private IPipelineMessage Transform(IPipelineContext ctx, TableConfiguration cfg, IPipelineMessage msg)
		{
			Console.WriteLine("voo doo!");
			return this.transformWithNext(ctx, cfg, msg, this.nextTransform);
		}

		#endregion
	}
}
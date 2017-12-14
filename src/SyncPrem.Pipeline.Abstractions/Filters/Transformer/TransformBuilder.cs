/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace SyncPrem.Pipeline.Abstractions.Filters.Transformer
{
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
}
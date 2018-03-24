/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Pipeline.Abstractions
{
	public abstract class Component : Lifecycle, IComponent
	{
		#region Constructors/Destructors

		protected Component()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly Guid componentId = new Guid();

		#endregion

		#region Properties/Indexers/Events

		public Guid ComponentId
		{
			get
			{
				return this.componentId;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void Create(bool creating)
		{
			if (creating)
			{
				// do nothing
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// do nothing
			}
		}

		#endregion
	}
}
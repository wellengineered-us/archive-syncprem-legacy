/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace TextMetal.Middleware.Solder.Injection
{
	public abstract class DependencyResolution : IDependencyResolution
	{
		#region Constructors/Destructors

		protected DependencyResolution(DependencyLifetime dependencyLifetime)
		{
			this.dependencyLifetime = dependencyLifetime;
		}

		#endregion

		#region Fields/Constants

		private readonly DependencyLifetime dependencyLifetime;

		#endregion

		#region Properties/Indexers/Events

		public DependencyLifetime DependencyLifetime
		{
			get
			{
				return this.dependencyLifetime;
			}
		}

		#endregion

		#region Methods/Operators

		protected abstract object CoreResolve(IDependencyManager dependencyManager, Type resolutionType, string selectorKey);

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected abstract void Dispose(bool disposing);

		public object Resolve(IDependencyManager dependencyManager, Type resolutionType, string selectorKey)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			return this.CoreResolve(dependencyManager, resolutionType, selectorKey);
		}

		#endregion
	}
}
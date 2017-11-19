/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace TextMetal.Middleware.Solder.Injection
{
	public sealed class InstanceDependencyResolution : DependencyResolution
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the InstanceDependencyResolution class.
		/// </summary>
		/// <param name="instance"> The instance to use for resolution. </param>
		public InstanceDependencyResolution(object instance)
			: base(DependencyLifetime.Instance)
		{
			this.instance = instance;
		}

		#endregion

		#region Fields/Constants

		private readonly object instance;

		#endregion

		#region Properties/Indexers/Events

		public object Instance
		{
			get
			{
				return this.instance;
			}
		}

		#endregion

		#region Methods/Operators

		protected override object CoreResolve(IDependencyManager dependencyManager, Type resolutionType, string selectorKey)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			return this.Instance;
		}

		protected override void Dispose(bool disposing)
		{
			// do nothing
		}

		#endregion
	}
}
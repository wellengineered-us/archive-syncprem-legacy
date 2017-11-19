/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace TextMetal.Middleware.Solder.Injection
{
	/// <summary>
	/// A dependency resolution implementation that executes a public, default constructor
	/// on the activation type each time a dependency resolution occurs.
	/// </summary>
	public class TransientDefaultConstructorDependencyResolution : DependencyResolution

	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the TransientDefaultConstructorDependencyResolution class.
		/// </summary>
		/// <param name="activatorType"> The activator type of the resolution. </param>
		public TransientDefaultConstructorDependencyResolution(Type activatorType)
			: base(DependencyLifetime.Transient)
		{
			if ((object)activatorType == null)
				throw new ArgumentNullException(nameof(activatorType));

			this.activatorType = activatorType;
		}

		#endregion

		#region Fields/Constants

		private readonly Type activatorType;

		#endregion

		#region Properties/Indexers/Events

		private Type ActivatorType
		{
			get
			{
				return this.activatorType;
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

			return Activator.CreateInstance(this.ActivatorType);
		}

		protected override void Dispose(bool disposing)
		{
			// do nothing
		}

		#endregion
	}
}
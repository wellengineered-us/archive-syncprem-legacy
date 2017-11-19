/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace TextMetal.Middleware.Solder.Injection
{
	/// <summary>
	/// A dependency resolution implementation that executes a
	/// factory method callback each time a dependency resolution occurs.
	/// </summary>
	public sealed class TransientFactoryMethodDependencyResolution : DependencyResolution
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the TransientFactoryMethodDependencyResolution class.
		/// </summary>
		/// <param name="factoryMethod"> The callback method to execute during resolution. </param>
		public TransientFactoryMethodDependencyResolution(Delegate factoryMethod)
			: base(DependencyLifetime.Transient)
		{
			if ((object)factoryMethod == null)
				throw new ArgumentNullException(nameof(factoryMethod));

			this.factoryMethod = factoryMethod;
		}

		#endregion

		#region Fields/Constants

		private readonly Delegate factoryMethod;

		#endregion

		#region Properties/Indexers/Events

		private Delegate FactoryMethod
		{
			get
			{
				return this.factoryMethod;
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

			return this.FactoryMethod.DynamicInvoke(null);
		}

		protected override void Dispose(bool disposing)
		{
			// do nothing
		}

		#endregion
	}
}
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
	public class TransientDefaultConstructorDependencyResolution<TResolution> : DependencyResolution<TResolution>
		where TResolution : new()
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the TransientDefaultConstructorDependencyResolution`1 class.
		/// </summary>
		public TransientDefaultConstructorDependencyResolution()
			: base(DependencyLifetime.Transient)
		{
		}

		#endregion

		#region Methods/Operators

		protected override TResolution CoreResolve(IDependencyManager dependencyManager, string selectorKey)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			return new TResolution();
		}

		protected override void Dispose(bool disposing)
		{
			// do nothing
		}

		#endregion
	}
}
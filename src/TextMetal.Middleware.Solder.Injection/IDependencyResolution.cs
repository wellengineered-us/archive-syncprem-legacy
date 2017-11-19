/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace TextMetal.Middleware.Solder.Injection
{
	/// <summary>
	/// Provides a unified Strategy Pattern for dependency resolution.
	/// </summary>
	public interface IDependencyResolution : IDisposable
	{
		#region Properties/Indexers/Events

		DependencyLifetime DependencyLifetime
		{
			get;
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// Resolves a dependency.
		/// </summary>
		/// <param name="dependencyManager"> The in-effect dependency manager requesting this dependency resolution. </param>
		/// <param name="resolutionType"> The contextual resolution type for this dependency resolution. </param>
		/// <param name="selectorKey"> The contextual selector key for this dependency resolution; a non-null, zero or greater length string is required. </param>
		/// <returns> An instance of an object assignable to the resolution type or an approapriate default value (e.g. null for reference types, logical 'zero' for value types). </returns>
		object Resolve(IDependencyManager dependencyManager, Type resolutionType, string selectorKey);

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace TextMetal.Middleware.Solder.Injection
{
	public enum DependencyLifetime
	{
		/// <summary>
		/// Unknown or undefined dependency lifetime.
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Transient lifetime dependencies are created each time they are requested.
		/// </summary>
		Transient,

		/// <summary>
		/// Scoped lifetime dependencies are created once per "request".
		/// </summary>
		Scoped,

		/// <summary>
		/// Singleton lifetime dependencies are created the first time they are requested, and then every subsequent request will use the same instance. If your application requires singleton behavior, allowing the dependency manager to manage the dependency’s lifetime is recommended instead of implementing the singleton design pattern and managing your object’s lifetime in the class yourself.
		/// </summary>
		Singleton,

		/// <summary>
		/// ReflectionFascadeLegacyInstance lifetime dependencies are those for which you can choose to add an instance directly to the dependency manager. If you do so, this instance will be used for all subsequent requests (this technique will create a Singleton-scoped instance). One key difference between ReflectionFascadeLegacyInstance dependencies and Singleton dependencies is that the ReflectionFascadeLegacyInstance dependencies is created in "your code", while the Singleton service is lazy-loaded the first time it is requested.
		/// </summary>
		Instance
	}
}
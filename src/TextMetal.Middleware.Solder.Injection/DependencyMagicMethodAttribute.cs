/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace TextMetal.Middleware.Solder.Injection
{
	/// <summary>
	/// Marks a static void (IDependencyManager) method as a dependency magic method.
	/// These methods are used to register dependencies when an assembly is loaded.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public sealed class DependencyMagicMethodAttribute : Attribute
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the DependencyMagicMethodAttribute class.
		/// </summary>
		public DependencyMagicMethodAttribute()
		{
		}

		#endregion
	}
}
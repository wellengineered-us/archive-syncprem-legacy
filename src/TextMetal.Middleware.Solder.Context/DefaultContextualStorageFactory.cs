/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace TextMetal.Middleware.Solder.Context
{
	/// <summary>
	/// Manages execution path storage of objects in a manner which is safe in standard executables, libraries, ASP.NET, and WCF code.
	/// </summary>
	public class DefaultContextualStorageFactory : IContextualStorageFactory
	{
		#region Constructors/Destructors

		public DefaultContextualStorageFactory(ContextScope contextScope)
		{
			this.contextScope = contextScope;
		}

		#endregion

		#region Fields/Constants

		private readonly ContextScope contextScope;

		#endregion

		#region Properties/Indexers/Events

		public ContextScope ContextScope
		{
			get
			{
				return this.contextScope;
			}
		}

		#endregion

		#region Methods/Operators

		public IContextualStorageStrategy GetContextualStorage()
		{
			switch (this.ContextScope)
			{
				case ContextScope.GlobalStaticUnsafe:
					return new GlobalStaticContextualStorageStrategy();
				case ContextScope.LocalThreadSafe:
					return new ThreadLocalContextualStorageStrategy();
				case ContextScope.LocalAsyncSafe:
					return new AsyncLocalContextualStorageStrategy();
				case ContextScope.LocalRequestSafe:
					return new HttpContextAccessorContextualStorageStrategy(null);
				default:
					throw new ArgumentOutOfRangeException(nameof(this.ContextScope));
			}
		}

		#endregion
	}
}
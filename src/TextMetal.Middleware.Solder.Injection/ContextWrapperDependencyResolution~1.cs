/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using TextMetal.Middleware.Solder.Context;

namespace TextMetal.Middleware.Solder.Injection
{
	/// <summary>
	/// A composing dependency resolution implementation that allows only a specific instance
	/// to be lazy created on demand/first use, cached, and reused; the specific instance is
	/// retrieved from the inner dependency resolution once per the lifetime of the specified context scope.
	/// NOTE: no explicit synchronization is assumed.
	/// </summary>
	public sealed class ContextWrapperDependencyResolution<TResolution> : DependencyResolution<TResolution>
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the ContextWrapperDependencyResolution`1 class.
		/// </summary>
		/// <param name="contextScope"> The context scope being requested. </param>
		/// <param name="innerDependencyResolution"> The chained dependency resolution which is called only once. </param>
		public ContextWrapperDependencyResolution(ContextScope contextScope, IDependencyResolution<TResolution> innerDependencyResolution)
			: this(new DefaultContextualStorageFactory(contextScope).GetContextualStorage(), innerDependencyResolution)
		{
		}

		public ContextWrapperDependencyResolution(IContextualStorageStrategy contextualStorageStrategy, IDependencyResolution<TResolution> innerDependencyResolution)
			: base(DependencyLifetime.Singleton)
		{
			if ((object)contextualStorageStrategy == null)
				throw new ArgumentNullException(nameof(contextualStorageStrategy));

			if ((object)innerDependencyResolution == null)
				throw new ArgumentNullException(nameof(innerDependencyResolution));

			this.contextualStorageStrategy = contextualStorageStrategy;
			this.innerDependencyResolution = innerDependencyResolution;
		}

		#endregion

		#region Fields/Constants

		private readonly IContextualStorageStrategy contextualStorageStrategy;
		private readonly IDependencyResolution<TResolution> innerDependencyResolution;
		private readonly IList<string> trackedContextKeys = new List<string>();

		#endregion

		#region Properties/Indexers/Events

		private IContextualStorageStrategy ContextualStorageStrategy
		{
			get
			{
				return this.contextualStorageStrategy;
			}
		}

		private IDependencyResolution<TResolution> InnerDependencyResolution
		{
			get
			{
				return this.innerDependencyResolution;
			}
		}

		private IList<string> TrackedContextKeys
		{
			get
			{
				return this.trackedContextKeys;
			}
		}

		#endregion

		#region Methods/Operators

		protected override TResolution CoreResolve(IDependencyManager dependencyManager, string selectorKey)
		{
			string contextKey;
			Type resolutionType;
			TResolution value;

			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			resolutionType = typeof(TResolution);
			contextKey = string.Format("{0}_{1}", resolutionType.FullName, selectorKey);

			if (!this.ContextualStorageStrategy.HasValue(contextKey))
			{
				value = this.InnerDependencyResolution.Resolve(dependencyManager, selectorKey);
				this.ContextualStorageStrategy.SetValue<TResolution>(contextKey, value);

				this.TrackedContextKeys.Add(contextKey);
			}
			else
				value = this.ContextualStorageStrategy.GetValue<TResolution>(contextKey);

			return value;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (string trackedContextKey in this.TrackedContextKeys)
				{
					if (this.ContextualStorageStrategy.HasValue(trackedContextKey))
						this.ContextualStorageStrategy.RemoveValue(trackedContextKey);
				}

				if ((object)this.InnerDependencyResolution != null)
					this.InnerDependencyResolution.Dispose();
			}
		}

		#endregion
	}
}
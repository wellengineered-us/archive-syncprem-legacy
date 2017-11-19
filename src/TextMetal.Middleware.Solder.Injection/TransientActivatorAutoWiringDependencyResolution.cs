/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Linq;
using System.Reflection;

using TextMetal.Middleware.Solder.Utilities;

namespace TextMetal.Middleware.Solder.Injection
{
	/// <summary>
	/// A dependency resolution implementation that uses Activator.CreateInstance(...)
	/// on the activation type each time a dependency resolution occurs and is the only
	/// implementation that allows for auto-wiring using the DependencyInjectionAttribute.
	/// </summary>
	public sealed class TransientActivatorAutoWiringDependencyResolution : DependencyResolution
	{
		#region Constructors/Destructors

		public TransientActivatorAutoWiringDependencyResolution(IReflectionFascade reflectionFascade, Type activatorType)
			: base(DependencyLifetime.Transient)
		{
			if ((object)reflectionFascade == null)
				throw new ArgumentNullException(nameof(reflectionFascade));

			if ((object)activatorType == null)
				throw new ArgumentNullException(nameof(activatorType));

			this.reflectionFascade = reflectionFascade;
			this.activatorType = activatorType;
		}

		#endregion

		#region Fields/Constants

		private readonly Type activatorType;
		private readonly IReflectionFascade reflectionFascade;

		#endregion

		#region Properties/Indexers/Events

		private Type ActivatorType
		{
			get
			{
				return this.activatorType;
			}
		}

		private IReflectionFascade ReflectionFascade
		{
			get
			{
				return this.reflectionFascade;
			}
		}

		#endregion

		#region Methods/Operators

		internal static TResolution AutoWireResolve<TResolution>(IReflectionFascade reflectionFascade, Type activatorType, IDependencyManager dependencyManager, Type resolutionType, string selectorKey)
		{
			ConstructorInfo constructorInfo;
			ConstructorInfo[] constructorInfos;
			ParameterInfo[] parameterInfos;

			DependencyInjectionAttribute dependencyInjectionAttribute;
			Lazy<TResolution> lazyConstructorInvokation = null;

			if ((object)reflectionFascade == null)
				throw new ArgumentNullException(nameof(reflectionFascade));

			if ((object)activatorType == null)
				throw new ArgumentNullException(nameof(activatorType));

			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			TypeInfo _activatorTypeInfo = activatorType.GetTypeInfo();

			// get public, instance .ctors for activation type
			constructorInfos = activatorType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

			if ((object)constructorInfos != null)
			{
				for (int constructorIndex = 0; constructorIndex < constructorInfos.Length; constructorIndex++)
				{
					Lazy<object>[] lazyConstructorArguments;

					constructorInfo = constructorInfos[constructorIndex];

					// on constructor
					dependencyInjectionAttribute = reflectionFascade.GetOneAttribute<DependencyInjectionAttribute>(constructorInfo);

					if ((object)dependencyInjectionAttribute == null)
						continue;

					if (dependencyInjectionAttribute.SelectorKey != selectorKey)
						continue;

					if ((object)lazyConstructorInvokation != null)
						throw new DependencyException(string.Format("More than one constructor for activator type '{0}' specified the '{1}' with selector key '{2}'.", activatorType.FullName, nameof(DependencyInjectionAttribute), selectorKey));

					parameterInfos = constructorInfo.GetParameters();
					lazyConstructorArguments = new Lazy<object>[parameterInfos.Length];

					for (int parameterIndex = 0; parameterIndex < parameterInfos.Length; parameterIndex++)
					{
						ParameterInfo parameterInfo;
						Type parameterType;
						Lazy<object> lazyConstructorArgument;
						DependencyInjectionAttribute parameterDependencyInjectionAttribute;

						parameterInfo = parameterInfos[parameterIndex];
						parameterType = parameterInfo.ParameterType;

						// on parameter
						parameterDependencyInjectionAttribute = reflectionFascade.GetOneAttribute<DependencyInjectionAttribute>(parameterInfo);

						if ((object)parameterDependencyInjectionAttribute == null)
							throw new DependencyException(string.Format("A constructor for activator type '{0}' specifying the '{1}' with selector key '{2}' had at least one parameter missing the '{1}': index='{3}';name='{4}';type='{5}'.", activatorType.FullName, nameof(DependencyInjectionAttribute), selectorKey, parameterIndex, parameterInfo.Name, parameterInfo.ParameterType.FullName));

						lazyConstructorArgument = new Lazy<object>(() =>
																	{
																		// prevent modified closure bug
																		IDependencyManager _dependencyManager = dependencyManager;
																		Type _resolutionType = parameterType;
																		DependencyInjectionAttribute _parameterDependencyInjectionAttribute = parameterDependencyInjectionAttribute;
																		return _dependencyManager.ResolveDependency(_resolutionType, _parameterDependencyInjectionAttribute.SelectorKey, true);
																	});

						lazyConstructorArguments[parameterIndex] = lazyConstructorArgument;
					}

					lazyConstructorInvokation = new Lazy<TResolution>(() =>
																	{
																		// prevent modified closure bug
																		Type _activatorType = activatorType;
																		Lazy<object>[] _lazyConstructorArguments = lazyConstructorArguments;
																		return (TResolution)Activator.CreateInstance(_activatorType, _lazyConstructorArguments.Select(l => l.Value).ToArray());
																	});
				}
			}

			if ((object)lazyConstructorInvokation == null)
				throw new DependencyException(string.Format("Cannot find a dependency injection constructor for activator type '{0}' with selector key '{1}'.", activatorType.FullName, selectorKey));

			return lazyConstructorInvokation.Value; // lazy loads a cascading chain of Lazy's...
		}

		protected override object CoreResolve(IDependencyManager dependencyManager, Type resolutionType, string selectorKey)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			if ((object)resolutionType == null)
				throw new ArgumentNullException(nameof(resolutionType));

			if ((object)selectorKey == null)
				throw new ArgumentNullException(nameof(selectorKey));

			return AutoWireResolve<object>(this.ReflectionFascade, this.ActivatorType, dependencyManager, resolutionType, selectorKey);
		}

		protected override void Dispose(bool disposing)
		{
			// do nothing
		}

		#endregion
	}
}
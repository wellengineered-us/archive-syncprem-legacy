/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

using TextMetal.Middleware.Solder.Primitives;
using TextMetal.Middleware.Solder.Utilities;

using DependencyMagicMethod = System.Action<TextMetal.Middleware.Solder.Injection.IDependencyManager>;

namespace TextMetal.Middleware.Solder.Injection
{
	/// <summary>
	/// Serves as a logical run-time boundary for assemblies.
	/// </summary>
	public sealed class AssemblyDomain : IDisposable
	{
		#region Constructors/Destructors

		private AssemblyDomain()
			: this(AppDomain.CurrentDomain)
		{
		}

		public AssemblyDomain(AppDomain appDomain)
		{
			if ((object)appDomain == null)
				throw new ArgumentNullException(nameof(appDomain));

			this.appDomain = appDomain;

			this.Initialize();
		}

		#endregion

		#region Fields/Constants

		private const string APP_CONFIG_FILE_NAME = "appconfig.json";
		private readonly AppDomain appDomain;
		private readonly IDependencyManager dependencyManager = new DependencyManager();
		private readonly IDictionary<AssemblyName, Assembly> knownAssemblies = new Dictionary<AssemblyName, Assembly>(___.ByValueEquality.AssemblyName);
		private readonly ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
		private bool disposed;
		private bool initialized;

		#endregion

		#region Properties/Indexers/Events

		public static AssemblyDomain Default
		{
			get
			{
				AssemblyDomain instance = AssemblyDomainLazySingleton.instance;

				// the only way this is null is if this property is invoked AGAIN while the lazy singleton is being contructed
				if ((object)instance == null)
					Environment.FailFast(string.Format("Not re-entrant; property is invoked AGAIN while the lazy singleton is being contructed: '{0}.{1}'", nameof(AssemblyDomain), nameof(Default)));

				return instance;
			}
		}

		private AppDomain AppDomain
		{
			get
			{
				return this.appDomain;
			}
		}

		/// <summary>
		/// Gets the dependency manager instance associated with the current assembly domain.
		/// </summary>
		public IDependencyManager DependencyManager
		{
			get
			{
				return this.dependencyManager;
			}
		}

		private IDictionary<AssemblyName, Assembly> KnownAssemblies
		{
			get
			{
				return this.knownAssemblies;
			}
		}

		private ReaderWriterLockSlim ReaderWriterLock
		{
			get
			{
				return this.readerWriterLock;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the current instance has been disposed.
		/// </summary>
		public bool Disposed
		{
			get
			{
				return this.disposed;
			}
			private set
			{
				this.disposed = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the current instance has been initialized.
		/// </summary>
		private bool Initialized
		{
			get
			{
				return this.initialized;
			}
			set
			{
				this.initialized = value;
			}
		}

		#endregion

		#region Methods/Operators

		private static void AddTrustedDependencies(IDependencyManager dependencyManager)
		{
			IDataTypeFascade dataTypeFascade;
			IReflectionFascade reflectionFascade;
			IConfigurationRoot configurationRoot;
			IAppConfigFascade appConfigFascade;
			IAssemblyInformationFascade assemblyInformationFascade;

			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			dataTypeFascade = new DataTypeFascade();
			reflectionFascade = new ReflectionFascade(dataTypeFascade);
			configurationRoot = LoadAppConfigFile(APP_CONFIG_FILE_NAME);
			appConfigFascade = new AppConfigFascade(configurationRoot, dataTypeFascade);
			assemblyInformationFascade = new AssemblyInformationFascade(reflectionFascade, Assembly.GetEntryAssembly());

			dependencyManager.AddResolution<IConfigurationRoot>(string.Empty, false, new SingletonWrapperDependencyResolution<IConfigurationRoot>(new InstanceDependencyResolution<IConfigurationRoot>(configurationRoot)));
			dependencyManager.AddResolution<IDataTypeFascade>(string.Empty, false, new SingletonWrapperDependencyResolution<IDataTypeFascade>(new InstanceDependencyResolution<IDataTypeFascade>(dataTypeFascade)));
			dependencyManager.AddResolution<IReflectionFascade>(string.Empty, false, new SingletonWrapperDependencyResolution<IReflectionFascade>(new InstanceDependencyResolution<IReflectionFascade>(reflectionFascade)));
			dependencyManager.AddResolution<IAppConfigFascade>(string.Empty, false, new SingletonWrapperDependencyResolution<IAppConfigFascade>(new InstanceDependencyResolution<IAppConfigFascade>(appConfigFascade)));
			dependencyManager.AddResolution<IAssemblyInformationFascade>(string.Empty, false, new SingletonWrapperDependencyResolution<IAssemblyInformationFascade>(new InstanceDependencyResolution<IAssemblyInformationFascade>(assemblyInformationFascade)));
		}

		private static IConfigurationRoot LoadAppConfigFile(string appConfigFilePath)
		{
			IConfigurationBuilder configurationBuilder;
			JsonConfigurationSource configurationSource;
			IConfigurationProvider configurationProvider;
			IConfigurationRoot configurationRoot;

			if ((object)appConfigFilePath == null)
				throw new ArgumentNullException(nameof(appConfigFilePath));

			configurationBuilder = new ConfigurationBuilder();
			configurationSource = new JsonConfigurationSource() { Optional = false, Path = appConfigFilePath };
			configurationProvider = new JsonConfigurationProvider(configurationSource);
			configurationBuilder.Add(configurationSource);
			configurationRoot = configurationBuilder.Build();

			return configurationRoot;
		}

		private void ___Dispose()
		{
			OnlyWhen._DEBUG_ThenPrint(string.Format("{0}::{1} --> {2}", nameof(AssemblyDomain), nameof(this.Dispose), Environment.CurrentManagedThreadId));

			// cop a reader lock
			this.ReaderWriterLock.EnterUpgradeableReadLock();

			try
			{
				if (this.Disposed)
					return;

				// cop a writer lock
				this.ReaderWriterLock.EnterWriteLock();

				try
				{
					if ((object)this.DependencyManager != null)
						this.DependencyManager.Dispose();

					if ((object)this.AppDomain != null)
					{
						// unhook appp domain events
						this.AppDomain.AssemblyLoad -= this.AppDomain_AssemblyLoad;
						this.AppDomain.ProcessExit -= this.AppDomain_ProcessExit;

						// DO NOT DISPOSE HERE
						//this.AppDomain.Dispose();
					}
				}
				finally
				{
					// special case here since this class wil execute under multi-threaded scenarios
					GC.SuppressFinalize(this);
					this.Disposed = true;

					this.ReaderWriterLock.ExitWriteLock();
				}
			}
			finally
			{
				this.ReaderWriterLock.ExitUpgradeableReadLock();
			}
		}

		private void AppDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs e)
		{
			if ((object)sender == null)
				throw new ArgumentNullException(nameof(sender));

			if ((object)e == null)
				throw new ArgumentNullException(nameof(e));

			this.OnAssemblyLoaded(e.LoadedAssembly);
		}

		private void AppDomain_ProcessExit(object sender, EventArgs e)
		{
			this.OnProcessExit();
		}

		public /*virtual*/ void Close()
		{
			if (this.Disposed)
				return;

			this.Dispose(true);
			// special case, see comments in call chain...
			//GC.SuppressFinalize(this);

			//this.IsDisposed = true;
		}

		public void Dispose()
		{
			this.Close();
		}

		private /*protected virtual*/ void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.___Dispose();
			}
		}

		private void FireDependencyMagicMethods(Type assemblyType)
		{
			MethodInfo[] methodInfos;
			DependencyMagicMethodAttribute dependencyMagicMethodAttribute;
			DependencyMagicMethod dependencyMagicMethod;

			IReflectionFascade reflectionFascade;

			// dogfooding here ;)
			//reflectionFascade = new ReflectionFascade(new DataTypeFascade());
			reflectionFascade = this.DependencyManager.ResolveDependency<IReflectionFascade>(string.Empty, false);

			methodInfos = assemblyType.GetMethods(BindingFlags.Public | BindingFlags.Static);

			if ((object)assemblyType == null)
				throw new ArgumentNullException(nameof(assemblyType));

			if ((object)methodInfos != null)
			{
				foreach (MethodInfo methodInfo in methodInfos)
				{
					dependencyMagicMethodAttribute = reflectionFascade.GetOneAttribute<DependencyMagicMethodAttribute>(methodInfo);

					if ((object)dependencyMagicMethodAttribute == null)
						continue;

					if (!methodInfo.IsStatic)
						continue;

					if (!methodInfo.IsPublic)
						continue;

					if (methodInfo.ReturnType != typeof(void))
						continue;

					if (methodInfo.GetParameters().Count() != 1 ||
						methodInfo.GetParameters()[0].ParameterType != typeof(IDependencyManager))
						continue;

					dependencyMagicMethod = (DependencyMagicMethod)(methodInfo.CreateDelegate(typeof(DependencyMagicMethod), null /* static */));

					if ((object)dependencyMagicMethod == null)
						continue;

					// notify
					OnlyWhen._DEBUG_ThenPrint(string.Format("{1}::{0}", methodInfo.Name, methodInfo.DeclaringType.FullName));

					// execute (assuming under existing SRWL)
					dependencyMagicMethod(this.DependencyManager);
				}
			}
		}

		/// <summary>
		/// Private thread-safe method which bootstraps an assembly/dependency domain.
		/// </summary>
		private void Initialize()
		{
			IEnumerable<Assembly> assemblies;

			// cop a reader lock
			this.ReaderWriterLock.EnterUpgradeableReadLock();

			try
			{
				if (this.Initialized)
					throw new DependencyException(string.Format("This instance has already been initialized."));

				if (this.Disposed)
					throw new ObjectDisposedException(typeof(AssemblyDomain).FullName);

				// cop a writer lock
				this.ReaderWriterLock.EnterWriteLock();

				try
				{
					OnlyWhen._DEBUG_ThenPrint(string.Format("{0}::{1} --> {2}", nameof(AssemblyDomain), nameof(this.Initialize), Environment.CurrentManagedThreadId));

					// add trusted dependencies
					AddTrustedDependencies(this.DependencyManager);

					// hook app domain events
					this.AppDomain.ProcessExit += this.AppDomain_ProcessExit;
					this.AppDomain.AssemblyLoad += this.AppDomain_AssemblyLoad;

					// probe known assemblies at run-time
					assemblies = this.AppDomain.GetAssemblies();
					this.ScanAssemblies(assemblies);

					this.Initialized = true;
				}
				finally
				{
					this.ReaderWriterLock.ExitWriteLock();
				}
			}
			finally
			{
				this.ReaderWriterLock.ExitUpgradeableReadLock();
			}
		}

		public Assembly LoadAssembly(AssemblyName assemblyName)
		{
			Assembly assembly;

			if ((object)assemblyName == null)
				throw new ArgumentNullException(nameof(assemblyName));

			// cop a reader lock
			this.ReaderWriterLock.EnterUpgradeableReadLock();

			try
			{
				if (this.Disposed)
					throw new ObjectDisposedException(typeof(AssemblyDomain).FullName);

				assembly = this.AppDomain.Load(assemblyName);

				// probe explicit dynamically loaded assmblies
				if ((object)assembly != null)
					this.ScanAssembly(assembly);

				return assembly;
			}
			finally
			{
				this.ReaderWriterLock.ExitUpgradeableReadLock();
			}
		}

		private void OnAssemblyLoaded(Assembly assembly)
		{
			if ((object)assembly == null)
				throw new ArgumentNullException(nameof(assembly));

			// cop a reader lock
			this.ReaderWriterLock.EnterUpgradeableReadLock();

			try
			{
				if (this.Disposed)
					throw new ObjectDisposedException(typeof(AssemblyDomain).FullName);

				// probe implicit dynamically loaded assmblies
				if ((object)assembly != null)
					this.ScanAssembly(assembly);
			}
			finally
			{
				this.ReaderWriterLock.ExitUpgradeableReadLock();
			}
		}

		private void OnProcessExit()
		{
			// simply dispose
			this.Dispose();
		}

		/// <summary>
		/// Private method that will scan all asemblies specified to perform dependency magic.
		/// </summary>
		/// <param name="assemblies"> An enumerable of assemblies to scan for dependency magic methods. </param>
		private void ScanAssemblies(IEnumerable<Assembly> assemblies)
		{
			if ((object)assemblies != null)
			{
				foreach (Assembly assembly in assemblies)
					this.ScanAssembly(assembly);
			}
		}

		private void ScanAssembly(Assembly assembly)
		{
			AssemblyName assemblyName;

			Type[] assemblyTypes;

			if ((object)assembly == null)
				throw new ArgumentNullException(nameof(assembly));

			assemblyName = assembly.GetName();

			if (this.KnownAssemblies.ContainsKey(assemblyName))
				return;

			OnlyWhen._DEBUG_ThenPrint(string.Format("{0}.", assembly.FullName));

			// track which ones we have seen - not sure if AN is fully ==...
			this.KnownAssemblies.Add(assemblyName, assembly);

			try
			{
				assemblyTypes = assembly.ExportedTypes.ToArray();
			}
			catch (ReflectionTypeLoadException rtlex)
			{
				// is this even needed?
				assemblyTypes = rtlex.Types.Where(t => (object)t != null).ToArray();
			}

			if ((object)assemblyTypes != null)
			{
				foreach (Type assemblyType in assemblyTypes)
				{
					// TODO: in the future we could support real auto-wire via type probing
					this.FireDependencyMagicMethods(assemblyType);
				}
			}
		}

		#endregion

		#region Classes/Structs/Interfaces/Enums/Delegates

		/// <summary>
		/// From: Microsoft.VisualStudio.Composition
		/// https://github.com/Microsoft/vs-mef/tree/master/src/Microsoft.VisualStudio.Composition
		/// Copyright (c) Microsoft Corporation, All rights reserved.
		/// License: MIT License
		/// Commit: 44cf565e2f9799bcf7f33f8af76115134c074324
		/// </summary>
		private static class ___
		{
			#region Classes/Structs/Interfaces/Enums/Delegates

			internal static partial class ByValueEquality
			{
				#region Properties/Indexers/Events

				internal static IEqualityComparer<byte[]> Buffer
				{
					get
					{
						return BufferComparer.Default;
					}
				}

				#endregion

				#region Classes/Structs/Interfaces/Enums/Delegates

				private class BufferComparer : IEqualityComparer<byte[]>
				{
					#region Constructors/Destructors

					private BufferComparer()
					{
					}

					#endregion

					#region Fields/Constants

					internal static readonly BufferComparer Default = new BufferComparer();

					#endregion

					#region Methods/Operators

					public bool Equals(byte[] x, byte[] y)
					{
						if (x == y)
						{
							return true;
						}

						if (x == null ^ y == null)
						{
							return false;
						}

						if (x.Length != y.Length)
						{
							return false;
						}

						for (int i = 0; i < x.Length; i++)
						{
							if (x[i] != y[i])
							{
								return false;
							}
						}

						return true;
					}

					public int GetHashCode(byte[] obj)
					{
						throw new NotImplementedException();
					}

					#endregion
				}

				#endregion
			}

			internal static partial class ByValueEquality
			{
				#region Properties/Indexers/Events

				internal static IEqualityComparer<AssemblyName> AssemblyName
				{
					get
					{
						return AssemblyNameComparer.Default;
					}
				}

				internal static IEqualityComparer<AssemblyName> AssemblyNameNoFastCheck
				{
					get
					{
						return AssemblyNameComparer.NoFastCheck;
					}
				}

				#endregion

				#region Classes/Structs/Interfaces/Enums/Delegates

				private class AssemblyNameComparer : IEqualityComparer<AssemblyName>
				{
					#region Constructors/Destructors

					internal AssemblyNameComparer(bool fastCheck = true)
					{
						this.fastCheck = fastCheck;
					}

					#endregion

					#region Fields/Constants

					internal static readonly AssemblyNameComparer Default = new AssemblyNameComparer();
					internal static readonly AssemblyNameComparer NoFastCheck = new AssemblyNameComparer(fastCheck: false);
					private bool fastCheck;

					#endregion

					#region Methods/Operators

					public bool Equals(AssemblyName x, AssemblyName y)
					{
						if (x == null ^ y == null)
						{
							return false;
						}

						if (x == null)
						{
							return true;
						}

#if NET45 // If fast check is enabled, we can compare the code bases
                if (this.fastCheck && x.CodeBase == y.CodeBase)
                {
                    return true;
                }
#endif

						// There are some cases where two AssemblyNames who are otherwise equivalent
						// have a null PublicKey but a correct PublicKeyToken, and vice versa. We should
						// compare the PublicKeys first, but then fall back to GetPublicKeyToken(), which
						// will generate a public key token for the AssemblyName that has a public key and
						// return the public key token for the other AssemblyName.
						byte[] xPublicKey = x.GetPublicKey();
						byte[] yPublicKey = y.GetPublicKey();

						// Testing on FullName is horrifically slow.
						// So test directly on its components instead.
						if (xPublicKey != null && yPublicKey != null)
						{
							return x.Name == y.Name
									&& x.Version.Equals(y.Version)
									&& string.Equals(x.CultureName, y.CultureName)
									&& Buffer.Equals(xPublicKey, yPublicKey);
						}

						return x.Name == y.Name
								&& x.Version.Equals(y.Version)
								&& string.Equals(x.CultureName, y.CultureName)
								&& Buffer.Equals(x.GetPublicKeyToken(), y.GetPublicKeyToken());
					}

					public int GetHashCode(AssemblyName obj)
					{
						return obj.Name.GetHashCode();
					}

					#endregion
				}

				#endregion
			}

			#endregion
		}

		/// <summary>
		/// http://www.yoda.arachsys.com/csharp/singleton.html
		/// </summary>
		private class AssemblyDomainLazySingleton
		{
			#region Constructors/Destructors

			/// <summary>
			/// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
			/// </summary>
			static AssemblyDomainLazySingleton()
			{
			}

			#endregion

			#region Fields/Constants

			internal static readonly AssemblyDomain instance = new AssemblyDomain();

			#endregion
		}

		#endregion
	}
}
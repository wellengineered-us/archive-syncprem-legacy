/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using Castle.DynamicProxy;

namespace TextMetal.Middleware.Solder.Interception
{
	/// <summary>
	/// Represents a dynamic run-time interception.
	/// </summary>
	public abstract class RuntimeInterception : IRuntimeInterception
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the RuntimeInterception class.
		/// </summary>
		protected RuntimeInterception()
		{
		}

		#endregion

		#region Fields/Constants

		private bool disposed;

		#endregion

		#region Properties/Indexers/Events

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

		#endregion

		#region Methods/Operators

		public static TTarget CreateProxy<TTarget>(IRuntimeInterception[] runtimeInterceptionChain)
		{
			Type targetType;

			if ((object)runtimeInterceptionChain == null)
				throw new ArgumentNullException(nameof(runtimeInterceptionChain));

			targetType = typeof(TTarget);

			return (TTarget)CreateProxy(targetType, runtimeInterceptionChain);
		}

		public static object CreateProxy(Type targetType, IRuntimeInterception[] runtimeInterceptionChain)
		{
			Type proxyType;
			object proxyInstance;
			object[] activationArgs;
			IProxyBuilder proxyBuilder;
			IInterceptor interceptor;

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)runtimeInterceptionChain == null)
				throw new ArgumentNullException(nameof(runtimeInterceptionChain));

			proxyBuilder = new DefaultProxyBuilder();
			proxyType = proxyBuilder.CreateInterfaceProxyTypeWithoutTarget(targetType, new Type[] { }, ProxyGenerationOptions.Default);
			interceptor = new __CastleToSolderInterceptor(runtimeInterceptionChain);
			activationArgs = new object[] { new IInterceptor[] { interceptor }, new object() };
			proxyInstance = Activator.CreateInstance(proxyType, activationArgs);
			return proxyInstance;
		}

		public virtual void Close()
		{
			if (this.Disposed)
				return;

			this.Dispose(true);
			GC.SuppressFinalize(this);

			this.Disposed = true;
		}

		public void Dispose()
		{
			this.Close();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// do nothing
			}
		}

		/// <summary>
		/// Represents a discrete run-time invocation.
		/// </summary>
		/// <param name="runtimeInvocation"> </param>
		/// <param name="runtimeContext"> </param>
		public void Invoke(IRuntimeInvocation runtimeInvocation, IRuntimeContext runtimeContext)
		{
			if ((object)runtimeInvocation == null)
				throw new ArgumentNullException(nameof(runtimeInvocation));

			if ((object)runtimeInvocation == null)
				throw new ArgumentNullException(nameof(runtimeContext));

			this.OnInvoke(runtimeInvocation, runtimeContext);
		}

		protected virtual void OnAfterInvoke(bool proceedWithInvocation, IRuntimeInvocation runtimeInvocation, ref Exception thrownException)
		{
			if ((object)runtimeInvocation == null)
				throw new ArgumentNullException(nameof(runtimeInvocation));
		}

		protected virtual void OnBeforeInvoke(IRuntimeInvocation runtimeInvocation, out bool proceedWithInvocation)
		{
			proceedWithInvocation = true;
		}

		protected virtual void OnInvoke(IRuntimeInvocation runtimeInvocation, IRuntimeContext runtimeContext)
		{
			Exception thrownException = null;
			bool proceedWithInvocation;

			if ((object)runtimeInvocation == null)
				throw new ArgumentNullException(nameof(runtimeInvocation));

			if ((object)runtimeContext == null)
				throw new ArgumentNullException(nameof(runtimeContext));

			if (!((object)runtimeInvocation.TargetMethod != null &&
				runtimeInvocation.TargetMethod.DeclaringType == typeof(IDisposable)) &&
				this.Disposed) // always forward dispose invocations
				throw new ObjectDisposedException(typeof(RuntimeInterception).FullName);

			this.OnBeforeInvoke(runtimeInvocation, out proceedWithInvocation);

			if (proceedWithInvocation)
				this.OnProceedInvoke(runtimeInvocation, out thrownException);

			this.OnAfterInvoke(proceedWithInvocation, runtimeInvocation, ref thrownException);

			if ((object)thrownException != null)
			{
				runtimeContext.AbortInterceptionChain();
				throw thrownException;
			}
		}

		protected virtual void OnMagicalSpellInvoke(IRuntimeInvocation runtimeInvocation)
		{
			if ((object)runtimeInvocation.WrappedInstance != null)
				runtimeInvocation.InvocationReturnValue = runtimeInvocation.TargetMethod.Invoke(runtimeInvocation.WrappedInstance, runtimeInvocation.InvocationArguments);
		}

		protected virtual void OnProceedInvoke(IRuntimeInvocation runtimeInvocation, out Exception thrownException)
		{
			if ((object)runtimeInvocation == null)
				throw new ArgumentNullException(nameof(runtimeInvocation));

			try
			{
				thrownException = null;

				this.OnMagicalSpellInvoke(runtimeInvocation);
			}
			catch (Exception ex)
			{
				thrownException = ex;
			}
		}

		#endregion

		#region Classes/Structs/Interfaces/Enums/Delegates

		internal class __CastleToSolderInterceptor : IInterceptor
		{
			#region Constructors/Destructors

			public __CastleToSolderInterceptor(IRuntimeInterception[] runtimeInterceptionChain)
			{
				if ((object)runtimeInterceptionChain == null)
					throw new ArgumentNullException(nameof(runtimeInterceptionChain));

				this.runtimeInterceptionChain = runtimeInterceptionChain;
			}

			#endregion

			#region Fields/Constants

			private readonly IRuntimeInterception[] runtimeInterceptionChain;

			#endregion

			#region Properties/Indexers/Events

			private IRuntimeInterception[] RuntimeInterceptionChain
			{
				get
				{
					return this.runtimeInterceptionChain;
				}
			}

			#endregion

			#region Methods/Operators

			public void Intercept(IInvocation invocation)
			{
				RuntimeContext runtimeContext;
				IRuntimeInvocation runtimeInvocation;
				IRuntimeInterception runtimeInterception;

				if ((object)invocation == null)
					throw new ArgumentNullException(nameof(invocation));

				runtimeContext = new RuntimeContext();
				runtimeContext.ContinueInterception = true;

				runtimeInvocation = new RuntimeInvocation(invocation.Proxy, /*invocation.TargetType*/ invocation.Method.DeclaringType, invocation.Method, invocation.Arguments, invocation.InvocationTarget);

				for (int index = 0; index < this.RuntimeInterceptionChain.Length && runtimeContext.ContinueInterception; index++)
				{
					runtimeContext.InterceptionCount = this.RuntimeInterceptionChain.Length + 1;
					runtimeContext.InterceptionIndex = index;

					runtimeInterception = this.RuntimeInterceptionChain[index];
					runtimeInterception.Invoke(runtimeInvocation, runtimeContext);
				}
			}

			#endregion
		}

		#endregion
	}
}
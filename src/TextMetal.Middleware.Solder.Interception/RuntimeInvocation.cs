/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Reflection;

namespace TextMetal.Middleware.Solder.Interception
{
	public sealed class RuntimeInvocation : IRuntimeInvocation
	{
		#region Constructors/Destructors

		public RuntimeInvocation(object proxyInstance, Type targetType, MethodInfo targetMethod, object[] invocationArguments, object wrappedInstance)
		{
			if ((object)proxyInstance == null)
				throw new ArgumentNullException(nameof(proxyInstance));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)targetMethod == null)
				throw new ArgumentNullException(nameof(targetMethod));

			if ((object)invocationArguments == null)
				throw new ArgumentNullException(nameof(invocationArguments));

			//if ((object)wrappedInstance == null)
			//throw new ArgumentNullException(nameof(wrappedInstance));

			this.proxyInstance = proxyInstance;
			this.targetType = targetType;
			this.targetMethod = targetMethod;
			this.invocationArguments = invocationArguments;
			this.wrappedInstance = wrappedInstance;
		}

		#endregion

		#region Fields/Constants

		private readonly object[] invocationArguments;
		private readonly object proxyInstance;
		private readonly MethodInfo targetMethod;
		private readonly Type targetType;
		private readonly object wrappedInstance;
		private bool disposed;
		private object invocationReturnValue;

		#endregion

		#region Properties/Indexers/Events

		/// <summary>
		/// Gets the arguments passed to the invoked method, if appliable.
		/// </summary>
		public object[] InvocationArguments
		{
			get
			{
				return this.invocationArguments;
			}
		}

		/// <summary>
		/// Gets the proxy object instance.
		/// </summary>
		public object ProxyInstance
		{
			get
			{
				return this.proxyInstance;
			}
		}

		/// <summary>
		/// Gets the MethodInfo of the target method (or property underlying method).
		/// </summary>
		public MethodInfo TargetMethod
		{
			get
			{
				return this.targetMethod;
			}
		}

		/// <summary>
		/// Gets the run-time type of the target type (may differ from MethodInfo.DeclaringType).
		/// </summary>
		public Type TargetType
		{
			get
			{
				return this.targetType;
			}
		}

		/// <summary>
		/// Gets the wrapped object instance or null if there is none.
		/// </summary>
		public object WrappedInstance
		{
			get
			{
				return this.wrappedInstance;
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
		/// Gets or sets the return value of the invoked method, if appliable.
		/// </summary>
		public object InvocationReturnValue
		{
			get
			{
				return this.invocationReturnValue;
			}
			set
			{
				this.invocationReturnValue = value;
			}
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if (this.Disposed)
				return;

			try
			{
			}
			finally
			{
				this.Disposed = true;
				GC.SuppressFinalize(this);
			}
		}

		#endregion
	}
}
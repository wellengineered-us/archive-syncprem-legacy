/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Reflection;

namespace TextMetal.Middleware.Solder.Interception
{
	/// <summary>
	/// Represents a run-time invocation.
	/// </summary>
	public interface IRuntimeInvocation : IDisposable
	{
		#region Properties/Indexers/Events

		/// <summary>
		/// Gets the arguments passed to the invoked method, if appliable.
		/// </summary>
		object[] InvocationArguments
		{
			get;
		}

		/// <summary>
		/// Gets the proxy object instance.
		/// </summary>
		object ProxyInstance
		{
			get;
		}

		/// <summary>
		/// Gets the MethodInfo of the target method (or property underlying method).
		/// </summary>
		MethodInfo TargetMethod
		{
			get;
		}

		/// <summary>
		/// Gets the run-time type of the target type (may differ from MethodInfo.DeclaringType).
		/// </summary>
		Type TargetType
		{
			get;
		}

		/// <summary>
		/// Gets the wrapped object instance or null if there is none.
		/// </summary>
		object WrappedInstance
		{
			get;
		}

		/// <summary>
		/// Gets or sets the return value of the invoked method, if appliable.
		/// </summary>
		object InvocationReturnValue
		{
			get;
			set;
		}

		#endregion
	}
}
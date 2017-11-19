/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace TextMetal.Middleware.Solder.Interception
{
	/// <summary>
	/// Represents a run-time interception.
	/// </summary>
	public interface IRuntimeInterception : IDisposable
	{
		#region Methods/Operators

		/// <summary>
		/// Represents a discrete run-time invocation.
		/// </summary>
		/// <param name="runtimeInvocation"> </param>
		/// <param name="runtimeContext"> </param>
		void Invoke(IRuntimeInvocation runtimeInvocation, IRuntimeContext runtimeContext);

		#endregion
	}
}
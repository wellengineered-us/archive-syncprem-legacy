#region Using

using System;
using NMock.Syntax;

#endregion

namespace NMock
{
	/// <summary>
	/// 
	/// </summary>
	public class DelegateInvoker : EventInvokerBase
	{
		internal DelegateInvoker(string eventName, IActionSyntax expectation)
			: base(eventName, expectation, true)
		{
		}

		/// <summary>
		/// Invokes the delegate with the specified parameters.
		/// </summary>
		/// <exception cref="InvalidOperationException"/>
		//[DebuggerStepThrough]
		public void Invoke(params object[] args)
		{
			RaiseEvent(args);
		}
	}
}
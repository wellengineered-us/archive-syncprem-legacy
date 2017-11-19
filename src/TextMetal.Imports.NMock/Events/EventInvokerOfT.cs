#region Using

using System;
using System.Diagnostics;
using NMock.Syntax;

#endregion

namespace NMock
{
	/// <summary>
	/// Can mock invoke any event it is feed with.
	/// </summary>
	/// <remarks>
	/// Code by Magnus Mårtensson
	/// http://blog.noop.se/archive/2007/08/17.aspx
	/// </remarks>
	public class EventInvoker<TEventArgs> : EventInvokerBase where TEventArgs : EventArgs
	{
		internal EventInvoker(string eventName, IActionSyntax expectation)
			: base(eventName, expectation, false)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		[DebuggerStepThrough]
		public void Invoke(TEventArgs e)
		{
			Invoke(null, e);
		}

		/// <summary>
		/// Invoke the event and send in parameter.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The arguments of the call.</param>
		[DebuggerStepThrough]
		public void Invoke(object sender, TEventArgs e)
		{
			RaiseEvent(sender, e);
		}
	}
}
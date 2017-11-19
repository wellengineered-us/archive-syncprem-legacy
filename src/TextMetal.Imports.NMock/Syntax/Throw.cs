#region Using

using System;
using NMock.Actions;

#endregion

namespace NMock
{
	/// <summary>
	/// Defines action for throwing actions.
	/// </summary>
	public static class Throw
	{
		/// <summary>
		/// Throws an exeception when the action is invoked.
		/// </summary>
		/// <param name="exception">The exception to throw when invoked.</param>
		/// <returns>Returns a new instance of the <see cref="ThrowAction"/> class.</returns>
		public static IAction Exception(Exception exception)
		{
			return new ThrowAction(exception);
		}
	}
}
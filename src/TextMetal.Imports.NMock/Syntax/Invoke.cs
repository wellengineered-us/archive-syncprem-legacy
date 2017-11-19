using System;
using NMock.Actions;

namespace NMock.Syntax
{
	/// <summary>
	/// A syntax class to create an <see cref="InvokeAction"/> in a easy to read way.
	/// </summary>
	public static class Invoke
	{
		/// <summary>
		/// A syntax method to create an <see cref="InvokeAction"/> class.
		/// </summary>
		/// <param name="action">The <see cref="Action"/> to invoke when the <see cref="IAction"/> in invoked.</param>
		/// <returns>An instance of an <see cref="InvokeAction"/>class.</returns>
		public static IAction Action(Action action)
		{
			return new InvokeAction(action);
		}
	}
}

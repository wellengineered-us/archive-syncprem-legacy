using System;
using System.IO;
using NMock.Monitoring;

namespace NMock.Actions
{
	/// <summary>
	/// An <see cref="IAction"/> that can invoke an <see cref="Action"/> when the expectation is met.
	/// </summary>
	public class InvokeAction : IAction
	{
		private readonly Action _action;

		/// <summary>
		/// Creates an <see cref="IAction"/> that will invoke the <paramref name="action"/>.
		/// </summary>
		/// <param name="action">The action to invoke.</param>
		public InvokeAction(Action action)
		{
			_action = action;
		}

		void IAction.Invoke(Invocation invocation)
		{
			_action();
		}

		void ISelfDescribing.DescribeTo(TextWriter writer)
		{
			writer.Write("Invokes the action supplied to the constructor.");
		}
	}
}

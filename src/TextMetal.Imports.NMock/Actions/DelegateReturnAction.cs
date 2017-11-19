using System;
using System.IO;
using NMock.Monitoring;

namespace NMock.Actions
{
	/// <summary>
	/// Represents an <see cref="IReturnAction"/> that can call a delegate to provide the return value.
	/// </summary>
	/// <typeparam name="T">The type to return.</typeparam>
	public class DelegateReturnAction<T> : IReturnAction
	{
		private readonly Func<T> _func;
		private readonly Func<Invocation, T> _invokeFunc;

		/// <summary>
		/// Creates an instance of this class with a <see cref="Func{T}"/> to call during invocation.
		/// </summary>
		/// <param name="func">The <see cref="Func{T}"/> to invoke.</param>
		public DelegateReturnAction(Func<T> func)
		{
			_func = func;
		}

		/// <summary>
		/// Creates an instance of this class with a <see cref="Func{T}"/> to call during invocation.
		/// </summary>
		/// <param name="func">The <see cref="Func{T}"/> to invoke.</param>
		public DelegateReturnAction(Func<Invocation, T> func)
		{
			_invokeFunc = func;
		}

		void ISelfDescribing.DescribeTo(TextWriter writer)
		{
			writer.Write("returns a value from a Func");
		}

		void IAction.Invoke(Invocation invocation)
		{
			if (_invokeFunc != null)
				invocation.Result = _invokeFunc(invocation);
			else
				invocation.Result = _func;
		}

		Type IReturnAction.ReturnType
		{
			get { return typeof(T); }
		}
	}
}

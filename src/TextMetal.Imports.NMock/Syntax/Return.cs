#region Using

using System;
using System.Collections.Generic;
using NMock.Actions;
using NMock.Monitoring;

#endregion

namespace NMock
{
	/// <summary>
	/// Provides shortcuts to <see cref="IAction"/>s that return values
	/// </summary>
	public static class Return
	{
		/// <summary>
		/// Returns the specified value as an object.
		/// </summary>
		/// <param name="result">The value to return.</param>
		/// <returns>An action that returns the specified value.</returns>
		public static IAction Value(object result)
		{
			return new ReturnAction(result);
		}

		/// <summary>
		/// Returns the specified value as a strongly-typed value.
		/// </summary>
		/// <param name="result">The value to return.</param>
		/// <returns>An action that returns the specified value.</returns>
		public static IAction Value<T>(T result)
		{
			return new ReturnAction<T>(result);
		}

		/// <summary>
		/// Calls a method that will provide the return value.
		/// </summary>
		/// <typeparam name="T">The type that needs to be returned.</typeparam>
		/// <param name="delegate">The method that will provide the return value. (anonymous or otherwise)</param>
		/// <returns>An action that will provide the return value.</returns>
		public static IAction Value<T>(Func<T> @delegate)
		{
			return new DelegateReturnAction<T>(@delegate);
		}

		/// <summary>
		/// Calls a method that will provide the return value.
		/// </summary>
		/// <typeparam name="T">The type that needs to be returned.</typeparam>
		/// <param name="delegate">The method that will provide the return value. (anonymous or otherwise)</param>
		/// <returns>An action that will provide the return value.</returns>
		public static IAction Value<T>(Func<Invocation, T> @delegate)
		{
			return new DelegateReturnAction<T>(@delegate);
		}

		/// <summary>
		/// Specifies a queue of return values to be used for each call to the expectation.
		/// </summary>
		/// <typeparam name="T">The type that needs to be returned.</typeparam>
		/// <param name="queue">The <see cref="Queue{T}"/> of items to be returned.</param>
		/// <returns>An action that will return one value each time the expectation is met.</returns>
		public static IAction Queue<T>(Queue<T> queue)
		{
			return new QueueAction<T>(queue);
		}

#if !SILVERLIGHT
		/// <summary>
		/// Returns a clone as method return value.
		/// </summary>
		/// <param name="prototype">The prototype to clone.</param>
		/// <returns>Action defining the return value of a method.</returns>
		public static IAction CloneOf(ICloneable prototype)
		{
			return new ReturnCloneAction(prototype);
		}
#endif

		/// <summary>
		/// Defines the value returned by an out parameter.
		/// </summary>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <param name="value">The value to return.</param>
		/// <returns>Action defining the value of an out parameter.</returns>
		public static IAction OutValue(string parameterName, object value)
		{
			return new SetNamedParameterAction(parameterName, value);
		}

		/// <summary>
		/// Defines the value returned by an out parameter.
		/// </summary>
		/// <param name="parameterIndex">Index of the parameter.</param>
		/// <param name="value">The value to return.</param>
		/// <returns>Action defining the value of an out parameter.</returns>
		public static IAction OutValue(int parameterIndex, object value)
		{
			return new SetIndexedParameterAction(parameterIndex, value);
		}
	}
}
#region Using

using System.Threading;
using NMock.Actions;

#endregion

namespace NMock
{
	/// <summary>
	/// Defines that an <see cref="EventWaitHandle"/> should be signaled.
	/// </summary>
	public static class Signal
	{
		/// <summary>
		/// Signals an <see cref="EventWaitHandle"/> to synchronizes threads.
		/// </summary>
		/// <param name="signal">The signal to set.</param>
		/// <returns>Action that signals an <see cref="EventWaitHandle"/>.</returns>
		public static IAction EventWaitHandle(EventWaitHandle signal)
		{
			return new SignalAction(signal);
		}
	}
}
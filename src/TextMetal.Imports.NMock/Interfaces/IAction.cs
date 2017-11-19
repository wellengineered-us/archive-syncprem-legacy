#region Using

using NMock.Monitoring;

#endregion

namespace NMock
{
	/// <summary>
	/// An action defines something that has to be done.
	/// For example the action to return a result value.
	/// </summary>
	public interface IAction : ISelfDescribing
	{
		/// <summary>
		/// Invokes this object.
		/// </summary>
		/// <param name="invocation">The invocation.</param>
		void Invoke(Invocation invocation);
	}
}
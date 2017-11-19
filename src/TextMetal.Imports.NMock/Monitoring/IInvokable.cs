namespace NMock.Monitoring
{
	/// <summary>
	/// IInvokable objects can be invoked.
	/// </summary>
	public interface IInvokable
	{
		/// <summary>
		/// Invokes this object.
		/// </summary>
		/// <param name="invocation">The invocation.</param>
		void Invoke(Invocation invocation);
	}
}
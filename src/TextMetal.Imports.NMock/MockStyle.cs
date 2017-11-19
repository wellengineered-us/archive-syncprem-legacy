namespace NMock
{
	/// <summary>
	/// Specifies how a mock object should behave when it is first created.
	/// </summary>
	public enum MockStyle
	{
		/// <summary>
		/// Calls to members that do not have expectations set will
		/// result in ExpectationExceptions.
		/// </summary>
		Default = 0,

		/// <summary>
		/// Calls to members that do not have expectations set will
		/// pass through to the underlying implementation on the class
		/// being mocked.
		/// </summary>
		Transparent = 1,

		/// <summary>
		/// Calls to members that do not have expectations set will
		/// be ignored. Default values are used for return values 
		/// (default value of the return type, stub or empty enumerable)
		/// and the same value is returned on every call to the same member.
		/// </summary>
		Stub = 2,

		/// <summary>
		/// Calls to members that do not have expectations set will
		/// be ignored. Default values are used for return values 
		/// (default value of the return type, stub or empty enumerable)
		/// and the same value is returned on every call to the same member.
		/// </summary>
		RecursiveStub = 3,
	}
}
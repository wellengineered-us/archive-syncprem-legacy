#region Using

using NMock.Internal;

#endregion

namespace NMock.Proxy
{
	/// <summary>
	/// Implementations of this interface are responsible for generating runtime
	/// proxies of classes and interfaces for use as mock objects.
	/// </summary>
	/// <remarks>
	/// Returned instances are expected to implement IMockObject and take care of
	/// intercepting calls to their public members. Intercepted calls should be
	/// forwarded on to the supplied MockFactory for processing against expectations.
	/// </remarks>
	public interface IMockObjectFactory
	{
		/// <summary>
		/// Creates a mock of the specified type(s).
		/// </summary>
		/// <param name="mockFactory">The mockFactory used to create this mock instance.</param>
		/// <param name="typesToMock">The type(s) to include in the mock.</param>
		/// <param name="name">The name to use for the mock instance.</param>
		/// <param name="mockStyle">The behaviour of the mock instance when first created.</param>
		/// <param name="constructorArgs">Constructor arguments for the class to be mocked. Only valid if mocking a class type.</param>
		/// <returns>A mock instance of the specified type(s).</returns>
		object CreateMock(MockFactory mockFactory, CompositeType typesToMock, string name, MockStyle mockStyle, object[] constructorArgs);
	}
}
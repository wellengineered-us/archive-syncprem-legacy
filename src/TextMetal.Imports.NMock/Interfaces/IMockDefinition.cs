#region Using

using System;
using System.ComponentModel;
using NMock.Proxy;

#endregion

namespace NMock
{
	/// <summary>
	/// The definition of a mock object.
	/// </summary>
	/// <remarks>
	/// Implementations of this interface encapsulate the details of
	/// how a mock object is defined, and provide the ability to be able to
	/// instantiate an instance of it.
	/// </remarks>
	public interface IMockDefinition
	{
		/// <summary>
		/// This method supports NMock infrastructure and is not intended to be called directly from your code.
		/// </summary>
		/// <param name="primaryType">The primary type that is being mocked.</param>
		/// <param name="mockFactory">The current <see cref="MockFactory"/> instance.</param>
		/// <param name="mockObjectFactory">An <see cref="IMockObjectFactory"/> to use when creating the mock.</param>
		/// <returns>A new mock instance.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		object Create(Type primaryType, MockFactory mockFactory, IMockObjectFactory mockObjectFactory);
	}
}
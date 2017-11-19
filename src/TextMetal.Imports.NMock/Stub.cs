#region Using

using System;
using NMock.Internal;
using NMock.Syntax;

#endregion

/* ^ */ using System.Reflection; /* dpbullington@gmail.com ^ */

namespace NMock
{
	/// <summary>
	/// This class represents a stub.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Stub<T>
		where T : class
	{
		private readonly object _proxy;

		internal Stub(object proxy)
		{
			if (proxy == null)
				throw new ArgumentNullException("proxy");

			_proxy = proxy;
		}

		/// <summary>
		/// A syntax property used to stub out data for this instance.
		/// </summary>
		public IStubSyntax<T> Out
		{
			get
			{
				/* ^ */ var _typeInfo = typeof(T).GetTypeInfo(); /* dpbullington@gmail.com ^ */

				if (/* ^ */ _typeInfo.IsInterface || _typeInfo.IsClass /* dpbullington@gmail.com ^ */)
					return new ExpectationBuilder<T>(ExpectationBuilder.STUB_DESCRIPTION, Is.AtLeast(0), Is.AtLeast(0), _proxy) { IsStub = true };

				throw new InvalidOperationException("The type mocked is not a class or interface.");
			}
		}
	}
}
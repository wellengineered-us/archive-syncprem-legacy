#region Using

using System;
using System.Collections.Generic;

#endregion

namespace NMock.Internal
{
	/// <summary>
	/// Provides functionality to map stubs and specific types of a stub to mock styles.
	/// </summary>
	internal class StubMockStyleDictionary
	{
		/// <summary>
		/// holds mappings from stub to mock style (holds for all types unless there is a mapping defined in <see cref="mockStyleForType"/>.
		/// </summary>
		private readonly Dictionary<IMockObject, MockStyle?> mockStyleForStub = new Dictionary<IMockObject, MockStyle?>();

		/// <summary>
		/// holds mappings from stub.type to mock style.
		/// </summary>
		private readonly Dictionary<Key, MockStyle?> mockStyleForType = new Dictionary<Key, MockStyle?>();

		/// <summary>
		/// Gets or sets the mock style for the specified mock.
		/// </summary>
		/// <param name="mock">the mock object</param>
		/// <value>mock style. null if no value defined.</value>
		public MockStyle? this[IMockObject mock]
		{
			get
			{
				return mockStyleForStub.ContainsKey(mock) ? mockStyleForStub[mock] : null;
			}

			set
			{
				mockStyleForStub[mock] = value;
			}
		}

		/// <summary>
		/// Gets or sets the mock style for the specified mock and type.
		/// </summary>
		/// <param name="mock">the mock object</param>
		/// <param name="nestedMockType">the type of the nested mock.</param>
		/// <value>mock style. null if no value defined.</value>
		public MockStyle? this[IMockObject mock, Type nestedMockType]
		{
			get
			{
				Key key = new Key(mock, nestedMockType);

				if (mockStyleForType.ContainsKey(key))
				{
					return mockStyleForType[key] ?? mockStyleForStub[mock];
				}

				return this[mock];
			}

			set
			{
				Key key = new Key(mock, nestedMockType);

				mockStyleForType[key] = value;
			}
		}

		#region Nested type: Key

		/// <summary>
		/// Key into the <see cref="StubMockStyleDictionary.mockStyleForType"/> dictionary.
		/// </summary>
		private class Key
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="Key"/> class.
			/// </summary>
			/// <param name="mock">The mock object.</param>
			/// <param name="nestedMockType">Type of the nested mock.</param>
			public Key(IMockObject mock, Type nestedMockType)
			{
				Mock = mock;
				NestedMockType = nestedMockType;
			}

			/// <summary>
			/// Gets the mock.
			/// </summary>
			/// <value>The mock object.</value>
			public IMockObject Mock { get; private set; }

			/// <summary>
			/// Gets the type of the nested mock.
			/// </summary>
			/// <value>The type of the nested mock.</value>
			public Type NestedMockType { get; private set; }

			/// <summary>
			/// Whether this instance equals the specified other.
			/// </summary>
			/// <param name="other">The other to compare to.</param>
			/// <returns>A value indicating whether both instances are equal.</returns>
			public override bool Equals(object other)
			{
				return other is Key && Equals((Key) other);
			}

			/// <summary>
			/// Whether this instance equals the specified other.
			/// </summary>
			/// <param name="other">The other to compare to.</param>
			/// <returns>A value indicating whether both instances are equal.</returns>
			public bool Equals(Key other)
			{
				return other.Mock == Mock && other.NestedMockType == NestedMockType;
			}

			/// <summary>
			/// Serves as a hash function for a particular type.
			/// </summary>
			/// <returns>
			/// A hash code for the current <see cref="T:System.Object"/>.
			/// </returns>
			public override int GetHashCode()
			{
				return Mock.GetHashCode() ^ NestedMockType.GetHashCode();
			}
		}

		#endregion
	}
}
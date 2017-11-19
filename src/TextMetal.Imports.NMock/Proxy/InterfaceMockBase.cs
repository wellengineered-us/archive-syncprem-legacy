namespace NMock.Proxy
{
	/// <summary>
	/// Used as a base for interface mocks in order to provide a holder
	/// for a meaningful ToString() value.
	/// </summary>
	public class InterfaceMockBase
	{
		internal string Name { get; set; }

		/// <summary>
		/// Default constructor used by Castle.DynamicProxy.
		/// </summary>
		/// <remarks>Do not remove.  This is needed by Castle.</remarks>
		public InterfaceMockBase() : this(string.Empty)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InterfaceMockBase"/> class.
		/// </summary>
		/// <param name="name">The name of this instance.</param>
		public InterfaceMockBase(string name)
		{
			Name = name;
		}

		#region Object Overrides
		/// <summary>
		/// Returns the name of this instance.
		/// </summary>
		/// <returns>The name of this instance.</returns>
		public override string ToString()
		{
			return Name;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="System.Object"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="System.Object"/>.</param>
		/// <returns>true if the specified <see cref="System.Object"/> is equal to the current <see cref="System.Object"/>; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>A hash code for the current System.Object.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion
	}
}
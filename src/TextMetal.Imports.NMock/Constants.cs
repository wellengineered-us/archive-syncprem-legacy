namespace NMock
{
	/// <summary>
	/// Defines public constants
	/// </summary>
	public static class Constants
	{
		internal const string GET = "get_";
		internal const string SET = "set_";
		internal const string GET_ITEM = "get_Item";
		internal const string SET_ITEM = "set_Item";
		internal const string ADD = "add_";
		internal const string REMOVE = "remove_";

		internal const string ERROR_OCCURED_IN_SETUP = "An error occured while setting up the MethodMatcher.";

		/// <summary>
		/// A string that represents the name and Public Key of the NMock assembly.
		/// </summary>
		/// <remarks>
		/// Use this field in your assembly when NMock needs access to internal types
		/// </remarks>
		/// <example>
		/// [assembly: InternalsVisibleTo(NMock.Constants.InternalsVisibleTo)]
		/// </example>
		/// <seealso cref="System.Runtime.CompilerServices.InternalsVisibleToAttribute"/>
		public const string InternalsVisibleToNMock = "NMock, PublicKey=00240000048000009400000006020000002400005253413100040000010001000b09e431bbf2a72b9fa2762be9bb5c578786d01e66f739ce978c17237d6157a8ab82c0587d1bafebb116bc4509c444ac46067d1f156747be6885e35b9dd4d6be4fab7af4034964d38ab34cc7e39fbf4348ec856af3fd97ef55b7e28f321ff11ef7dec0e8fb1f262ec57f7ca2b35ff5ad91246a119de227061fa475616feee7f7";

		/// <summary>
		/// A string that represents the name and Public Key of the DynamicProxyGenAssembly assembly.
		/// </summary>
		/// <remarks>
		/// Use this field in your assembly when NMock needs access to internal types
		/// </remarks>
		/// <example>
		/// [assembly: InternalsVisibleTo(NMock.Constants.InternalsVisibleTo)]
		/// </example>
		/// <seealso cref="System.Runtime.CompilerServices.InternalsVisibleToAttribute"/>
		public const string InternalsVisibleToDynamicProxy = "DynamicProxyGenAssembly2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7";

		/// <summary>
		/// The NMock Public Key string.
		/// </summary>
		/// <remarks>
		/// This string is used internally to reference the public key.
		/// </remarks>
		public const string NMockPublicKey = "PublicKey=00240000048000009400000006020000002400005253413100040000010001000b09e431bbf2a72b9fa2762be9bb5c578786d01e66f739ce978c17237d6157a8ab82c0587d1bafebb116bc4509c444ac46067d1f156747be6885e35b9dd4d6be4fab7af4034964d38ab34cc7e39fbf4348ec856af3fd97ef55b7e28f321ff11ef7dec0e8fb1f262ec57f7ca2b35ff5ad91246a119de227061fa475616feee7f7";
	}
}

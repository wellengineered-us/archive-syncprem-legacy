#region Using

using System;
using NMock.Actions;
using NMock.Monitoring;

#endregion

namespace NMock
{
	/// <summary>
	/// short cut to initialize GetArgumentAction
	/// </summary>
	public static class GetArguments
	{
		/// <summary>
		/// Create a GetArgumentAction to get arments of invoked method
		/// </summary>
		/// <param name="handler">delegate used to get argments of executed method</param>
		/// <returns></returns>
		public static GetArgumentsAction WhenCalled(Action<ParameterList> handler)
		{
			return new GetArgumentsAction(handler);
		}
	}
}
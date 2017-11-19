#region Using

using System;
using System.IO;
using NMock.Monitoring;

#endregion

namespace NMock.Actions
{
	/// <summary>
	/// Action that executes the delegate passed to the constructor to get argments of executed method.
	/// </summary>
	public class GetArgumentsAction : IAction
	{
		private readonly Action<ParameterList> handler;


		/// <summary>
		/// constustor of GetArgumentsAction
		/// </summary>
		/// <param name="handler">delegate used to get argments of executed method</param>
		public GetArgumentsAction(Action<ParameterList> handler)
		{
			this.handler = handler;
		}

		#region IAction Members

		/// <summary>
		/// Invokes this object.
		/// </summary>
		/// <param name="invocation">The invocation.</param>
		void IAction.Invoke(Invocation invocation)
		{
			if (handler != null)
				handler.Invoke(invocation.Parameters);
		}

		/// <summary>
		/// Describes this object.
		/// </summary>
		/// <param name="writer">The text writer the description is added to.</param>
		void ISelfDescribing.DescribeTo(TextWriter writer)
		{
			if (writer == null) 
				throw new ArgumentNullException("writer");

			writer.Write("Get arguments");
		}

		#endregion
	}
}
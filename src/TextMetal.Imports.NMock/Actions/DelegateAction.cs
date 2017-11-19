#region Using

using System;
using System.IO;
using NMock.Monitoring;

#endregion

namespace NMock.Actions
{
	/// <summary>
	/// Action that executes the delegate passed to the constructor.
	/// </summary>
	[Obsolete("Please use the InvokeAction.")]
	public class DelegateAction : IAction
	{
		#region Delegates

		/// <summary>
		/// Delegate that is executed on invocation of the action.
		/// </summary>
		public delegate void Handler();

		#endregion

		/// <summary>
		/// Stores the handler of the delegate action.
		/// </summary>
		private readonly Handler _handler;

		/// <summary>
		/// Initializes a new instance of the <see cref="DelegateAction"/> class.
		/// </summary>
		/// <param name="actionHandler">The action handler.</param>
		public DelegateAction(Handler actionHandler)
		{
			_handler = actionHandler;
		}

		#region IAction Members

		/// <summary>
		/// Invokes this object.
		/// </summary>
		/// <param name="invocation">The invocation.</param>
		void IAction.Invoke(Invocation invocation)
		{
			Handler handler = _handler;
			if(handler != null)
				handler();
		}

		/// <summary>
		/// Describes this object.
		/// </summary>
		/// <param name="writer">The text writer the description is added to.</param>
		void ISelfDescribing.DescribeTo(TextWriter writer)
		{
			if (writer == null) 
				throw new ArgumentNullException("writer");

			writer.Write("execute delegate");
		}

		#endregion
	}
}
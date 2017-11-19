#region Using

using System;
using System.IO;
using NMock.Monitoring;

#endregion

namespace NMock.Actions
{
#if !SILVERLIGHT
	/// <summary>
	/// Action that set the result value of an invocation to a clone of the specified prototype.
	/// </summary>
	public class ReturnCloneAction: IReturnAction
	{
		/// <summary>
		/// Stores the prototype that will be cloned.
		/// </summary>
		private readonly ICloneable prototype;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReturnCloneAction"/> class.
		/// </summary>
		/// <param name="prototype">The prototype.</param>
		public ReturnCloneAction(ICloneable prototype)
		{
			this.prototype = prototype;
		}

		#region IAction Members

		/// <summary>
		/// Describes this object.
		/// </summary>
		/// <param name="writer">The text writer the description is added to.</param>
		void ISelfDescribing.DescribeTo(TextWriter writer)
		{
			writer.Write("a clone of ");
			writer.Write(prototype);
		}

		void IAction.Invoke(Invocation invocation)
		{
			invocation.Result = prototype.Clone();
		}

		#endregion

		public Type ReturnType
		{
			get { return prototype.GetType(); }
		}
	}
#endif
}
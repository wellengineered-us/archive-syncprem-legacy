#region Using

using System.IO;
using NMock.Monitoring;

#endregion

namespace NMock.Actions
{
	/// <summary>
	/// Action that sets a parameter (method argument) of the invocation to the specified value.
	/// </summary>
	public class SetIndexedParameterAction : IAction
	{
		/// <summary>
		/// Stores the index of the paremter to set.
		/// </summary>
		private readonly int index;

		/// <summary>
		/// Stores the value of the parameter to set.
		/// </summary>
		private readonly object value;

		/// <summary>
		/// Initializes a new instance of the <see cref="SetIndexedParameterAction"/> class.
		/// </summary>
		/// <param name="index">The index of the parameter to set.</param>
		/// <param name="value">The value.</param>
		public SetIndexedParameterAction(int index, object value)
		{
			this.index = index;
			this.value = value;
		}

		#region IAction Members

		/// <summary>
		/// Invokes this object. Sets the parameter at the specified index of the invocation to the specified value.
		/// </summary>
		/// <param name="invocation">The invocation.</param>
		void IAction.Invoke(Invocation invocation)
		{
			invocation.Parameters[index] = value;
		}

		/// <summary>
		/// Describes this object.
		/// </summary>
		/// <param name="writer">The text writer the description is added to.</param>
		void ISelfDescribing.DescribeTo(TextWriter writer)
		{
			writer.Write("set arg ");
			writer.Write(index);
			writer.Write("=");
			writer.Write(value);
		}

		#endregion
	}
}
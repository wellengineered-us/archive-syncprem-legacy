#region Using

using System;
using System.IO;
using NMock.Monitoring;

#endregion

namespace NMock.Actions
{
	/// <summary>
	/// Action that calls the collect delegate passed to constructor with the n-th element of the arguments to an invocation.
	/// </summary>
	/// <typeparam name="T">Type of the argument to collect.</typeparam>
	public class CollectAction<T> : IAction
	{
		#region Delegates

		/// <summary>
		/// Delegate that is called on collecting an argument.
		/// </summary>
		/// <param name="collectedParameter">The collected generic parameter.</param>
		public delegate void Collect(T collectedParameter);

		#endregion

		/// <summary>
		/// Stores the index of the argument.
		/// </summary>
		private readonly int argumentIndex;

		/// <summary>
		/// Stores the collect delegate.
		/// </summary>
		private readonly Collect collectDelegate;

		/// <summary>
		/// Initializes a new instance of the <see cref="CollectAction&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="argumentIndex">Index of the argument.</param>
		/// <param name="collectDelegate">The collect delegate.</param>
		public CollectAction(int argumentIndex, Collect collectDelegate)
		{
			this.argumentIndex = argumentIndex;
			this.collectDelegate = collectDelegate;
		}

		#region IAction Members

		/// <summary>
		/// Invokes this object.
		/// </summary>
		/// <param name="invocation">The invocation.</param>
		void IAction.Invoke(Invocation invocation)
		{
			if (invocation == null) 
				throw new ArgumentNullException("invocation");

			collectDelegate((T) invocation.Parameters[argumentIndex]);
		}

		/// <summary>
		/// Describes this object.
		/// </summary>
		/// <param name="writer">The text writer the description is added to.</param>
		void ISelfDescribing.DescribeTo(TextWriter writer)
		{
			if (writer == null) 
				throw new ArgumentNullException("writer");

			writer.Write("collect argument at index ");
			writer.Write(argumentIndex);
		}

		#endregion
	}
}
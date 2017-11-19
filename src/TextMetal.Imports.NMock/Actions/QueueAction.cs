using System;
using System.Collections.Generic;
using System.IO;
using NMock.Monitoring;

namespace NMock.Actions
{
	/// <summary>
	/// Action that returns an item from the queue 
	/// </summary>
	public class QueueAction<T> : IReturnAction
	{
		private readonly Queue<T> _queue;

		/// <summary>
		/// Initializes a new instance of the <see cref="QueueAction{T}"/> class with the queue of values.
		/// </summary>
		/// <param name="queue"></param>
		public QueueAction(Queue<T> queue)
		{
			_queue = queue;
		}

		void IAction.Invoke(Invocation invocation)
		{
			invocation.Result = _queue.Dequeue();
		}

		void ISelfDescribing.DescribeTo(TextWriter writer)
		{
			writer.Write("return a '" + typeof(T).Name + "' from a queue");
		}

		Type IReturnAction.ReturnType
		{
			get { return typeof (T); }
		}
	}
}
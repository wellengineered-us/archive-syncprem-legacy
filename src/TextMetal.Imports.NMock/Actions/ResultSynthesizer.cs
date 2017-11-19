#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NMock.Monitoring;

#endregion

/* ^ */ using System.Reflection; /* dpbullington@gmail.com ^ */

namespace NMock.Actions
{
	/// <summary>
	/// Responsible for handling the results of an invocation.
	/// </summary>
	public class ResultSynthesizer : IAction
	{
		/// <summary>
		/// Stores the default results.
		/// </summary>
		private static readonly Dictionary<Type, IAction> defaultResults = new Dictionary<Type, IAction>();

		/// <summary>
		/// Stores the results.
		/// </summary>
		private readonly Dictionary<Type, IAction> _results = new Dictionary<Type, IAction>();

		private readonly object _lock = new object();

		/// <summary>
		/// Initializes static members of the <see cref="ResultSynthesizer"/> class.
		/// </summary>
		static ResultSynthesizer()
		{
			defaultResults[typeof(string)] = new ReturnAction(String.Empty);
#if !SILVERLIGHT
			defaultResults[typeof(ArrayList)] = new ReturnCloneAction(new ArrayList());
			defaultResults[typeof(SortedList)] = new ReturnCloneAction(new SortedList());
			defaultResults[typeof(Hashtable)] = new ReturnCloneAction(new Hashtable());
			defaultResults[typeof(Queue)] = new ReturnCloneAction(new Queue());
			defaultResults[typeof(Stack)] = new ReturnCloneAction(new Stack());
#endif
		}

		#region IAction Members

		/// <summary>
		/// Invokes this object.
		/// </summary>
		/// <param name="invocation">The invocation.</param>
		void IAction.Invoke(Invocation invocation)
		{
			Type returnType = invocation.MethodReturnType;

			/* ^ */ var _returnType = returnType.GetTypeInfo(); /* dpbullington@gmail.com ^ */

			if (returnType == typeof(void))
			{
				return; // sanity check
			}

			lock (_lock)
			{
				if (_results.ContainsKey(returnType))
				{
					var action = _results[returnType];
					action.Invoke(invocation);
				}
				else if (returnType.IsArray)
				{
					invocation.Result = NewEmptyArray(returnType);
				}
				else if (/* ^ */ _returnType.IsValueType /* dpbullington@gmail.com ^ */)
				{
					invocation.Result = Activator.CreateInstance(returnType);
				}
				else if (defaultResults.ContainsKey(returnType))
				{
					var action = defaultResults[returnType];
					action.Invoke(invocation);
				}
				else
				{
					throw new InvalidOperationException("No action registered for return type " + returnType);
				}
			}
		}

		/// <summary>
		/// Describes this object to the specified <paramref name="writer"/>.
		/// </summary>
		/// <param name="writer">The text writer the description is added to.</param>
		void ISelfDescribing.DescribeTo(TextWriter writer)
		{
			writer.Write("a synthesized result");
		}

		#endregion

		/// <summary>
		/// Sets the result of the specified <paramref name="returnType"/>.
		/// </summary>
		/// <param name="returnType">The type to be returned as a result.</param>
		/// <param name="result">The result to be set.</param>
		public void SetResult(Type returnType, object result)
		{
			lock (_lock)
				_results[returnType] = Return.Value(result);
		}

		/// <summary>
		/// Gets a new the empty array of the specified <paramref name="arrayType"/>.
		/// </summary>
		/// <param name="arrayType">Type of the array to be returned.</param>
		/// <returns>
		/// Returns a new empty array of the specified <paramref name="arrayType"/>.
		/// </returns>
		private static object NewEmptyArray(Type arrayType)
		{
			var rank = arrayType.GetArrayRank();
			var dimensions = new int[rank];

			return Array.CreateInstance(arrayType.GetElementType(), dimensions);
		}
	}
}
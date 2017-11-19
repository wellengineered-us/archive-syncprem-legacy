#region Using

using System;
using System.Collections;
using System.Reflection;

#endregion

namespace NMock.Monitoring
{
	/// <summary>
	/// Manages a list of parameters for a mocked method together with the parameter's values.
	/// </summary>
	public class ParameterList
	{
		/// <summary>
		/// Holds a boolean for each value if it was set or not.
		/// </summary>
		private readonly BitArray isValueSet;

		/// <summary>
		/// Holds the method to be mocked.
		/// </summary>
		private readonly MethodBase method;

		/// <summary>
		/// An array holding the values of the parameters.
		/// </summary>
		private readonly object[] values;

		/// <summary>
		/// Initializes a new instance of the <see cref="ParameterList"/> class.
		/// </summary>
		/// <param name="method">The method to be mocked.</param>
		/// <param name="values">The values of the parameters.</param>
		public ParameterList(MethodBase method, object[] values)
		{
			if (method == null)
				throw new ArgumentNullException("method");
			if (values == null)
				throw new ArgumentNullException("values");

			this.method = method;
			this.values = values;
			isValueSet = new BitArray(values.Length);

			ParameterInfo[] parameters = method.GetParameters();
			for (int i = 0; i < parameters.Length; i++)
			{
				isValueSet[i] = !parameters[i].IsOut;
			}
		}

		/// <summary>
		/// Gets the number of values.
		/// </summary>
		/// <value>The number of values.</value>
		public int Count
		{
			get
			{
				return values.Length;
			}
		}

		/// <summary>
		/// Gets the values as array.
		/// </summary>
		/// <value>Values as array.</value>
		internal object[] AsArray
		{
			get
			{
				return values;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="System.Object"/> with the specified index.
		/// </summary>
		/// <param name="index">The index of the value to be get or set.</param>
		/// <value>
		/// The value of a parameter specified by its <paramref name="index"/>.
		/// </value>
		public object this[int index]
		{
			get
			{
				if (IsValueSet(index))
				{
					return values[index];
				}

				throw new InvalidOperationException(string.Format("Parameter '{0}' has not been set.", GetParameterName(index)));
			}

			set
			{
				if (CanValueBeSet(index))
				{
					values[index] = value;
					isValueSet[index] = true;
				}
				else
				{
					throw new InvalidOperationException(string.Format("Cannot set the value of in parameter '{0}'", GetParameterName(index)));
				}
			}
		}

		/// <summary>
		/// Determines whether the value specified by index was set.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>
		/// Returns <c>true</c> if value specified by index was set; otherwise, <c>false</c>.
		/// </returns>
		public bool IsValueSet(int index)
		{
			return isValueSet[index];
		}

		/// <summary>
		/// Marks all values as set.
		/// </summary>
		internal void MarkAllValuesAsSet()
		{
			isValueSet.SetAll(true);
		}

		/// <summary>
		/// Determines whether the parameter specified by index can be set.
		/// </summary>
		/// <param name="index">The index of the parameter.</param>
		/// <returns>
		/// Returns <c>true</c> if the parameter specified by index can be set; otherwise, <c>false</c>.
		/// </returns>
		private bool CanValueBeSet(int index)
		{
			ParameterAttributes attributes = method.GetParameters()[index].Attributes;
			return (attributes & ParameterAttributes.In) == ParameterAttributes.None;
		}

		/// <summary>
		/// Gets the parameter name by index.
		/// </summary>
		/// <param name="index">The index of the parameter name to get.</param>
		/// <returns>
		/// Returns the parameter name with the given index.
		/// </returns>
		private string GetParameterName(int index)
		{
			return method.GetParameters()[index].Name;
		}
	}
}
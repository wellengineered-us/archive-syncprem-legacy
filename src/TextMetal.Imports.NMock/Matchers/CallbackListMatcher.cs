using System;
using System.Collections.Generic;
using System.IO;

namespace NMock.Matchers
{
	/// <summary>
	/// A matcher that stores a list of delegates to call back to later.
	/// </summary>
	/// <typeparam name="T">Typically a <see cref="Action"/>, <see cref="Predicate{T}"/>, <see cref="Func{TResult}"/>, or their equivalents.</typeparam>
	public class CallbackListMatcher<T> : Matcher
	{
		private List<T> _callbacks;

		/// <summary>
		/// Gets a list of references to the callbacks assigned during the matching operation of the expectation.
		/// </summary>
		public List<T> Callbacks
		{
			get { return _callbacks ?? (_callbacks = new List<T>()); }
		}

		/// <summary>
		/// Determines if the parameter is the same type as <typeparamref name="T"/>.
		/// </summary>
		/// <param name="o">The value the matcher will evaluate.</param>
		/// <returns>A value indicating if the parameter matches or not.</returns>
		public override bool Matches(object o)
		{
			var isTypeOf = o is T;
			if (isTypeOf)
			{
				Callbacks.Add((T)o);
			}

			return isTypeOf;
		}

		/// <summary>
		/// Describes this matcher.
		/// </summary>
		/// <param name="writer">The text writer to which the description is added.</param>
		public override void DescribeTo(TextWriter writer)
		{
			writer.Write("CallbackListMatcher of type " + typeof(T));
		}
	}
}
using System;
using System.IO;

namespace NMock.Matchers
{
	/// <summary>
	/// A matcher that stores a delegate to call back to later.
	/// </summary>
	/// <typeparam name="T">Typically a <see cref="Action"/>, <see cref="Predicate{T}"/>, <see cref="Func{TResult}"/>, or their equivalents.</typeparam>
	public class CallbackMatcher<T> : Matcher
	{
		/// <summary>
		/// Gets a reference to the callback assigned during the matching operation of the expectation.
		/// </summary>
		public T Callback { get; private set; }

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
				Callback = (T)o;
			}

			return isTypeOf;
		}

		/// <summary>
		/// Describes this matcher.
		/// </summary>
		/// <param name="writer">The text writer to which the description is added.</param>
		public override void DescribeTo(TextWriter writer)
		{
			writer.Write("CallbackMatcher of type " + typeof(T));
		}
	}
}
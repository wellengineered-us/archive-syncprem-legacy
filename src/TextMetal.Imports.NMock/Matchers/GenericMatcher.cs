#region Using

using System;

#endregion

namespace NMock.Matchers
{
	/// <summary>
	/// Matcher that checks whether a value matches the check provided as a delegate.
	/// the expectation.
	/// </summary>
	/// <typeparam name="T">The type of the expected value.</typeparam>
	[Obsolete("Use the Is.Matcher<T>() shortcut or PredicateMatcher<T>.  This class used a custom delegate that created noise in the API.")]
	public class GenericMatcher<T> : Matcher
	{
		#region Delegates

		/// <summary>
		/// The test that is performed to check if the <paramref name="value"/> matches the expectation.
		/// </summary>
		/// <param name="value">The actually received value.</param>
		/// <returns>True then value matches the expectation.</returns>
		public delegate bool MatchExpression(T value);

		#endregion

		/// <summary>
		/// The test that is performed to see if the value matches the expectation.
		/// </summary>
		private readonly MatchExpression matchExpression;

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericMatcher{T}"/> class.
		/// </summary>
		/// <param name="matchExpression">The test that is performed to check if the value matches expectation.</param>
		/// <exception cref="ArgumentNullException"><c>matchExpression</c> is null.</exception>
		public GenericMatcher(MatchExpression matchExpression)
			: base("generic match")
		{
			if (matchExpression == null)
			{
				throw new ArgumentNullException("matchExpression", "matchExpression must not be null.");
			}

			this.matchExpression = matchExpression;
		}

		/// <summary>
		/// Matches the specified object to this matcher and returns whether it matches.
		/// </summary>
		/// <param name="o">The object to match.</param>
		/// <returns>Whether the object matches.</returns>
		public override bool Matches(object o)
		{
			return o is T && matchExpression((T) o);
		}
	}
}
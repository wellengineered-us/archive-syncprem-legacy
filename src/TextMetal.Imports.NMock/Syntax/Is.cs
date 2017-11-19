#region Using

using System;
using System.Collections;
using NMock.Matchers;

#endregion

namespace NMock
{
	/// <summary>
	/// Provides shortcuts to <see cref="Matcher"/>s.
	/// </summary>
	public static class Is
	{
		/// <summary>
		/// Matches anything.
		/// </summary>
		public static readonly Matcher Anything = new AlwaysMatcher(true, "anything");

		/// <summary>
		/// Matches nothing.
		/// </summary>
		public static readonly Matcher Nothing = new AlwaysMatcher(false, "nothing");

		/// <summary>
		/// Matches if the value is null.
		/// </summary>
		public static readonly Matcher Null = new NullMatcher();

		/// <summary>
		/// Matches if the value is not null.
		/// </summary>
		public static readonly Matcher NotNull = new NotMatcher(Null);

		/// <summary>
		/// Matches out parameters of methods.
		/// </summary>
		public static readonly Matcher Out = new ArgumentsMatcher.OutMatcher();

		/// <summary>
		/// Matches objects the are equal to the expected object.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <returns>Returns a new instance of the <see cref="EqualMatcher"/> class.</returns>
		public static Matcher EqualTo(object expected)
		{
			return new EqualMatcher(expected);
		}

		/// <summary>
		/// Matches an expected object.
		/// <seealso cref="EqualTo"/>
		/// </summary>
		/// <param name="expected">The expected object.</param>
		/// <returns>Returns a new instance of the <see cref="EqualMatcher"/> class.</returns>
		[Obsolete]
		public static Matcher Same(object expected)
		{
			return new EqualMatcher(expected);
		}

		/// <summary>
		/// Matches objects that implement the <see cref="IComparable"/> interface.
		/// </summary>
		/// <param name="expected">The instance to be compared.</param>
		/// <returns>Returns a new instance of the <see cref="ObjectMatcher"/> class.</returns>
		public static Matcher ComparableTo(object expected)
		{
			return new ObjectMatcher(expected);
		}
		
		/// <summary>
		/// Matches strings containing the specified <paramref name="substring"/>.
		/// </summary>
		/// <param name="substring">The substring.</param>
		/// <returns>Returns a new instance of the <see cref="StringContainsMatcher"/> class.</returns>
		public static Matcher StringContaining(string substring)
		{
			return new StringContainsMatcher(substring);
		}

		/// <summary>
		/// Matches objects that are greater than <paramref name="value"/>.
		/// </summary>
		/// <param name="value">The value to compare.</param>
		/// <returns>Returns a new instance of the <see cref="ComparisonMatcher"/> class.</returns>
		public static Matcher GreaterThan(IComparable value)
		{
			return new ComparisonMatcher(value, 1, 1);
		}

		/// <summary>
		/// Matches objects that are at least equal to <paramref name="value"/>.
		/// </summary>
		/// <param name="value">The value to compare.</param>
		/// <returns>Returns a new instance of the <see cref="ComparisonMatcher"/> class.</returns>
		public static ComparisonMatcher AtLeast(IComparable value)
		{
			return new ComparisonMatcher(value, 0, 1);
		}

		/// <summary>
		/// Matches objects less than <paramref name="value"/>.
		/// </summary>
		/// <param name="value">The value to compare.</param>
		/// <returns>Returns a new instance of the <see cref="ComparisonMatcher"/> class.</returns>
		public static Matcher LessThan(IComparable value)
		{
			return new ComparisonMatcher(value, -1, -1);
		}

		/// <summary>
		/// Matches objects that are less or equal to <paramref name="value"/>.
		/// </summary>
		/// <param name="value">The value to compare.</param>
		/// <returns>Returns a new instance of the <see cref="ComparisonMatcher"/> class.</returns>
		public static ComparisonMatcher AtMost(IComparable value)
		{
			return new ComparisonMatcher(value, -1, 0);
		}

		/// <summary>
		/// Matches objects in the specified collection.
		/// </summary>
		/// <param name="collection">The collection with objects to match.</param>
		/// <returns>Returns a new instance of the <see cref="ElementMatcher"/> class.</returns>
		public static Matcher In(ICollection collection)
		{
			return new ElementMatcher(collection);
		}

		/// <summary>
		/// Matches objects in the specified elements.
		/// </summary>
		/// <param name="elements">The elements to match.</param>
		/// <returns>Returns a new instance of the <see cref="ElementMatcher"/> class.</returns>
		public static Matcher OneOf(params object[] elements)
		{
			return new ElementMatcher(elements);
		}

		/// <summary>
		/// Matches objects of the specified type.
		/// </summary>
		/// <param name="type">The type to match.</param>
		/// <returns>Returns a new instance of the <see cref="TypeMatcher"/> class.</returns>
		public static Matcher TypeOf(Type type)
		{
			return new TypeMatcher(type);
		}

		/// <summary>
		/// Matches objects of the specified type.
		/// </summary>
		/// <typeparam name="T">The type to match.</typeparam>
		/// <returns>
		/// Returns a new instance of the <see cref="TypeMatcher"/> class.
		/// </returns>
		public static Matcher TypeOf<T>()
		{
			return new TypeMatcher(typeof (T));
		}

		/// <summary>
		/// Matches objects against the specified expression.
		/// </summary>
		/// <typeparam name="T">Type of the value to match.</typeparam>
		/// <param name="expression">The match expression.</param>
		/// <returns>returns a new instance of the <see cref="PredicateMatcher{T}"/>.</returns>
		public static Matcher Match<T>(Predicate<T> expression)
		{
			// <pex>
			if (expression == null)
				throw new ArgumentNullException("expression");
			// </pex>
			return new PredicateMatcher<T>(expression);
		}
	}
}
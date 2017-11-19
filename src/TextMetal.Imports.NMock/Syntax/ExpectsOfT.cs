#region Using

using System;
using NMock.Internal;
using NMock.Syntax;

#endregion

namespace NMock
{
	/// <summary>
	/// This syntax class contains <see cref="IMethodSyntax{TInterface}"/> properties that control
	/// the number of expectations added for the referenced <see cref="Mock{T}"/>.
	/// </summary>
	/// <typeparam name="T">The <c>class</c> or <c>interface</c> that is being mocked.</typeparam>
	/// <remarks>
	/// <see cref="Expects{T}"/> is the return type of the main property used on a <see cref="Mock{T}"/> instance.  You
	/// access it by typing <c>mockObject.Expects</c>.<para>The <see cref="Expects{T}"/> class provides access to
	/// all other methods that help create expectations.</para>
	/// </remarks>
	public class Expects<T>
		where T : class
	{
		private readonly object _proxy;

		/// <summary>
		/// Initializes a new instance of the <see cref="Expects{T}"/> class with the specified type as the template.
		/// </summary>
		/// <param name="proxy"></param>
		internal Expects(object proxy)
		{
			_proxy = proxy;
		}

		public IMethodSyntax<T> Any
		{
			get
			{
				return GetExpectationBuilder("any number of times", Is.Anything, Is.Anything, _proxy);
			}
		}

		/// <summary>
		/// Creates an expectation for at least one call of the referenced member.
		/// </summary>
		public IMethodSyntax<T> AtLeastOne
		{
			get
			{
				return AtLeast(1);
			}
		}

		/// <summary>
		/// Creates an expectation for at most one call of the referenced member.
		/// </summary>
		/// <returns></returns>
		[Obsolete("Use AtMostOne")]
		public IMethodSyntax<T> AtMostOnce
		{
			get
			{
				return AtMost(1);
			}
		}

		/// <summary>
		/// Creates an expectation for at most one call of the referenced member.
		/// </summary>
		/// <returns></returns>
		public IMethodSyntax<T> AtMostOne
		{
			get
			{
				return AtMost(1);
			}
		}

		/// <summary>
		/// Specifies that there should be no calls to the referenced member.
		/// </summary>
		public IMethodSyntax<T> No
		{
			get
			{
				return GetExpectationBuilder("never", Is.EqualTo(0), Is.EqualTo(0), _proxy);
			}
		}

		/// <summary>
		/// Creates an expectation for one call of the referenced member.
		/// </summary>
		public IMethodSyntax<T> One
		{
			get
			{
				return Exactly(1);
			}
		}

		/// <summary>
		/// Creates an expectation for at least <c>count</c> number of calls for the referenced member.
		/// </summary>
		/// <param name="count">The minimum number of calls expect</param>
		/// <returns>An <see cref="IMethodSyntax{T}"/> to reference the expected call.</returns>
		public IMethodSyntax<T> AtLeast(int count)
		{
			return GetExpectationBuilder("at least " + count.Times(), Is.AtLeast(count), Is.Anything, _proxy);
		}

		/// <summary>
		/// Creates an expectation for at most <c>count</c> number of calls for the referenced member.
		/// </summary>
		/// <param name="count">The maximum number of calls expect</param>
		/// <returns>An <see cref="IMethodSyntax{T}"/> to reference the expected call.</returns>
		public IMethodSyntax<T> AtMost(int count)
		{
			return GetExpectationBuilder("at most " + count.Times(), Is.Anything, Is.AtMost(count), _proxy);
		}

		/// <summary>
		/// Creates an expectation for a range from <c>minCount</c> to <c>maxCount</c> number of calls for the referenced member.
		/// </summary>
		/// <param name="minCount">The minimum number of expected calls.</param>
		/// <param name="maxCount">The maximum number of expected calls.</param>
		/// <returns>An <see cref="IMethodSyntax{T}"/> to reference the expected call.</returns>
		public IMethodSyntax<T> Between(int minCount, int maxCount)
		{
			return GetExpectationBuilder(minCount + " to " + maxCount + " times", Is.AtLeast(minCount), Is.AtMost(maxCount), _proxy);
		}

		/// <summary>
		/// Creates an expectation for exactly <c>count</c> number of calls for the referenced member.
		/// </summary>
		/// <param name="count">The exact number of calls expect</param>
		/// <returns>An <see cref="IMethodSyntax{T}"/> to reference the expected call.</returns>
		public IMethodSyntax<T> Exactly(int count)
		{
			return GetExpectationBuilder(count.Times(), Is.AtLeast(count), Is.AtMost(count), _proxy);
		}

		private IMethodSyntax<T> GetExpectationBuilder(string description, Matcher requiredCountMatcher, Matcher matchingCountMatcher, object proxy)
		{
			return new ExpectationBuilder<T>(description, requiredCountMatcher, matchingCountMatcher, proxy);
		}
	}
}
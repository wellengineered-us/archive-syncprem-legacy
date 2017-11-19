#region Using

using System;
using System.Collections;
using System.IO;
using System.Linq;

#endregion

namespace NMock.Matchers
{
	/// <summary>
	/// Matcher that checks whether the expected and actual value are equal.
	/// </summary>
	public class EqualMatcher : Matcher
	{
		internal const string EQUAL_TO = "equal to ";

		private readonly object expected;

		/// <summary>
		/// Initializes a new instance of the <see cref="EqualMatcher"/> class.
		/// </summary>
		/// <param name="expected">The expected value.</param>
		public EqualMatcher(object expected)
		{
			this.expected = expected;
		}

		/// <summary>
		/// Matcheses the specified actual.
		/// </summary>
		/// <param name="actual">The actual value.</param>
		/// <returns>Whether the expected value is equal to the actual value.</returns>
		public override bool Matches(object actual)
		{
			return AreEqual(expected, actual);
		}

		/// <summary>
		/// Describes this matcher.
		/// </summary>
		/// <param name="writer">The text writer to which the description is added.</param>
		public override void DescribeTo(TextWriter writer)
		{
			writer.Write(EQUAL_TO);
			writer.Write(expected);
		}

		private bool AreEqual(object o1, object o2)
		{
			if (o1 is Array)
			{
				return o2 is Array && ArraysEqual((Array) o1, (Array) o2);
			}

			if (o1 is IList)
			{
				return o2 is IList && ListsEqual((IList) o1, (IList) o2);
			}
			else
			{
				return Equals(o1, o2);
			}
		}

		private bool ArraysEqual(Array a1, Array a2)
		{
			return a1.Rank == a2.Rank
			       && ArrayDimensionsEqual(a1, a2)
			       && ArrayElementsEqual(a1, a2, new int[a1.Rank], 0);
		}

		private bool ArrayDimensionsEqual(Array a1, Array a2)
		{
			for (int dimension = 0; dimension < a1.Rank; dimension++)
			{
				if (a1.GetLength(dimension) != a2.GetLength(dimension))
				{
					return false;
				}
			}

			return true;
		}

		private bool ArrayElementsEqual(Array a1, Array a2, int[] indices, int dimension)
		{
			if (dimension == a1.Rank)
			{
				return AreEqual(a1.GetValue(indices), a2.GetValue(indices));
			}
			else
			{
				for (indices[dimension] = 0;
				     indices[dimension] < a1.GetLength(dimension);
				     indices[dimension]++)
				{
					if (!ArrayElementsEqual(a1, a2, indices, dimension + 1))
					{
						return false;
					}
				}

				return true;
			}
		}

		private bool ListsEqual(IList l1, IList l2)
		{
			return l1.Count == l2.Count && ListElementsEqual(l1, l2);
		}

		private bool ListElementsEqual(IList l1, IList l2)
		{
			return !l1.Cast<object>().Where((t, i) => !AreEqual(t, l2[i])).Any();
		}
	}
}
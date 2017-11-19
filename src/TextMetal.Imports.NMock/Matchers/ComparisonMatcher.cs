#region Using

using System;
using System.IO;

#endregion

namespace NMock.Matchers
{
	/// <summary>
	/// Matcher that checks a value against upper and lower bounds.
	/// </summary>
	public class ComparisonMatcher : Matcher
	{
		/// <summary>
		/// Stores the maximum comparison result for a successful match.
		/// </summary>
		private readonly int maxComparisonResult;

		/// <summary>
		/// Stores the minimum comparison result for a successful match.
		/// </summary>
		private readonly int minComparisonResult;

		/// <summary>
		/// Stores the value to be compared.
		/// </summary>
		private readonly IComparable value;

		/// <summary>
		/// Initializes a new instance of the <see cref="ComparisonMatcher"/> class.
		/// </summary>
		/// <param name="value">The value to compare.</param>
		/// <param name="comparisonResult1">The first allowed comparison result (result of value.CompareTo(other)).</param>
		/// <param name="comparisonResult2">The second allowed comparison result (result of value.CompareTo(other)).</param>
		/// <exception cref="ArgumentException">Thrown when one value is -1 and the other is 1.</exception>
		public ComparisonMatcher(IComparable value, int comparisonResult1, int comparisonResult2)
		{
			this.value = value;
			minComparisonResult = Math.Min(comparisonResult1, comparisonResult2);
			maxComparisonResult = Math.Max(comparisonResult1, comparisonResult2);

			if (minComparisonResult == -1 && maxComparisonResult == 1)
			{
				throw new ArgumentException("comparison result range too large");
			}
		}

		/// <summary>
		/// Matches the specified object to this matcher and returns whether it matches.
		/// </summary>
		/// <param name="o">The object to match.</param>
		/// <returns>Whether the object compared to the value resulted in either of both specified comparison results.</returns>
		public override bool Matches(object o)
		{
			if (o.GetType() == value.GetType())
			{
				int comparisonResult = -value.CompareTo(o);
				return comparisonResult >= minComparisonResult
				       && comparisonResult <= maxComparisonResult;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Describes this matcher.
		/// </summary>
		/// <param name="writer">The text writer to which the description is added.</param>
		public override void DescribeTo(TextWriter writer)
		{
			writer.Write("? ");
			if (minComparisonResult == -1)
			{
				writer.Write("<");
			}

			if (maxComparisonResult == 1)
			{
				writer.Write(">");
			}

			if (minComparisonResult == 0 || maxComparisonResult == 0)
			{
				writer.Write("=");
			}

			writer.Write(" ");
			writer.Write(value);
		}
	}
}
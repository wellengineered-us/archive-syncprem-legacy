#region Using

using System.IO;

#endregion

namespace NMock.Matchers
{
	/// <summary>
	/// Matcher that negates another matcher.
	/// </summary>
	public class NotMatcher : Matcher
	{
		/// <summary>
		/// Holds the matcher to negate.
		/// </summary>
		private readonly Matcher negated;

		/// <summary>
		/// Initializes a new instance of the <see cref="NotMatcher"/> class.
		/// </summary>
		/// <param name="negated">The matcher to negate.</param>
		public NotMatcher(Matcher negated)
		{
			this.negated = negated;
		}

		/// <summary>
		/// Matches the specified object to this matcher and returns whether it matches.
		/// </summary>
		/// <param name="o">The object to match.</param>
		/// <returns>Whether the object does not matche the wrapped matcher.</returns>
		public override bool Matches(object o)
		{
			return !negated.Matches(o);
		}

		/// <summary>
		/// Describes this matcher.
		/// </summary>
		/// <param name="writer">The text writer to which the description is added.</param>
		public override void DescribeTo(TextWriter writer)
		{
			writer.Write("not ");
			negated.DescribeTo(writer);
		}
	}
}
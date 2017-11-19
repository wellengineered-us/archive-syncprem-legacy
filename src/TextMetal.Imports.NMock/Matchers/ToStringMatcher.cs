#region Using

using System.IO;

#endregion

namespace NMock.Matchers
{
	/// <summary>
	/// Matcher that checks whether the actual value in string representation (actual.ToString())
	/// matches with the wrapped matcher.
	/// </summary>
	public class ToStringMatcher : Matcher
	{
		/// <summary>
		/// Holds the wrapped matcher.
		/// </summary>
		private readonly Matcher matcher;

		/// <summary>
		/// Initializes a new instance of the <see cref="ToStringMatcher"/> class.
		/// </summary>
		/// <param name="matcher">The wrapped matcher.</param>
		public ToStringMatcher(Matcher matcher)
		{
			this.matcher = matcher;
		}

		/// <summary>
		/// Matches the specified object to this matcher and returns whether it matches.
		/// </summary>
		/// <param name="o">The object to match.</param>
		/// <returns>Whether the object in string representation (o.ToString()) matches the wrapped matcher.</returns>
		public override bool Matches(object o)
		{
			return matcher.Matches(o.ToString());
		}

		/// <summary>
		/// Describes this matcher.
		/// </summary>
		/// <param name="writer">The text writer to which the description is added.</param>
		public override void DescribeTo(TextWriter writer)
		{
			writer.Write("an object with a string representation that is ");
			matcher.DescribeTo(writer);
		}
	}
}
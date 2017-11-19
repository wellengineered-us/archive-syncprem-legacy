#region Using

using System.IO;

#endregion

namespace NMock.Matchers
{
	/// <summary>
	/// Matcher for indexer setters. Checks that the arguments passed to the indexer match.
	/// </summary>
	public class IndexSetterArgumentsMatcher : ArgumentsMatcher
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IndexSetterArgumentsMatcher"/> class.
		/// </summary>
		/// <param name="valueMatchers">The value matchers. This is an ordered list of matchers, each matching a single method argument.</param>
		public IndexSetterArgumentsMatcher(params Matcher[] valueMatchers) : base(valueMatchers)
		{
		}

		/// <summary>
		/// Describes this matcher.
		/// </summary>
		/// <param name="writer">The text writer to which the description is added.</param>
		public override void DescribeTo(TextWriter writer)
		{
			writer.Write("[");
			WriteListOfMatchers(Count - 1, writer);
			writer.Write("] = (");
			LastMatcher().DescribeTo(writer);
			writer.Write(")");
		}
	}
}
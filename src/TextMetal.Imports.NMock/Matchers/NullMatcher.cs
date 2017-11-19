#region Using

using System.IO;

#endregion

namespace NMock.Matchers
{
	/// <summary>
	/// Matcher that checks whether to actual value is equal to null.
	/// </summary>
	public class NullMatcher : Matcher
	{
		/// <summary>
		/// Initializes an instance of the <see cref="NullMatcher"/> class with a description of null.
		/// </summary>
		public NullMatcher() : base("null")
		{}

		/// <summary>
		/// Matches the specified object to this matcher and returns whether it matches.
		/// </summary>
		/// <param name="o">The object to match.</param>
		/// <returns>Whether the object is equal to null.</returns>
		public override bool Matches(object o)
		{
			return o == null;
		}
	}
}
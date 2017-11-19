namespace NMock.Matchers
{
	/// <summary>
	/// Matcher that is used to change the description the wrapped matcher.
	/// </summary>
	public class DescriptionOverride : Matcher
	{
		/// <summary>
		/// Stores the matcher to wrap.
		/// </summary>
		public Matcher WrappedMatcher { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="DescriptionOverride"/> class.
		/// </summary>
		/// <param name="description">The new description for the wrapped matcher.</param>
		/// <param name="otherMatcher">The matcher to wrap.</param>
		public DescriptionOverride(string description, Matcher otherMatcher) : base(description)
		{
			WrappedMatcher = otherMatcher;
		}

		/// <summary>
		/// Matches the specified object to this matcher and returns whether it matches.
		/// </summary>
		/// <param name="o">The object to match.</param>
		/// <returns>Whether the wrapped matcher matches.</returns>
		public override bool Matches(object o)
		{
			return WrappedMatcher.Matches(o);
		}
	}
}
#region Using

using System.IO;
using NMock.Internal;
using NMock.Matchers;

#endregion

namespace NMock
{
	/// <summary>
	/// A matcher is used to match objects against it.
	/// </summary>
	public abstract class Matcher : ISelfDescribing
	{
		/// <summary>
		/// A default description used when calling <see cref="DescribeTo"/>
		/// </summary>
		public string Description { get; protected set; }

		/// <summary>
		/// Initializes an instance of the <see cref="Matcher"/> class with an empty <see cref="Description"/>
		/// </summary>
		protected Matcher()
		{
			//Description = string.Empty;
		}

		/// <summary>
		/// Initialized an instance of the <see cref="Matcher"/> class with the description argument.
		/// </summary>
		/// <param name="description">The value used when calling the <see cref="DescribeTo"/> method.</param>
		protected Matcher(string description)
		{
			Description = description;
		}

		#region ISelfDescribing Members

		/// <summary>
		/// Describes this matcher.
		/// </summary>
		/// <param name="writer">The text writer the description is added to.</param>
		public virtual void DescribeTo(TextWriter writer)
		{
			if(Description == null)
				writer.Write("[" + GetType().Name + "]");
			else if(!Description.Equals(string.Empty))
				writer.Write(Description);
		}

		#endregion

		/// <summary>
		/// Logical and of to matchers.
		/// </summary>
		/// <param name="m1">First matcher.</param>
		/// <param name="m2">Second matcher.</param>
		/// <returns>Matcher combining the two operands.</returns>
		public static Matcher operator &(Matcher m1, Matcher m2)
		{
			return new AndMatcher(m1, m2);
		}

		/// <summary>
		/// Logical or of to matchers.
		/// </summary>
		/// <param name="m1">First matcher.</param>
		/// <param name="m2">Second matcher.</param>
		/// <returns>Matcher combining the two operands.</returns>
		public static Matcher operator |(Matcher m1, Matcher m2)
		{
			return new OrMatcher(m1, m2);
		}

		/// <summary>
		/// Negation of a matcher.
		/// </summary>
		/// <param name="m">Matcher to negate.</param>
		/// <returns>Negation of the specified matcher.</returns>
		public static Matcher operator !(Matcher m)
		{
			return new NotMatcher(m);
		}

		/// <summary>
		/// Matches the specified object to this matcher and returns whether it matches.
		/// </summary>
		/// <param name="o">The object to match.</param>
		/// <returns>Whether the object matches.</returns>
		public abstract bool Matches(object o);

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			var writer = new DescriptionWriter();
			DescribeTo(writer);
			return writer.ToString();
		}
	}
}
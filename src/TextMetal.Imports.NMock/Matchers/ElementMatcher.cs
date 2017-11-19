#region Using

using System.Collections;
using System.IO;
using System.Linq;

#endregion

namespace NMock.Matchers
{
	/// <summary>
	/// Matcher that checks whether a single object is in a collection of elements.
	/// </summary>
	public class ElementMatcher : Matcher
	{
		private readonly ICollection collection;

		/// <summary>
		/// Initializes a new instance of the <see cref="ElementMatcher"/> class.
		/// </summary>
		/// <param name="collection">The collection to match against.</param>
		public ElementMatcher(ICollection collection)
		{
			this.collection = collection;
		}

		/// <summary>
		/// Matches the specified object to this matcher and returns whether it matches.
		/// </summary>
		/// <param name="actual">The object to match.</param>
		/// <returns>Whether to object matches.</returns>
		public override bool Matches(object actual)
		{
			return collection.Cast<object>().Contains(actual);
		}

		/// <summary>
		/// Describes this matcher.
		/// </summary>
		/// <param name="writer">The text writer to which the description is added.</param>
		public override void DescribeTo(TextWriter writer)
		{
			writer.Write("element of [");

			bool separate = false;
			foreach (object element in collection)
			{
				if (separate)
				{
					writer.Write(", ");
				}

				writer.Write(element);
				separate = true;
			}

			writer.Write("]");
		}
	}
}
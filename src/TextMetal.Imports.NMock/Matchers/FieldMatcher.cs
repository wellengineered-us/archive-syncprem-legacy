#region Using

using System;
using System.IO;
using System.Reflection;

#endregion

namespace NMock.Matchers
{
	/// <summary>
	/// Matcher that checks whether the specified field of the actual object matches with the specified matcher. 
	/// </summary>
	public class FieldMatcher : Matcher
	{
		/// <summary>
		/// Name of the field to match against the <seealso cref="valueMatcher"/>.
		/// </summary>
		private readonly string fieldName;

		/// <summary>
		/// The value <see cref="Matcher"/> used to match the field of the object under investigation.
		/// </summary>
		private readonly Matcher valueMatcher;

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldMatcher"/> class.
		/// </summary>
		/// <param name="fieldName">Name of the field to match against the <paramref name="valueMatcher"/>.</param>
		/// <param name="valueMatcher">The value matcher.</param>
		public FieldMatcher(string fieldName, Matcher valueMatcher)
		{
			this.fieldName = fieldName;
			this.valueMatcher = valueMatcher;
		}

		/// <summary>
		/// Matches the specified object to this matcher and returns whether it matches.
		/// </summary>
		/// <param name="o">The object to match.</param>
		/// <returns>Whether the object matches.</returns>
		public override bool Matches(object o)
		{
			Type type = o.GetType();
			FieldInfo field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);

			if (field == null)
			{
				return false;
			}

			object value = field.GetValue(o);
			return valueMatcher.Matches(value);
		}

		/// <summary>
		/// Describes this matcher.
		/// </summary>
		/// <param name="writer">The text writer to which the description is added.</param>
		public override void DescribeTo(TextWriter writer)
		{
			writer.Write(string.Format("field '{0}' ", fieldName));
			valueMatcher.DescribeTo(writer);
		}
	}
}
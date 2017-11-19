#region Using

using System;
using System.IO;
using System.Reflection;

#endregion

namespace NMock.Matchers
{
	/// <summary>
	/// Matcher that checks whether the actual object has a property with the specified name 
	/// and its value matches the specified matcher.
	/// </summary>
	public class PropertyMatcher : Matcher
	{
		private readonly string propertyName;
		private readonly Matcher valueMatcher;

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyMatcher"/> class.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="valueMatcher">The value matcher.</param>
		public PropertyMatcher(string propertyName, Matcher valueMatcher)
		{
			this.propertyName = propertyName;
			this.valueMatcher = valueMatcher;
		}

		/// <summary>
		/// Matches the specified object to this matcher and returns whether it matches.
		/// </summary>
		/// <param name="o">The object to match.</param>
		/// <returns>Whether the object has a property with the expected name and expected value.</returns>
		public override bool Matches(object o)
		{
			Type type = o.GetType();
			PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

			if (property == null)
			{
				return false;
			}

			if (!property.CanRead)
			{
				return false;
			}

			object value = property.GetValue(o, null);
			return valueMatcher.Matches(value);
		}

		/// <summary>
		/// Describes this matcher.
		/// </summary>
		/// <param name="writer">The text writer to which the description is added.</param>
		public override void DescribeTo(TextWriter writer)
		{
			writer.Write(string.Format("property '{0}' ", propertyName));
			valueMatcher.DescribeTo(writer);
		}
	}
}
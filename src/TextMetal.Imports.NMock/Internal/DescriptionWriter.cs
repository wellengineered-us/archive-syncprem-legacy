#region Using

using System.IO;

#endregion

namespace NMock.Internal
{
	/// <summary>
	/// Used to describe Matchers and other classes for exception handling.
	/// </summary>
	internal class DescriptionWriter : StringWriter
	{
		/// <summary>
		/// Writes the text representation of an object to the text stream by calling ToString on that object.
		/// </summary>
		/// <param name="value">The object to write.</param>
		/// <exception cref="T:System.ObjectDisposedException">
		/// The <see cref="T:System.IO.TextWriter"/> is closed.
		/// </exception>
		/// <exception cref="T:System.IO.IOException">
		/// An I/O error occurs.
		/// </exception>
		public override void Write(object value)
		{
			Write(FormatValue(value));
		}

		public override void Write(string value)
		{
			base.Write(value);
		}

		public override void Write(string format, object arg0)
		{
			base.Write(format, arg0);
		}

		/// <summary>
		/// Formats the given <paramref name="value"/> depending on null and the type of the value.
		/// </summary>
		/// <param name="value">The value to format.</param>
		/// <returns>Returns the formatted string.</returns>
		private string FormatValue(object value)
		{
			if (value == null)
			{
				return "null";
			}
			else if (value is string)
			{
				return FormatString((string) value);
			}
			else
			{
				return "<" + value + ">(" + value.GetType() + ")";
			}
		}

		/// <summary>
		/// Replaces backslashes with three escaped backslashes.
		/// </summary>
		/// <param name="s">The string to replace backslashes.</param>
		/// <returns>Returns the escaped string.</returns>
		private string FormatString(string s)
		{
			const string Quote = "\"";
			const string EscapedQuote = "\\\"";

			return Quote + s.Replace(Quote, EscapedQuote) + Quote;
		}
	}
}
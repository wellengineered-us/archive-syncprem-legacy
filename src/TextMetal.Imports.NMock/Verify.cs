#region Using

using System;
using System.IO;
using NMock.Internal;

#endregion

namespace NMock
{
	/// <summary>
	/// Verify that a condition is met.
	/// </summary>
	public static class Verify
	{
		/// <summary>
		/// Verifies that the <paramref name="actualValue"/> is matched by the <paramref name="matcher"/>.
		/// </summary>
		/// <param name="actualValue">The actual value to match.</param>
		/// <param name="matcher">The matcher.</param>
		/// <param name="message">The error message.</param>
		/// <param name="formatArgs">The format args for the error message.</param>
		/// <exception cref="ExpectationException">Thrown if value does not match.</exception>
		public static void That(object actualValue, Matcher matcher, string message, params object[] formatArgs)
		{
			if (matcher == null) 
				throw new ArgumentNullException("matcher");

			if (!matcher.Matches(actualValue))
			{
				var writer = new DescriptionWriter();
				writer.Write(message, formatArgs);
				WriteDescriptionOfFailedMatch(writer, actualValue, matcher);

				throw new ExpectationException(writer.ToString());
			}
		}

		/// <summary>
		/// Verifies that the <paramref name="actualValue"/> is matched by the <paramref name="matcher"/>.
		/// </summary>
		/// <param name="actualValue">The actual value.</param>
		/// <param name="matcher">The matcher.</param>
		/// <exception cref="ExpectationException">Thrown if value does not match.</exception>
		public static void That(object actualValue, Matcher matcher)
		{
			if (matcher == null) 
				throw new ArgumentNullException("matcher");

			if (!matcher.Matches(actualValue))
			{
				var writer = new DescriptionWriter();
				WriteDescriptionOfFailedMatch(writer, actualValue, matcher);

				throw new ExpectationException(writer.ToString());
			}
		}

		/// <summary>
		/// Writes the description of a failed match to the specified <paramref name="writer"/>.
		/// </summary>
		/// <param name="writer">The <see cref="TextWriter"/> where the description is written to.</param>
		/// <param name="actualValue">The actual value to be written.</param>
		/// <param name="matcher">The matcher which is used for the expected value to be written.</param>
		private static void WriteDescriptionOfFailedMatch(TextWriter writer, object actualValue, Matcher matcher)
		{
			writer.WriteLine();
			writer.Write("Expected: ");
			matcher.DescribeTo(writer);
			writer.WriteLine();
			writer.Write("Actual:   ");
			writer.Write(actualValue);
		}
	}
}
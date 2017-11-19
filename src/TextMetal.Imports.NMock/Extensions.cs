using System;
using System.Reflection;

namespace NMock
{
	/// <summary>
	/// This class contains extension methods to support other classes.
	/// </summary>
	internal static class Extensions
	{
		/// <summary>
		/// Returns a string representing grammatically correctness of n times depending on the value of <paramref name="count"/>.
		/// </summary>
		/// <param name="count">An integer value representing n times.</param>
		/// <returns>The string ' time' or ' times'.</returns>
		internal static string Times(this int count)
		{
			return count + ((count == 1) ? " time" : " times");
		}

		/// <summary>
		/// Returns the default name for a type that is used to name mocks.
		/// </summary>
		/// <param name="type">The type to get the default name for.</param>
		/// <returns>Default name for the specified type.</returns>
		internal static string DefaultNameFor(this Type type)
		{
			string name = type.Name;
			int firstLower = name.FirstLowerCaseChar();

			return firstLower == name.Length ? name.ToLower() : name.Substring(firstLower - 1, 1).ToLower() + name.Substring(firstLower);
		}

		/// <summary>
		/// Finds the first lower case char in the specified string.
		/// </summary>
		/// <param name="s">The string to inspect.</param>
		/// <returns>the first lower case char in the specified string.</returns>
		private static int FirstLowerCaseChar(this string s)
		{
			int i = 0;
			while (i < s.Length && !Char.IsLower(s[i]))
			{
				i++;
			}

			return i;
		}

		internal static bool IsProperty(this MethodInfo method)
		{
			return method.IsSpecialName && (method.Name.StartsWith(Constants.GET) || method.Name.StartsWith(Constants.SET));
		}

		internal static bool IsEvent(this MethodInfo method)
		{
			return method.IsSpecialName && (method.Name.StartsWith(Constants.ADD) || method.Name.StartsWith(Constants.REMOVE));
		}

		internal static bool IsIndexer(this MethodInfo method)
		{
			return method.IsSpecialName && (method.Name == Constants.GET_ITEM || method.Name == Constants.SET_ITEM);
		}
	}
}
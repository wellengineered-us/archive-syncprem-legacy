#region Using

using System;
using System.IO;
using System.Reflection;

#endregion

namespace NMock.Matchers
{
	/// <summary>
	/// Matcher that checks whether the actual object is a <see cref="MethodInfo"/> and its name is equal to the expected name.
	/// </summary>
	public class MethodNameMatcher : Matcher
	{
		private readonly Type _declaringType;

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodNameMatcher"/> class.
		/// </summary>
		/// <param name="methodName">The expected name of the method.</param>
		public MethodNameMatcher(string methodName) : this(methodName, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodNameMatcher"/> class with a method name and declaring type
		/// </summary>
		/// <param name="methodName"></param>
		/// <param name="declaringType"></param>
		public MethodNameMatcher(string methodName, Type declaringType) : base(methodName)
		{
			_declaringType = declaringType;
		}

		/// <summary>
		/// Matches the specified object to this matcher and returns whether it matches.
		/// </summary>
		/// <param name="o">The MethodInfo to match.</param>
		/// <returns>Whether the object is a MethodInfo and its name matches the expected one.</returns>
		public override bool Matches(object o)
		{
			MethodInfo methodInfo = o as MethodInfo;

			if (methodInfo == null)
				return false;

			return Matches(methodInfo);
		}

		protected bool Matches(MethodInfo methodInfo)
		{
			if (methodInfo != null)
			{
				string methodName = methodInfo.Name;

				if (Description.Equals(methodName))
				{
					return true;
				}

				//TODO: when is this used?
				if (_declaringType != null && _declaringType == methodInfo.DeclaringType && methodName.EndsWith(Description))
				{
					return true;
				}
			}
			return false;
		}
	}
}
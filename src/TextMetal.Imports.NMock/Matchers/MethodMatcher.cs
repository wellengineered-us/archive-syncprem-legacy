#region Using

using System;
using System.IO;
using System.Reflection;

#endregion

namespace NMock.Matchers
{
	/// <summary>
	/// Matcher that checks whether the actual object is a <see cref="MethodInfo"/> and its signature matches the expected signature.
	/// </summary>
	public class MethodMatcher : MethodNameMatcher
	{
		internal readonly MethodInfo _methodInfo;
		private GenericMethodTypeParametersMatcher _genericParametersMatcher;
		ParameterInfo[] _expectedParameters;

		internal string ReturnType
		{
			get { return _methodInfo.ReturnType.FullName; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodMatcher"/> class.
		/// </summary>
		/// <param name="methodInfo">The expected method reference.</param>
		public MethodMatcher(MethodInfo methodInfo)
			: base(methodInfo.Name, methodInfo.DeclaringType)
		{
			if (methodInfo == null) throw new ArgumentNullException("methodInfo");

			_methodInfo = methodInfo;
			_genericParametersMatcher = new GenericMethodTypeParametersMatcher(_methodInfo.GetGenericArguments());
			_expectedParameters = _methodInfo.GetParameters();
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
			
			if (methodInfo.IsGenericMethodDefinition && _methodInfo.IsGenericMethod && methodInfo.GetGenericMethodDefinition() == _methodInfo.GetGenericMethodDefinition()) //then this is an internal check and not a declaration
				return true;

			if (!Matches(methodInfo)) //call to the base to match the name
				return false;

			if (!_genericParametersMatcher.MatchesTypes(methodInfo))
				return false;

			//check parameters
			var actualParameters = methodInfo.GetParameters();

			if (_expectedParameters.Length != actualParameters.Length)
			{
				return false;
			}

			for (int i = 0; i < _expectedParameters.Length; i++)
			{
				if (!_expectedParameters[i].ParameterType.IsAssignableFrom(actualParameters[i].ParameterType))
				{
					return false;
				}
			}

			return true;
		}

		public override void DescribeTo(TextWriter writer)
		{
			writer.Write(Description);
			_genericParametersMatcher.DescribeTo(writer);
		}
	}
}
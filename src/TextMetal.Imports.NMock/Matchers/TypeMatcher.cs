#region Using

using System;
using System.IO;
using NMock.Monitoring;

#endregion

/* ^ */ using System.Reflection; /* dpbullington@gmail.com ^ */

namespace NMock.Matchers
{
	/// <summary>
	/// Matcher that checks whether the actual object can be assigned to the expected type.
	/// </summary>
	public class TypeMatcher : Matcher
	{
		private readonly Type type;

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeMatcher"/> class.
		/// </summary>
		/// <param name="type">The expected type.</param>
		public TypeMatcher(Type type)
		{
			this.type = type;
		}

		/// <summary>
		/// Matches the specified object to this matcher and returns whether it matches.
		/// </summary>
		/// <param name="o">The object to match.</param>
		/// <returns>Whether the object castable to the expected type.</returns>
		public override bool Matches(object o)
		{
			if (type == null && o == null)
				return true;

			if (type == null || o == null)
				return false;

			if (o is Type && type == (Type)o)
				return true;

			/* ^ */
			var _typeInfo = this.type.GetTypeInfo();
			var _oTypeInfo = o.GetType().GetTypeInfo();
			/* dpbullington@gmail.com ^ */

			if (/* ^ */ _typeInfo.IsAssignableFrom(_oTypeInfo) /* dpbullington@gmail.com ^ */)
				return true;

			if (o is Invocation)
			{
				Invocation invocation = o as Invocation;

				/* ^ */ var _invocationTypeInfo = invocation.MethodReturnType.GetTypeInfo(); /* dpbullington@gmail.com ^ */

				/* ^ */ return _typeInfo.IsAssignableFrom(_invocationTypeInfo); /* dpbullington@gmail.com ^ */
			}

			return false;
		}

		/// <summary>
		/// Describes this matcher.
		/// </summary>
		/// <param name="writer">The text writer to which the description is added.</param>
		public override void DescribeTo(TextWriter writer)
		{
			//if (!type.IsValueType && type != typeof(string))
			//    writer.Write("type assignable to ");
			writer.Write(type.FullName);
		}
	}
}
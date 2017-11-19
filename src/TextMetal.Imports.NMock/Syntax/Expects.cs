using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMock.Internal;
using NMock.Syntax;

namespace NMock
{
	//TODO: vNext
/*
	public class Expects
	{

		public IMethodSyntax One
		{
			get
			{
				return Exactly(1);
			}
		}

		/// <summary>
		/// Creates an expectation for exactly <c>count</c> number of calls for the referenced member.
		/// </summary>
		/// <param name="count">The exact number of calls expect</param>
		/// <returns>An <see cref="IMethodSyntax{T}"/> to reference the expected call.</returns>
		public IMethodSyntax Exactly(int count)
		{
			return GetExpectationBuilder(count.Times(), Is.AtLeast(count), Is.AtMost(count));
		}

		private IMethodSyntax GetExpectationBuilder(string description, Matcher requiredCountMatcher, Matcher matchingCountMatcher)
		{
			return new ExpectationBuilder(description, requiredCountMatcher, matchingCountMatcher);
		}

	}
 */
}

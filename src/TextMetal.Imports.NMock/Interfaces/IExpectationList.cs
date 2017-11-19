using System.Collections.Generic;
using NMock.Monitoring;

namespace NMock
{
	/// <summary>
	/// Represents an ExpectationList
	/// </summary>
	internal interface IExpectationList : IExpectation, IList<IExpectation>
	{
		/// <summary>
		/// Adds an expectation.
		/// </summary>
		/// <param name="expectation">The expectation to add.</param>
		void AddExpectation(IExpectation expectation);

		/// <summary>
		/// Removes the specified expectation.
		/// </summary>
		/// <param name="expectation">The expectation to remove.</param>
		void RemoveExpectation(IExpectation expectation);

		/// <summary>
		/// Determines if the <see cref="Invocation"/> is in an ordered list.
		/// </summary>
		/// <param name="invocation">The <see cref="Invocation"/> to look for.</param>
		/// <returns></returns>
		bool ContainsOrderedExpectationFor(Invocation invocation);

		/// <summary>
		/// The nesting level of this <see cref="IExpectationList"/>
		/// </summary>
		int Depth { get; }
	}
}

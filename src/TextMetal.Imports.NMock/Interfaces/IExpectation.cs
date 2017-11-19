#region Using

using System.Collections.Generic;
using System.IO;
using NMock.Monitoring;

#endregion

namespace NMock
{
	/// <summary>
	/// Represents an expectation.
	/// </summary>
	public interface IExpectation : ISelfDescribing, IVerifyableExpectation
	{
		/// <summary>
		/// Gets a value indicating whether this instance is active.
		/// </summary>
		/// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
		bool IsActive { get; }

		/// <summary>
		/// Gets a value indicating whether this instance has been met.
		/// </summary>
		/// <value>
		///     <c>true</c> if this instance has been met; otherwise, <c>false</c>.
		/// </value>
		bool HasBeenMet { get; }

		/// <summary>
		/// Gets a value indicating that the expectation is valid.
		/// </summary>
		bool IsValid { get; }

		/// <summary>
		/// Checks whether stored expectations matches the specified invocation.
		/// </summary>
		/// <param name="invocation">The invocation to check.</param>
		/// <returns>Returns whether one of the stored expectations has met the specified invocation.</returns>
		bool Matches(Invocation invocation);

		/// <summary>
		/// Matcheses the ignoring is active.
		/// </summary>
		/// <param name="invocation">The invocation.</param>
		/// <returns></returns>
		bool MatchesIgnoringIsActive(Invocation invocation);

		/// <summary>
		/// Performs the specified invocation.
		/// </summary>
		/// <param name="invocation">The invocation.</param>
		/// <returns>A value indicating if the <paramref name="invocation"/> was successfully performed.</returns>
		bool Perform(Invocation invocation);

		/// <summary>
		/// Describes the active expectations to.
		/// </summary>
		/// <param name="writer">The writer.</param>
		void DescribeActiveExpectationsTo(TextWriter writer);

		/// <summary>
		/// Describes the unmet expectations to.
		/// </summary>
		/// <param name="writer">The writer.</param>
		void DescribeUnmetExpectationsTo(TextWriter writer);

		/// <summary>
		/// Adds all expectations to <paramref name="result"/> that are associated to <paramref name="mock"/>.
		/// </summary>
		/// <param name="mock">The mock for which expectations are queried.</param>
		/// <param name="result">The result to add matching expectations to.</param>
		void QueryExpectationsBelongingTo(IMockObject mock, IList<IExpectation> result);

		/// <summary>
		/// Returns a list of expectation validation errors
		/// </summary>
		/// <returns></returns>
		string ValidationErrors();
	}
}
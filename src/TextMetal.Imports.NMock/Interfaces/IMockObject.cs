#region Using

using System.Collections.Generic;
using System.Reflection;
using NMock.Monitoring;
using NMock.Proxy;

#endregion

namespace NMock
{
	/// <summary>
	/// Interface for mocks.
	/// </summary>
	public interface IMockObject
	{
		// Q: What happens if any of these members are also defined on class/interface to be mocked???
		// - Implement explicitly?

		/// <summary>
		/// Gets the name of the mock instance. This is often used in error messages
		/// to identify a specific mock instance.
		/// </summary>
		string MockName { get; }

		/// <summary>
		/// Gets a <see cref="CompositeType"/> that represents all types to be mocked.
		/// </summary>
		CompositeType MockedTypes { get; }

		/// <summary>
		/// Retrieves all matching methods on this mock. 
		/// </summary>
		/// <param name="methodMatcher">A Matcher to use in identifying the methods.</param>
		/// <returns>A list of zero or more matching MethodInfo instances.</returns>
		IList<MethodInfo> GetMethodsMatching(Matcher methodMatcher);

		/// <summary>
		/// Adds an expectation to this mock.
		/// </summary>
		/// <param name="expectation">The expectation to add.</param>
		void AddExpectation(IExpectation expectation);

		/// <summary>
		/// Raises an event on this mock.
		/// </summary>
		/// <param name="eventName">Name of the event to fire.</param>
		/// <param name="args">The arguments passed to the event.</param>
		void RaiseEvent(string eventName, params object[] args);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="invocation"></param>
		void ProcessEventHandlers(Invocation invocation);

		/// <summary>
		/// Gets or sets a value indicating if this <see cref="Mock{T}"/> should ignore unexpected invocations to properties, methods, or events.
		/// </summary>
		/// <remarks>
		/// Use the property to have a Mock ignore calls with no expectations.  By default, this works fine for property setters, void methods,
		/// and events.  Property getters and non-void methods will need to indicate how they will be implemented as they <i>need</i> to return
		/// a value.
		/// </remarks>
		bool IgnoreUnexpectedInvocations { get; set; }

		/// <summary>
		/// A <see cref="Dictionary{TKey,TValue}"/> that stores property values from internal operations.
		/// </summary>
		Dictionary<string, object> InterceptedValues { get; set; }
	}
}
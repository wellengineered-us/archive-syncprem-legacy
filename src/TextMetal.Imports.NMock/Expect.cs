using System;

namespace NMock
{
	/// <summary>
	/// Used to set up expectations on types that are not mocks.
	/// </summary>
	public static class Expect
	{
		/// <summary>
		/// Creates an expectation related to the type of <see cref="Exception"/> thrown.
		/// </summary>
		/// <param name="action">The method that should cause the exception.  Hint: use () =&gt; to convert a method to an action.</param>
		/// <returns>An object to complete the expectation</returns>
		/// <remarks>Use this in place of a [ExpectedException] unit test attribute.</remarks>
		public static That That(Action action)
		{
			if (action == null) 
				throw new ArgumentNullException("action");

			return new That(action);
		}

		/// <summary>
		/// Default expectation, specifies that a method, property, etc. that has to be called at least once.
		/// </summary>
		/// <param name="receiver">The receiver.</param>
		/// <returns>Returns a receiver of a method, property, etc. that has to be called at least once.</returns>
		public static Expects<T> On<T>(T receiver) where T : class
		{
			if(typeof(T) == typeof(Mock<>))
			{
				throw new ArgumentException("Do not use the On method with a type that is a Mock<T>.  Use this method on types returned from MockFactory.NewMock<T>().");
			}
			return new Expects<T>(receiver);
		}

	}
}
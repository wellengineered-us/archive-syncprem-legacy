#region Using

using System;
using NMock.Internal;
using NMock.Monitoring;

#endregion

namespace NMock
{
	/// <summary>
	/// Exception representing an expectation exception.
	/// </summary>
	public class ExpectationException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExpectationException"/> class.
		/// </summary>
		public ExpectationException()
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExpectationException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public ExpectationException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExpectationException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		protected ExpectationException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>
		/// Creates and returns a string representation of the current exception.
		/// </summary>
		/// <returns>
		/// A string representation of the current exception.
		/// </returns>
		public override string ToString()
		{
			return Environment.NewLine + base.ToString();
		}
	}

	/// <summary>
	/// Represents an unexpected action during the course of exercising a unit test
	/// </summary>
	public class UnexpectedInvocationException : ExpectationException
	{
		private const string MESSAGE_PREFIX = "Unexpected invocation of:";
		private const string UNEXPECTED_PROPERTY_SETTER_PREFIX = "Unexpected property setter:";
		private const string UNEXPECTED_PROPERTY_GETTER_PREFIX = "Unexpected property getter:";
		private readonly string _message;

		internal MockFactory Factory { get; private set; }

		/// <summary>
		/// Constructs a <see cref="UnexpectedInvocationException"/> with the given parameters.
		/// </summary>
		/// <param name="factory">The MockFactory that threw this exception</param>
		/// <param name="invocation">The unexpected invocation</param>
		/// <param name="expectations">The expectations collection to describe</param>
		/// <param name="message">A message to help the user understand what was unexpected</param>
		internal UnexpectedInvocationException(MockFactory factory, Invocation invocation, IExpectationList expectations, string message)
		{
			if (factory == null)
				throw new ArgumentNullException("factory");
			if (invocation == null)
				throw new ArgumentNullException("invocation");
			if (expectations == null) 
				throw new ArgumentNullException("expectations");

			Factory = factory;

			var writer = new DescriptionWriter();
			writer.WriteLine();
			if (invocation.IsPropertySetAccessor)
				writer.WriteLine(UNEXPECTED_PROPERTY_SETTER_PREFIX);
			else if (invocation.IsPropertyGetAccessor)
				writer.WriteLine(UNEXPECTED_PROPERTY_GETTER_PREFIX);
			else
				writer.WriteLine(MESSAGE_PREFIX);
			writer.Write("  ");
			((ISelfDescribing)invocation).DescribeTo(writer);
			writer.Write(message);
			//expectations.DescribeActiveExpectationsTo(writer);
			//expectations.DescribeUnmetExpectationsTo(writer);
			expectations.DescribeTo(writer);

			_message = writer.ToString();
		}

		/// <summary>
		/// Gets the exception's message
		/// </summary>
		public override string Message
		{
			get
			{
				return _message;
			}
		}
	}

	/// <summary>
	/// Represents an expectation that was not met after <see cref="MockFactory.VerifyAllExpectationsHaveBeenMet()"/> has been called
	/// </summary>
	public class UnmetExpectationException : ExpectationException
	{
		/// <summary>
		/// Initializes the exception with a message
		/// </summary>
		/// <param name="message"></param>
		public UnmetExpectationException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Wraps an exception and provides a message
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public UnmetExpectationException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}

	/// <summary>
	/// Represents an expectation that was not completely filled out
	/// </summary>
	public class IncompleteExpectationException : ExpectationException
	{
		/// <summary>
		/// Initializes an exception with a message
		/// </summary>
		/// <param name="message"></param>
		public IncompleteExpectationException(string message) : base(message)
		{}

		public IncompleteExpectationException(IExpectation expectation)
			: base(expectation.ValidationErrors())
		{
		}
	}
}
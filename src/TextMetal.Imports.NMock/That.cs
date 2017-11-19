using System;
using NMock.Internal;

namespace NMock
{
	/// <summary>
	/// A syntax class to setup expectations on methods when they throw exceptions.
	/// </summary>
	public class That
	{
		private readonly Action _action;
		private Exception _exception;

		/// <summary>
		/// Creates an instance of this class specifying the action that will throw an exception
		/// </summary>
		/// <param name="action"></param>
		public That(Action action)
		{
			_action = action;
		}

		/// <summary>
		/// Indicates that this method will throw an <see cref="Exception"/>.
		/// </summary>
		public void Throws()
		{
			Throws<Exception>();
		}

		/// <summary>
		/// Indicates that this method will throw an <see cref="Exception"/>.
		/// </summary>
		/// <param name="matchers">An array of matchers to match the exception string.</param>
		public void Throws(params Matcher[] matchers)
		{
			Throws<Exception>(string.Empty, matchers);
		}

		/// <summary>
		/// Indicates that this method will throw an <see cref="Exception"/>.
		/// </summary>
		/// <param name="comment">A description of the reason for this expectation.</param>
		public void Throws(string comment)
		{
			Throws<Exception>(comment, null);
		}

		/// <summary>
		/// Indicates that this method will throw an <see cref="Exception"/>.
		/// </summary>
		/// <param name="comment">A description of the reason for this expectation.</param>
		/// <param name="matchers">An array of matchers to match the exception string.</param>
		public void Throws(string comment, params Matcher[] matchers)
		{
			Throws<Exception>(comment, matchers);
		}

		/// <summary>
		/// Indicates that this method will throw an <see cref="Exception"/>.
		/// </summary>
		/// <typeparam name="T">The type of <see cref="Exception"/> to throw.</typeparam>
		public void Throws<T>()
			where T : Exception
		{
			Throws<T>(string.Empty, null);
		}

		/// <summary>
		/// Indicates that this method will throw an <see cref="Exception"/>.
		/// </summary>
		/// <typeparam name="T">The type of <see cref="Exception"/> to throw.</typeparam>
		/// <param name="comment">A description of the reason for this expectation.</param>
		public void Throws<T>(string comment)
			where T : Exception
		{
			Throws<T>(comment, null);
		}

		/// <summary>
		/// Indicates that this method will throw an <see cref="Exception"/>.
		/// </summary>
		/// <typeparam name="T">The type of <see cref="Exception"/> to throw.</typeparam>
		/// <param name="matchers">An array of matchers to match the exception string.</param>
		public void Throws<T>(params Matcher[] matchers)
			where T : Exception
		{
			Throws<T>(string.Empty, matchers);
		}

		/// <summary>
		/// Indicates that this method will throw an <see cref="Exception"/>.
		/// </summary>
		/// <typeparam name="T">The type of <see cref="Exception"/> to throw.</typeparam>
		/// <param name="comment">A description of the reason for this expectation.</param>
		/// <param name="matchers">An array of matchers to match the exception string.</param>
		public void Throws<T>(string comment, params Matcher[] matchers)
			where T : Exception
		{
			try
			{
				_action();
			}
			catch (T ex)
			{
				_exception = ex;

				if (matchers != null)
				{
					foreach (var matcher in matchers)
					{
						if (!matcher.Matches(ex.ToString()))
						{
							var writer = new DescriptionWriter();
							writer.Write("Matching failed due to ");
							matcher.DescribeTo(writer);
							throw new Exception(Environment.NewLine + writer + Environment.NewLine, ex);
						}
					}
				}

				//all matchers matched so this was expected

				var uie = ex as UnexpectedInvocationException;
				if (uie != null)
				{
					//clear this exception because it was expected
					uie.Factory.ClearException();
				}
			}

			if (_exception == null)
			{
				if (string.IsNullOrEmpty(comment))
				{
					throw new UnmetExpectationException("The expected action did not throw an exception when it was invoked.");
				}
				else
				{
					throw new UnmetExpectationException(comment);
				}
			}
		}
	}
}
#region Using

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NMock.Actions;
using NMock.Matchers;
using NMock.Monitoring;

#endregion

namespace NMock.Internal
{
	internal class BuildableExpectation : IExpectation, IVerifyableExpectation
	{
		private readonly string _expectationDescription;
		private readonly Matcher _requiredCountMatcher;
		private readonly Matcher _matchingCountMatcher;

		private readonly List<IAction> _actions = new List<IAction>();
		private readonly List<Matcher> _extraMatchers = new List<Matcher>();
		private int _callCount;
		private string _expectationComment;

		private string _methodSeparator = ".";

		/// <summary>
		/// Initializes a new instance of the <see cref="BuildableExpectation"/> class.
		/// </summary>
		/// <param name="expectationDescription">The expectation description.</param>
		/// <param name="requiredCountMatcher">The required count matcher.</param>
		/// <param name="matchingCountMatcher">The matching count matcher.</param>
		public BuildableExpectation(string expectationDescription, Matcher requiredCountMatcher, Matcher matchingCountMatcher)
		{
			_expectationDescription = expectationDescription;
			_requiredCountMatcher = requiredCountMatcher;
			_matchingCountMatcher = matchingCountMatcher;

			ArgumentsMatcher = new ArgumentsMatcher();

			IsValid = false;
		}

		public IMockObject Receiver { private get; set; }

		private Matcher _methodMatcher;
		public Matcher MethodMatcher
		{
			internal get
			{
				return _methodMatcher;
			}
			set
			{
				_methodMatcher = value;
				Validate();
			}
		}

		private Matcher _argumentsMatcher;
		public Matcher ArgumentsMatcher
		{
			private get { return _argumentsMatcher; }
			set
			{
				_argumentsMatcher = value;
				Validate();
			}
		}

		#region IExpectation Members

		public bool IsActive
		{
			get
			{
				return _matchingCountMatcher.Matches(_callCount + 1);
			}
		}

		public bool HasBeenMet
		{
			get
			{
				var maximumMet = _matchingCountMatcher.Matches(_callCount);
				var minimumMet =  _requiredCountMatcher.Matches(_callCount);

				return maximumMet && minimumMet;
			}
		}

		public bool IsValid { get; set; }

		/// <summary>
		/// Checks whether stored expectations matches the specified invocation.
		/// </summary>
		/// <param name="invocation">The invocation to check.</param>
		/// <returns>Returns whether one of the stored expectations has met the specified invocation.</returns>
		public bool Matches(Invocation invocation)
		{
			if (!IsActive)
				return false;
			return MatchesIgnoringIsActive(invocation);
		}

		public bool MatchesIgnoringIsActive(Invocation invocation)
		{
			if (Receiver != invocation.Receiver)
				return false;
			if (!MethodMatcher.Matches(invocation.Method))
				return false;
			if (!ArgumentsMatcher.Matches(invocation))
				return false;
			if (!ExtraMatchersMatch(invocation))
				return false;
			return true;
		}

		public bool Perform(Invocation invocation)
		{
			_callCount++;
			ProcessEventHandlers(invocation);
			foreach (IAction action in _actions)
			{
				action.Invoke(invocation);
			}

			//check that return value was set
			if (invocation.Result == Missing.Value && invocation.Method.ReturnType != typeof(void))
			{
				string message = string.Empty;

				if (invocation.IsPropertyGetAccessor)
					message = string.Format(
						"An expectation match was found but the expectation was incomplete.  A return value for property '{0}' on '{1}' mock must be set.",
						invocation.MethodName,
						Receiver.MockName);
				else if (invocation.IsMethod)
					message = string.Format(
						"An expectation match was found but the expectation was incomplete.  A return value for method '{0}' on '{1}' mock must be set.",
						invocation.MethodSignature,
						Receiver.MockName);

				var exception = new IncompleteExpectationException(message);

				throw exception;
			}
			return true;
		}

		public void DescribeActiveExpectationsTo(TextWriter writer)
		{
			if (IsActive)
			{
				DescribeTo(writer);
			}
		}

		public void DescribeUnmetExpectationsTo(TextWriter writer)
		{
			if (!HasBeenMet)
			{
				DescribeTo(writer);
			}
		}


		/// <summary>
		/// Adds itself to the <paramref name="result"/> if the <see cref="Receiver"/> matches
		/// the specified <paramref name="mock"/>.
		/// </summary>
		/// <param name="mock">The mock for which expectations are queried.</param>
		/// <param name="result">The result to add matching expectations to.</param>
		public void QueryExpectationsBelongingTo(IMockObject mock, IList<IExpectation> result)
		{
			if (Receiver == mock)
			{
				result.Add(this);
			}
		}

		#endregion

		#region IVerifyableExpectation

		public void Assert()
		{
			if(!HasBeenMet)
			{
				var writer = new DescriptionWriter();
				DescribeTo(writer);
				throw new UnmetExpectationException(writer.ToString());
			}
		}

		#endregion

		internal void IncreaseCallCount()
		{
			_callCount++;
		}

		public void AddInvocationMatcher(Matcher matcher)
		{
			_extraMatchers.Add(matcher);
			Validate();
		}

		public void AddAction(IAction action)
		{
			_actions.Add(action);
			//Validate();
		}

		public void AddComment(string comment)
		{
			_expectationComment = comment;
		}

		public void DescribeAsIndexer()
		{
			_methodSeparator = string.Empty;
		}

		private static void ProcessEventHandlers(Invocation invocation)
		{
			if (invocation.IsEventAccessor)
			{
				var mockObject = invocation.Receiver as IMockObject;
				if (mockObject != null)
				{
					mockObject.ProcessEventHandlers(invocation);
				}
			}
		}

		private bool ExtraMatchersMatch(Invocation invocation)
		{
			return _extraMatchers.All(matcher => matcher.Matches(invocation));
		}

		public void DescribeTo(TextWriter writer)
		{
			if (MethodMatcher is MethodMatcher)
			{
				writer.Write(((MethodMatcher)MethodMatcher).ReturnType);
				writer.Write(" ");
			}

			writer.Write(Receiver.MockName);
			writer.Write(_methodSeparator);
			if (MethodMatcher != null)
				MethodMatcher.DescribeTo(writer);
			ArgumentsMatcher.DescribeTo(writer);

			if (_extraMatchers.Count > 0)
			{
				writer.Write(" Matching[");
				for (int i = 0; i < _extraMatchers.Count; i++)
				{
					if (i != 0)
						writer.Write(", ");

					_extraMatchers[i].DescribeTo(writer);
				}
				writer.Write("]");
			}

			if (_actions.Count > 0)
			{
				writer.Write(" will ");
				(_actions[0]).DescribeTo(writer);
				for (int i = 1; i < _actions.Count; i++)
				{
					if (i != 0)
						writer.Write(", ");
					_actions[i].DescribeTo(writer);
				}
			}

			if (IsValid)
			{
				writer.Write(" [EXPECTED: ");
				writer.Write(_expectationDescription);

				writer.Write(" CALLED: ");
				writer.Write(_callCount);
				writer.Write(" time");
				if (_callCount != 1)
				{
					writer.Write("s");
				}
				writer.Write("]");
			}
			else
			{
				writer.Write(" [EXPECTATION NOT VALID because of runtime error or incomplete setup]");
			}

			if (!string.IsNullOrEmpty(_expectationComment))
			{
				writer.Write(" Comment: ");
				writer.Write(_expectationComment);
			}
			writer.WriteLine();
		}

		public override string ToString()
		{
			var writer = new DescriptionWriter();
			DescribeTo(writer);
			return writer.ToString();
		}

		private void Validate()
		{
			//must have a method matcher
			if (!IsMethodMatcherValid())
			{
				IsValid = false;
				return;
			}

			if (!IsArgumentsMatcherValidForProperties())
			{
				IsValid = false;
				return;
			}

			IsValid = true;

			/*
			if (methodMatcher != null && methodMatcher._methodInfo.ReturnType != typeof(void))
			{
				var found = false;
				foreach (var action in _actions)
				{
					var returnAction = action as IReturnAction;
					if ((action is ThrowAction) || (returnAction != null && methodMatcher._methodInfo.ReturnType.IsAssignableFrom(returnAction.ReturnType)))
					{
						found = true;
					}
				}

				if (!found)
				{
					IsValid = false;
					return;
				}
			}

			//check argumentmatchers
			if (methodMatcher != null)
			{
				var @params = methodMatcher._methodInfo.GetParameters();
				var argumentsMatcher = ArgumentsMatcher as ArgumentsMatcher;
				if ((argumentsMatcher != null && @params.Count() != argumentsMatcher.Count) 
					|| (argumentsMatcher.Count > 0 && argumentsMatcher[0].GetType() != typeof(AlwaysMatcher)))
				{
					IsValid = false;
					return;
				}
			}

			IsValid = true;
			 */
		}

		/// <summary>
		/// Returns a list of validation errors
		/// </summary>
		/// <returns></returns>
		public string ValidationErrors()
		{
			var errors = new StringBuilder();

			if (!IsMethodMatcherValid())
			{
				errors.AppendLine("A method matcher is missing on the mock: " + Receiver.MockName);
			}

			if (!IsArgumentsMatcherValidForProperties())
			{
				errors.AppendLine("A property is missing a matcher on the mock: " + Receiver.MockName);
			}

			/*
			var methodMatcher = _methodMatcher as MethodMatcher;
			if (methodMatcher != null && methodMatcher._methodInfo.ReturnType != typeof(void))
			{
				var found = false;
				foreach (var action in _actions)
				{
					var returnAction = action as IReturnAction;
					if ((action is ThrowAction) || (returnAction != null && methodMatcher._methodInfo.ReturnType.IsAssignableFrom(returnAction.ReturnType)))
					{
						found = true;
					}
				}

				if (!found)
				{
					errors.AppendLine("At least one IReturnAction or ThrowAction must be specified for a non-void method.  Matcher: " + methodMatcher);
				}
			}

			//check argumentmatchers
			if (methodMatcher != null)
			{
				var @params = methodMatcher._methodInfo.GetParameters();
				var argumentsMatcher = ArgumentsMatcher as ArgumentsMatcher;
				if ((argumentsMatcher != null && @params.Count() != argumentsMatcher.Count)
					|| (argumentsMatcher.Count > 0 && argumentsMatcher[0].GetType() != typeof(AlwaysMatcher)))
				{
					errors.AppendLine(string.Format("The number of argument matcher(s) specified is invalid.  The method expects {0} parameters and {1} matchers were specified.  (If you are matching a method, use the 'MethodWith' method or '.With(...)' after your method matcher.)", @params.Count(), argumentsMatcher.Count));
				}
			}

			*/

			return errors.ToString();
		}

		private bool IsMethodMatcherValid()
		{
			return _methodMatcher != null;
		}

		private bool IsArgumentsMatcherValidForProperties()
		{
			var matcher = RemoveDescriptionOverride(_methodMatcher);

			var methodMatcher = matcher as MethodNameMatcher;

			if (methodMatcher != null)
			{
				if (methodMatcher.Description.StartsWith(Constants.SET)) //set prop needs an argument
				{
					var argumentsMatcher = ArgumentsMatcher as ArgumentsMatcher;
					if (argumentsMatcher != null && argumentsMatcher.Count == 0)
					{
						return false;
					}
				}
			}

			return true;
		}

		private Matcher RemoveDescriptionOverride(Matcher matcher)
		{
			var descriptionOverride = matcher as DescriptionOverride;

			return descriptionOverride != null ? RemoveDescriptionOverride(descriptionOverride.WrappedMatcher) : matcher;
		}
	}
}
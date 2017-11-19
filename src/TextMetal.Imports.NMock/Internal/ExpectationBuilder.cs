#region Using

using System;
using System.Reflection;
using NMock.Actions;
using NMock.Matchers;
using NMock.Syntax;

#endregion

namespace NMock.Internal
{
	internal class ExpectationBuilder : IMethodSyntax, IArgumentSyntax, IMatchSyntax
	{
		internal const string STUB_DESCRIPTION = "Stub";

		protected internal readonly BuildableExpectation BuildableExpectation;
		protected internal IMockObject MockObject;
		private MethodInfo _method;

		/// <summary>
		/// A flag that specifies if the expectation was set up using 'MethodWith' in which case an ExpectationException is not thrown
		/// when the method takes no arguments.
		/// </summary>
		protected bool UsingMethodWith;

		private ExpectationBuilder(string description, Matcher requiredCountMatcher, Matcher matchingCountMatcher)
		{
			BuildableExpectation = new BuildableExpectation(description, requiredCountMatcher, matchingCountMatcher);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExpectationBuilder"/> class.
		/// </summary>
		/// <param name="description">The description.</param>
		/// <param name="requiredCountMatcher">The required count matcher.</param>
		/// <param name="matchingCountMatcher">The matching count matcher.</param>
		/// <param name="proxy"></param>
		protected ExpectationBuilder(string description, Matcher requiredCountMatcher, Matcher matchingCountMatcher, object proxy)
			: this(description, requiredCountMatcher, matchingCountMatcher)
		{
			On(proxy as IMockObject);
		}

		#region IArgumentSyntax Members

		/// <summary>
		/// Defines the arguments that are expected on the method call.
		/// </summary>
		/// <param name="expectedArguments">The expected arguments.</param>
		/// <returns>Matcher syntax.</returns>
		public IActionSyntax With(params object[] expectedArguments)
		{
			// check that non-matchers match the method definition
			if (_method != null)
			{
				var parameters = _method.GetParameters();

				//Test for "null"
				// Handle the case of .With(null)
				if(parameters.Length == 1 && expectedArguments == null)
				{
					expectedArguments = new object[] {null};
				}

				for (int i = 0; i < expectedArguments.Length; i++)
				{
					var expectedArgument = expectedArguments[i];
					if (expectedArgument as Matcher == null)
					{
						var type = parameters[i].ParameterType;

						/* ^ */ var _typeInfo = type.GetTypeInfo(); /* dpbullington@gmail.com ^ */

						var isNull = expectedArgument == null;
						/* ^ */ var isNullable = _typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>) || !_typeInfo.IsValueType; /* dpbullington@gmail.com ^ */
						var isOfType = type.IsInstanceOfType(expectedArgument);

						if(
							(isNull && !isNullable)
							|| (!isNull && !isOfType)
							)
						{
							var typeName = expectedArgument == null ? "null" : expectedArgument.GetType().FullName;

							throw new InvalidOperationException(String.Format("An argument of type '{0}' at index {1} does not match the method definition of '{2}' at {1} of the {3} method.", typeName, i, type, _method.Name));
						}
					}
				}
			}
			return With(ArgumentMatchers(expectedArguments));
		}

		/// <summary>
		/// Defines that no arguments are expected on the method call.
		/// </summary>
		/// <returns>Matcher syntax.</returns>
		public IActionSyntax WithNoArguments()
		{
			return With(new DescriptionOverride("(no arguments)", new ArgumentsMatcher()));
		}

		/// <summary>
		/// Defines that all arguments are allowed on the method call.
		/// </summary>
		/// <returns>Matcher syntax.</returns>
		public IActionSyntax WithAnyArguments()
		{
			return With(new AlwaysMatcher(true, "(any arguments)"));
		}

		protected IActionSyntax With(params Matcher[] argumentMatchers)
		{
			return With(new ArgumentsMatcher(argumentMatchers));
		}

		public IMatchSyntax WithArguments(params Matcher[] argumentMatchers)
		{
			return With(new ArgumentsMatcher(argumentMatchers));
		}

		protected internal IMatchSyntax With(Matcher matcher)
		{
			if(_method != null && _method.GetParameters().Length == 0 && !UsingMethodWith)
			{
				throw new ExpectationException("Using a 'With' clause on a method that accepts 0 parameters is not valid.");
			}

			BuildableExpectation.ArgumentsMatcher = matcher;
			return this;
		}

		#endregion

		/// <summary>
		/// Defines a matching criteria.
		/// </summary>
		/// <param name="matcher">The matcher.</param>
		/// <returns>
		/// Action syntax defining the action to take.
		/// </returns>
		public IActionSyntax Matching(Matcher matcher)
		{
			BuildableExpectation.AddInvocationMatcher(matcher);
			return this;
		}

		/// <summary>
		/// Defines what will happen.
		/// </summary>
		/// <param name="actions">The actions to take.</param>
		/// <returns>
		/// Returns the comment syntax defined after will.
		/// </returns>
		public ICommentSyntax Will(params IAction[] actions)
		{
			//check the return type against the return value of the method
			if(_method != null && _method.ReturnType != typeof(void))
			{
				foreach (var action in actions)
				{
					var returnAction = action as IReturnAction;
					if (returnAction != null && !_method.ReturnType.IsAssignableFrom(returnAction.ReturnType))
					{
						throw new InvalidOperationException(String.Format("The return type '{0}' does not match the return type '{1}' on method {2}.", returnAction.ReturnType, _method.ReturnType, _method.Name));
					}
				}
			}

			foreach (IAction action in actions)
			{
				BuildableExpectation.AddAction(action);
			}

			return this;
		}

		/// <summary>
		/// Adds a comment for the expectation that is added to the error message if the expectation is not met.
		/// </summary>
		/// <param name="comment">The comment that is shown in the error message if this expectation is not met.
		/// You can describe here why this expectation has to be met.</param>
		public IVerifyableExpectation Comment(string comment)
		{
			BuildableExpectation.AddComment(comment);
			return BuildableExpectation;
		}

		#region IMethodSyntax Members

		/// <summary>
		/// Methods the specified method name.
		/// </summary>
		/// <param name="methodName">Name of the method.</param>
		/// <returns></returns>
		public IArgumentSyntax ProtectedMethod(string methodName)
		{
			if (String.IsNullOrEmpty(methodName))
				throw new ArgumentNullException("methodName");

			return Method(new MethodNameMatcher(methodName, MockObject.MockedTypes.PrimaryType));
		}

		//TODO: vNext
		/*
		public IAutoArgumentSyntax<T> Method<T>(Expression<Func<T>> expression) where T : class
		{
			MethodCallExpression methodCallExpression = expression.Body as MethodCallExpression;

			if (methodCallExpression != null)
			{
				ConstantExpression constantExpression = methodCallExpression.Object as ConstantExpression;
				On(constantExpression.Value as IMockObject);
				Method(methodCallExpression.Method);
				return new PropertyArgumentBuilder<T>(this);
			}

			throw new ArgumentException("Expression did not evaluate to a method call.");
		}
		*/

		protected IArgumentSyntax Method(MethodInfo methodInfo)
		{
			if (methodInfo == null)
				throw new ArgumentNullException("methodInfo");

			_method = methodInfo;

			return Method(new MethodMatcher(methodInfo));
		}

		/// <summary>
		/// Defines a method.
		/// </summary>
		/// <param name="methodMatcher">Matcher for matching the method on an invocation.</param>
		/// <returns>
		/// Argument syntax defining the arguments of the method.
		/// </returns>
		private IArgumentSyntax Method(Matcher methodMatcher)
		{
			//I don't think this is needed anymore because we aren't doing string matching.
			EnsureMatchingMethodExistsOnMock(methodMatcher, "a method matching " + methodMatcher);

			BuildableExpectation.MethodMatcher = methodMatcher;

			return this;
		}

		/// <summary>
		/// Gets the property.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns></returns>
		protected void GetProperty(string propertyName)
		{
			Matcher methodMatcher = new DescriptionOverride(propertyName, new MethodNameMatcher(Constants.GET + propertyName, MockObject.MockedTypes.PrimaryType));

			EnsureMatchingMethodExistsOnMock(methodMatcher, "a getter for property " + propertyName);

			BuildableExpectation.MethodMatcher = methodMatcher;
			BuildableExpectation.ArgumentsMatcher = new DescriptionOverride(String.Empty, new ArgumentsMatcher());
		}

		/// <summary>
		/// Sets the property.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns></returns>
		protected internal IValueSyntaxBuilder SetProperty(string propertyName)
		{
			Matcher methodMatcher = new DescriptionOverride(propertyName + " = ", new MethodNameMatcher(Constants.SET + propertyName, MockObject.MockedTypes.PrimaryType));

			EnsureMatchingMethodExistsOnMock(methodMatcher, "a setter for property " + propertyName);

			BuildableExpectation.MethodMatcher = methodMatcher;
			return new PropertyValueBuilder(this);
		}

		protected IValueSyntaxBuilder SetIndexer(params object[] expectedArguments)
		{
			Matcher methodMatcher = new DescriptionOverride(String.Empty, new MethodNameMatcher(Constants.SET_ITEM, MockObject.MockedTypes.PrimaryType));
			EnsureMatchingMethodExistsOnMock(methodMatcher, "an indexed setter");

			BuildableExpectation.DescribeAsIndexer();
			BuildableExpectation.MethodMatcher = methodMatcher;
			return new IndexSetterBuilder(BuildableExpectation, this).WithParameters(expectedArguments);
		}

		#endregion

		#region IVerifyableExpectation

		public void Assert()
		{
			BuildableExpectation.Assert();
		}

		#endregion



		/// <summary>
		/// Defines the receiver.
		/// </summary>
		/// <param name="receiver">The dynamic mock on which the expectation or stub is applied.</param>
		/// <returns>Method syntax defining the method, property or event.</returns>
		private void On(IMockObject receiver)
		{
			MockObject = receiver;

			BuildableExpectation.Receiver = MockObject;
			MockObject.AddExpectation(BuildableExpectation);
		}

		/// <summary>
		/// Converts the object array into a List of matchers.
		/// </summary>
		/// <param name="expectedArguments">The expected arguments.</param>
		/// <returns></returns>
		protected static Matcher[] ArgumentMatchers(object[] expectedArguments)
		{
			var matchers = new Matcher[expectedArguments.Length];
			for (var i = 0; i < matchers.Length; i++)
			{
				var o = expectedArguments[i];
				matchers[i] = (o is Matcher) ? (Matcher)o : new EqualMatcher(o);
			}

			return matchers;
		}

		/// <summary>
		/// Ensures the matching method exists on mock.
		/// </summary>
		/// <param name="methodMatcher">The method matcher.</param>
		/// <param name="methodDescription">The method description.</param>
		protected virtual void EnsureMatchingMethodExistsOnMock(Matcher methodMatcher, string methodDescription)
		{
			var matches = MockObject.GetMethodsMatching(methodMatcher);

			if (matches.Count == 0)
			{
				throw new ArgumentException("mock object " + MockObject.MockName + " does not have " + methodDescription);
			}

			foreach (var methodInfo in matches)
			{
				// Note that methods on classes that are implementations of an interface
				// method are considered virtual regardless of whether they are actually
				// marked as virtual or not. Hence the additional call to IsFinal.
				if ((methodInfo.IsVirtual || methodInfo.IsAbstract) && !methodInfo.IsFinal)
				{
					return;
				}
			}

			throw new ArgumentException("mock object " + MockObject.MockName + " has " + methodDescription + ", but it is not virtual or abstract");
		}

		#region Nested type: IndexSetterBuilder

		private class IndexSetterBuilder : IValueSyntaxBuilder
		{
			private readonly ExpectationBuilder _builder;
			private readonly BuildableExpectation _expectation;
			private Matcher[] _matchers;

			/// <summary>
			/// Initializes a new instance of the <see cref="IndexSetterBuilder"/> class.
			/// </summary>
			/// <param name="expectation">The expectation.</param>
			/// <param name="builder">The builder.</param>
			public IndexSetterBuilder(BuildableExpectation expectation, ExpectationBuilder builder)
			{
				_expectation = expectation;
				_builder = builder;
			}

			#region ISetIndexerSyntax Members

			public IValueSyntaxBuilder WithParameters(params object[] expectedArguments)
			{
				var indexerMatchers = ArgumentMatchers(expectedArguments);

				//initialize the matchers to hold one more than the args passed in
				_matchers = new Matcher[indexerMatchers.Length + 1];

				//copy the 
				Array.Copy(indexerMatchers, _matchers, indexerMatchers.Length);
				return this;
			}

			#endregion

			#region IValueSyntaxBuilder Members

			public void Will(params IAction[] action)
			{
				_builder.Will(action);
			}

			public IActionSyntax To(Matcher matcher)
			{
				//add the matcher for the value being set to the indexer matchers
				_matchers[_matchers.Length - 1] = matcher;

				//set up the display
				_expectation.ArgumentsMatcher = new IndexSetterArgumentsMatcher(_matchers);
				return _builder;
			}

			#endregion
		}

		#endregion

		#region Nested type: PropertyValueBuilder

		private class PropertyValueBuilder : IValueSyntaxBuilder
		{
			private readonly ExpectationBuilder _builder;

			/// <summary>
			/// Initializes a new instance of the <see cref="PropertyValueBuilder"/> class.
			/// </summary>
			/// <param name="builder">The builder.</param>
			public PropertyValueBuilder(ExpectationBuilder builder)
			{
				_builder = builder;
			}

			#region IValueSyntaxBuilder Members

			public void Will(params IAction[] action)
			{
				_builder.Will(action);
			}

			public IActionSyntax To(Matcher valueMatcher)
			{
				return _builder.With(new ArgumentsMatcher(valueMatcher));
			}

			#endregion
		}

		#endregion
	}
}
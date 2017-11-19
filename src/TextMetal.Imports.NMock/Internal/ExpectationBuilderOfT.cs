using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NMock.Matchers;
using NMock.Monitoring;
using NMock.Syntax;

namespace NMock.Internal
{
	internal sealed class ExpectationBuilder<T> : ExpectationBuilder, IMethodSyntax<T>
		where T : class
	{
		public ExpectationBuilder(string description, Matcher requiredCountMatcher, Matcher matchingCountMatcher, object proxy)
			: base(description, requiredCountMatcher, matchingCountMatcher, proxy)
		{
			/* ^ */ var _typeInfo = typeof(T).GetTypeInfo(); /* dpbullington@gmail.com ^ */

			if (/* ^ */ !_typeInfo.IsInterface && !_typeInfo.IsClass /* dpbullington@gmail.com ^ */)
				throw new InvalidOperationException("The type mocked is not a class or interface.");
		}

		internal bool IsStub { private get; set; }

		private T mock
		{
			get
			{
				return MockObject as T;
			}
		}

		#region Method

		public IArgumentSyntax Method(Expression<Action<T>> expression)
		{
			return Method(((MethodCallExpression)expression.Body).Method);
		}

		public IAutoArgumentSyntax<TProperty> Method<TProperty>(Expression<Func<T, TProperty>> expression)
		{
			Method(((MethodCallExpression)expression.Body).Method);
			return new PropertyArgumentBuilder<TProperty>(this);
		}

		public IActionSyntax MethodWith(Expression<Action<T>> expression)
		{
			UsingMethodWith = true;

			var methodCallExpression = expression.Body as MethodCallExpression;

			if (methodCallExpression == null)
			{
				if (expression.Body is MemberExpression)
				{
					throw new ArgumentException("This method expects an expression that is a method call.  You supplied a property expression.  Change the argument to be a method call expression or change MethodWith to GetProperty(...) if you are trying to set an expectation for a property.", "expression");
				}
				throw new ArgumentException("The expression argument is not recognized as a method call expression.", "expression");
			}
			ParseMethodExpression(expression.Body as MethodCallExpression);
			return this;
		}

		public IAutoActionSyntax<TProperty> MethodWith<TProperty>(Expression<Func<T, TProperty>> expression)
		{
			UsingMethodWith = true;

			ParseMethodExpression(expression.Body as MethodCallExpression);
			return new PropertyArgumentBuilder<TProperty>(this);
		}

		public ICommentSyntax MethodWith<TProperty>(Expression<Func<T, TProperty>> expression, TProperty returnValue)
		{
			return MethodWith(expression).Will(Return.Value(returnValue));
		}

		#endregion

		#region SetProperty

		public IAutoValueSyntax<TProperty> SetProperty<TProperty>(Func<T, TProperty> function)
		{
			MethodInfo method;
			object[] args;
			using (var recorder = new InvocationRecorder())
			{
				try
				{
					function(mock);
				}
// ReSharper disable EmptyGeneralCatchClause
				catch { }
// ReSharper restore EmptyGeneralCatchClause
				method = recorder.Invocation.Method;
				args = recorder.Invocation.Arguments;
			}

			if (!method.IsProperty())
				throw new InvalidOperationException("This method expects a property, not a method, or event.");

			IValueSyntaxBuilder valueSyntax;
			if (method.IsIndexer())
			{
				//properties have args when used like SetProperty(m=>m.prop[1] = "d") or SetProperty(m=>m.prop[1]) or SetProperty(m=>m.prop[1,3])
				valueSyntax = method.Name == Constants.GET_ITEM ? SetIndexer(args) : SetIndexer(args.Take(args.Length - 1).ToArray());
			}
			else
			{
				var name = method.Name.Replace(Constants.SET, string.Empty).Replace(Constants.GET, string.Empty); //get is needed if is used like SetProperty(m=>m.prop)
				valueSyntax = SetProperty(name);
			}

			return new PropertyValueBuilder<TProperty>(valueSyntax);
		}

		public IActionSyntax SetPropertyTo(Action<T> action)
		{
			MethodInfo method;
			object[] args;
			using (InvocationRecorder recorder = new InvocationRecorder())
			{
				try
				{
					action(mock);
				}
// ReSharper disable EmptyGeneralCatchClause
				catch { }
// ReSharper restore EmptyGeneralCatchClause
				method = recorder.Invocation.Method;
				args = recorder.Invocation.Arguments;
			}

			if(!method.IsProperty())
				throw new InvalidOperationException("This method expects a property, not a method, or event.");

			if (method.IsSpecialName && method.Name == Constants.SET_ITEM)
			{
				//properties have args when used as indexer like SetPropertyTo(m=>m.prop[1] = "d")
				IValueSyntax valueSyntax = SetIndexer(args.Take(args.Length - 1).ToArray());
				valueSyntax.To(Is.EqualTo(args.Last()));
			}
			else if (method.IsSpecialName && method.Name == Constants.GET_ITEM)
			{
				throw new InvalidOperationException("Using a property as a getter in the SetPropertyTo method is not supported.");
			}
			else
			{
				IValueSyntax valueSyntax = SetProperty(method.Name.Substring(Constants.SET.Length));
				valueSyntax.To(Is.EqualTo(args[0]));
			}

			return this;
		}

		#endregion

		#region GetProperty

		public IAutoPropertyActionSyntax<TProperty> GetProperty<TProperty>(Expression<Func<T, TProperty>> expression)
		{
			if (expression.Body is MemberExpression)
			{
				var name = ((MemberExpression)expression.Body).Member.Name;
				GetProperty(name);
			}
			else if (expression.Body is MethodCallExpression) //indexer
			{
				var expectedArguments = ParseArguments(expression.Body as MethodCallExpression);
				GetIndexer(expectedArguments);
			}
			else
			{
				throw new InvalidOperationException("Unexpected type of expression: " + expression.Body.GetType());
			}

			return new PropertyMatchBuilder<TProperty>(this);
		}

		public ICommentSyntax GetProperty<TProperty>(Expression<Func<T, TProperty>> expression, TProperty returnValue)
		{
			IAutoActionSyntax<TProperty> matchSyntax = GetProperty(expression);
			return matchSyntax.Will(Return.Value(returnValue));
		}

		/// <summary>
		/// Sets up the Get Indexer expectations
		/// </summary>
		/// <param name="expectedArguments"></param>
		/// <remarks>No need to pass the method name because it is always 'get_Item'.</remarks>
		private void GetIndexer(object[] expectedArguments)
		{
			Matcher methodMatcher = new DescriptionOverride(string.Empty, new MethodNameMatcher(Constants.GET_ITEM, MockObject.MockedTypes.PrimaryType));
			EnsureMatchingMethodExistsOnMock(methodMatcher, "an indexed getter");
			BuildableExpectation.DescribeAsIndexer();
			BuildableExpectation.MethodMatcher = methodMatcher;
			BuildableExpectation.ArgumentsMatcher = new IndexGetterArgumentsMatcher(ArgumentMatchers(expectedArguments));

			//*DevNote: Use the section below when Get[] is refactored
			//name = ((MethodCallExpression) expression.Body).Method.Name.Replace(GET, string.Empty);
			//IMatchSyntax matchSyntax = GetProperty(name);
			////check for args
			//if (expectedArguments.Length > 0)
			//{
			//	//properties have args when used like GetProperty(m=>m.prop[1])
			//	base.With(expectedArguments);
			//}
		}
		#endregion

		#region Events

		public DelegateInvoker DelegateBinding(Action<T> action)
		{
			string eventName;
			IMatchSyntax expectation = GetEventExpectation(action, out eventName);
			return new DelegateInvoker(eventName, expectation);
		}

		public EventInvoker EventBinding(Action<T> action)
		{
			string eventName;
			IMatchSyntax expectation = GetEventExpectation(action, out eventName);
			return new EventInvoker(eventName, expectation);
		}

		public EventInvoker<TEventArgs> EventBinding<TEventArgs>(Action<T> action) where TEventArgs : EventArgs
		{
			string eventName;
			IMatchSyntax expectation = GetEventExpectation(action, out eventName);
			return new EventInvoker<TEventArgs>(eventName, expectation);
		}

		#endregion

		#region Parse
		internal IExpectMatchSyntax<TResult> Parse<TResult>(Func<T, TResult> function)
		{
			MethodInfo method;
			object[] args;
			using (var recorder = new InvocationRecorder())
			{
				try
				{
					function(mock);
				}
// ReSharper disable EmptyGeneralCatchClause
				catch { }
// ReSharper restore EmptyGeneralCatchClause
				method = recorder.Invocation.Method;
				args = recorder.Invocation.Arguments;
			}

			if (method.IsProperty())
			{
				SetupPropertyExpectation(method, args);
			}
			else //is a method
			{
				SetupMethodExpectation(method, args);
			}

			return new ExpectationSyntax<T, TResult>(this);
		}

		internal IExpectMatchSyntax Parse(Action<T> action)
		{
			MethodInfo method;
			object[] args;
			using (var recorder = new InvocationRecorder())
			{
				try
				{
					action(mock);
				}
// ReSharper disable EmptyGeneralCatchClause
				catch { }
// ReSharper restore EmptyGeneralCatchClause
				method = recorder.Invocation.Method;
				args = recorder.Invocation.Arguments;
			}

			if (method.IsEvent())
			{
				string eventName;
				GetEventExpectation(action, out eventName);

				return new ExpectationSyntax<T>(this, eventName);
			}
			else if (method.IsProperty())
			{
				SetupPropertyExpectation(method, args);
			}
			else
			{
				SetupMethodExpectation(method, args);
			}

			return new ExpectationSyntax<T>(this);
		}

		private void SetupPropertyExpectation(MethodInfo method, object[] args)
		{
			var methodName = method.Name;

			if (methodName.StartsWith(Constants.GET))
			{
				if (args.Any()) //indexer
				{
					GetIndexer(args);
				}
				else
				{
					GetProperty(methodName.Substring(Constants.GET.Length));
				}
			}
			else if (methodName.StartsWith(Constants.SET))
			{
				if (args.Count() == 1)
				{
					IValueSyntax valueSyntax = SetProperty(methodName.Substring(Constants.SET.Length));
					valueSyntax.To(Is.EqualTo(args[0]));
				}
				else if (args.Count() > 1) //indexer
				{
					IValueSyntax valueSyntax = SetIndexer(args.Take(args.Length - 1).ToArray());
					valueSyntax.To(Is.EqualTo(args.Last()));
				}
			}
		}

		private void SetupMethodExpectation(MethodInfo method, object[] args)
		{
			var argumentSyntax = Method(method);
			if (args.Any())
				argumentSyntax.With(args);
		}

		#endregion

		#region Overrides

		protected override void EnsureMatchingMethodExistsOnMock(Matcher methodMatcher, string methodDescription)
		{
			try
			{
				base.EnsureMatchingMethodExistsOnMock(methodMatcher, methodDescription);
			}
			catch (Exception err)
			{
				if (IsStub && err.Message.Contains("but it is not virtual"))
					return;
				throw;
			}
		}
		#endregion

		#region Private

		private void ParseMethodExpression(MethodCallExpression expression)
		{
			SetupMethodExpectation(expression.Method, ParseArguments(expression));
		}

		private object[] ParseArguments(MethodCallExpression expression)
		{
			var argsList = new List<object>();
			foreach (Expression argument in expression.Arguments)
			{
				object value;
				if (TryEvaluate(argument, out value))
					argsList.Add(value);
				else
					throw new InvalidOperationException("The expression used for this argument is too complex.  Please try to simplify.  " + argument);
			}

			return argsList.ToArray();
		}

		//Method from Marc Gravell
		//http://stackoverflow.com/questions/2616638/access-the-value-of-a-member-expression
		//http://code.google.com/p/protobuf-net/source/browse/trunk/protobuf-net.Extensions/ServiceModel/Client/ProtoClientExtensions.cs
		private static bool TryEvaluate(Expression operation, out object value)
		{
			if (operation == null)
			{
				// used for static fields, etc
				value = null;
				return true;
			}
			switch (operation.NodeType)
			{
				case ExpressionType.Constant:
					value = ((ConstantExpression)operation).Value;
					return true;
				case ExpressionType.MemberAccess:
					var me = (MemberExpression)operation;
					object target;
					if (TryEvaluate(me.Expression, out target))
					{
						// instance target
						/* ^ */
						var _fieldInfo = me.Member as FieldInfo;
						var _propertyInfo = me.Member as PropertyInfo;

						if ((object)_fieldInfo != null)
						{
							value = _fieldInfo.GetValue(target);
							return true;
						}
						else if ((object)_propertyInfo != null)
						{
							value = _propertyInfo.GetValue(target, null);
							return true;
						}
						else
						{
							// who knows?
						}
						/* dpbullington@gmail.com ^ */
					}
					break;
				default:
					//value = Expression.Lambda(operation).Compile().DynamicInvoke();
					var d = Expression.Lambda<Func<object>>(operation).Compile();
					value = d();
					return true;
			}

			value = null;
			return false;
		}

		#region Events

		private IMatchSyntax GetEventExpectation(Action<T> action, out string eventName)
		{
			object[] arguments;
			using (var recorder = new InvocationRecorder())
			{
				action(mock);
				eventName = recorder.Invocation.Method.Name;
				arguments = recorder.Invocation.Arguments;
			}

			var symbol = eventName.StartsWith(Constants.ADD) ? " += " : " -= ";
			var methodMatcher = new MethodNameMatcher(eventName, MockObject.MockedTypes.PrimaryType);

			var descriptionOverride = new DescriptionOverride(eventName.Replace(Constants.ADD, string.Empty).Replace(Constants.REMOVE, string.Empty) + symbol, methodMatcher);
			EnsureMatchingMethodExistsOnMock(methodMatcher, "an event named " + eventName);

			BuildableExpectation.MethodMatcher = descriptionOverride;

			/* ^ */
			if (arguments.Length == 1 && arguments[0] != null /*&& !((MulticastDelegate)arguments[0]).Method.Name.Contains(">")*/)
			{
				var _methodInfo = ((MulticastDelegate)arguments[0]).GetMethodInfo();

				if (_methodInfo.Name.Contains(">"))
					WithArguments(new DelegateMatcher((MulticastDelegate)arguments[0]));
			}
			else
			{
				WithArguments(Is.Anything);
			}
			/* dpbullington@gmail.com ^ */

			return this;
		}
		#endregion

		#endregion
	}
}

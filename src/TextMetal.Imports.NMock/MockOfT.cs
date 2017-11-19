#region Using

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using NMock.Internal;
using NMock.Syntax;

#endregion

namespace NMock
{

	/// <summary>
	/// This class represents a mock object of an interface or class.  To create a <see cref="Mock{T}"/>, 
	/// use a <see cref="MockFactory"/>.
	/// </summary>
	/// <typeparam name="T">The type to mock.</typeparam>
	/// <example>
	/// <code>
	/// 
	/// public class TestClass
	/// {
	///		public void TestMethod()
	///		{
	///			MockFactory factory = new MockFactory();
	///			Mock&lt;ISample&gt; mock = factory.CreateMock&lt;ISample&gt;();
	/// 
	///			//create an expectation that the property Prop will be set to 3
	///			mock.Expects.One.SetPropertyTo(m=>m.Prop=3);
	/// 
	///			new Driver(mock.MockObject).Drive(3);
	///		}
	/// }
	/// 
	/// public interface ISample
	/// {
	///		int Prop {get; set;}
	/// }
	/// 
	/// public class Driver
	/// {
	///		public Driver(ISample sample)
	///		{
	///			Sample = sample;
	///		}
	///		public int Sample {get;set;}
	///		public void Drive(int value)
	///		{
	///			Sample.Prop = value;
	///		}
	/// }
	/// 
	/// </code>
	/// </example>
	public class Mock<T> where T : class
	{
		private readonly object _proxy;
		private readonly MockFactory _factory;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="proxy"></param>
		/// <param name="factory"></param>
		/// <exception cref="ArgumentNullException" />
		internal Mock(object proxy, MockFactory factory)
		{
			if (proxy == null)
				throw new ArgumentNullException("proxy");
			if (factory == null) 
				throw new ArgumentNullException("factory");

			_proxy = proxy;
			_factory = factory;
		}

		/// <summary>
		/// An object of type <typeparamref name="T" /> to be used whenever the Mocked object is needed.
		/// </summary>
		/// <remarks>
		/// This property provides access to the proxy that is mocking the actual type.  Use this property
		/// when a reference to an object that is of the mocked type is needed.
		/// </remarks>
		/// <example>
		/// This example shows how a presenter needs arguments of the right type during construction.  The
		/// <c>MockObject</c> property is used because the presenter can't be instantiated with a <see cref="Mock{T}"/>
		/// argument.
		/// <code>
		/// interface IView { ... }
		/// interface IModel { ... }
		/// 
		/// Mock&lt;IView&gt; view = factory.CreateMock&lt;IView&gt;();
		/// Mock&lt;IModel&gt; view = factory.CreateMock&lt;IModel&gt;();
		/// 
		/// Presenter p = new Presenter(view.MockObject, model.MockObject);
		/// </code>
		/// </example>
		public T MockObject
		{
			get
			{
				return (T) _proxy;
			}
		}

		/// <summary>
		/// A syntax property used to access members that specify the number of times an expectation could occur.
		/// </summary>
		/// <remarks>This would be considered to be the main property of a <see cref="Mock{T}"/> class.  This property
		/// provides access to all other expectation setup methods.</remarks>
		/// <example>
		/// In this example:
		/// <list type="bullet">
		/// <item>
		/// <term><c>mock</c></term>
		/// <description>is an instance of a <see cref="Mock{T}"/>.</description>
		/// </item>
		/// <item>
		/// <term><c>Expects</c></term>
		/// <description>refers to this property.</description>
		/// </item>
		/// <item>
		/// <term><c>One</c></term>
		/// <description>is the number of times this action is expected.</description>
		/// </item>
		/// <item>
		///	<term><c>SetPropertyTo</c></term>
		/// <description>is the type of expection.  <see cref="IMethodSyntax{TInterface}.SetPropertyTo"/> means the mock expects a property to be set to a value.</description>
		/// </item>
		/// <item>
		/// <term><c>m</c></term>
		/// <description>is the variable in our anonymous method that represents the mocked interface or class.</description>
		/// </item>
		/// <item>
		///	<term><c>Prop</c></term>
		/// <description>is the property on the mocked interface or class that the expectation is for.  It will be set to the value 3.</description>
		/// </item>
		/// </list>
		/// <code>
		/// 
		/// //create an expectation that the property Prop will be set to 3
		/// mock.Expects.One.SetPropertyTo(m=>m.Prop=3);
		/// 
		/// </code>
		/// View more of this code in the <see cref="Mock{T}"/> example.
		/// </example>
		public Expects<T> Expects
		{
			get
			{
				return new Expects<T>(_proxy);
			}
		}

		/// <summary>
		/// A syntax property that returns a <see cref="NMock.Stub{T}"/> class to stub out a member on the Mock.
		/// </summary>
		public Stub<T> Stub
		{
			get
			{
				return new Stub<T>(_proxy);
			}
		}

		/// <summary>
		/// Duck Typing method: Returns a <see cref="Mock{T}" /> instance of the type being specified in <typeparamref name="TType"/>.
		/// </summary>
		/// <typeparam name="TType"></typeparam>
		/// <returns></returns>
		/// <remarks>This method will not throw an exception if the cast is not successful.  It will return null.  Use <c cref="Mock{T}.As{TType}(bool)"/> to specify if an exception should be thrown.</remarks>
		public Mock<TType> As<TType>() where TType : class
		{
			return As<TType>(false);
		}

		/// <summary>
		/// Duck Typing method: Returns a <see cref="Mock{T}" /> instance of the type being specified in <typeparamref name="TType"/>.
		/// </summary>
		/// <typeparam name="TType"></typeparam>
		/// <param name="throwException"></param>
		/// <returns></returns>
		/// <exception cref="InvalidCastException"></exception>
		/// <remarks>Use this method to specify if an exception should be thrown when a cast is not valid.</remarks>
		public Mock<TType> As<TType>(bool throwException) where TType : class
		{
			if (typeof (TType) == typeof (T))
				throw new InvalidOperationException(string.Format("The mock's primary type is '{0}'.  Using the As method to convert the type to itself is not allowed.", typeof (T).Name));
			TType mock = _proxy as TType;
			if (mock != null)
				return new Mock<TType>(_proxy, _factory);
			else
			{
				if (throwException)
					throw new InvalidCastException(string.Format("The mock does not implement '{0}'.  Use the CreateMock method that accepts a list of Types to mock more than the default type.", typeof (TType).Name));
				else
					return null;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating if this <see cref="Mock{T}"/> should ignore unexpected invocations to properties, methods, or events.
		/// </summary>
		/// <remarks>
		/// Use the property to have a Mock ignore calls with no expectations.  By default, this works fine for property setters, void methods,
		/// and events.  Property getters and non-void methods will need to indicate how they will be implemented as they <i>need</i> to return
		/// a value.
		/// </remarks>
		public bool IgnoreUnexpectedInvocations
		{
			get
			{
				return ((IMockObject) _proxy).IgnoreUnexpectedInvocations;
			}
			set
			{
				((IMockObject) _proxy).IgnoreUnexpectedInvocations = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating if an exception should be thrown when the <see cref="ToString()"/> is called.
		/// </summary>
		public bool ThrowToStringException { get; set; }

		/// <summary>
		/// Overriden ToString method that throws an exception when called so that the deloper does not
		/// confuse this ToString with the ToString of the <see cref="MockObject"/> property.
		/// </summary>
		/// <returns>The name of the <see cref="Mock{T}"/></returns>
		/// <seealso cref="ThrowToStringException"/>
		public override string ToString()
		{
			if (ThrowToStringException)
			{
				throw new InvalidOperationException("ToString was called on the Mock instance and not the MockObject property.  If you intended to see the ToString value of this Mock instance, set the ThrowToStringException property to false.");
			}
			else
			{
				return Name;
			}
		}

		/// <summary>
		/// Returns the name of the underlying proxy
		/// </summary>
		public string Name
		{
			get
			{
				return ((IMockObject) _proxy).MockName;
			}
		}

		/// <summary>
		/// Clears all expectations of this mock.
		/// </summary>
		/// <remarks>
		/// Use this method to clear expectations in a test before the test cleanup runs
		/// to avoid unmet expectations.  It is useful when testing error conditions.
		/// </remarks>
		public void ClearExpectations()
		{
			_factory.ClearExpectations(_proxy as IMockObject);
		}

		public IExpectMatchSyntax<TResult> Arrange<TResult>(Func<T, TResult> expression)
		{
			return new ExpectationBuilder<T>("1 time", Is.AtLeast(1), Is.AtMost(1), _proxy).Parse(expression);
		}

		public IExpectMatchSyntax Arrange(Action<T> expression)
		{
			return new ExpectationBuilder<T>("1 time", Is.AtLeast(1), Is.AtMost(1), _proxy).Parse(expression);
		}

		public void AssertAll()
		{
			_factory.AssertAll(_proxy as IMockObject);
		}
	}
}
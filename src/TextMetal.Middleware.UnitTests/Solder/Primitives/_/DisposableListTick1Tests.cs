/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using NMock;

using NUnit.Framework;

using TextMetal.Middleware.Solder.Primitives;

namespace TextMetal.Middleware.UnitTests.Solder.Primitives._
{
	[TestFixture]
	public class DisposableListTick1Tests
	{
		#region Constructors/Destructors

		public DisposableListTick1Tests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateTest()
		{
			DisposableList<IDisposable> disposableList;
			MockFactory mockFactory;
			IDisposable mockDisposable;

			mockFactory = new MockFactory();
			mockDisposable = mockFactory.CreateInstance<IDisposable>();

			Expect.On(mockDisposable).One.Method(m => m.Dispose());

			disposableList = new DisposableList<IDisposable>();

			Assert.IsEmpty(disposableList);

			disposableList.Add(mockDisposable);

			Assert.IsNotEmpty(disposableList);

			disposableList.Dispose();
		}

		#endregion
	}
}
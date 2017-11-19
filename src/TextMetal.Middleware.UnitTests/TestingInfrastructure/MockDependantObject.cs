/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using TextMetal.Middleware.Solder.Injection;

namespace TextMetal.Middleware.UnitTests.TestingInfrastructure
{
	public class MockDependantObject
	{
		#region Constructors/Destructors

		public MockDependantObject()
		{
			this.text = null;
		}

		public MockDependantObject(string text)
		{
			this.text = text ?? string.Empty;
		}

		[DependencyInjection]
		public MockDependantObject([DependencyInjection(SelectorKey = "named_dep_obj")] MockDependantObject left, [DependencyInjection] MockDependantObject right)
		{
			this.text = string.Empty;
			this.left = left;
			this.right = right;
		}

		[DependencyInjection(SelectorKey = "named_dep_obj")]
		public MockDependantObject([DependencyInjection] MockDependantObject both)
		{
			this.text = string.Empty;
			this.left = both;
			this.right = both;
		}

		#endregion

		#region Fields/Constants

		private readonly MockDependantObject left;
		private readonly MockDependantObject right;
		private readonly string text;

		#endregion

		#region Properties/Indexers/Events

		public MockDependantObject Left
		{
			get
			{
				return this.left;
			}
		}

		public MockDependantObject Right
		{
			get
			{
				return this.right;
			}
		}

		public string Text
		{
			get
			{
				return this.text;
			}
		}

		#endregion
	}
}
using System.Collections;
using System.IO;
using System.Linq;

namespace NMock.Matchers
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class CountMatcher<T> : Matcher where T : IEnumerable
	{
		private readonly int _expectedCount;
		private int _actualCount;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="expectedCount"></param>
		public CountMatcher(int expectedCount)
		{
			_expectedCount = expectedCount;
		}

		public override bool Matches(object o)
		{
			var input = (T)o;
			if (input == null)
			{
				return false;
			}
			_actualCount = input.Cast<object>().Count();
			return _actualCount == _expectedCount;
		}

		public override void DescribeTo(TextWriter writer)
		{
			writer.Write(string.Format("CountMatcher: Actual count of {0} did not match expected count of {1}", _actualCount, _expectedCount));
		}
	}
}

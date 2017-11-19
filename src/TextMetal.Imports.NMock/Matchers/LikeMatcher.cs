#region Using

using System;
using System.IO;

#endregion

namespace NMock.Matchers
{
	/// <summary>
	/// Matches 2 objects using IComparable
	/// </summary>
	public class ObjectMatcher : Matcher
	{
		private readonly object _o;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="o"></param>
		public ObjectMatcher(object o)
		{
			_o = o;
		}

		/// <summary>
		/// Compares 2 objects using IComparable
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public override bool Matches(object o)
		{
			if (o == null)
				throw new ArgumentNullException("o");

			if (_o == null)
				throw new InvalidOperationException("The object of comparison is null.");

			if (o as IComparable == null)
				throw new InvalidOperationException(string.Format("The object being compared does not support IComparable.  Type: {0}", o.GetType()));

			if (_o as IComparable == null)
				throw new InvalidOperationException(string.Format("The object of comparison does not support IComparable.  Type: {0}", _o.GetType()));

			return ((IComparable) _o).CompareTo(o) == 0;
		}

		/// <summary>
		/// Describes this matcher.
		/// </summary>
		/// <param name="writer">The text writer to which the description is added.</param>
		public override void DescribeTo(TextWriter writer)
		{
			if (_o == null)
				writer.Write("Object in ObjectMatcher is null.");
			else
				writer.Write(_o.ToString());
		}
	}
}
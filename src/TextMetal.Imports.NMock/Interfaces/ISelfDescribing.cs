#region Using

using System.IO;

#endregion

namespace NMock
{
	/// <summary>
	/// This interface is used to get a description of the implementator.
	/// </summary>
	public interface ISelfDescribing
	{
		/// <summary>
		/// Describes this object.
		/// </summary>
		/// <param name="writer">The text writer the description is added to.</param>
		void DescribeTo(TextWriter writer);
	}
}
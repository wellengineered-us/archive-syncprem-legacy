/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace SyncPrem.StreamingIO.Primitives
{
	/// <summary>
	/// The exception thrown when a SyncPrem runtime error occurs.
	/// </summary>
	public class SyncPremStreamingIOException : SyncPremException
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the SyncPremException class.
		/// </summary>
		public SyncPremStreamingIOException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the SyncPremException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		public SyncPremStreamingIOException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the SyncPremException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		/// <param name="innerException"> The inner exception. </param>
		public SyncPremStreamingIOException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		#endregion
	}
}
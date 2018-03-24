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
	public class SyncPremException : Exception
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the SyncPremException class.
		/// </summary>
		public SyncPremException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the SyncPremException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		public SyncPremException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the SyncPremException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		/// <param name="innerException"> The inner exception. </param>
		public SyncPremException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		#endregion
	}
}
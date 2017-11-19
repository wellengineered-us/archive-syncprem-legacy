/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace TextMetal.Middleware.Solder.Context
{
	public enum ContextScope
	{
		/// <summary>
		/// Unknowm, undefined, or invalid scope.
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Globally static visible and implicitly NOT thread-safe.
		/// </summary>
		GlobalStaticUnsafe,

		/// <summary>
		/// Local thread visible and thread-safe obviously.
		/// </summary>
		LocalThreadSafe,

		/// <summary>
		/// Local async visible and async-safe (and perhaps thread-safe).
		/// </summary>
		LocalAsyncSafe,

		/// <summary>
		/// Local request visble, context-agility-safe, and NOT thread-safe.
		/// Commonly used with web server frameworks that are not guaranteed
		/// to be exhibit thread affinity (e.g. context can be forklifted thread to thread under load).
		/// </summary>
		LocalRequestSafe
	}
}
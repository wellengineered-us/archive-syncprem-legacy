/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace TextMetal.Middleware.Solder.Primitives
{
	public interface IMessage
	{
		#region Properties/Indexers/Events

		/// <summary>
		/// Gets the message category.
		/// </summary>
		string Category
		{
			get;
		}

		/// <summary>
		/// Gets the message description.
		/// </summary>
		string Description
		{
			get;
		}

		/// <summary>
		/// Gets the message severity.
		/// </summary>
		Severity Severity
		{
			get;
		}

		#endregion
	}
}
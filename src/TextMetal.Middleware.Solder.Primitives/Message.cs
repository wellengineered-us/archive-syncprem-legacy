/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace TextMetal.Middleware.Solder.Primitives
{
	/// <summary>
	/// Represents a message with a category, description, and severity.
	/// </summary>
	public sealed class Message
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the Message class.
		/// </summary>
		/// <param name="category"> The category of the message. </param>
		/// <param name="description"> The description of the message. </param>
		/// <param name="severity"> The severity of the message. </param>
		public Message(string category, string description, Severity severity)
		{
			if ((object)category == null)
				throw new ArgumentNullException(nameof(category));

			if ((object)description == null)
				throw new ArgumentNullException(nameof(description));

			this.category = category;
			this.description = description;
			this.severity = severity;
		}

		#endregion

		#region Fields/Constants

		private readonly string category;
		private readonly string description;
		private readonly Severity severity;

		#endregion

		#region Properties/Indexers/Events

		/// <summary>
		/// Gets the message category.
		/// </summary>
		public string Category
		{
			get
			{
				return this.category;
			}
		}

		/// <summary>
		/// Gets the message description.
		/// </summary>
		public string Description
		{
			get
			{
				return this.description;
			}
		}

		/// <summary>
		/// Gets the message severity.
		/// </summary>
		public Severity Severity
		{
			get
			{
				return this.severity;
			}
		}

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using TextMetal.Middleware.Solder.Primitives;

namespace SyncPrem.Infrastructure.Configuration
{
	/// <summary>
	/// Represents an configuration object and it's "schema".
	/// </summary>
	public interface IConfigurationObject : IValidate
	{
		#region Properties/Indexers/Events

		/// <summary>
		/// Gets an array of allowed child configuration object types.
		/// </summary>
		Type[] AllowedChildTypes
		{
			get;
		}

		/// <summary>
		/// Gets a list of configuration object items.
		/// </summary>
		IConfigurationObjectCollection<IConfigurationObject> Items
		{
			get;
		}

		/// <summary>
		/// Gets or sets the optional single configuration content object.
		/// </summary>
		IConfigurationObject Content
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the parent configuration object or null if this is the configuration root.
		/// </summary>
		IConfigurationObject Parent
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the surrounding configuration object or null if this is not surrounded (in a collection).
		/// </summary>
		IConfigurationObjectCollection Surround
		{
			get;
			set;
		}

		#endregion
	}
}
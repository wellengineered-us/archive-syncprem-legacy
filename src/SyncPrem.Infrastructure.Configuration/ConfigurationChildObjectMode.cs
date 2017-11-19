/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace SyncPrem.Infrastructure.Configuration
{
	/// <summary>
	/// Specifies the configuration child object mode (applicable only to those which are not well-known via properties with implicit or explicit mapping attributes).
	/// </summary>
	public enum ConfigurationChildObjectMode
	{
		/// <summary>
		/// This configuration object is not allowed to have any child objects. This is the default.
		/// </summary>
		None = 0,

		/// <summary>
		/// This configuration object can have ONE non-well-known child object. Use the Content property to access the possibly null value.
		/// </summary>
		Content = 1,

		/// <summary>
		/// This configuration object can have MANY non-well-known child objects. Use the Items property to access the possibly empty list of values.
		/// </summary>
		Items = 2
	}
}
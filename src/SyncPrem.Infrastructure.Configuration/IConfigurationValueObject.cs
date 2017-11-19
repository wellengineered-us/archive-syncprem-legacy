/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using CfgObjName = System.String;

namespace SyncPrem.Infrastructure.Configuration
{
	/// <summary>
	/// Represents an value-based configuration object and it's "name".
	/// </summary>
	public interface IConfigurationValueObject<TValue> : IConfigurationObject
	{
		#region Properties/Indexers/Events

		/// <summary>
		/// Gets or sets the configuration object name.
		/// </summary>
		CfgObjName Name
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		TValue Value
		{
			get;
			set;
		}

		#endregion
	}
}
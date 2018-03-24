/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using SyncPrem.Infrastructure.Configuration;

using _Message = TextMetal.Middleware.Solder.Primitives.Message;

namespace SyncPrem.Pipeline.Abstractions.Configuration
{
	public abstract class ComponentConfiguration : ConfigurationObject
	{
		#region Constructors/Destructors

		protected ComponentConfiguration()
		{
		}

		#endregion
	}
}
/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace SyncPrem.Pipeline.Abstractions
{
	public static class Constants
	{
		#region Fields/Constants

		private const string CONTEXT_COMPONENT_SCOPED_SCHEMA = "context_component_scoped_schema";

		#endregion

		#region Properties/Indexers/Events

		public static string ContextComponentScopedSchema
		{
			get
			{
				return CONTEXT_COMPONENT_SCOPED_SCHEMA;
			}
		}

		#endregion
	}
}
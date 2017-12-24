/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Collections.Generic;

namespace SyncPrem.StreamingIO.DataMasking
{
	public interface IObfuscationSpec
	{
		#region Properties/Indexers/Events

		IEnumerable<IDictionarySpec> DictionarySpecs
		{
			get;
			set;
		}

		bool? DisableEngineCaches
		{
			get;
			set;
		}

		bool? EnablePassThru
		{
			get;
			set;
		}

		IHashSpec HashSpec
		{
			get;
			set;
		}

		ITableSpec TableSpec
		{
			get;
			set;
		}

		#endregion

		#region Methods/Operators

		void AssertValid();

		#endregion
	}
}
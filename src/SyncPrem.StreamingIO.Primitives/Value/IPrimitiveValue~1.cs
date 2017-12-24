/*
	Copyright �2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace SyncPrem.StreamingIO.Primitives.Value
{
	public interface IPrimitiveValue<TValue> : IValue
	{
		TValue Value
		{
			get;
		}
	}
}
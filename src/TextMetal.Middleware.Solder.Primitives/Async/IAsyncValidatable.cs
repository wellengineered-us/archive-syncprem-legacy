/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TextMetal.Middleware.Solder.Primitives.Async
{
	public interface IAsyncValidatable
	{
		#region Methods/Operators

		Task<IEnumerable<Message>> ValidateAsync();

		Task<IEnumerable<Message>> ValidateAsync(object context);

		#endregion
	}
}
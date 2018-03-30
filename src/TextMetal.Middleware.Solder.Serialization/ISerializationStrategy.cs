/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace TextMetal.Middleware.Solder.Serialization
{
	/// <summary>
	/// Provides a strategy pattern around serializing and deserializing objects.
	/// </summary>
	public interface ISerializationStrategy : IStreamSerializationStrategy, IFileSerializationStrategy
	{
	}
}
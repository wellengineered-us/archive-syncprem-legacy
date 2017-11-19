/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace TextMetal.Middleware.Solder.Context
{
	public interface IContextualStorageStrategy
	{
		#region Methods/Operators

		T GetValue<T>(string key);

		bool HasValue(string key);

		void RemoveValue(string key);

		void SetValue<T>(string key, T value);

		#endregion
	}
}
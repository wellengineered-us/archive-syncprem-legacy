using System;

namespace TextMetal.Middleware.Solder.Utilities
{
	public interface IAppConfigFascade
	{
		#region Methods/Operators

		/// <summary>
		/// Gets the value of an app settings for the current application's default configuration. A AppConfigException is thrown if the key does not exist.
		/// </summary>
		/// <typeparam name="TValue"> The type to convert the app settings value. </typeparam>
		/// <param name="key"> The key to get a value. </param>
		/// <returns> The app settings as type TValue. </returns>
		TValue GetAppSetting<TValue>(string key);

		/// <summary>
		/// Gets the value of an app settings for the current application's default configuration. A AppConfigException is thrown if the key does not exist.
		/// </summary>
		/// <param name="valueType"> The type to convert the app settings value. </param>
		/// <param name="key"> The key to get a value. </param>
		/// <returns> The app settings as a string. </returns>
		object GetAppSetting(Type valueType, string key);

		/// <summary>
		/// Checks to see if an app settings key exists for the current application's default configuration.
		/// </summary>
		/// <param name="key"> The key to check. </param>
		/// <returns> A boolean value indicating the app setting key presence. </returns>
		bool HasAppSetting(string key);

		#endregion
	}
}
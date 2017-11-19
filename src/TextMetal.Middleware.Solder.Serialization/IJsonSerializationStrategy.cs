/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using Newtonsoft.Json;

namespace TextMetal.Middleware.Solder.Serialization
{
	/// <summary>
	/// Provides a strategy pattern around serializing and deserializing objects using JSON semantics.
	/// </summary>
	public interface IJsonSerializationStrategy : ISerializationStrategy
	{
		#region Methods/Operators

		/// <summary>
		/// Deserializes an object from the specified json reader.
		/// </summary>
		/// <param name="jsonReader"> The json reader to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		object GetObjectFromReader(JsonReader jsonReader, Type targetType);

		/// <summary>
		/// Deserializes an object from the specified json reader. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="jsonReader"> The json reader to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		TObject GetObjectFromReader<TObject>(JsonReader jsonReader);

		/// <summary>
		/// Serializes an object to the specified json writer.
		/// </summary>
		/// <param name="jsonWriter"> The json writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		void SetObjectToWriter(JsonWriter jsonWriter, Type targetType, object obj);

		/// <summary>
		/// Serializes an object to the specified json writer. This is the generic overload.
		/// </summary>
		/// <param name="jsonWriter"> The json writer to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		void SetObjectToWriter<TObject>(JsonWriter jsonWriter, TObject obj);

		#endregion
	}
}
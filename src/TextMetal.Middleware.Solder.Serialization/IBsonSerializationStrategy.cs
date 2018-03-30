/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using Newtonsoft.Json.Bson;

namespace TextMetal.Middleware.Solder.Serialization
{
	/// <summary>
	/// Provides a strategy pattern around serializing and deserializing objects using BSON semantics.
	/// </summary>
	public interface IBsonSerializationStrategy : ISerializationStrategy
	{
		#region Methods/Operators

		/// <summary>
		/// Deserializes an object from the specified bson reader.
		/// </summary>
		/// <param name="bsonReader"> The bson reader to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		object GetObjectFromReader(BsonReader bsonReader, Type targetType);

		/// <summary>
		/// Deserializes an object from the specified bson reader. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="bsonReader"> The bson reader to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		TObject GetObjectFromReader<TObject>(BsonReader bsonReader);

		/// <summary>
		/// Serializes an object to the specified bson writer.
		/// </summary>
		/// <param name="bsonWriter"> The bson writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		void SetObjectToWriter(BsonWriter bsonWriter, Type targetType, object obj);

		/// <summary>
		/// Serializes an object to the specified bson writer. This is the generic overload.
		/// </summary>
		/// <param name="bsonWriter"> The bson writer to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		void SetObjectToWriter<TObject>(BsonWriter bsonWriter, TObject obj);

		#endregion
	}
}
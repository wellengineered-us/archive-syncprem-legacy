/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace TextMetal.Middleware.Solder.Serialization
{
	public class BsonSerializationStrategy : BinarySerializationStrategy, IBsonSerializationStrategy
	{
		#region Constructors/Destructors

		public BsonSerializationStrategy()
		{
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// Deserializes an object from the specified text reader.
		/// </summary>
		/// <param name="binaryReader"> The text reader to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public override object GetObjectFromReader(BinaryReader binaryReader, Type targetType)
		{
			object obj;

			if ((object)binaryReader == null)
				throw new ArgumentNullException(nameof(binaryReader));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			using (BsonReader bsonReader = new BsonReader(binaryReader))
				obj = this.GetObjectFromReader(bsonReader, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified bson reader.
		/// </summary>
		/// <param name="bsonReader"> The bson reader to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public object GetObjectFromReader(BsonReader bsonReader, Type targetType)
		{
			JsonSerializer jsonSerializer;
			object obj;

			if ((object)bsonReader == null)
				throw new ArgumentNullException(nameof(bsonReader));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			jsonSerializer = GetJsonSerializer();
			obj = jsonSerializer.Deserialize(bsonReader, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified bson reader. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="bsonReader"> The bson reader to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		public TObject GetObjectFromReader<TObject>(BsonReader bsonReader)
		{
			TObject obj;
			Type targetType;

			if ((object)bsonReader == null)
				throw new ArgumentNullException(nameof(bsonReader));

			targetType = typeof(TObject);
			obj = (TObject)this.GetObjectFromReader(bsonReader, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified readable stream.
		/// </summary>
		/// <param name="stream"> The readable stream to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public override object GetObjectFromStream(Stream stream, Type targetType)
		{
			object obj;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			using (BsonReader bsonReader = new BsonReader(stream))
				obj = this.GetObjectFromReader(bsonReader, targetType);

			return obj;
		}

		/// <summary>
		/// Serializes an object to the specified writable stream.
		/// </summary>
		/// <param name="stream"> The writable stream to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public override void SetObjectToStream(Stream stream, Type targetType, object obj)
		{
			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (BsonWriter bsonWriter = new BsonWriter(stream))
				this.SetObjectToWriter(bsonWriter, targetType, obj);
		}

		/// <summary>
		/// Serializes an object to the specified text writer.
		/// </summary>
		/// <param name="binaryWriter"> The text writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public override void SetObjectToWriter(BinaryWriter binaryWriter, Type targetType, object obj)
		{
			if ((object)binaryWriter == null)
				throw new ArgumentNullException(nameof(binaryWriter));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (BsonWriter bsonWriter = new BsonWriter(binaryWriter))
				this.SetObjectToWriter(bsonWriter, targetType, obj);
		}

		private static JsonSerializer GetJsonSerializer()
		{
			return JsonSerializer.Create(new JsonSerializerSettings()
			{
				Formatting = Formatting.Indented,
				TypeNameHandling = TypeNameHandling.None,
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			});
		}

		/// <summary>
		/// Serializes an object to the specified bson writer.
		/// </summary>
		/// <param name="bsonWriter"> The bson writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SetObjectToWriter(BsonWriter bsonWriter, Type targetType, object obj)
		{
			JsonSerializer jsonSerializer;

			if ((object)bsonWriter == null)
				throw new ArgumentNullException(nameof(bsonWriter));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			jsonSerializer = GetJsonSerializer();
			jsonSerializer.Serialize(bsonWriter, obj, targetType);
		}

		/// <summary>
		/// Serializes an object to the specified bson writer. This is the generic overload.
		/// </summary>
		/// <param name="bsonWriter"> The bson writer to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SetObjectToWriter<TObject>(BsonWriter bsonWriter, TObject obj)
		{
			Type targetType;

			if ((object)bsonWriter == null)
				throw new ArgumentNullException(nameof(bsonWriter));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			this.SetObjectToWriter(bsonWriter, targetType, (object)obj);
		}

		#endregion
	}
}
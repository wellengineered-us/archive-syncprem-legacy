/*
	Copyright ©2002-2017 Daniel P. Bullington (dpbullington@gmail.com)
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.IO;

using Newtonsoft.Json;

namespace TextMetal.Middleware.Solder.Serialization
{
	public class JsonSerializationStrategy : IJsonSerializationStrategy, ITextSerializationStrategy
	{
		#region Constructors/Destructors

		public JsonSerializationStrategy()
		{
		}

		#endregion

		#region Fields/Constants

		private static readonly JsonSerializationStrategy instance = new JsonSerializationStrategy();

		#endregion

		#region Properties/Indexers/Events

		public static JsonSerializationStrategy Instance
		{
			get
			{
				return instance;
			}
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// Deserializes an object from the specified input file.
		/// </summary>
		/// <param name="inputFilePath"> The input file path to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public object GetObjectFromFile(string inputFilePath, Type targetType)
		{
			object obj;

			if ((object)inputFilePath == null)
				throw new ArgumentNullException(nameof(inputFilePath));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			using (Stream stream = File.Open(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
				obj = this.GetObjectFromStream(stream, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified input file. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="inputFilePath"> The input file path to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		public TObject GetObjectFromFile<TObject>(string inputFilePath)
		{
			TObject obj;
			Type targetType;

			if ((object)inputFilePath == null)
				throw new ArgumentNullException(nameof(inputFilePath));

			targetType = typeof(TObject);
			obj = (TObject)this.GetObjectFromFile(inputFilePath, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified text reader.
		/// </summary>
		/// <param name="textReader"> The text reader to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public object GetObjectFromReader(TextReader textReader, Type targetType)
		{
			JsonSerializer jsonSerializer;
			object obj;

			if ((object)textReader == null)
				throw new ArgumentNullException(nameof(textReader));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings()
													{
														Formatting = Formatting.Indented,
														TypeNameHandling = TypeNameHandling.None,
														ReferenceLoopHandling = ReferenceLoopHandling.Ignore
													});

			using (JsonReader jsonReader = new JsonTextReader(textReader))
				obj = (object)jsonSerializer.Deserialize(jsonReader, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified text reader. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="textReader"> The text reader to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		public TObject GetObjectFromReader<TObject>(TextReader textReader)
		{
			TObject obj;
			Type targetType;

			if ((object)textReader == null)
				throw new ArgumentNullException(nameof(textReader));

			targetType = typeof(TObject);
			obj = (TObject)this.GetObjectFromReader(textReader, targetType);

			return obj;
		}

		object IJsonSerializationStrategy.GetObjectFromReader(JsonReader jsonReader, Type targetType)
		{
			JsonSerializer jsonSerializer;
			object obj;

			if ((object)jsonReader == null)
				throw new ArgumentNullException(nameof(jsonReader));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			jsonSerializer = new JsonSerializer();
			obj = jsonSerializer.Deserialize(jsonReader);

			return obj;
		}

		TObject IJsonSerializationStrategy.GetObjectFromReader<TObject>(JsonReader jsonReader)
		{
			return default(TObject);
		}

		/// <summary>
		/// Deserializes an object from the specified readable stream.
		/// </summary>
		/// <param name="stream"> The readable stream to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public object GetObjectFromStream(Stream stream, Type targetType)
		{
			JsonSerializer jsonSerializer;
			object obj;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings()
													{
														Formatting = Formatting.Indented,
														TypeNameHandling = TypeNameHandling.None,
														ReferenceLoopHandling = ReferenceLoopHandling.Ignore
													});

			using (StreamReader streamReader = new StreamReader(stream))
			{
				using (JsonReader jsonReader = new JsonTextReader(streamReader))
					obj = (object)jsonSerializer.Deserialize(jsonReader, targetType);
			}

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified readable stream. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="stream"> The readable stream to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		public TObject GetObjectFromStream<TObject>(Stream stream)
		{
			TObject obj;
			Type targetType;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			targetType = typeof(TObject);
			obj = (TObject)this.GetObjectFromStream(stream, targetType);

			return obj;
		}

		/// <summary>
		/// Deserializes an object from the specified string value.
		/// </summary>
		/// <param name="value"> The string value to deserialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the deserialized object graph. </param>
		/// <returns> An object of the target type or null. </returns>
		public object GetObjectFromString(string value, Type targetType)
		{
			object obj;
			StringReader stringReader;

			using (stringReader = new StringReader(value))
			{
				obj = this.GetObjectFromReader(stringReader, targetType);
				return obj;
			}
		}

		/// <summary>
		/// Deserializes an object from the specified text value. This is the generic overload.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the deserialized object graph. </typeparam>
		/// <param name="value"> The string value to deserialize. </param>
		/// <returns> An object of the target type or null. </returns>
		public TObject GetObjectFromString<TObject>(string value)
		{
			TObject obj;
			Type targetType;

			if ((object)value == null)
				throw new ArgumentNullException(nameof(value));

			targetType = typeof(TObject);
			obj = (TObject)this.GetObjectFromString(value, targetType);

			return obj;
		}

		/// <summary>
		/// Serializes an object to the specified output file.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the object graph to serialize. </typeparam>
		/// <param name="outputFilePath"> The output file path to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SetObjectToFile<TObject>(string outputFilePath, TObject obj)
		{
			Type targetType;

			if ((object)outputFilePath == null)
				throw new ArgumentNullException(nameof(outputFilePath));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			this.SetObjectToFile(outputFilePath, targetType, (object)obj);
		}

		/// <summary>
		/// Serializes an object to the specified output file.
		/// </summary>
		/// <param name="outputFilePath"> The output file path to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SetObjectToFile(string outputFilePath, Type targetType, object obj)
		{
			if ((object)outputFilePath == null)
				throw new ArgumentNullException(nameof(outputFilePath));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (Stream stream = File.Open(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
				this.SetObjectToStream(stream, targetType, obj);
		}

		/// <summary>
		/// Serializes an object to the specified writable stream.
		/// </summary>
		/// <typeparam name="TObject"> The target run-time type of the root of the object graph to serialize. </typeparam>
		/// <param name="stream"> The writable stream to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SetObjectToStream<TObject>(Stream stream, TObject obj)
		{
			Type targetType;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			this.SetObjectToStream(stream, targetType, (object)obj);
		}

		/// <summary>
		/// Serializes an object to the specified writable stream.
		/// </summary>
		/// <param name="stream"> The writable stream to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SetObjectToStream(Stream stream, Type targetType, object obj)
		{
			JsonSerializer jsonSerializer;

			if ((object)stream == null)
				throw new ArgumentNullException(nameof(stream));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings()
													{
														Formatting = Formatting.Indented,
														TypeNameHandling = TypeNameHandling.None,
														ReferenceLoopHandling = ReferenceLoopHandling.Ignore
													});

			using (StreamWriter streamWriter = new StreamWriter(stream))
			{
				using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
					jsonSerializer.Serialize(jsonWriter, obj, targetType);
			}
		}

		/// <summary>
		/// Serializes an object to a string value.
		/// </summary>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		/// <returns> A string representation of the object graph. </returns>
		public string SetObjectToString(Type targetType, object obj)
		{
			StringWriter stringWriter;

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			using (stringWriter = new StringWriter())
			{
				this.SetObjectToWriter(stringWriter, obj);
				return stringWriter.ToString();
			}
		}

		/// <summary>
		/// Serializes an object to a string value. This is the generic overload.
		/// </summary>
		/// <param name="obj"> The object graph to serialize. </param>
		/// <returns> A string representation of the object graph. </returns>
		public string SetObjectToString<TObject>(TObject obj)
		{
			Type targetType;

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			return this.SetObjectToString(targetType, (object)obj);
		}

		/// <summary>
		/// Serializes an object to the specified text writer.
		/// </summary>
		/// <param name="textWriter"> The text writer to serialize. </param>
		/// <param name="targetType"> The target run-time type of the root of the object graph to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SetObjectToWriter(TextWriter textWriter, Type targetType, object obj)
		{
			JsonSerializer jsonSerializer;

			if ((object)textWriter == null)
				throw new ArgumentNullException(nameof(textWriter));

			if ((object)targetType == null)
				throw new ArgumentNullException(nameof(targetType));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings()
													{
														Formatting = Formatting.Indented,
														TypeNameHandling = TypeNameHandling.None,
														ReferenceLoopHandling = ReferenceLoopHandling.Ignore
													});

			using (JsonWriter jsonWriter = new JsonTextWriter(textWriter))
				jsonSerializer.Serialize(jsonWriter, obj, targetType);
		}

		/// <summary>
		/// Serializes an object to the specified text writer. This is the generic overload.
		/// </summary>
		/// <param name="textWriter"> The text writer to serialize. </param>
		/// <param name="obj"> The object graph to serialize. </param>
		public void SetObjectToWriter<TObject>(TextWriter textWriter, TObject obj)
		{
			Type targetType;

			if ((object)textWriter == null)
				throw new ArgumentNullException(nameof(textWriter));

			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			targetType = obj.GetType();

			this.SetObjectToWriter(textWriter, targetType, (object)obj);
		}

		void IJsonSerializationStrategy.SetObjectToWriter(JsonWriter jsonWriter, Type targetType, object obj)
		{
		}

		void IJsonSerializationStrategy.SetObjectToWriter<TObject>(JsonWriter jsonWriter, TObject obj)
		{
		}

		#endregion
	}
}
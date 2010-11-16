#region License

// Copyright 2009 The Sixth Form College Farnborough (http://www.farnborough.ac.uk)
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://github.com/JeremySkinner/SagePayMvc

#endregion

using System;
using System.Reflection;

namespace SagePayMvc.Internal {
	/// <summary>
	/// Used for deserializing SagePay response data. 
	/// </summary>
	public class ResponseSerializer {
		/// <summary>
		/// Deserializes the response into an instance of type T.
		/// </summary>
		public void Deserialize<T>(string input, T objectToDeserializeInto) {
			Deserialize(typeof (T), input, objectToDeserializeInto);
		}

		/// <summary>
		/// Deserializes the response into an object of type T.
		/// </summary>
		public T Deserialize<T>(string input) where T : new() {
			var instance = new T();
			Deserialize(typeof (T), input, instance);
			return instance;
		}

		/// <summary>
		/// Deserializes the response into an object of the specified type.
		/// </summary>
		public void Deserialize(Type type, string input, object objectToDeserializeInto) {
			if (string.IsNullOrEmpty(input)) return;

			var bits = input.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

			foreach (var nameValuePairCombined in bits) {
				int index = nameValuePairCombined.IndexOf('=');
				string name = nameValuePairCombined.Substring(0, index);
				string value = nameValuePairCombined.Substring(index + 1);

				var prop = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);

				if (prop == null) {
					throw new InvalidOperationException(string.Format("Could not find a property on Type '{0}' named '{1}'", type.Name,
					                                                  name));
				}

				//TODO: Investigate building a method of defining custom serializers

				object convertedValue;

				if (prop.PropertyType == typeof (ResponseType)) {
					convertedValue = ConvertStringToSagePayResponseType(value);
				}
				else {
					convertedValue = Convert.ChangeType(value, prop.PropertyType);
				}

				prop.SetValue(objectToDeserializeInto, convertedValue, null);
			}
		}

		/// <summary>
		/// Deserializes the response into an object of the specified type.
		/// </summary>
		public object Deserialize(Type type, string input) {
			var instance = Activator.CreateInstance(type);
			Deserialize(type, input, instance);
			return instance;
		}

		/// <summary>
		/// Utility method for converting a string into a ResponseType. 
		/// </summary>
		public static ResponseType ConvertStringToSagePayResponseType(string input) {
			if (!string.IsNullOrEmpty(input)) {
				if (input.StartsWith("OK")) {
					return ResponseType.Ok;
				}

				if (input.StartsWith("NOTAUTHED")) {
					return ResponseType.NotAuthed;
				}

				if (input.StartsWith("ABORT")) {
					return ResponseType.Abort;
				}

				if (input.StartsWith("REJECTED")) {
					return ResponseType.Rejected;
				}

				if (input.StartsWith("MALFORMED")) {
					return ResponseType.Malformed;
				}

				if (input.StartsWith("AUTHENTICATED")) {
					return ResponseType.Authenticated;
				}

				if (input.StartsWith("INVALID")) {
					return ResponseType.Invalid;
				}

				if (input.StartsWith("REGISTERED")) {
					return ResponseType.Registered;
				}

				if (input.StartsWith("ERROR")) {
					return ResponseType.Error;
				}
			}
			return ResponseType.Unknown;
		}
	}
}
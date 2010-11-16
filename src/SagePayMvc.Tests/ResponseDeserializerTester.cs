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

using NUnit.Framework;
using SagePayMvc.Internal;

namespace SagePayMvc.Tests {
	[TestFixture]
	public class ResponseDeserializerTester {
		[Test]
		public void Deserializes_object() {
			string input = "Name=Foo\r\nId=1";
			var serializer = new ResponseSerializer();

			var result = serializer.Deserialize<TestClass>(input);
			result.Name.ShouldEqual("Foo");
			result.Id.ShouldEqual(1);
		}

		[Test]
		public void Deserializes_response_type_ok() {
			string input = "Response=OK";
			var serializer = new ResponseSerializer();
			var result = serializer.Deserialize<TestClass>(input);
			result.Response.ShouldEqual(ResponseType.Ok);
		}

		[Test]
		public void Deserializes_response_type_invalid() {
			string input = "Response=INVALID";
			var serializer = new ResponseSerializer();
			var result = serializer.Deserialize<TestClass>(input);
			result.Response.ShouldEqual(ResponseType.Invalid);
		}

		[Test]
		public void Deserializes_response_type_starting_with_ok() {
			string input = "Response=OK 1 2 3";
			var serializer = new ResponseSerializer();
			var result = serializer.Deserialize<TestClass>(input);
			result.Response.ShouldEqual(ResponseType.Ok);
		}

		[Test]
		public void Deserializes_response_type_error() {
			string input = "Response=ERROR";
			var serializer = new ResponseSerializer();
			var result = serializer.Deserialize<TestClass>(input);
			result.Response.ShouldEqual(ResponseType.Error);
		}


		[Test]
		public void Deserializes_response_type_notauthed() {
			string input = "Response=NOTAUTHED";
			var serializer = new ResponseSerializer();
			var result = serializer.Deserialize<TestClass>(input);
			result.Response.ShouldEqual(ResponseType.NotAuthed);
		}

		[Test]
		public void Deserializes_response_type_abort() {
			string input = "Response=ABORT";
			var serializer = new ResponseSerializer();
			var result = serializer.Deserialize<TestClass>(input);
			result.Response.ShouldEqual(ResponseType.Abort);
		}

		[Test]
		public void Deserializes_response_type_rejected() {
			string input = "Response=REJECTED";
			var serializer = new ResponseSerializer();
			var result = serializer.Deserialize<TestClass>(input);
			result.Response.ShouldEqual(ResponseType.Rejected);
		}

		[Test]
		public void Deserializes_response_type_authenticated() {
			string input = "Response=AUTHENTICATED";
			var serializer = new ResponseSerializer();
			var result = serializer.Deserialize<TestClass>(input);
			result.Response.ShouldEqual(ResponseType.Authenticated);
		}

		[Test]
		public void Deserializes_response_type_Registered() {
			string input = "Response=REGISTERED";
			var serializer = new ResponseSerializer();
			var result = serializer.Deserialize<TestClass>(input);
			result.Response.ShouldEqual(ResponseType.Registered);
		}

		[Test]
		public void Deserializes_response_type_unknown() {
			string input = "Response=UNKNOWN";
			var serializer = new ResponseSerializer();
			var result = serializer.Deserialize<TestClass>(input);
			result.Response.ShouldEqual(ResponseType.Unknown);
		}

		[Test]
		public void Deserializes_response_type_malformed() {
			string input = "Response=MALFORMED";
			var serializer = new ResponseSerializer();
			var result = serializer.Deserialize<TestClass>(input);
			result.Response.ShouldEqual(ResponseType.Malformed);
		}

		[Test]
		public void Deserializes_response_with_equals_in_value() {
			string input = "Name=foo=bar\r\nId=0";
			var serializer = new ResponseSerializer();
			var result = serializer.Deserialize<TestClass>(input);
			result.Name.ShouldEqual("foo=bar");
		}

		class TestClass {
			public string Name { get; set; }
			public int Id { get; set; }
			public ResponseType Response { get; set; }
		}
	}
}
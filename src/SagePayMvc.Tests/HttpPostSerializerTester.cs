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
	public class HttpPostSerializerTester {
		[Test]
		public void Serialized_properties() {
			var test = new TestClass {Name = "foo", Age = 4};
			var serializer = new HttpPostSerializer();

			var result = serializer.Serialize(test);
			result.ShouldEqual("Name=foo&Age=4");
		}

		[Test]
		public void Excludes_optional_Item() {
			var test = new TestClass {Age = 4};
			var result = new HttpPostSerializer().Serialize(test);

			result.ShouldEqual("Age=4");
		}

		[Test]
		public void Formats_property() {
			var test = new TestClassWithFormat {Age = 5.5m};
			var result = new HttpPostSerializer().Serialize(test);
			result.ShouldEqual("Age=5.50");
		}

		[Test]
		public void Encodes_property() {
			var test = new TestClass {Name = "hello there"};
			var result = new HttpPostSerializer().Serialize(test);
			result.ShouldEqual("Name=hello+there&Age=0");
		}

		[Test]
		public void Does_not_encode_when_Unencoded_attribute_used() {
			var test = new TestClassWithUnencodedProperty {Name = "hello there"};
			var result = new HttpPostSerializer().Serialize(test);
			result.ShouldEqual("Name=hello there");
		}

		public class TestClass {
			[Optional]
			public string Name { get; set; }

			public int Age { get; set; }
		}

		public class TestClassWithFormat {
			[Format("f2")]
			public decimal Age { get; set; }
		}

		public class TestClassWithUnencodedProperty {
			[Unencoded]
			public string Name { get; set; }
		}
	}
}
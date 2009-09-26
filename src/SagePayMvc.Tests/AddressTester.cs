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

namespace SagePayMvc.Tests {
	[TestFixture]
	public class AddressTester {
		[Test]
		public void Converts_address_to_string() {
			string expected = "&BillingSurname=Bar&BillingFirstnames=Foo&BillingAddress1=Address+1&BillingAddress2=Address+2";
			expected += "&BillingCity=My+City&BillingPostCode=XX11+1XX&BillingCountry=UK&BillingState=Some+state&BillingPhone=123";

			var address = new Address {
			                          	Firstnames = "Foo",
			                          	Surname = "Bar",
			                          	Address1 = "Address 1",
			                          	Address2 = "Address 2",
			                          	City = "My City",
			                          	Country = "UK",
			                          	State = "Some state",
			                          	Phone = "123",
			                          	PostCode = "XX11 1XX"
			                          };

			var result = address.ToString(AddressType.Billing);
			result.ShouldEqual(expected);
		}

		[Test]
		public void Address2_should_be_optional() {
			string expected = "&BillingSurname=Bar&BillingFirstnames=Foo&BillingAddress1=Address+1";
			expected += "&BillingCity=My+City&BillingPostCode=XX11+1XX&BillingCountry=UK&BillingState=Some+state&BillingPhone=123";

			var address = new Address {
			                          	Firstnames = "Foo",
			                          	Surname = "Bar",
			                          	Address1 = "Address 1",
			                          	City = "My City",
			                          	Country = "UK",
			                          	State = "Some state",
			                          	Phone = "123",
			                          	PostCode = "XX11 1XX"
			                          };

			var result = address.ToString(AddressType.Billing);
			result.ShouldEqual(expected);
		}

		[Test]
		public void State_should_be_optional() {
			string expected = "&BillingSurname=Bar&BillingFirstnames=Foo&BillingAddress1=Address+1&BillingAddress2=Address+2";
			expected += "&BillingCity=My+City&BillingPostCode=XX11+1XX&BillingCountry=UK&BillingPhone=123";

			var address = new Address {
			                          	Firstnames = "Foo",
			                          	Surname = "Bar",
			                          	Address1 = "Address 1",
			                          	Address2 = "Address 2",
			                          	City = "My City",
			                          	Country = "UK",
			                          	Phone = "123",
			                          	PostCode = "XX11 1XX"
			                          };

			var result = address.ToString(AddressType.Billing);
			result.ShouldEqual(expected);
		}

		[Test]
		public void Phone_should_be_optional() {
			string expected = "&BillingSurname=Bar&BillingFirstnames=Foo&BillingAddress1=Address+1&BillingAddress2=Address+2";
			expected += "&BillingCity=My+City&BillingPostCode=XX11+1XX&BillingCountry=UK&BillingState=Some+state";

			var address = new Address {
			                          	Firstnames = "Foo",
			                          	Surname = "Bar",
			                          	Address1 = "Address 1",
			                          	Address2 = "Address 2",
			                          	City = "My City",
			                          	Country = "UK",
			                          	State = "Some state",
			                          	PostCode = "XX11 1XX"
			                          };

			var result = address.ToString(AddressType.Billing);
			result.ShouldEqual(expected);
		}
	}
}
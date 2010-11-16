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
using Moq;
using NUnit.Framework;

namespace SagePayMvc.Tests {
	[TestFixture]
	public class RefundRegistrationTester {
		Mock<IHttpRequestSender> requestFactory;
		RefundRegistrar registration;
		Configuration config;

		[SetUp]
		public void Setup() {
			requestFactory = new Mock<IHttpRequestSender>();
			config = new Configuration {VendorName = "TestVendor"};
			registration = new RefundRegistrar(config, requestFactory.Object);
		}

		[Test]
		public void Creates_correct_post() {
			string expected = "VPSProtocol=2.23&TxType=REFUND&Vendor=TestVendor&VendorTxCode=REF-foo&Amount=5.00&Currency=GBP";
			expected += "&Description=Refund+Reason&RelatedVPSTxId=abc123&RelatedVendorTxCode=def456";
			expected += "&RelatedSecurityKey=12345&RelatedTxAuthNo=67890";

			string actual = null;

			requestFactory.Setup(x => x.SendRequest(It.IsAny<string>(), It.IsAny<string>())).Callback(new Action<string, string>((url, post) => { actual = post; }));

			registration.Send(
				/*vendor tx code*/  "REF-foo",
				                    /*description*/ "Refund Reason",
				                    /*amount*/ 5,
				                    /*Related VPS Tx ID*/ "abc123",
				                    /*related vendor tx code */"def456",
				                    /*related security key*/"12345",
				                    /*related auth no*/"67890");

			actual.ShouldEqual(expected);
		}

		[Test]
		public void Deserializes_result() {
			string response = "VPSProtocol=2.23\r\nStatus=OK\r\nStatusDetail=detail\r\nVPSTxId=123\r\nTxAuthNo=456\r\n";
			requestFactory.Setup(x => x.SendRequest(It.IsAny<string>(), It.IsAny<string>())).Returns(response);

			var result = registration.Send(
				/*vendor tx code*/  "REF-foo",
				                    /*description*/ "Refund Reason",
				                    /*amount*/ 5,
				                    /*Related VPS Tx ID*/ "abc123",
				                    /*related vendor tx code */"def456",
				                    /*related security key*/"12345",
				                    /*related auth no*/"67890");

			result.VPSProtocol.ShouldEqual("2.23");
			result.Status.ShouldEqual(ResponseType.Ok);
			result.StatusDetail.ShouldEqual("detail");
			result.VPSTxId.ShouldEqual("123");
			result.TxAuthNo.ShouldEqual("456");
		}
	}
}
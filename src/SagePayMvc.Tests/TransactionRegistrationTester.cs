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
using System.Web;
using Moq;
using NUnit.Framework;

namespace SagePayMvc.Tests {
	[TestFixture]
	public class TransactionRegistrationTester {
		Address deliveryAddress;
		Address billingAddress;
		ShoppingBasket basket;
		Configuration config;
		StubUrlResolver urlResolver;
		Mock<IHttpRequestSender> requestFactory;
		ITransactionRegistrar registration;

		[SetUp]
		public void Setup() {
			deliveryAddress = new Address {
				Surname = "delivery-surname",
				Firstnames = "delivery-firstname",
				Address1 = "delivery-address1",
				Address2 = "delivery-address2",
				City = "delivery-city",
				PostCode = "delivery-postcode",
				State = "delivery-state",
				Phone = "delivery-phone",
				Country = "delivery-country"
			};

			billingAddress = new Address {
				Surname = "Surname",
				Firstnames = "Firstname",
				Address1 = "Address1",
				Address2 = "Address2",
				City = "City",
				PostCode = "postcode",
				Country = "country",
				State = "state",
				Phone = "phone"
			};

			basket = new ShoppingBasket("My basket") {
     			new BasketItem(1, "foo", 10, 2)
			};

			config = new Configuration { VendorName = "TestVendor" };

			urlResolver = new StubUrlResolver();
			requestFactory = new Mock<IHttpRequestSender>();
			registration = new TransactionRegistrar(config, urlResolver, requestFactory.Object);
		}


		[Test]
		public void Creates_correct_post() {
			//yuck
			string expected = "VPSProtocol=2.23&TxType=PAYMENT&Vendor=TestVendor&VendorTxCode=foo&Amount=20.00&Currency=GBP&Description=My+basket";
			expected += "&NotificationURL=" + StubUrlResolver.NotificationUrl + "&BillingSurname=Surname&BillingFirstnames=Firstname&BillingAddress1=Address1&BillingAddress2=Address2";
			expected += "&BillingCity=City&BillingPostCode=postcode&BillingCountry=country&BillingState=state&BillingPhone=phone";
			expected += "&DeliverySurname=delivery-surname&DeliveryFirstnames=delivery-firstname&DeliveryAddress1=delivery-address1&DeliveryAddress2=delivery-address2";
			expected += "&DeliveryCity=delivery-city&DeliveryPostCode=delivery-postcode&DeliveryCountry=delivery-country&DeliveryState=delivery-state&DeliveryPhone=delivery-phone&CustomerEMail=email%40address.com";
			expected += "&Basket=" + HttpUtility.UrlEncode(basket.ToString());
			expected += "&AllowGiftAid=0&Apply3DSecure=0&Profile=NORMAL";

			string actual = null;

			requestFactory.Setup(x => x.SendRequest(It.IsAny<string>(), It.IsAny<string>())).Callback(new Action<string, string>((url, post) => { actual = post; }));

			registration.Send(null, "foo", basket, billingAddress, deliveryAddress, "email@address.com", PaymentFormProfile.Normal);

			actual.ShouldEqual(expected);
		}

		[Test]
		public void Deserialzies_result() {
			string sagePayResponse = "VPSProtocol=2.23\r\nStatus=AUTHENTICATED\r\nStatusDetail=detail goes here\r\nVPSTxId=12345\r\nSecurityKey=abcde\r\nNextURL=http://foo.com";
			requestFactory.Setup(x => x.SendRequest(It.IsAny<string>(), It.IsAny<string>())).Returns(sagePayResponse);

			var result = registration.Send(null, "foo", basket, billingAddress, deliveryAddress, "email@address.com", PaymentFormProfile.Normal);
			result.NextURL.ShouldEqual("http://foo.com");
			result.VPSProtocol.ShouldEqual("2.23");
			result.Status.ShouldEqual(ResponseType.Authenticated);
			result.StatusDetail.ShouldEqual("detail goes here");
			result.VPSTxId.ShouldEqual("12345");
			result.SecurityKey.ShouldEqual("abcde");
		}

        [Test]
        public void Sets_profile_correctly() {
            string actual = null;

            requestFactory.Setup(x => x.SendRequest(It.IsAny<string>(), It.IsAny<string>())).Callback(new Action<string, string>((url, post) => { actual = post; }));

            registration.Send(null, "foo", basket, billingAddress, deliveryAddress, "email@address.com", PaymentFormProfile.Low);

            actual.ShouldContain("Profile=LOW");
        }
	}
}
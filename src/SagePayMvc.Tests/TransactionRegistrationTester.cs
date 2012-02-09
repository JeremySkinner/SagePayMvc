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
using System.Globalization;
using System.Threading;
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
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");


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
     			new BasketItem(1, "foo", 10.5m, 2.5m)
			};

			config = new Configuration { VendorName = "TestVendor" };

			urlResolver = new StubUrlResolver();
			requestFactory = new Mock<IHttpRequestSender>();
			registration = new TransactionRegistrar(config, urlResolver, requestFactory.Object);
		}


		[Test]
		public void Creates_correct_post() {
			//yuck
			string expected = "VPSProtocol=2.23&TxType=PAYMENT&Vendor=TestVendor&VendorTxCode=foo&Amount=26.25&Currency=GBP&Description=My+basket";
			expected += "&NotificationURL=http://stub/notification";
			expected += "&BillingSurname=Surname&BillingFirstnames=Firstname&BillingAddress1=Address1&BillingAddress2=Address2&BillingCity=City&BillingPostCode=postcode&BillingCountry=country&BillingState=state";
			expected += "&BillingPhone=phone&DeliverySurname=delivery-surname&DeliveryFirstnames=delivery-firstname&DeliveryAddress1=delivery-address1&DeliveryAddress2=delivery-address2&DeliveryCity=delivery-city";
			expected += "&DeliveryPostCode=delivery-postcode&DeliveryCountry=delivery-country&DeliveryState=delivery-state&DeliveryPhone=delivery-phone&CustomerEMail=email%40address.com";
			expected += "&Basket=1%3afoo%3a1%3a10.50%3a15.75%3a26.25%3a26.25&AllowGiftAid=0&Apply3DSecure=0&Profile=NORMAL";

			string actual = null;

			requestFactory.Setup(x => x.SendRequest(It.IsAny<string>(), It.IsAny<string>())).Callback(new Action<string, string>((url, post) => { actual = post; }));

			registration.Send(null, "foo", basket, billingAddress, deliveryAddress, "email@address.com");

			actual.ShouldEqual(expected);
		}

		[Test]
		public void Creates_correct_post_when_using_other_culture() {
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-de");
			Thread.CurrentThread.CurrentCulture = new CultureInfo("de-de");

			//yuck
			string expected = "VPSProtocol=2.23&TxType=PAYMENT&Vendor=TestVendor&VendorTxCode=foo&Amount=26.25&Currency=GBP&Description=My+basket";
			expected += "&NotificationURL=http://stub/notification";
			expected += "&BillingSurname=Surname&BillingFirstnames=Firstname&BillingAddress1=Address1&BillingAddress2=Address2&BillingCity=City&BillingPostCode=postcode&BillingCountry=country&BillingState=state";
			expected += "&BillingPhone=phone&DeliverySurname=delivery-surname&DeliveryFirstnames=delivery-firstname&DeliveryAddress1=delivery-address1&DeliveryAddress2=delivery-address2&DeliveryCity=delivery-city";
			expected += "&DeliveryPostCode=delivery-postcode&DeliveryCountry=delivery-country&DeliveryState=delivery-state&DeliveryPhone=delivery-phone&CustomerEMail=email%40address.com";
			expected += "&Basket=1%3afoo%3a1%3a10.50%3a15.75%3a26.25%3a26.25&AllowGiftAid=0&Apply3DSecure=0&Profile=NORMAL";


			string actual = null;

			requestFactory.Setup(x => x.SendRequest(It.IsAny<string>(), It.IsAny<string>())).Callback(new Action<string, string>((url, post) => { actual = post; }));

			registration.Send(null, "foo", basket, billingAddress, deliveryAddress, "email@address.com");

			actual.ShouldEqual(expected);
		}

		[Test]
		public void Using_alternate_currency() {
			string actual = null;
			requestFactory.Setup(x => x.SendRequest(It.IsAny<string>(), It.IsAny<string>())).Callback(new Action<string, string>((url, post) => { actual = post; }));
			registration.Send(null, "foo", basket, billingAddress, deliveryAddress, "email@address.com", currencyCode: "EUR");
			StringAssert.Contains("Currency=EUR", actual);
		}

		[Test]
		public void Deserialzies_result() {
			string sagePayResponse = "VPSProtocol=2.23\r\nStatus=AUTHENTICATED\r\nStatusDetail=detail goes here\r\nVPSTxId=12345\r\nSecurityKey=abcde\r\nNextURL=http://foo.com";
			requestFactory.Setup(x => x.SendRequest(It.IsAny<string>(), It.IsAny<string>())).Returns(sagePayResponse);

			var result = registration.Send(null, "foo", basket, billingAddress, deliveryAddress, "email@address.com");
			result.NextURL.ShouldEqual("http://foo.com");
			result.VPSProtocol.ShouldEqual("2.23");
			result.Status.ShouldEqual(ResponseType.Authenticated);
			result.StatusDetail.ShouldEqual("detail goes here");
			result.VPSTxId.ShouldEqual("12345");
			result.SecurityKey.ShouldEqual("abcde");
		}
	}
}
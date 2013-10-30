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
using System.Security.Policy;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;

namespace SagePayMvc.Tests {
	[TestFixture]
	public class DefaultUrlResolverTester {
		DefaultUrlResolver resolver;
		ControllerContext context;

		[TestFixtureSetUp]
		public void TestFixtureSetup() {
			RouteTable.Routes.Clear();

			Configuration.Configure(new Configuration {NotificationHostName = "foo.com"});
			RouteTable.Routes.MapRoute("payment-response", "{controller}/{action}/{vendorTxCode}", new {action = "Index", vendorTxCode = ""});
		}

		[TestFixtureTearDown]
		public void TestFixtureTeardown() {
			RouteTable.Routes.Clear();
			Configuration.Configure(null);
		}

		[SetUp]
		public void Setup() {
			resolver = new DefaultUrlResolver();
			var httpContext = new MockHttpContext();
			httpContext.HttpRequest.Setup(x => x.Url).Returns(new Uri("http://foo.com/fake/path"));
			context = new TestController(httpContext).ControllerContext;
		}

		[Test]
		public void Resolves_successful_url() {
			string url = resolver.BuildSuccessfulTransactionUrl(context.RequestContext, "foo");
			url.ShouldEqual("http://foo.com/PaymentResponse/Success/foo");
		}

		[Test]
		public void Resolves_failed_url() {
			string url = resolver.BuildFailedTransactionUrl(context.RequestContext, "foo");
			url.ShouldEqual("http://foo.com/PaymentResponse/Failed/foo");
		}

		[Test]
		public void Resolves_notification_url() {
			string url = resolver.BuildNotificationUrl(context.RequestContext);
			url.ShouldEqual("http://foo.com/PaymentResponse");
		}

		[Test]
		public void Uses_raw_notification_url_if_notification_controller_null() {
		}

		[Test]
		public void Uses_https() {
			Configuration.Current.Protocol = "https";
			var url = resolver.BuildSuccessfulTransactionUrl(context.RequestContext, "foo");
			Configuration.Current.Protocol = "http";
			url.ShouldEqual("https://foo.com/PaymentResponse/Success/foo");
		}
	}
}
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
using NUnit.Framework;
using SagePayMvc.ActionResults;

namespace SagePayMvc.Tests {
	[TestFixture]
	public class SuccessfulTransactionResultTester {
		ValidOrderResult result;
		TestController controller;
		SagePayResponse response;
		MockHttpContext context;

		[TestFixtureSetUp]
		public void TestFixtureSetup() {
			UrlResolver.Initialize(() => new StubUrlResolver());
		}

		[TestFixtureTearDown]
		public void TestFixtureTeardown() {
			UrlResolver.Initialize(null);
		}

		[SetUp]
		public void Setup() {
			context = new MockHttpContext();
			controller = new TestController(context);
			response = new SagePayResponse();
			result = new ValidOrderResult("foo", response);
		}

		[Test]
		public void Sets_content_type() {
			result.ExecuteResult(controller.ControllerContext);
			context.Object.Response.ContentType.ShouldEqual("text/plain");
		}

		[Test]
		public void Sets_status_ok_if_response_status_not_error() {
			response.Status = ResponseType.Ok;

			result.ExecuteResult(controller.ControllerContext);
			var output = context.Object.Response.Output.ToString();
			output.ShouldStartWith("Status=OK\r\n");
		}

		[Test]
		public void Sets_status_invalid_if_response_status_is_error() {
			response.Status = ResponseType.Error;

			result.ExecuteResult(controller.ControllerContext);
			var output = context.Object.Response.Output.ToString();
			output.ShouldStartWith("Status=INVALID\r\n");
		}

		[Test]
		public void Redirect_to_order_success_page_if_status_is_ok() {
			response.Status = ResponseType.Ok;

			result.ExecuteResult(controller.ControllerContext);
			var output = context.Object.Response.Output.ToString().Split(new[] {"\r\n"}, StringSplitOptions.None);
			output[1].ShouldEqual("RedirectURL=" + StubUrlResolver.SuccessUrl);
		}

		[Test]
		public void Redirect_to_order_success_page_if_status_is_authenticated() {
			response.Status = ResponseType.Authenticated;

			result.ExecuteResult(controller.ControllerContext);
			var output = context.Object.Response.Output.ToString().Split(new[] {"\r\n"}, StringSplitOptions.None);
			output[1].ShouldEqual("RedirectURL=" + StubUrlResolver.SuccessUrl);
		}

		[Test]
		public void Redirect_to_order_success_page_if_status_is_registered() {
			response.Status = ResponseType.Registered;

			result.ExecuteResult(controller.ControllerContext);
			var output = context.Object.Response.Output.ToString().Split(new[] {"\r\n"}, StringSplitOptions.None);
			output[1].ShouldEqual("RedirectURL=" + StubUrlResolver.SuccessUrl);
		}

		[Test]
		public void Redirect_to_order_failed_page_if_status_not_one_of_ok_authenticated_registered() {
			response.Status = ResponseType.Error;

			result.ExecuteResult(controller.ControllerContext);
			var output = context.Object.Response.Output.ToString().Split(new[] {"\r\n"}, StringSplitOptions.None);
			output[1].ShouldEqual("RedirectURL=" + StubUrlResolver.FailUrl);
		}
	}
}
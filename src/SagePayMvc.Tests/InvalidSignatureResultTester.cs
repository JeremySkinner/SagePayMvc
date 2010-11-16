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
	public class InvalidSignatureResultTester {
		InvalidSignatureResult result;
		MockHttpContext context;
		TestController controller;

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
			result = new InvalidSignatureResult("foo");
			context = new MockHttpContext();
			controller = new TestController(context);
		}

		[Test]
		public void Sets_content_type() {
			context.HttpResponse.SetupProperty(x => x.ContentType);
			result.ExecuteResult(controller.ControllerContext);
			context.HttpResponse.Object.ContentType.ShouldEqual("text/plain");
		}

		[Test]
		public void Sets_status_to_invalid() {
			result.ExecuteResult(controller.ControllerContext);
			var output = context.Object.Response.Output.ToString();
			output.ShouldStartWith("Status=INVALID" + Environment.NewLine);
		}

		[Test]
		public void Sets_redirectUrl() {
			result.ExecuteResult(controller.ControllerContext);
			var output = context.Object.Response.Output.ToString().Split(new[] {Environment.NewLine}, StringSplitOptions.None);
			output[1].ShouldEqual("RedirectURL=" + StubUrlResolver.FailUrl);
		}

		[Test]
		public void Sets_statusDetail() {
			result.ExecuteResult(controller.ControllerContext);
			var output = context.Object.Response.Output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
			output[2].ShouldEqual("StatusDetail=Cannot match the MD5 Hash. Order might be tampered with.");
		}
	}
}
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
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using Moq;

namespace SagePayMvc.Tests {
	public class MockHttpContext : Mock<HttpContextBase> {
		readonly Mock<HttpServerUtilityBase> server = new Mock<HttpServerUtilityBase>();

		public MockHttpContext() {
			HttpRequest = new HttpRequestMock();
			HttpResponse = new HttpResponseMock();

			Setup(c => c.Request).Returns(HttpRequest.Object);
			Setup(c => c.Response).Returns(HttpResponse.Object);
			Setup(x => x.Session).Returns(new MockSessionState());
			Setup(x => x.Server).Returns(server.Object);
			Setup(x => x.Items).Returns(new Hashtable());
			SetupProperty(x => x.User);
		}

		public HttpRequestMock HttpRequest { get; private set; }
		public HttpResponseMock HttpResponse { get; private set; }
	}


	public class HttpRequestMock : Mock<HttpRequestBase> {
		readonly NameValueCollection form = new NameValueCollection();
		readonly NameValueCollection querystring = new NameValueCollection();

		public HttpRequestMock() {
			SetupProperty(r => r.ContentType);
			Setup(r => r.QueryString).Returns(querystring);
			Setup(r => r.ApplicationPath).Returns("/");
			Setup(r => r.Form).Returns(form);
			Setup(x => x.AppRelativeCurrentExecutionFilePath).Returns("/");
			Setup(x => x.Url).Returns(new Uri("http://foo.com/fake/path"));
		}
	}

	public class HttpResponseMock : Mock<HttpResponseBase> {
		readonly TextWriter output = new StringWriter();

		public HttpResponseMock() {
			Setup(m => m.Output).Returns(output);
			Setup(x => x.ApplyAppPathModifier(It.IsAny<string>())).Returns((string s) => s);
			SetupProperty(x => x.ContentType);
		}
	}

	public class MockSessionState : HttpSessionStateBase {
		readonly Hashtable hash = new Hashtable();

		public override object this[string name] {
			get { return hash[name]; }
			set { hash[name] = value; }
		}

		public override void Abandon() {
			Clear();
		}

		public override void Remove(string name) {
			hash.Remove(name);
		}

		public override void RemoveAll() {
			Clear();
		}

		public override HttpSessionStateBase Contents {
			get { return this; }
		}

		public override void Clear() {
			hash.Clear();
		}
	}
}
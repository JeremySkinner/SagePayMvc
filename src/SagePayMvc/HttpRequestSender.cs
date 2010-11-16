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
using System.IO;
using System.Net;
using System.Text;

namespace SagePayMvc {
	/// <summary>
	/// Default implementation of IHttpRequestSender
	/// </summary>
	public class HttpRequestSender : IHttpRequestSender {
		/// <summary>
		/// Sends some data to a URL using an HTTP POST.
		/// </summary>
		/// <param name="url">Url to send to</param>
		/// <param name="postData">The data to send</param>
		public string SendRequest(string url, string postData) {
			var uri = new Uri(url);
			var request = WebRequest.Create(uri);
			var encoding = new UTF8Encoding();
			var requestData = encoding.GetBytes(postData);

			request.ContentType = "application/x-www-form-urlencoded";
			request.Method = "POST";
			request.Timeout = (300*1000); //TODO: Move timeout to config
			request.ContentLength = requestData.Length;

			using (var stream = request.GetRequestStream()) {
				stream.Write(requestData, 0, requestData.Length);
			}

			var response = request.GetResponse();

			string result;

			using (var reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII)) {
				result = reader.ReadToEnd();
			}

			return result;
		}
	}
}
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

using SagePayMvc.Internal;

namespace SagePayMvc {
	public class RefundRegistrar : IRefundRegistrar {
		readonly Configuration configuration;
		readonly IHttpRequestSender requestSender;

		public RefundRegistrar(Configuration configuration, IHttpRequestSender requestSender) {
			this.configuration = configuration;
			this.requestSender = requestSender;
		}

		public RefundRegistrar() : this(Configuration.Current, new HttpRequestSender()) {
		}

		public RefundResponse Send(string vendorTxCode, string refundReason, decimal amount, string relatedVpsTxId,
		                           string relatedVendorTxCode, string relatedSecurityKey, string relatedAuthNo) {
			var registration = new RefundRegistration(configuration.VendorName,
			                                          vendorTxCode,
			                                          amount,
			                                          refundReason,
			                                          relatedVpsTxId,
			                                          relatedVendorTxCode,
			                                          relatedSecurityKey,
			                                          relatedAuthNo);

			string sagePayUrl = configuration.RefundUrl;

			var serializer = new HttpPostSerializer();
			var postData = serializer.Serialize(registration);
			var response = requestSender.SendRequest(sagePayUrl, postData);
			var deserializer = new ResponseSerializer();
			return deserializer.Deserialize<RefundResponse>(response);
		}
	}
}
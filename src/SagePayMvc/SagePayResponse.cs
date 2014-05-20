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

using System.Text;
using System.Web.Mvc;
using System.Web.Security;

namespace SagePayMvc {
	/// <summary>
	/// Object that represents a notification POST from SagePay
	/// </summary>
	[ModelBinder(typeof (SagePayBinder))]
	public class SagePayResponse {
		public ResponseType Status { get; set; }
		public string VendorTxCode { get; set; }
		public string VPSTxId { get; set; }
		public string VPSSignature { get; set; }
		public string StatusDetail { get; set; }
		public string TxAuthNo { get; set; }
		public string AVSCV2 { get; set; }
		public string AddressResult { get; set; }
		public string PostCodeResult { get; set; }
		public string CV2Result { get; set; }
		public string GiftAid { get; set; }
		public string ThreeDSecureStatus { get; set; }
		public string CAVV { get; set; }
		public string AddressStatus { get; set; }
		public string PayerStatus { get; set; }
		public string CardType { get; set; }
		public string Last4Digits { get; set; }

		/// <summary>
		/// Was the transaction successful?
		/// </summary>
		public virtual bool WasTransactionSuccessful {
			get {
				return (Status == ResponseType.Ok ||
				        Status == ResponseType.Authenticated ||
				        Status == ResponseType.Registered);
			}
		}

		/// <summary>
		/// Is the signature valid
		/// </summary>
		public virtual bool IsSignatureValid(string securityKey, string vendorName) {
			return GenerateSignature(securityKey, vendorName) == VPSSignature;
		}

		/// <summary>
		/// Generates the VPS Signature from the parameters of the POST.
		/// </summary>
		public virtual string GenerateSignature(string securityKey, string vendorName) {
			var builder = new StringBuilder();
			builder.Append(VPSTxId);
			builder.Append(VendorTxCode);
			builder.Append(Status.ToString().ToUpper());
			builder.Append(TxAuthNo);
			builder.Append(vendorName.ToLower());
			builder.Append(AVSCV2);
			builder.Append(securityKey);
			builder.Append(AddressResult);
			builder.Append(PostCodeResult);
			builder.Append(CV2Result);
			builder.Append(GiftAid);
			builder.Append(ThreeDSecureStatus);
			builder.Append(CAVV);
			builder.Append(AddressStatus);
			builder.Append(PayerStatus);
			builder.Append(CardType);
			builder.Append(Last4Digits);
			var hash = FormsAuthentication.HashPasswordForStoringInConfigFile(builder.ToString(), "MD5");
			return hash;
		}
	}
}
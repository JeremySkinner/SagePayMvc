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

namespace SagePayMvc.Internal {
	public class RefundRegistration {
		public RefundRegistration(string vendorName, string vendorTxCode, decimal amount, string refundReason,
		                          string relatedVpsTxId, string relatedVendorTxCode, string relatedSecurityKey,
		                          string relatedAuthNo) {
			Vendor = vendorName;
			VendorTxCode = vendorTxCode;
			Description = refundReason;
			RelatedVPSTxId = relatedVpsTxId;
			RelatedVendorTxCode = relatedVendorTxCode;
			RelatedSecurityKey = relatedSecurityKey;
			RelatedTxAuthNo = relatedAuthNo;
			Amount = amount;
		}

		public string VPSProtocol {
			get { return Configuration.ProtocolVersion; }
		}

		public string TxType {
			get { return "REFUND"; }
		}

		public string Vendor { get; set; }

		public string VendorTxCode { get; private set; }

		[Format("f2")]
		public decimal Amount { get; private set; }

		public string Currency {
			get { return "GBP"; } //NOTE: Only GBP supported atm
		}

		public string Description { get; set; }

		public string RelatedVPSTxId { get; set; }
		public string RelatedVendorTxCode { get; set; }
		public string RelatedSecurityKey { get; set; }
		public string RelatedTxAuthNo { get; set; }
	}
}
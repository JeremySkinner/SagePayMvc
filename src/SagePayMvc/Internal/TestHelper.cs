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
	/// <summary>
	/// Utility class to help with unit testing.
	/// </summary>
	public static class TestHelper {
		public const string ValidSecurityKey = "testkey123";
		public const string ValidSignature = "46EAE2D06815F3406C41776422711985";
		public const string VendorName = "testvendor";

		public static SagePayResponse CreateValidResponse() {
			return new SagePayResponse {
			                           	VendorTxCode = "2005296940fc9522c0704cabaf4016b561b36311",
			                           	VPSTxId = "{EBCC5038-1460-9E2F-8A57-24D148B8CD53}",
			                           	Status = ResponseType.Ok,
			                           	StatusDetail = "0000 : The Authorisation was Successful.",
			                           	TxAuthNo = "4665060",
			                           	AVSCV2 = "ALL MATCH",
			                           	AddressResult = "MATCHED",
			                           	PostCodeResult = "MATCHED",
			                           	CV2Result = "MATCHED",
			                           	GiftAid = "0",
			                           	ThreeDSecureStatus = "NOTCHECKED",
			                           	VPSSignature = ValidSignature
			                           };
		}
	}
}
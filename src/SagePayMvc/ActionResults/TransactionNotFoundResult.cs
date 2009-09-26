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

using System.Web.Mvc;

namespace SagePayMvc.ActionResults {
	/// <summary>
	/// Action Result to be returned when the transaction with the specified VendorTxCode could not be found.
	/// </summary>
	public class TransactionNotFoundResult : SagePayResult {
		public TransactionNotFoundResult(string vendorTxCode) : base(vendorTxCode) {
		}

		public override void ExecuteResult(ControllerContext context) {
			context.HttpContext.Response.ContentType = "text/plain";
			context.HttpContext.Response.Output.WriteLine("Status=INVALID");
			context.HttpContext.Response.Output.WriteLine("RedirectURL={0}", BuildFailedUrl(context));
			context.HttpContext.Response.Output.WriteLine("StatusDetail=Unable to find the transaction in our database.");
		}
	}
}
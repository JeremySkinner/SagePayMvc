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

using System.Web.Routing;

namespace SagePayMvc {
	/// <summary>
	/// URL Resolution service
	/// </summary>
	public interface IUrlResolver {
		/// <summary>
		/// Resolves the Failed Transaction URL to be sent to SagePay when the payment fails.
		/// </summary>
		string BuildFailedTransactionUrl(RequestContext context, string vendorTxCode);

		/// <summary>
		/// Resolves the Successful Transaction URL to be sent to SagePay when the payment succeeds.
		/// </summary>
		string BuildSuccessfulTransactionUrl(RequestContext context, string vendorTxCode);

		/// <summary>
		/// Builds the notification URL.
		/// </summary>
		string BuildNotificationUrl(RequestContext context);
	}
}
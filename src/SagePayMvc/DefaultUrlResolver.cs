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
using System.Web.Routing;

namespace SagePayMvc {
	/// <summary>
	/// Default IUrlResolver implementation.
	/// </summary>
	public class DefaultUrlResolver : IUrlResolver {
		public const string DefaultControllerName = "PaymentResponse";
		public const string FailedActionName = "Failed";
		public const string SuccessfulActionName = "Success";

		readonly Configuration configuration;

		/// <summary>
		/// Creates a new instance of the DefaultUrlResolver using the configuration specified in the web.conf.
		/// </summary>
		public DefaultUrlResolver() : this (Configuration.Current) {
		}

		public DefaultUrlResolver(Configuration configuration)
		{
			this.configuration = configuration;
		}

		public virtual string BuildFailedTransactionUrl(RequestContext context, string vendorTxCode) {
			var urlHelper = new UrlHelper(context);
			var routeValues = new RouteValueDictionary(new {controller = configuration.FailedController, action = configuration.FailedAction, vendorTxCode});

			var hostName = configuration.FailedHostName ?? GetNotificationHostName(context);
			string url = urlHelper.RouteUrl(null, routeValues, configuration.Protocol, hostName);
			return url;
		}

		public virtual string BuildSuccessfulTransactionUrl(RequestContext context, string vendorTxCode) {
			var urlHelper = new UrlHelper(context);
			var routeValues = new RouteValueDictionary(new {controller = configuration.SuccessController, action = configuration.SuccessAction, vendorTxCode});

			var hostName = configuration.SuccessHostName ?? GetNotificationHostName(context);
			string url = urlHelper.RouteUrl(null, routeValues, configuration.Protocol, hostName);
			return url;
		}

		public virtual string BuildNotificationUrl(RequestContext context) {
			var urlHelper = new UrlHelper(context);
			var routeValues = new RouteValueDictionary(new {controller = configuration.NotificationController, action = configuration.NotificationAction});

			string url = urlHelper.RouteUrl(null, routeValues, configuration.Protocol, GetNotificationHostName(context));
			return url;
		}

		public string GetNotificationHostName(RequestContext context) {
			return configuration.NotificationHostName ?? context.HttpContext.Request.Url.Host;
		}
	}
}
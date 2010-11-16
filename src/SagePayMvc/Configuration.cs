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
using System.Collections.Specialized;
using System.Configuration;
using System.Linq.Expressions;

namespace SagePayMvc {
	/// <summary>
	/// Configuration data
	/// </summary>
	public class Configuration {
		public const string ProtocolVersion = "2.23";
		public const string DefaultControllerName = "PaymentResponse";
		public const string DefaultFailedAction = "Failed";
		public const string DefaultSuccessAction = "Success";
		public const decimal DefaultVatMultiplier = 1.175m;
		public const string DefaultResponseAction = "Index";

		public const string LiveUrl = "https://live.sagepay.com/gateway/service/vspserver-register.vsp";
		public const string TestUrl = "https://test.sagepay.com/gateway/service/vspserver-register.vsp";
		public const string SimulatorUrl = "https://test.sagepay.com/simulator/VSPServerGateway.asp?Service=VendorRegisterTx";

		public const string LiveRefundUrl = "https://live.sagepay.com/gateway/service/refund.vsp";
		public const string TestRefundUrl = "https://test.sagepay.com/gateway/service/refund.vsp";
		public const string SimulatorRefundUrl = "https://test.sagepay.com/simulator/vspserverGateway.asp?Service=VendorRefundTx";

		string notificationController = DefaultControllerName;
		string notificationAction = DefaultResponseAction;
		string successAction = DefaultSuccessAction;
		string failedAction = DefaultFailedAction;
		string successController = DefaultControllerName;
		string failedController = DefaultControllerName;
		decimal vatMultiplier = DefaultVatMultiplier;

		string vendorName;
		string notificationHostName;

		/// <summary>
		/// Vendor name. This is required.
		/// </summary>
		public string VendorName {
			get {
				if (string.IsNullOrEmpty(vendorName)) {
					throw new ArgumentNullException("vendorName", "VendorName must be specified in the configuration.");
				}
				return vendorName;
			}
			set {
				if (string.IsNullOrEmpty(value)) {
					throw new ArgumentNullException("value", "VendorName must be specified in the configuration.");
				}
				vendorName = value;
			}
		}


		/// <summary>
		/// Notification host name. This is required. 
		/// </summary>
		public string NotificationHostName {
			get {
				if (string.IsNullOrEmpty(notificationHostName)) {
					throw new ArgumentNullException("notificationHostName", "NotificationHostName must be specified in the configuration.");
				}
				return notificationHostName;
			}
			set {
				if (string.IsNullOrEmpty(value)) {
					throw new ArgumentNullException("value", "NotificationHostName must be specified in the configuration.");
				}
				notificationHostName = value;
			}
		}

		/// <summary>
		/// Server mode (simulator, test, live)
		/// </summary>
		public VspServerMode Mode { get; set; }

		/// <summary>
		/// The controller name to use when when generating the notification url. Default is "PaymentResponse".
		/// </summary>
		public string NotificationController {
			get { return notificationController; }
			set {
				if (!string.IsNullOrEmpty(value)) {
					notificationController = value;
				}
			}
		}

		/// <summary>
		/// Action name to use when generating the notification url. Default is "Index"
		/// </summary>
		public string NotificationAction {
			get { return notificationAction; }
			set {
				if (!string.IsNullOrEmpty(value)) {
					notificationAction = value;
				}
			}
		}

		/// <summary>
		/// Action name to use when generating the success URL. Default is "Success"
		/// </summary>
		public string SuccessAction {
			get { return successAction; }
			set {
				if (!string.IsNullOrEmpty(value))
					successAction = value;
			}
		}

		/// <summary>
		/// Action name to use when generating the failure url. Defualt is "Failed"
		/// </summary>
		public string FailedAction {
			get { return failedAction; }
			set {
				if (!string.IsNullOrEmpty(value))
					failedAction = value;
			}
		}

		/// <summary>
		/// Controller name to use when generating the success URL. Default is "PaymentResponse"
		/// </summary>
		public string SuccessController {
			get { return successController; }
			set {
				if (!string.IsNullOrEmpty(value))
					successController = value;
			}
		}

		/// <summary>
		/// Controller name to use when generating the failed URL. Default is "PaymentResponse"
		/// </summary>
		public string FailedController {
			get { return failedController; }
			set {
				if (!string.IsNullOrEmpty(value))
					failedController = value;
			}
		}


		/// <summary>
		/// VAT multiplier. Default is 1.15. 
		/// </summary>
		public decimal VatMultiplier {
			get { return vatMultiplier; }
			set {
				if (value > 0) {
					vatMultiplier = value;
				}
			}
		}


		static Configuration currentConfiguration;

		/// <summary>
		/// Sets up the configuration using a manually generated Configuration instance rather than using the Web.config file. 
		/// </summary>
		/// <param name="configuration"></param>
		public static void Configure(Configuration configuration) {
			currentConfiguration = configuration;
		}

		/// <summary>
		/// Gets the current configuration. If none has been specified using Configuration.Configure, it is loaded from the web.config
		/// </summary>
		public static Configuration Current {
			get {
				if (currentConfiguration == null) {
					currentConfiguration = LoadConfigurationFromConfigFile();
				}

				return currentConfiguration;
			}
		}

		/// <summary>
		/// The registration URL
		/// </summary>
		public string RegistrationUrl {
			get {
				switch (Mode) {
					case VspServerMode.Simulator:
						return SimulatorUrl;
					case VspServerMode.Test:
						return TestUrl;
					case VspServerMode.Live:
						return LiveUrl;
				}
				return null;
			}
		}

		public string RefundUrl {
			get {
				switch (Mode) {
					case VspServerMode.Simulator:
						return SimulatorRefundUrl;
					case VspServerMode.Test:
						return TestRefundUrl;
					case VspServerMode.Live:
						return LiveRefundUrl;
				}

				return null;
			}
		}


		static Configuration LoadConfigurationFromConfigFile() {
			var section = ConfigurationManager.GetSection("sagePay") as NameValueCollection;

			if (section == null) {
				return new Configuration();
			}

			var configuration = new Configuration {
			                                      	NotificationHostName = GetValue(x => x.NotificationHostName, section),
			                                      	NotificationController = GetValue(x => x.NotificationController, section),
			                                      	notificationAction = GetValue(x => x.NotificationAction, section),
			                                      	SuccessAction = GetValue(x => x.SuccessAction, section),
			                                      	FailedAction = GetValue(x => x.FailedAction, section),
			                                      	SuccessController = GetValue(x => x.SuccessController, section),
			                                      	FailedController = GetValue(x => x.FailedController, section),
			                                      	VatMultiplier = Convert.ToDecimal(GetValue(x => x.VatMultiplier, section) ?? "0"),
			                                      	VendorName = GetValue(x => x.VendorName, section),
			                                      	Mode = (VspServerMode) Enum.Parse(typeof (VspServerMode), (GetValue(x => x.Mode, section) ?? "Simulator"))
			                                      };
			return configuration;
		}

		static string GetValue(Expression<Func<Configuration, object>> expression, NameValueCollection collection) {
			var body = expression.Body as MemberExpression;
			if (body == null && expression.Body is UnaryExpression) {
				body = ((UnaryExpression) expression.Body).Operand as MemberExpression;
			}

			string name = body.Member.Name;
			return collection[name];
		}
	}
}
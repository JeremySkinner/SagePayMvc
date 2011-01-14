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

using NUnit.Framework;

namespace SagePayMvc.Tests {
	[TestFixture]
	public class ConfigurationTester {
		[Test]
		public void Loads_configuration_from_config_file() {
			var configuration = Configuration.Current;
			configuration.NotificationHostName.ShouldEqual("mysite.com");

			configuration.NotificationController.ShouldEqual("Controller");
			configuration.NotificationAction.ShouldEqual("response-action");
			configuration.SuccessAction.ShouldEqual("success-action");
			configuration.SuccessController.ShouldEqual("success-controller");
			configuration.FailedAction.ShouldEqual("failed-action");
			configuration.FailedController.ShouldEqual("failed-controller");
			configuration.VatMultiplier.ShouldEqual(5);
			configuration.VendorName.ShouldEqual("MyVendor");
		}

		[Test]
		public void When_no_controller_name_is_specified_the_default_is_used() {
			var configuration = new Configuration();
			configuration.NotificationController.ShouldEqual("PaymentResponse");
		}

		[Test]
		public void When_failed_action_name_not_specified_default_is_used() {
			var configuration = new Configuration();
			configuration.FailedAction.ShouldEqual("Failed");
		}

		[Test]
		public void When_success_action_name_not_specified_default_is_used() {
			var configuration = new Configuration();
			configuration.SuccessAction.ShouldEqual("Success");
		}

		[Test]
		public void When_failed_controller_not_specified_the_default_is_used() {
			var configuration = new Configuration();
			configuration.SuccessController.ShouldEqual("PaymentResponse");
		}

		[Test]
		public void When_success_controller_not_specified_the_default_is_used() {
			var configuration = new Configuration();
			configuration.SuccessController.ShouldEqual("PaymentResponse");
		}

		[Test]
		public void When_VatMultiplier_not_specified_the_default_is_used() {
			var configuration = new Configuration();
			configuration.VatMultiplier.ShouldEqual(1.2);
		}

		[Test]
		public void When_Mode_not_specified_the_Default_is_used() {
			var configuration = new Configuration();
			configuration.Mode.ShouldEqual(VspServerMode.Simulator);
		}

		[Test]
		public void Loads_custom_configuration() {
			var config = new Configuration();
			Configuration.Configure(config);
			Configuration.Current.ShouldBeTheSameAs(config);

			Configuration.Configure(null);
		}
	}
}
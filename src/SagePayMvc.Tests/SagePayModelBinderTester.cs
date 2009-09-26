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
using NUnit.Framework;

namespace SagePayMvc.Tests {
	[TestFixture]
	public class SagePayModelBinderTester {
		ModelBindingContext bindingContext;

		SagePayBinder SetupBinder(FormCollection post) {
			bindingContext = new ModelBindingContext {
				ModelType = typeof(SagePayResponse),
				ValueProvider = post.ToValueProvider()
			};

			return new SagePayBinder();
		}

		[Test]
		public void Creates_SagePayResponse_with_status_ok() {
			var post = new FormCollection { { "Status", "OK" } };
			var binder = SetupBinder(post);

			var result = (SagePayResponse)binder.BindModel(new ControllerContext(), bindingContext);
			result.Status.ShouldEqual(ResponseType.Ok);
		}

		[Test]
		public void Creates_SagePayResponse_with_status_unknown() {
			var post = new FormCollection { { "Status", "Trousers" } };
			var binder = SetupBinder(post);

			var result = (SagePayResponse)binder.BindModel(new ControllerContext(), bindingContext);
			result.Status.ShouldEqual(ResponseType.Unknown);
		}

		[Test]
		public void Creates_SagePayResponse_with_status_declined() {
			var post = new FormCollection { { "Status", "NOTAUTHED" } };
			var binder = SetupBinder(post);

			var result = (SagePayResponse)binder.BindModel(new ControllerContext(), bindingContext);
			result.Status.ShouldEqual(ResponseType.NotAuthed);
		}

		[Test]
		public void Creates_SagePayResponse_with_status_aborted() {
			var post = new FormCollection { { "Status", "ABORT" } };
			var binder = SetupBinder(post);

			var result = (SagePayResponse)binder.BindModel(new ControllerContext(), bindingContext);
			result.Status.ShouldEqual(ResponseType.Abort);
		}

		[Test]
		public void Creates_SagePayResponse_with_status_rejected() {
			var post = new FormCollection { { "Status", "REJECTED" } };
			var binder = SetupBinder(post);

			var result = (SagePayResponse)binder.BindModel(new ControllerContext(), bindingContext);
			result.Status.ShouldEqual(ResponseType.Rejected);
		}

		[Test]
		public void Creates_SagePayResponse_with_status_authenticated() {
			var post = new FormCollection { { "Status", "AUTHENTICATED" } };
			var binder = SetupBinder(post);

			var result = (SagePayResponse)binder.BindModel(new ControllerContext(), bindingContext);
			result.Status.ShouldEqual(ResponseType.Authenticated);
		}

		[Test]
		public void Creates_SagePayResponse_with_status_registered() {
			var post = new FormCollection { { "Status", "REGISTERED" } };
			var binder = SetupBinder(post);

			var result = (SagePayResponse)binder.BindModel(new ControllerContext(), bindingContext);
			result.Status.ShouldEqual(ResponseType.Registered);
		}

		[Test]
		public void Creates_SagePayResponse_with_status_error() {
			var post = new FormCollection { { "Status", "ERROR" } };
			var binder = SetupBinder(post);

			var result = (SagePayResponse)binder.BindModel(new ControllerContext(), bindingContext);
			result.Status.ShouldEqual(ResponseType.Error);
		}

		[Test]
		public void Creates_SagePayResponse_with_status_unknown_from_post_empty() {
			var post = new FormCollection();
			var binder = SetupBinder(post);

			var result = (SagePayResponse)binder.BindModel(new ControllerContext(), bindingContext);
			result.Status.ShouldEqual(ResponseType.Unknown);
		}

		[Test]
		public void Creates_SagePay_Response_with_fields_filled() {
			var post = new FormCollection {
          		{"Status", "OK"},
          		{"VendorTxCode", "20036839SomeGUIDGoesHere"},
          		{"VPSTxId", "Foo"}, {"VPSSignature", "Hash"},
          		{"StatusDetail", "Foobar"}, {"TxAuthNo", "12345"},
          		{"AddressResult", "Thing"}, {"PostCodeResult", "TH11NG"},
          		{"CV2Result", "Bar"}, {"GiftAid", "1"}, {"AVSCV2", "EtcEtc"},
          		{"3DSecureStatus", "Baz"}, {"CAVV", "MyCavv"},
          		{"AddressStatus", "Boo"}, {"PayerStatus", "BillyBob"},
          		{"CardType", "BooCard"}, {"Last4Digits", "9876"}
			};
			var binder = SetupBinder(post);

			var result = (SagePayResponse)binder.BindModel(new ControllerContext(), bindingContext);
			result.Status.ShouldEqual(ResponseType.Ok);
			result.VendorTxCode.ShouldEqual("20036839SomeGUIDGoesHere");
			result.VPSTxId.ShouldEqual("Foo");
			result.VPSSignature.ShouldEqual("Hash");
			result.StatusDetail.ShouldEqual("Foobar");
			result.TxAuthNo.ShouldEqual("12345");
			result.AVSCV2.ShouldEqual("EtcEtc");
			result.AddressResult.ShouldEqual("Thing");
			result.PostCodeResult.ShouldEqual("TH11NG");
			result.CV2Result.ShouldEqual("Bar");
			result.GiftAid.ShouldEqual("1");
			result.ThreeDSecureStatus.ShouldEqual("Baz");
			result.CAVV.ShouldEqual("MyCavv");
			result.AddressStatus.ShouldEqual("Boo");
			result.PayerStatus.ShouldEqual("BillyBob");
			result.CardType.ShouldEqual("BooCard");
			result.Last4Digits.ShouldEqual("9876");
		}
	}
}
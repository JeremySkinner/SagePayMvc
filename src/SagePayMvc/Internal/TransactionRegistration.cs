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
	/// Represents a transaction registration that is sent to SagePay. 
	/// This should be serialized using the HttpPostSerializer.
	/// </summary>
	public class TransactionRegistration {
		readonly ShoppingBasket basket;
		readonly Address billingAddress;
		readonly Address deliveryAddress;
		readonly string customerEMail;
		readonly string vendorName;
	    readonly string profile;
		readonly string currency;

	    const string NormalFormMode = "NORMAL";
        const string LowProfileFormMode = "LOW";

		public TransactionRegistration(string vendorTxCode, ShoppingBasket basket, string notificationUrl,
		                               Address billingAddress, Address deliveryAddress, string customerEmail,
		                               string vendorName, PaymentFormProfile paymentFormProfile, string currencyCode) {
			VendorTxCode = vendorTxCode;
			NotificationURL = notificationUrl;
			this.basket = basket;
			this.billingAddress = billingAddress;
			this.deliveryAddress = deliveryAddress;
			customerEMail = customerEmail;
			this.vendorName = vendorName;
		    switch (paymentFormProfile) {
		        case PaymentFormProfile.Low:
		            profile = LowProfileFormMode;
		            break;
                default:
		            profile = NormalFormMode;
		            break;
		    }
			this.currency = currencyCode;
		}

		public string VPSProtocol {
			get { return Configuration.ProtocolVersion; }
		}

		public string TxType {
			get { return "PAYMENT"; }
		}

		public string Vendor {
			get { return vendorName; }
		}

		public string VendorTxCode { get; private set; }

		[Format("f2")]
		public decimal Amount {
			get { return basket.Total; }
		}

		public string Currency {
			get { return currency; }
		}

		public string Description {
			get { return basket.Name; }
		}

		[Unencoded]
		public string NotificationURL { get; private set; }

		public string BillingSurname {
			get { return billingAddress.Surname; }
		}

		public string BillingFirstnames {
			get { return billingAddress.Firstnames; }
		}

		public string BillingAddress1 {
			get { return billingAddress.Address1; }
		}

		[Optional]
		public string BillingAddress2 {
			get { return billingAddress.Address2; }
		}

		public string BillingCity {
			get { return billingAddress.City; }
		}

		public string BillingPostCode {
			get { return billingAddress.PostCode; }
		}

		public string BillingCountry {
			get { return billingAddress.Country; }
		}

		[Optional]
		public string BillingState {
			get { return billingAddress.State; }
		}

		[Optional]
		public string BillingPhone {
			get { return billingAddress.Phone; }
		}

		public string DeliverySurname {
			get { return deliveryAddress.Surname; }
		}

		public string DeliveryFirstnames {
			get { return deliveryAddress.Firstnames; }
		}

		public string DeliveryAddress1 {
			get { return deliveryAddress.Address1; }
		}

		[Optional]
		public string DeliveryAddress2 {
			get { return deliveryAddress.Address2; }
		}

		public string DeliveryCity {
			get { return deliveryAddress.City; }
		}

		public string DeliveryPostCode {
			get { return deliveryAddress.PostCode; }
		}

		public string DeliveryCountry {
			get { return deliveryAddress.Country; }
		}

		[Optional]
		public string DeliveryState {
			get { return deliveryAddress.State; }
		}

		[Optional]
		public string DeliveryPhone {
			get { return deliveryAddress.Phone; }
		}

		public string CustomerEMail {
			get { return customerEMail; }
		}

		public string Basket {
			get { return basket.ToString(); }
		}

		//NOTE: Not currently supported
		public int AllowGiftAid {
			get { return 0; }
		}

		public int Apply3DSecure {
			get { return 0; }
		}

		public string Profile {
			get { return profile; }
		}
	}
}
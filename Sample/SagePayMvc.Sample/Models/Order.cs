namespace SagePayMvc.Sample.Models {
	using System;

	public class Order {
		public string VendorTxCode { get; set; }
		public string SecurityKey { get; set; }

		public string VpsTxId { get; set; }

		public string RedirectUrl { get; set; }

		public DateTime DateInitialised { get; set; }
	}
}
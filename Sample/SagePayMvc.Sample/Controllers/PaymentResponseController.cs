namespace SagePayMvc.Sample.Controllers {
	using System.Web.Mvc;
	using SagePayMvc.ActionResults;
	using SagePayMvc.Sample.Models;

	public class PaymentResponseController : Controller {
		IOrderRepository _orderRepository;

		public PaymentResponseController(IOrderRepository orderRepository) {
			_orderRepository = orderRepository;
		}

		public ActionResult Notify(SagePayResponse response) {
			// SagePay should have sent back the order ID
			if (string.IsNullOrEmpty(response.VendorTxCode)) {
				return new ErrorResult();
			}

			// Get the order out of our "database"
			var order = _orderRepository.GetById(response.VendorTxCode);

			// IF there was no matching order, send a TransactionNotfound error
			if (order == null) {
				return new TransactionNotFoundResult(response.VendorTxCode);
			}

			// Check if the signature is valid.
			// Note that we need to look up the vendor name from our configuration.
			if (!response.IsSignatureValid(order.SecurityKey, SagePayMvc.Configuration.Current.VendorName)) {
				return new InvalidSignatureResult(response.VendorTxCode);
			}

			// All good - tell SagePay it's safe to charge the customer.
			return new ValidOrderResult(order.VendorTxCode, response);
		}

		public ActionResult Failed(string vendorTxCode) {
			return View();
		}

		public ActionResult Success(string vendorTxCode) {
			return View();
		}
	}
}
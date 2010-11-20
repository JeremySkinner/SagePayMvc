namespace SagePayMvc.Sample.Models {
	using System;
	using System.Web.Routing;

	public interface ITransactionService {
		TransactionRegistrationResponse SendTransaction(IShoppingBasket storeBasket, RequestContext context, User user);
	}

	public class TransactionService : ITransactionService {
		private ITransactionRegistrar _transactionRegistrar;
		private IOrderRepository _orderRepository;

		public TransactionService(ITransactionRegistrar transactionRegistrar, IOrderRepository orderRepository) {
			_transactionRegistrar = transactionRegistrar;
			_orderRepository = orderRepository;
		}

		public TransactionRegistrationResponse SendTransaction(IShoppingBasket storeBasket, RequestContext context, User user) {

			// Construct a SagePay basket from our Store basket.
			// We don't use the SagePay basket directly from the application as it only requires a subset of the information

			var basket = new ShoppingBasket("Shopping Basket for " + user.Name);

			//Fill the basket. The VAT multiplier is not specified here as it is taken from the web.config
			foreach (var item in storeBasket.GetItemsInBasket()) {
				basket.Add(new BasketItem(item.Quantity, item.Product.Name, item.Product.Price));
			}

			// Using the same address for billing and shipping.
			// In reality, you would allow the option of specifying either.
			var sagePayAddress = new Address() {
				Address1 = user.Address1,
				Address2 = user.Address2,
				Surname = user.Surname,
				Firstnames = user.Forename,
				City = user.Town,
				Country = "GB",
				Phone = user.Telephone,
				PostCode = user.Postcode
			};

			var orderId = Guid.NewGuid().ToString();

			var response = _transactionRegistrar.Send(context, orderId, basket, sagePayAddress, sagePayAddress, null);

			if (response.Status != ResponseType.Ok) {
				string error = "Transaction {0} did not register successfully. Status returned was {1} ({2})";
				error = string.Format(error, orderId, response.Status, response.StatusDetail);
				throw new Exception(error);
			}

			var order = new Order {
				VendorTxCode = orderId,
				VpsTxId = response.VPSTxId,
				SecurityKey = response.SecurityKey,
				RedirectUrl = response.NextURL,
				DateInitialised = DateTime.Now
			};

			// In reality you would store more information about the order...
			// ..like the user who made the order and each of the products in the order.

			_orderRepository.StoreOrder(order);

			return response;

		}
	}
}
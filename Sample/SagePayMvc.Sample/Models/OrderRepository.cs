namespace SagePayMvc.Sample.Models {
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class OrderRepository : IOrderRepository {
		// NOTE: In a real application we would store the orders in a database
		// The static list is just used for demo purposes.
		private static List<Order> _orders = new List<Order>();
		
		public void StoreOrder(Order order) {
			_orders.Add(order);
		}

		public Order GetById(string id) {
			return _orders.SingleOrDefault(x => x.VendorTxCode == id);
		}
	}

	public interface IOrderRepository {
		void StoreOrder(Order order);
		Order GetById(string id);
	}
}
namespace SagePayMvc.Sample.Models {
	using System.Collections.Generic;
	using System.Linq;

	public class ProductRepository : IProductRepository {
		// NOTE: This class simply uses an in-memory list to store the products.
		// In a real application, this repository would communicate with a database
		// ...and probably use an ORM to store and retrieve object instances.

		private static List<Product> _allProducts = new List<Product> {
			new Product { Id = 1, Name = "Ice Cream", Price = (decimal)0.99 },
			new Product{ Id = 2, Name = "iPhone", Price = 599 },
			new Product{ Id = 3, Name = "Shoes", Price = 150 }
		};

		/// <summary>
		/// Gets a product by an ID
		/// </summary>
		public Product FindById(int id) {
			return _allProducts.SingleOrDefault(x => x.Id == id);
		}

		/// <summary>
		/// Gets all products
		/// </summary>
		/// <returns></returns>
		public Product[] GetAllProducts() {
			return _allProducts.ToArray();
		}
	}

	public interface IProductRepository {
		Product FindById(int id);
		Product[] GetAllProducts();
	}
}
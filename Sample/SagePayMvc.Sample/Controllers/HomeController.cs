using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SagePayMvc.Sample.Controllers {
	using SagePayMvc.Sample.Models;

	[HandleError]
	public class HomeController : Controller {
		IProductRepository _productRepository;

		public HomeController(IProductRepository productRepository) {
			_productRepository = productRepository;
		}

		public ActionResult Index() {
			var products = _productRepository.GetAllProducts();
			return View(products);
		}


	}
}

using System.Web.Mvc;
using System.Web.Routing;

namespace SagePayMvc.Sample {
	using SagePayMvc.Sample.Models;
	using StructureMap;

	public class MvcApplication : System.Web.HttpApplication {
		public static void RegisterRoutes(RouteCollection routes) {
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);

		}

		protected void Application_Start() {
			AreaRegistration.RegisterAllAreas();

			RegisterRoutes(RouteTable.Routes);

			// Configure SturctureMap so it knows about the types in this assembly.

			ObjectFactory.Configure(cfg => {
				cfg.For<IProductRepository>().Use<ProductRepository>();
				cfg.For<IShoppingBasket>().Use<ShoppingBasket>();
			});

			ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());
		}
	}
}
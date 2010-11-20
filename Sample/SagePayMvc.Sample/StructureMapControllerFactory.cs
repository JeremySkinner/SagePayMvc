namespace SagePayMvc.Sample {
	using System.Web;
	using System.Web.Mvc;
	using StructureMap;

	// Simple IControllerFactory implementation that uses StructureMap
	public class StructureMapControllerFactory : DefaultControllerFactory {
		protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, System.Type controllerType) {
			if (controllerType == null) return base.GetControllerInstance(requestContext, controllerType);

			return (IController) ObjectFactory.GetInstance(controllerType);
		}
	}
}
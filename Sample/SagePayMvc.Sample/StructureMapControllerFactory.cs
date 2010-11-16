namespace SagePayMvc.Sample {
	using System.Web.Mvc;
	using StructureMap;

	// Simple IControllerFactory implementation that uses StructureMap
	public class StructureMapControllerFactory : DefaultControllerFactory {
		protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, System.Type controllerType) {
			return (IController) ObjectFactory.GetInstance(controllerType);
		}
	}
}
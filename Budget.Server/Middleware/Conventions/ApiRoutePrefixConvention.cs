using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Budget.Server.Middleware.Conventions
{
    public class ApiRoutePrefixConvention : IApplicationModelConvention
    {
        public const string PREFIX = "api";

        private readonly AttributeRouteModel _prefix = new(new RouteAttribute(PREFIX));

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                foreach (var selector in controller.Selectors)
                {
                    if (selector.AttributeRouteModel == null)
                    {
                        selector.AttributeRouteModel = _prefix;
                    }
                    else
                    {
                        selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_prefix, selector.AttributeRouteModel);
                    }
                }
            }
        }
    }
}
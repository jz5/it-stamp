Imports System.Web.Optimization
Imports System.ComponentModel.DataAnnotations

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Protected Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
        DefaultModelBinder.ResourceClassKey = "MyResource"
        DataAnnotationsModelValidatorProvider.RegisterAdapter(GetType(RequiredAttribute), GetType(MyRequiredAttributeAdapter))
        GlobalFilters.Filters.Add(New ValidateInputAttribute(False))
    End Sub
End Class

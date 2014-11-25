Imports System.Web.Optimization
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Entity.Migrations

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

        If Boolean.Parse(ConfigurationManager.AppSettings("MigrateDatabaseToLatestVersion")) Then
            Dim configuration = New Migrations.Configuration
            Dim migrator = New DbMigrator(configuration)
            migrator.Update()
        End If

    End Sub
End Class

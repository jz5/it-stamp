Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports System.Web.Routing

Public Module RouteConfig
    Public Sub RegisterRoutes(ByVal routes As RouteCollection)
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")

        routes.MapRoute(
            name:="Communities",
            url:="Communities/{id}",
            defaults:=New With {.controller = "Communities", .action = "Details"},
            constraints:=New With {.id = "\d+"}
        )
        routes.MapRoute(
            name:="Events",
            url:="Events/{id}",
            defaults:=New With {.controller = "Events", .action = "Details"},
            constraints:=New With {.id = "\d+"}
        )
        routes.MapRoute(
            name:="Stamprally2015",
            url:="Stamprally/2015/{action}/",
            defaults:=New With {.controller = "Stamprally2015", .action = "Index"}
        )
        routes.MapRoute(
            name:="Users",
            url:="Users/{userName}/{action}",
            defaults:=New With {.controller = "Users", .action = "Details"},
            constraints:=New With {.userName = "[A-Za-z0-9_]*"}
        )

        routes.MapRoute(
            name:="Default",
            url:="{controller}/{action}/{id}",
            defaults:=New With {.controller = "Home", .action = "Index", .id = UrlParameter.Optional}
        )



    End Sub
End Module
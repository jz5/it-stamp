Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Entity
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Net
Imports System.Web
Imports System.Web.Mvc
Imports System.Security.Claims
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin.Security
Imports Owin
Imports System.Data.Entity.Core.Objects

<RequireHttps>
Public Class Stamprally2015Controller
    Inherits System.Web.Mvc.Controller


    Private db As New ApplicationDbContext
    Private _userManager As ApplicationUserManager

    Public Sub New()
    End Sub

    Public Sub New(manager As ApplicationUserManager)
        UserManager = manager
    End Sub

    Public Property UserManager() As ApplicationUserManager
        Get
            Return If(_userManager, HttpContext.GetOwinContext().GetUserManager(Of ApplicationUserManager)())
        End Get
        Private Set(value As ApplicationUserManager)
            _userManager = value
        End Set
    End Property

    Function Index() As ActionResult
        Return View()
    End Function

    Function Events() As ActionResult
        Return View()
    End Function

    Function Communities() As ActionResult
        Return View()
    End Function

    Function QA() As ActionResult
        Return View()
    End Function

    Function Sponsors() As ActionResult
        Return View()
    End Function

    Function Join() As ActionResult
        Return View()
    End Function

    Function Committee() As ActionResult
        Return View()
    End Function

    Function Resources() As ActionResult
        Return View()
    End Function

    Function Contact() As ActionResult
        Return View()
    End Function
End Class

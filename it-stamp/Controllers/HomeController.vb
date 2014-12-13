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
Public Class HomeController
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
        Dim viewModel = New SearchEventsViewModel

        Dim now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.ToUniversalTime(), "Tokyo Standard Time")
        Dim n = now.Date
        Dim results = db.Events.Where(Function(e) Not e.IsHidden AndAlso e.EndDateTime >= n).OrderBy(Function(e) e.StartDateTime)
        Dim count = 20
        viewModel.Results = results.Take(count).ToList
        viewModel.CurrentPage = 1
        viewModel.StartPage = 1
        viewModel.TotalPages = (results.Count - 1) \ count + 1
        viewModel.EndPage = If(viewModel.TotalPages < 5, viewModel.TotalPages, 5)

        Return View(viewModel)
    End Function

    Function About() As ActionResult
        Return View()
    End Function

    Function QA() As ActionResult
        Return View()
    End Function

    Function Contact() As ActionResult
        Return View()
    End Function

    Function TOS() As ActionResult
        Return View()
    End Function



End Class

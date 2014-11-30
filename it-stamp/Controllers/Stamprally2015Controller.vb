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

    ' GET: Events
    <AllowAnonymous>
    Function Events(page As Integer?, past As Boolean?) As ActionResult

        Dim results As IQueryable(Of [Event])
        Dim n = Now.Date
        If past.HasValue AndAlso past.Value = True Then
            ' 過去
            results = db.Events.Where(Function(e) Not e.IsHidden AndAlso e.EndDateTime < n AndAlso (e.SpecialEvents.Any(Function(ev) ev.SpecialEvent.Id = 1))).OrderByDescending(Function(e) e.StartDateTime)
        Else
            ' 開催予定
            results = db.Events.Where(Function(e) Not e.IsHidden AndAlso e.EndDateTime >= n AndAlso (e.SpecialEvents.Any(Function(ev) ev.SpecialEvent.Id = 1))).OrderBy(Function(e) e.StartDateTime)
        End If

        Dim viewModel = New SearchEventsViewModel With {
            .TotalCount = results.Count}

        Dim count = 20
        Dim pagenationCount = 5

        ' Total page
        viewModel.TotalPages = (results.Count - 1) \ count + 1

        ' Current page
        If Not page.HasValue OrElse viewModel.TotalPages > page.Value Then
            viewModel.CurrentPage = 1
        Else
            viewModel.CurrentPage = page.Value
        End If

        ' Start page
        viewModel.StartPage = viewModel.CurrentPage - (pagenationCount \ 2)
        If viewModel.StartPage < 1 Then
            viewModel.StartPage = 1
        End If

        ' End page
        viewModel.EndPage = viewModel.StartPage + pagenationCount - 1
        If viewModel.EndPage > viewModel.TotalPages Then
            viewModel.EndPage = viewModel.TotalPages
        End If

        If viewModel.EndPage - viewModel.StartPage + 1 < pagenationCount Then
            viewModel.StartPage = viewModel.EndPage - pagenationCount + 1
        End If
        If viewModel.StartPage < 1 Then
            viewModel.StartPage = 1
        End If

        viewModel.Results = results.Skip((viewModel.CurrentPage - 1) * count).Take(count).ToList

        Return View(viewModel)
    End Function


    ' GET: Communities
    <AllowAnonymous>
    Function Communities(page As Integer?) As ActionResult

        Dim results = db.Events.Where(Function(e) Not e.IsHidden AndAlso e.SpecialEvents.Any(Function(ev) ev.Id = 1) AndAlso e.Community IsNot Nothing AndAlso Not e.Community.IsHidden).Select(Function(e) e.Community).Distinct.OrderBy(Function(c) c.Name)

        Dim viewModel = New SearchCommunitiesViewModel With {
            .TotalCount = results.Count}

        Dim count = 20
        Dim pagenationCount = 5

        ' Total page
        viewModel.TotalPages = (results.Count - 1) \ count + 1

        ' Current page
        If Not page.HasValue OrElse viewModel.TotalPages > page.Value Then
            viewModel.CurrentPage = 1
        Else
            viewModel.CurrentPage = page.Value
        End If

        ' Start page
        viewModel.StartPage = viewModel.CurrentPage - (pagenationCount \ 2)
        If viewModel.StartPage < 1 Then
            viewModel.StartPage = 1
        End If

        ' End page
        viewModel.EndPage = viewModel.StartPage + pagenationCount - 1
        If viewModel.EndPage > viewModel.TotalPages Then
            viewModel.EndPage = viewModel.TotalPages
        End If

        If viewModel.EndPage - viewModel.StartPage + 1 < pagenationCount Then
            viewModel.StartPage = viewModel.EndPage - pagenationCount + 1
        End If
        If viewModel.StartPage < 1 Then
            viewModel.StartPage = 1
        End If


        viewModel.Results = results.Skip((viewModel.CurrentPage - 1) * count).Take(count).ToList

        Return View(viewModel)
    End Function

    Function Schedule() As ActionResult
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

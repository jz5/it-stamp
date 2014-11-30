Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.AspNet.Identity.EntityFramework
Imports System.Collections.Generic
Imports System.Data.Entity
Imports System.Linq
Imports System.Net
Imports System.Threading.Tasks
Imports System.Web
Imports System.Web.Mvc
Imports System
Imports System.Data
Imports ItStamp

<RequireHttps>
<Authorize(Roles:="Admin")>
Public Class UsersAdminController
    Inherits Controller


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

    Public Sub New(userManager As ApplicationUserManager, roleManager As ApplicationRoleManager)
        Me.UserManager = userManager
        Me.RoleManager = roleManager
    End Sub

    Private _roleManager As ApplicationRoleManager
    Public Property RoleManager() As ApplicationRoleManager
        Get
            Return If(_roleManager, HttpContext.GetOwinContext().[Get](Of ApplicationRoleManager)())
        End Get
        Private Set(value As ApplicationRoleManager)
            _roleManager = value
        End Set
    End Property

    '
    ' GET: /Users/
    Public Async Function Index() As Task(Of ActionResult)
        Return View(Await UserManager.Users.ToListAsync())
    End Function

    '
    ' GET: /Users/Details/5
    Public Async Function Details(id As String) As Task(Of ActionResult)
        If id Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If
        Dim user = Await UserManager.FindByIdAsync(id)

        ViewBag.RoleNames = Await UserManager.GetRolesAsync(user.Id)
        ViewBag.Communities = user.Communities
        ViewBag.OwnerCommunities = user.OwnerCommunities

        Return View(user)
    End Function

    '
    ' GET: /Users/Create
    Public Async Function Create() As Task(Of ActionResult)
        'Get the list of Roles
        ViewBag.RoleId = New SelectList(Await RoleManager.Roles.ToListAsync(), "Name", "Name")
        Return View()
    End Function

    '
    ' POST: /Users/Create
    <HttpPost>
    Public Async Function Create(userViewModel As RegisterViewModel, ParamArray selectedRoles As String()) As Task(Of ActionResult)
        If ModelState.IsValid Then
            Dim user = New ApplicationUser With {
                .UserName = userViewModel.UserName,
                .Email = userViewModel.Email
            }
            Dim adminresult = Await UserManager.CreateAsync(user, userViewModel.Password)

            'Add User to the selected Roles 
            If adminresult.Succeeded Then
                If selectedRoles IsNot Nothing Then
                    Dim result = Await UserManager.AddToRolesAsync(user.Id, selectedRoles)
                    If Not result.Succeeded Then
                        ModelState.AddModelError("", result.Errors.First())
                        ViewBag.RoleId = New SelectList(Await RoleManager.Roles.ToListAsync(), "Name", "Name")
                        Return View()
                    End If
                End If
            Else
                ModelState.AddModelError("", adminresult.Errors.First())
                ViewBag.RoleId = New SelectList(RoleManager.Roles, "Name", "Name")

                Return View()
            End If
            Return RedirectToAction("Index")
        End If
        ViewBag.RoleId = New SelectList(RoleManager.Roles, "Name", "Name")
        Return View()
    End Function

    '
    ' GET: /Users/Edit/1
    Public Async Function Edit(id As String) As Task(Of ActionResult)
        If id Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If
        Dim user = Await UserManager.FindByIdAsync(id)
        If user Is Nothing Then
            Return HttpNotFound()
        End If

        Dim userRoles = Await UserManager.GetRolesAsync(user.Id)

        Return View(New EditUserViewModel With {
             .Id = user.Id,
             .Email = user.Email,
             .RolesList = RoleManager.Roles.ToList.Select(Function(x) New SelectListItem With {
                 .Selected = userRoles.Contains(x.Name),
                 .Text = x.Name,
                 .Value = x.Name}),
             .OwnerCommunitiesList = New SelectList(user.OwnerCommunities, "Id", "Name"),
             .CommunitiesList = New SelectList(user.Communities, "Id", "Name"),
             .CommunitiesSelectList = New SelectList(db.Communities, "Id", "Name")
         })
    End Function

    '
    ' POST: /Users/Edit/5
    <HttpPost>
    <ValidateAntiForgeryToken>
    Public Async Function Edit(<Bind(Include:="Email,Id,CommunityId,OwnerCommunityId")> viewModel As EditUserViewModel, selectedRole As String(), selectedCommunities As Integer(), selectedOwnerCommunities As Integer()) As Task(Of ActionResult)
        If Not ModelState.IsValid Then
            Return View()
        End If

        Dim user = Await UserManager.FindByIdAsync(viewModel.Id)
        If user Is Nothing Then
            Return HttpNotFound()
        End If

        user.UserName = viewModel.Email
        user.Email = viewModel.Email

        ' Role
        Dim userRoles = Await UserManager.GetRolesAsync(user.Id)

        selectedRole = If(selectedRole, New String() {})
        If selectedRole.Any Then
            Dim idResult As IdentityResult
            idResult = Await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray)
            If Not idResult.Succeeded Then
                ModelState.AddModelError("", idResult.Errors.First())
                Return View()
            End If

            idResult = Await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray)
            If Not idResult.Succeeded Then
                ModelState.AddModelError("", idResult.Errors.First())
                Return View()
            End If
        End If

        ' Community
        Dim dbUser = db.Users.Where(Function(u) u.Id = user.Id).FirstOrDefault
        If selectedCommunities IsNot Nothing Then

            For Each com In dbUser.Communities.ToList
                If Not selectedCommunities.Contains(com.Id) Then
                    dbUser.Communities.Remove(db.Communities.Where(Function(c) c.Id = com.Id).FirstOrDefault)
                End If
            Next

            db.SaveChanges()
        End If

        If viewModel.CommunityId.HasValue Then
            dbUser.Communities.Add(db.Communities.Where(Function(c) c.Id = viewModel.CommunityId.Value).FirstOrDefault)
            db.SaveChanges()
        End If

        ' Owner Community
        If selectedOwnerCommunities IsNot Nothing Then

            For Each com In dbUser.OwnerCommunities.ToList
                If Not selectedOwnerCommunities.Contains(com.Id) Then
                    dbUser.OwnerCommunities.Remove(db.Communities.Where(Function(c) c.Id = com.Id).FirstOrDefault)
                End If
            Next

            db.SaveChanges()
        End If

        If viewModel.OwnerCommunityId.HasValue Then
            dbUser.OwnerCommunities.Add(db.Communities.Where(Function(c) c.Id = viewModel.OwnerCommunityId.Value).FirstOrDefault)
            db.SaveChanges()
        End If

        Return RedirectToAction("Details", "UsersAdmin", New With {.id = user.Id})

    End Function

    '
    ' GET: /Users/Delete/5
    Public Async Function Delete(id As String) As Task(Of ActionResult)
        If id Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If
        Dim user = Await UserManager.FindByIdAsync(id)
        If user Is Nothing Then
            Return HttpNotFound()
        End If
        Return View(user)
    End Function

    '
    ' POST: /Users/Delete/5
    <HttpPost, ActionName("Delete")>
    <ValidateAntiForgeryToken>
    Public Async Function DeleteConfirmed(id As String) As Task(Of ActionResult)
        If ModelState.IsValid Then
            If id Is Nothing Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If

            Dim user = Await UserManager.FindByIdAsync(id)
            If user Is Nothing Then
                Return HttpNotFound()
            End If
            Dim result = Await UserManager.DeleteAsync(user)
            If Not result.Succeeded Then
                ModelState.AddModelError("", result.Errors.First())
                Return View()
            End If
            Return RedirectToAction("Index")
        End If
        Return View()
    End Function
End Class

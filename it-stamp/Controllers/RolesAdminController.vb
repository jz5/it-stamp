Imports System.Security.Claims
Imports System.Threading.Tasks
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin.Security
Imports Owin
Imports System.Net

<RequireHttps>
<Authorize(Roles:="Admin")>
Public Class RolesAdminController
    Inherits Controller

    Public Sub New()
    End Sub

    Public Sub New(userManager As ApplicationUserManager, roleManager As ApplicationRoleManager)
        Me.UserManager = userManager
        Me.RoleManager = roleManager
    End Sub

    Private _userManager As ApplicationUserManager
    Public Property UserManager() As ApplicationUserManager
        Get
            Return If(_userManager, HttpContext.GetOwinContext().GetUserManager(Of ApplicationUserManager))
        End Get
        Set(value As ApplicationUserManager)
            _userManager = value
        End Set
    End Property

    Private _roleManager As ApplicationRoleManager
    Public Property RoleManager() As ApplicationRoleManager
        Get
            Return If(_roleManager, HttpContext.GetOwinContext().Get(Of ApplicationRoleManager))
        End Get
        Private Set(value As ApplicationRoleManager)
            _roleManager = value
        End Set
    End Property

    '
    ' GET: /Roles/
    Public Function Index() As ActionResult
        Return View(RoleManager.Roles)
    End Function

    '
    ' GET: /Roles/Details/5
    Public Async Function Details(id As String) As Task(Of ActionResult)
        If id Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If
        Dim role = Await RoleManager.FindByIdAsync(id)
        ' Get the list of Users in this Role
        Dim users = New List(Of ApplicationUser)

        ' Get the list of Users in this Role
        For Each usr In UserManager.Users.ToList
            If Await UserManager.IsInRoleAsync(usr.Id, role.Name) Then
                users.Add(usr)
            End If
        Next

        ViewBag.Users = users
        ViewBag.UserCount = users.Count
        Return View(role)
    End Function

    '
    ' GET: /Roles/Create
    Public Function Create() As ActionResult
        Return View()
    End Function

    '
    ' POST: /Roles/Create
    <HttpPost> _
    Public Async Function Create(roleViewModel As RoleViewModel) As Task(Of ActionResult)
        If ModelState.IsValid Then
            Dim role = New IdentityRole(roleViewModel.Name)
            Dim roleresult = Await RoleManager.CreateAsync(role)
            If Not roleresult.Succeeded Then
                ModelState.AddModelError("", roleresult.Errors.First())
                Return View()
            End If
            Return RedirectToAction("Index")
        End If
        Return View()
    End Function

    '
    ' GET: /Roles/Edit/Admin
    Public Async Function Edit(id As String) As Task(Of ActionResult)
        If id Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If
        Dim role = Await RoleManager.FindByIdAsync(id)
        If role Is Nothing Then
            Return HttpNotFound()
        End If
        Dim roleModel As New RoleViewModel With {
            .Id = role.Id,
            .Name = role.Name
        }
        Return View(roleModel)
    End Function

    '
    ' POST: /Roles/Edit/5

    <HttpPost> _
    <ValidateAntiForgeryToken> _
    Public Async Function Edit(<Bind(Include:="Name,Id")> roleModel As RoleViewModel) As Task(Of ActionResult)
        If ModelState.IsValid Then
            Dim role = Await RoleManager.FindByIdAsync(roleModel.Id)
            role.Name = roleModel.Name
            Await RoleManager.UpdateAsync(role)
            Return RedirectToAction("Index")
        End If
        Return View()
    End Function

    '
    ' GET: /Roles/Delete/5
    Public Async Function Delete(id As String) As Task(Of ActionResult)
        If id Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If
        Dim role = Await RoleManager.FindByIdAsync(id)
        If role Is Nothing Then
            Return HttpNotFound()
        End If
        Return View(role)
    End Function

    '
    ' POST: /Roles/Delete/5
    <HttpPost, ActionName("Delete")>
    <ValidateAntiForgeryToken>
    Public Async Function DeleteConfirmed(id As String, deleteUser As String) As Task(Of ActionResult)
        If ModelState.IsValid Then
            If id Is Nothing Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim role = Await RoleManager.FindByIdAsync(id)
            If role Is Nothing Then
                Return HttpNotFound()
            End If
            Dim result As IdentityResult
            If deleteUser IsNot Nothing Then
                result = Await RoleManager.DeleteAsync(role)
            Else
                result = Await RoleManager.DeleteAsync(role)
            End If
            If Not result.Succeeded Then
                ModelState.AddModelError("", result.Errors.First())
                Return View()
            End If
            Return RedirectToAction("Index")
        End If
        Return View()
    End Function

End Class

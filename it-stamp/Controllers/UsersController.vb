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
Imports System.Drawing

<Authorize>
<RequireHttps>
Public Class UsersController
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


    <AllowAnonymous>
    Function Index() As ActionResult
        Return RedirectToAction("Index", "Home")
    End Function

    <AllowAnonymous>
    Function Details(userName As String) As ActionResult
        If userName = "" Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim user = db.Users.Where(Function(u) u.UserName = userName).SingleOrDefault
        If user Is Nothing Then
            Return HttpNotFound()
        End If

        Return View(user)
    End Function

    ' GET: Users/UserName/Edit
    Async Function Edit(message As Message?) As Task(Of ActionResult)

        Dim id = User.Identity.GetUserId
        Dim appUser = Await db.Users.Where(Function(u) u.Id = id).SingleOrDefaultAsync
        If appUser Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        ' Message
        Dim msg As String
        Select Case message
            Case UsersController.Message.Edit
                msg = "保存しました。"
            Case Else
                msg = ""
        End Select
        ViewBag.StatusMessage = msg

        Return View(appUser)
    End Function


    ' POST: Users/UserName/Edit
    <HttpPost>
    <ValidateAntiForgeryToken>
    Async Function Edit(model As ApplicationUser) As Task(Of ActionResult)
        If Not ModelState.IsValid Then
            Return View(model)
        End If

        Try
            Dim id = User.Identity.GetUserId
            Dim appUser = Await db.Users.Where(Function(u) u.Id = id).SingleOrDefaultAsync
            If appUser Is Nothing Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If


            UpdateModel(Of ApplicationUser)(appUser)
            Await db.SaveChangesAsync()

            Session("DisplayName") = model.DisplayName

            Return RedirectToAction("Edit", New With {.message = Message.Edit})

        Catch ex As Exception
            ModelState.AddInternalError(User, ex)
            Return View(model)
        End Try
    End Function

    ' GET: Users/UserName/Upload
    Async Function Upload(ByVal userName As String) As Task(Of ActionResult)

        Dim id = User.Identity.GetUserId
        Dim appUser = Await db.Users.Where(Function(u) u.Id = id).SingleOrDefaultAsync
        If appUser Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim viewModel = New UploadUserIconViewModel With {.IconPath = appUser.IconPath}

        Return View(viewModel)
    End Function

    <HttpPost>
    <ValidateAntiForgeryToken>
    Async Function Upload(viewModel As UploadUserIconViewModel) As Task(Of ActionResult)
        If Not ModelState.IsValid Then
            Return View(viewModel)
        End If

        Dim id = User.Identity.GetUserId
        Dim appUser = Await db.Users.Where(Function(u) u.Id = id).SingleOrDefaultAsync
        If appUser Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim helper = New UploadHelper(viewModel.File, Server.MapPath("~/App_Data/Uploads/"))
        Dim iconPath = helper.GetIconPath("Users", appUser.UserName)

        If Not helper.IsSupportedImageFormat Then
            ModelState.AddModelError("File", "PNG/JPEG形式の画像をアップロードしてください。")
            Return View(viewModel)
        End If

        Try
            ' Resize
            Dim icon As Bitmap
            Using bmp = New Bitmap(viewModel.File.InputStream)
                icon = bmp.ResizeTo(New Size(96, 96))
            End Using

            ' Delete, Save
            helper.RelpaceFile(appUser.IconPath, iconPath, icon)

            ' Update
            appUser.IconPath = iconPath
            Await db.SaveChangesAsync()

            Session("IconPath") = iconPath

            Return RedirectToAction("Edit", New With {.userName = appUser.UserName, .message = Message.Edit})

        Catch ex As Exception
            ModelState.AddInternalError(User, ex)
            Return View(viewModel)
        End Try
    End Function

    Public Enum Message
        Edit
    End Enum

End Class

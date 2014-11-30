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

        Dim userId = User.Identity.GetUserId

        Dim appUser = db.Users.Where(Function(u) u.UserName = userName).SingleOrDefault
        If appUser Is Nothing Then
            Return HttpNotFound()
        ElseIf appUser.IsRemoved OrElse appUser.IsPrivate Then ' Deitals は自分のページも見れない
            Return View("PrivateOrRemovedUser", appUser)
        End If

        ' フォロー済みか
        Dim followed = False
        If userId <> appUser.Id Then
            followed = db.Followers.Any(Function(f) f.CreatedBy.Id = userId AndAlso f.User.Id = appUser.Id)
        End If
        ViewBag.Followed = followed

        Return View(appUser)
    End Function

    ' GET: Users/UserName/My
    Async Function My(userName As String) As Task(Of ActionResult)

        Dim id = User.Identity.GetUserId
        Dim appUser = Await db.Users.Where(Function(u) u.Id = id).SingleOrDefaultAsync
        If appUser Is Nothing OrElse appUser.UserName <> userName Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim n = Now.Date
        Dim es = db.Events.Where(Function(e) e.EndDateTime >= n AndAlso e.Community IsNot Nothing AndAlso appUser.Communities.Any(Function(c) c.Id = e.Community.Id)).OrderBy(Function(e) e.StartDateTime)


        Return View(appUser)
    End Function

    ' GET: Users/UserName/MyFollowing
    Async Function MyFollowing(userName As String) As Task(Of ActionResult)

        Dim id = User.Identity.GetUserId
        Dim appUser = Await db.Users.Where(Function(u) u.Id = id).SingleOrDefaultAsync
        If appUser Is Nothing OrElse appUser.UserName <> userName Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Return View(appUser)
    End Function

    ' GET: Users/UserName/Manage
    Async Function Manage(userName As String) As Task(Of ActionResult)

        Dim id = User.Identity.GetUserId
        Dim appUser = Await db.Users.Where(Function(u) u.Id = id).SingleOrDefaultAsync
        If appUser Is Nothing OrElse appUser.UserName <> userName Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim coms = db.Communities.Where(Function(c) c.CreatedBy.Id = appUser.Id).OrderBy(Function(c) c.Name)
        Dim events = db.Events.Where(Function(e) e.CreatedBy.Id = appUser.Id).OrderByDescending(Function(e) e.StartDateTime)

        ViewBag.Communities = coms.ToList
        ViewBag.Events = events.ToList

        Return View(appUser)
    End Function

    <AllowAnonymous>
    Function CheckIns(userName As String) As ActionResult
        If userName = "" Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim userId = User.Identity.GetUserId

        Dim appUser = db.Users.Where(Function(u) u.UserName = userName).SingleOrDefault
        If appUser Is Nothing Then
            Return HttpNotFound()
        ElseIf appUser.Id <> userId AndAlso (appUser.IsRemoved OrElse appUser.IsPrivate) Then
            Return View("PrivateOrRemovedUser", appUser)
        End If

        ' フォロー済みか
        Dim followed = False
        If userId <> appUser.Id Then
            followed = db.Followers.Any(Function(f) f.CreatedBy.Id = userId AndAlso f.User.Id = appUser.Id)
        End If
        ViewBag.Followed = followed

        Return View(appUser)
    End Function

    ' GET: Users/UserName/Edit
    Async Function Edit(userName As String, message As Message?) As Task(Of ActionResult)

        Dim id = User.Identity.GetUserId
        Dim appUser = Await db.Users.Where(Function(u) u.Id = id).SingleOrDefaultAsync
        If appUser Is Nothing OrElse appUser.UserName <> userName Then
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

            Session("DisplayName") = appUser.DisplayName

            Return RedirectToAction("Edit", New With {.message = Message.Edit})

        Catch ex As Exception
            ModelState.AddInternalError(User, ex)
            Return View(model)
        End Try
    End Function

    ' GET: Users/UserName/Upload
    Async Function Upload(userName As String) As Task(Of ActionResult)

        Dim id = User.Identity.GetUserId
        Dim appUser = Await db.Users.Where(Function(u) u.Id = id).SingleOrDefaultAsync
        If appUser Is Nothing OrElse appUser.UserName <> userName Then
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

    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Async Function Follow(userName As String) As Task(Of ActionResult)

        Dim userId = User.Identity.GetUserId
        If userId Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        ' ログインユーザー
        Dim appUser = Await db.Users.Where(Function(u) u.Id = userId).SingleOrDefaultAsync
        If appUser Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        ' フォロー対象のユーザー
        Dim targetUser = db.Users.Where(Function(u) u.UserName = userName).SingleOrDefault
        If targetUser Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        If userId = targetUser.Id Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Try
            ' フォロー済みか
            Dim follower = db.Followers.Where(Function(f) f.CreatedBy.Id = userId AndAlso f.User.Id = targetUser.Id).SingleOrDefault
            Dim followed = follower IsNot Nothing

            ' Switch following
            If followed Then
                db.Followers.Remove(follower)
            Else
                Dim fl = New Follower With {
                    .CreatedBy = appUser,
                    .User = targetUser,
                    .CreationDateTime = Now}
                db.Followers.Add(fl)
            End If

            Await db.SaveChangesAsync

            ' for Ajax
            If Request.IsAjaxRequest Then
                Return Json(New With {.followed = Not followed})
            End If

            Return RedirectToAction("Details", "Users", New With {.userName = appUser.UserName})

        Catch eEx As System.Data.Entity.Validation.DbEntityValidationException
            For Each er In eEx.EntityValidationErrors
                For Each e In er.ValidationErrors
                    Debug.Print(e.ErrorMessage)
                Next
            Next
        Catch ex As Exception
            ModelState.AddInternalError(User, ex)
        End Try

        If Request.IsAjaxRequest Then
            Return Json(Nothing)
        Else
            Return View()
        End If

    End Function


    Public Enum Message
        Edit
    End Enum

End Class

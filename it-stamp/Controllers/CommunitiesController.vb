﻿Imports System
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
Imports System.IO
Imports System.Drawing

<Authorize>
<RequireHttps>
Public Class CommunitiesController
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


    ' GET: Communities
    <AllowAnonymous>
    Function Index(page As Integer?) As ActionResult

        Dim results = db.Communities.Where(Function(e) Not e.IsHidden).OrderBy(Function(e) e.Id)

        Dim viewModel = New SearchCommunitiesViewModel With {
            .TotalCount = results.Count
            }

        Dim count = 10
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
        viewModel.StartPage = viewModel.CurrentPage - pagenationCount
        If viewModel.StartPage < 1 Then
            viewModel.StartPage = 1
        End If

        ' End page
        viewModel.EndPage = viewModel.StartPage + pagenationCount - 1
        If viewModel.EndPage > viewModel.TotalPages Then
            viewModel.EndPage = viewModel.TotalPages
        End If

        viewModel.Results = results.Skip((viewModel.CurrentPage - 1) * count).Take(count).ToList

        Return View(viewModel)
    End Function

    ' GET: Communities/5
    <AllowAnonymous>
    Async Function Details(ByVal id As Integer?, message As DetailsMessage?) As Task(Of ActionResult)
        If IsNothing(id) Then
            Return RedirectToAction("Index")
        End If

        Dim com = Await db.Communities.FindAsync(id)
        If IsNothing(com) Then
            Return HttpNotFound()
        End If

        ' コミュニティの編集権限があるかどうか
        Dim appUser = Await UserManager.FindByIdAsync(User.Identity.GetUserId)
        ViewBag.CanEdit = CanEdit(appUser, com)

        ' フォロー済みか
        If appUser IsNot Nothing Then
            Dim followed = appUser.Communities.Where(Function(c) c.Id = com.Id).Count > 0
            ViewBag.Followd = followed
        Else
            ViewBag.Followd = False
        End If

        ' Message
        Dim msg As String
        Select Case message
            Case DetailsMessage.Add
                msg = "登録しました。"
            Case DetailsMessage.Edit
                msg = "編集しました。"
            Case Else
                msg = ""
        End Select
        ViewBag.StatusMessage = msg

        Return View(com)
    End Function

    ' Get: Communities/Add
    Function Add() As ActionResult
        Return View()
    End Function

    ' POST: Communities/Add
    <HttpPost>
    <ValidateAntiForgeryToken>
    Async Function Add(model As Community) As Task(Of ActionResult)
        If Not ModelState.IsValid Then
            Return View(model)
        End If

        Try
            If db.Communities.Where(Function(c) c.Name = model.Name).FirstOrDefault IsNot Nothing Then
                ModelState.AddModelError("Name", "既に登録されている名前です。")
                Return View(model)
            End If

            If db.Communities.Where(Function(c) c.Url = model.Url).FirstOrDefault IsNot Nothing Then
                ModelState.AddModelError("Url", "既に登録されているURLです。")
                Return View(model)
            End If

            ' 編集権限の確認
            Dim appUser = Await db.Users.Where(Function(u) u.Id = User.Identity.GetUserId).SingleOrDefaultAsync
            If appUser Is Nothing Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If

            Dim time = Now
            model.CreatedBy = appUser
            model.CreationDateTime = time
            model.LastUpdatedBy = appUser
            model.LastUpdatedDateTime = time

            Dim com = db.Communities.Add(model)
            Await db.SaveChangesAsync

            Return RedirectToAction("Details", New With {.id = com.Id, .message = DetailsMessage.Add})

        Catch ex As Exception
            ModelState.AddInternalError(User, ex)
            Return View(model)
        End Try
    End Function

    ' GET: Communities/Edit/5
    Async Function Edit(ByVal id As Integer?) As Task(Of ActionResult)
        If IsNothing(id) Then
            Return RedirectToAction("Index")
        End If

        Dim com = Await db.Communities.FindAsync(id)
        If IsNothing(com) Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        ' 編集権限の確認
        Dim appUser = Await UserManager.FindByIdAsync(User.Identity.GetUserId)
        If Not CanEdit(appUser, com) Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Return View(com)
    End Function


    ' POST: Communities/Edit
    <HttpPost>
    <ValidateAntiForgeryToken>
    Async Function Edit(model As Community) As Task(Of ActionResult)
        If Not ModelState.IsValid Then
            Return View(model)
        End If

        Try
            Dim com = db.Communities.Where(Function(c) c.Id = model.Id).FirstOrDefault
            If com Is Nothing Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If

            If db.Communities.Where(Function(c) c.Id <> model.Id AndAlso c.Name = model.Name).FirstOrDefault IsNot Nothing Then
                ModelState.AddModelError("Name", "既に登録されている名前です。")
                Return View(model)
            End If

            If db.Communities.Where(Function(c) c.Id <> model.Id AndAlso c.Url = model.Url).FirstOrDefault IsNot Nothing Then
                ModelState.AddModelError("Url", "既に登録されているURLです。")
                Return View(model)
            End If

            ' 編集権限の確認
            Dim id = User.Identity.GetUserId
            Dim appUser = Await db.Users.Where(Function(u) u.Id = id).SingleOrDefaultAsync
            If Not CanEdit(appUser, com) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If

            UpdateModel(Of Community)(com)
            com.LastUpdatedBy = appUser
            com.LastUpdatedDateTime = Now

            Await db.SaveChangesAsync()

            Return RedirectToAction("Details", New With {.id = com.Id, .message = DetailsMessage.Edit})

        Catch ex As Exception
            ModelState.AddInternalError(User, ex)
            Return View(model)
        End Try
    End Function

    ' GET: Communities/Upload/5
    Async Function Upload(ByVal id As Integer?) As Task(Of ActionResult)
        If IsNothing(id) Then
            Return RedirectToAction("Index")
        End If

        Dim com = Await db.Communities.FindAsync(id)
        If IsNothing(com) Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        ' 編集権限の確認
        Dim userId = User.Identity.GetUserId
        Dim appUser = Await db.Users.Where(Function(u) u.Id = userId).SingleOrDefaultAsync
        If Not CanEdit(appUser, com) Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim viewModel = New UploadCommunityIconViewModel With {
            .Id = com.Id,
            .Name = com.Name,
            .IconPath = com.IconPath
            }

        Return View(viewModel)
    End Function

    <HttpPost>
    <ValidateAntiForgeryToken>
    Async Function Upload(viewModel As UploadCommunityIconViewModel) As Task(Of ActionResult)
        If Not ModelState.IsValid Then
            Return View(viewModel)
        End If

        Dim com = db.Communities.Where(Function(c) c.Id = viewModel.Id).FirstOrDefault
        If com Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        ' 編集権限の確認
        Dim id = User.Identity.GetUserId
        Dim appUser = Await db.Users.Where(Function(u) u.Id = id).SingleOrDefaultAsync
        If Not CanEdit(appUser, com) Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim helper = New UploadHelper(viewModel.File, Server.MapPath("~/App_Data/Uploads/"))
        Dim iconPath = helper.GetIconPath("Communities", viewModel.Id.ToString)

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
            helper.RelpaceFile(com.IconPath, iconPath, icon)

            ' Update
            com.IconPath = iconPath
            com.LastUpdatedBy = appUser
            com.LastUpdatedDateTime = Now

            Await db.SaveChangesAsync

            Return RedirectToAction("Details", New With {.id = com.Id, .message = DetailsMessage.Edit})

        Catch ex As Exception
            ModelState.AddInternalError(User, ex)
            Return View(viewModel)
        End Try
    End Function

    Async Function Follow(id As Integer?) As Task(Of ActionResult)
        If Not id.HasValue Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim userId = User.Identity.GetUserId
        Dim appUser = Await db.Users.Where(Function(u) u.Id = userId).SingleOrDefaultAsync
        If appUser Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim com = db.Communities.Where(Function(c) c.Id = id.Value).SingleOrDefault
        If com Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        ' フォロー済みか
        Dim followed = appUser.Communities.Where(Function(c) c.Id = com.Id).Count > 0
        ViewBag.Followd = followed

        If com.IsHidden Then
            ViewBag.StatusMessage = "このコミュニティはフォローできません。"
            Return View(com)
        End If

        Return View(com)
    End Function

    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Async Function Follow(model As Community) As Task(Of ActionResult)
        Dim userId = User.Identity.GetUserId
        Dim appUser = Await db.Users.Where(Function(u) u.Id = userId).SingleOrDefaultAsync
        If appUser Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim id = model.Id
        Dim com = db.Communities.Where(Function(c) c.Id = id).SingleOrDefault
        If com Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Try
            ' フォロー済みか
            Dim followed = appUser.Communities.Where(Function(c) c.Id = com.Id).Count > 0
            ViewBag.Followd = followed

            If Not ModelState.IsValid Then
                Return View(model)
            End If

            If com.IsHidden Then
                ViewBag.ErrorMessage = "このコミュニティはフォローできません。"
                Return View(model)
            End If

            ' Switch following
            If followed Then
                appUser.Communities.Remove(com)
            Else
                appUser.Communities.Add(com)
            End If

            Await db.SaveChangesAsync

            Return RedirectToAction("Details", "Communities", New With {.id = com.Id})

        Catch eEx As System.Data.Entity.Validation.DbEntityValidationException
            For Each er In eEx.EntityValidationErrors
                For Each e In er.ValidationErrors
                    Debug.Print(e.ErrorMessage)
                Next
            Next
            Return View(model)
        Catch ex As Exception
            ModelState.AddInternalError(User, ex)
            Return View(model)
        End Try

    End Function


    Async Function Delete(id As Integer?) As Task(Of ActionResult)
        If Not id.HasValue Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim com = db.Communities.Where(Function(c) c.Id = id.Value).SingleOrDefault
        If com Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim userId = User.Identity.GetUserId
        Dim appUser = Await db.Users.Where(Function(u) u.Id = userId).SingleOrDefaultAsync
        If appUser Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        ViewBag.CanDelete = CanDelete(appUser, com)

        Return View(com)
    End Function

    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Async Function Delete(model As Community) As Task(Of ActionResult)

        Dim userId = User.Identity.GetUserId
        Dim appUser = Await db.Users.Where(Function(u) u.Id = userId).SingleOrDefaultAsync

        Dim id = model.Id
        Dim com = db.Communities.Where(Function(c) c.Id = id).SingleOrDefault

        If Not CanDelete(appUser, com) Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Try
            If Not ModelState.IsValid Then
                Return View(model)
            End If

            db.Communities.Remove(com)
            Await db.SaveChangesAsync

            Return RedirectToAction("Index", "Communities")

        Catch eEx As System.Data.Entity.Validation.DbEntityValidationException
            For Each er In eEx.EntityValidationErrors
                For Each e In er.ValidationErrors
                    Debug.Print(e.ErrorMessage)
                Next
            Next
            Return View(model)
        Catch ex As Exception
            ModelState.AddInternalError(User, ex)
            Return View(model)
        End Try

    End Function

    Private Function CanEdit(appUser As ApplicationUser, community As Community) As Boolean
        If appUser Is Nothing OrElse community Is Nothing Then
            Return False
        ElseIf Not community.IsLocked Then
            ' 一般ユーザー
            Return True
        ElseIf community.Owners.Contains(appUser) Then
            ' コミュニティオーナー
            Return True
        ElseIf User.IsInRole("Admin") Then
            Return True
        End If
        Return False
    End Function

    Private Function CanEditDetails(appUser As ApplicationUser, community As Community) As Boolean
        If appUser Is Nothing OrElse community Is Nothing Then
            Return False
        ElseIf community.Owners.Contains(appUser) Then
            ' コミュニティオーナー
            Return True
        ElseIf User.IsInRole("Admin") Then
            Return True
        End If
        Return False
    End Function

    Private Function CanDelete(appUser As ApplicationUser, community As Community) As Boolean
        If appUser Is Nothing OrElse community Is Nothing Then
            Return False
        ElseIf community.Members.Count > 0 OrElse community.Owners.Count > 0 OrElse db.Events.Where(Function(e) e.Community IsNot Nothing AndAlso e.Community.Id = community.Id).Count > 0 Then
            Return False
        Else
            Return True
        End If
        Return False
    End Function

    Public Enum DetailsMessage
        Add
        Edit
    End Enum

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing) Then
            db.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
End Class

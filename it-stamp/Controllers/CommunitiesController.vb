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
Imports System.IO
Imports System.Drawing

<Authorize>
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
    Async Function Details(ByVal id As Integer?, message As DetailsMessageId?) As Task(Of ActionResult)
        If IsNothing(id) Then
            Return RedirectToAction("Index")
        End If

        Dim community = Await db.Communities.FindAsync(id)
        If IsNothing(community) Then
            Return HttpNotFound()
        End If

        ' コミュニティの編集権限があるかどうか
        Dim appUser = Await UserManager.FindByIdAsync(User.Identity.GetUserId)
        ViewBag.CanEdit = CanEdit(appUser, community)

        ' Message
        Dim msg As String
        Select Case message
            Case DetailsMessageId.Add
                msg = "登録しました。"
            Case DetailsMessageId.Edit
                msg = "編集しました。"
            Case Else
                msg = ""
        End Select
        ViewBag.StatusMessage = msg

        Return View(community)
    End Function

    ' Get: Communities/Add
    ' TODO delete AllowAnonymous
    <AllowAnonymous>
    Function Add() As ActionResult
        Return View()
    End Function

    ' POST: Communities/Add
    ' TODO delete AllowAnonymous
    <AllowAnonymous>
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

            Dim appUser = Await UserManager.FindByIdAsync(User.Identity.GetUserId)
            If appUser Is Nothing Then
                ' TODO Return View()
            End If

            Dim time = Now
            model.CreatedBy = appUser
            model.CreationDateTime = time
            model.LastUpdatedBy = appUser
            model.LastUpdatedDateTime = time

            Dim com = db.Communities.Add(model)
            Await db.SaveChangesAsync

            Return RedirectToAction("Details", New With {.id = com.Id, .message = DetailsMessageId.Add})

        Catch ex As Exception
            ModelState.AddInternalError(User, ex)
            Return View(model)
        End Try
    End Function

    ' GET: Communities/Edit/5
    ' TODO delete AllowAnonymous
    <AllowAnonymous>
    Async Function Edit(ByVal id As Integer?) As Task(Of ActionResult)
        If IsNothing(id) Then
            Return RedirectToAction("Index")
        End If

        Dim com = Await db.Communities.FindAsync(id)
        If IsNothing(com) Then
            Return HttpNotFound()
        End If

        ' 編集権限の確認
        Dim appUser = Await UserManager.FindByIdAsync(User.Identity.GetUserId)
        If Not CanEdit(appUser, com) Then
            'Return View("Details", New With {.id = id})
        End If

        'Dim viewModel = New CommunityViewModel With {
        '    .Id = com.Id,
        '    .Name = com.Name,
        '    .Description = com.Description,
        '    .Url = com.Url,
        '    .IconPath = com.IconPath
        '    }

        Return View(com)
    End Function


    ' POST: Communities/Edit
    ' TODO delete AllowAnonymous
    <AllowAnonymous>
    <HttpPost>
    <ValidateAntiForgeryToken>
    Async Function Edit(model As Community) As Task(Of ActionResult)
        If Not ModelState.IsValid Then
            Return View(model)
        End If

        Try
            Dim com = db.Communities.Where(Function(c) c.Id = model.Id).FirstOrDefault
            If com Is Nothing Then
                Return View("Index")
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
            Dim appUser = Await UserManager.FindByIdAsync(User.Identity.GetUserId)
            If Not CanEdit(appUser, com) Then
                'TODO Return View("Details", New With {.id = id})
            End If


            UpdateModel(Of Community)(com)

            com.LastUpdatedBy = appUser
            com.LastUpdatedDateTime = Now

            Await db.SaveChangesAsync()

            Return RedirectToAction("Details", New With {.id = com.Id, .message = DetailsMessageId.Edit})

        Catch ex As Exception
            ModelState.AddInternalError(User, ex)
            Return View(model)
        End Try
    End Function

    ' GET: Communities/Upload/5
    ' TODO delete AllowAnonymous
    <AllowAnonymous>
    Async Function Upload(ByVal id As Integer?) As Task(Of ActionResult)
        If IsNothing(id) Then
            Return RedirectToAction("Index")
        End If

        Dim com = Await db.Communities.FindAsync(id)
        If IsNothing(com) Then
            Return HttpNotFound()
        End If

        ' 編集権限の確認
        Dim appUser = Await UserManager.FindByIdAsync(User.Identity.GetUserId)
        If Not CanEdit(appUser, com) Then
            'Return View("Details", New With {.id = id})
        End If

        Dim viewModel = New UploadCommunityIconViewModel With {
            .Id = com.Id,
            .Name = com.Name,
            .IconPath = com.IconPath
            }

        Return View(viewModel)
    End Function

    ' TODO delete AllowAnonymous
    <AllowAnonymous>
    <HttpPost>
    <ValidateAntiForgeryToken>
    Async Function Upload(viewModel As UploadCommunityIconViewModel) As Task(Of ActionResult)
        If Not ModelState.IsValid Then
            Return View(viewModel)
        End If

        Dim com = db.Communities.Where(Function(c) c.Id = viewModel.Id).FirstOrDefault
        If com Is Nothing Then
            Return View("Index")
        End If

        ' コミュニティの編集権限があるかどうか
        Dim appUser = Await UserManager.FindByIdAsync(User.Identity.GetUserId)
        If Not CanEdit(appUser, com) Then
            'TODO Return View("Details", New With {.id = id})
        End If

        ' Content-Type
        Dim contentType = viewModel.File.ContentType
        Dim format As System.Drawing.Imaging.ImageFormat = Nothing

        If contentType.Contains("jpeg") Then
            format = System.Drawing.Imaging.ImageFormat.Jpeg
        ElseIf contentType.Contains("png") Then
            format = System.Drawing.Imaging.ImageFormat.Png
        End If

        If Not contentType.StartsWith("image/") OrElse format Is Nothing Then
            ModelState.AddModelError("File", "PNG/JPEG形式の画像をアップロードしてください。")
            Return View(viewModel)
        End If


        ' Create
        Dim bmp = New Bitmap(viewModel.File.InputStream)
        Dim icon = New Bitmap(96, 96)
        Dim g = Graphics.FromImage(icon)

        Dim srcX, srcY, srcSize As Integer
        Dim srcWidth = bmp.Width
        Dim srcHeight = bmp.Height

        If bmp.Width > bmp.Height Then
            srcX = (bmp.Width - bmp.Height) \ 2
            srcY = 0
            srcSize = bmp.Height
        Else
            srcX = 0
            srcY = (bmp.Height - bmp.Width) \ 2
            srcSize = bmp.Width
        End If

        g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

        g.DrawImage(bmp, New Rectangle(0, 0, icon.Width, icon.Height), New Rectangle(srcX, srcY, srcSize, srcSize), GraphicsUnit.Pixel)
        g.Dispose()

        ' Delete
        Dim deleteFile = IO.Path.Combine(Server.MapPath("~/App_Data/Uploads/"), com.IconPath)
        If IO.File.Exists(deleteFile) Then
            IO.File.Delete(deleteFile)
        End If

        ' Save
        Dim y = Now.Year.ToString
        Dim m = Now.Month.ToString
        Dim folder = Server.MapPath("~/App_Data/Uploads/Communities")
        Dim yearFolder = IO.Path.Combine(folder, y)
        Dim monthFolder = IO.Path.Combine(yearFolder, m)

        If Not IO.Directory.Exists(yearFolder) Then
            IO.Directory.CreateDirectory(yearFolder)
        End If
        If Not IO.Directory.Exists(monthFolder) Then
            IO.Directory.CreateDirectory(monthFolder)
        End If

        Dim iconPath = String.Format("Communities/{0}/{1}/{2}.{3}", y, m, viewModel.Id, If(format Is Drawing.Imaging.ImageFormat.Png, "png", "jpeg"))
        Dim upfile = IO.Path.Combine(Server.MapPath("~/App_Data/Uploads/"), iconPath)
        icon.Save(upfile, format)

        ' Update
        com.IconPath = iconPath
        com.LastUpdatedBy = appUser
        com.LastUpdatedDateTime = Now

        Await db.SaveChangesAsync()

        Return RedirectToAction("Details", New With {.id = com.Id, .message = DetailsMessageId.Edit})

    End Function


    Private Function CanEdit(appUser As ApplicationUser, community As Community) As Boolean
        If appUser Is Nothing Then
            Return False
        ElseIf Not community.IsLocked OrElse community.Owners.Contains(appUser) OrElse User.IsInRole("Admin") Then
            Return True
        End If
        Return False
    End Function


    Public Enum DetailsMessageId
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

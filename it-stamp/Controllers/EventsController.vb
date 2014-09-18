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

Public Class EventsController
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


    ' GET: Events
    Function Index(page As Integer?) As ActionResult

        Dim results = db.Events.Where(Function(e) Not e.IsHidden).OrderBy(Function(e) e.StartDateTime)

        Dim viewModel = New SearchEventsViewModel With {
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

    ' GET: Events/5
    Async Function Details(ByVal id As Integer?, message As DetailsMessageId?) As Task(Of ActionResult)
        If IsNothing(id) Then
            Return RedirectToAction("Index")
        End If

        Dim ev As [Event] = Await db.Events.FindAsync(id)
        If IsNothing(ev) Then
            Return HttpNotFound()
        End If

        ' イベントの編集権限があるかどうか
        Dim appUser = UserManager.FindById(User.Identity.GetUserId)
        ViewBag.CanEdit = CanEdit(appUser, ev)

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

        Return View(ev)
    End Function


    ' GET: Events/Add
    Function Add() As ActionResult
        Dim viewModel = New AddEventViewModel With {
            .StartDate = Now,
            .PrefectureSelectList = New SelectList(db.Prefectures, "Id", "Name")}

        Return View(viewModel)
    End Function

    ' POST: Events/Add
    <HttpPost>
    <ValidateAntiForgeryToken>
    Function Add(model As AddEventViewModel) As ActionResult
        If Not ModelState.IsValid Then
            model.PrefectureSelectList = New SelectList(db.Prefectures, "Id", "Name")
            Return View(model)
        End If

        Return RedirectToAction("AddDetails", New With {.PrefectureId = model.PrefectureId, .StartDate = model.StartDate})
    End Function

    ' GET: Events/AddDetails
    Function AddDetails(prefectureId As Integer?, startDate As DateTime?) As ActionResult

        If Not prefectureId.HasValue OrElse Not startDate.HasValue Then
            Return RedirectToAction("Add")
        End If

        Dim pref = db.Prefectures.Where(Function(p) p.Id = prefectureId.Value).FirstOrDefault
        If pref Is Nothing Then
            Return Redirect("Add")
        End If

        Dim viewModel = New AddEventDetailsViewModel With {
            .StartDate = startDate.Value,
            .PrefectureId = prefectureId.Value,
            .Prefecture = pref.Name,
            .CommunitiesSelectList = New SelectList(db.Communities.Where(Function(c) Not c.IsHidden).OrderBy(Function(c) c.Name), "Id", "Name")
            }

        Return View(viewModel)
    End Function

    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Async Function AddDetails(ByVal viewModel As AddEventDetailsViewModel) As Task(Of ActionResult)
        viewModel.CommunitiesSelectList = New SelectList(db.Communities.Where(Function(c) Not c.IsHidden).OrderBy(Function(c) c.Name), "Id", "Name")

        If Not ModelState.IsValid Then
            Return View(viewModel)
        End If

        ' 登録
        Try
            Dim ev = New [Event] With {
                .Prefecture = db.Prefectures.Where(Function(p) p.Id = viewModel.PrefectureId).FirstOrDefault}

            ' 編集権限の確認
            Dim appUser = Await UserManager.FindByIdAsync(User.Identity.GetUserId)
            If appUser Is Nothing Then
                'TODO Return View()
            End If

            ' 日時処理
            SetDateTime(viewModel.StartDate, viewModel.StartTime, viewModel.EndDate, viewModel.EndTime, ev.StartDateTime, ev.EndDateTime)
            If Not ModelState.IsValid Then
                Return View(viewModel)
            End If

            ' 単純なプロパティ更新
            UpdateModel(Of [Event])(ev)


            If viewModel.CommunityId.HasValue Then
                ev.Community = db.Communities.Where(Function(c) c.Id = viewModel.CommunityId.Value).FirstOrDefault
            End If

            Dim time = Now
            ev.CreatedBy = appUser
            ev.CreationDateTime = time
            ev.LastUpdatedBy = appUser
            ev.LastUpdatedDateTime = time

            Dim newEvent = db.Events.Add(ev)
            Await db.SaveChangesAsync()
            Return RedirectToAction("Details", New With {.id = newEvent.Id, .message = DetailsMessageId.Add})

        Catch eEx As System.Data.Entity.Validation.DbEntityValidationException
            For Each er In eEx.EntityValidationErrors
                For Each e In er.ValidationErrors
                    Debug.Print(e.ErrorMessage)
                Next
            Next
            Return View(viewModel)

        Catch ex As Exception
            ModelState.AddInternalError(User, ex)
            Return View(viewModel)
        End Try

    End Function

    ' GET: Events/Edit
    Async Function Edit(id As Integer?) As Task(Of ActionResult)
        If Not id.HasValue Then
            Return HttpNotFound()
        End If

        Dim ev As [Event] = Await db.Events.FindAsync(id)
        If IsNothing(ev) Then
            Return HttpNotFound()
        End If

        ' 編集権限の確認
        Dim appUser = UserManager.FindById(User.Identity.GetUserId)
        If Not CanEdit(appUser, ev) Then
            'Return View()
        End If

        Dim n = Now
        Dim hasTime = Not (ev.StartDateTime.Date = ev.EndDateTime.Date AndAlso ev.StartDateTime.TimeOfDay = ev.EndDateTime.TimeOfDay)
        Dim viewModel = New EventDetailsViewModel With {
            .Id = ev.Id,
            .Name = ev.Name,
            .Description = ev.Description,
            .StartDate = ev.StartDateTime.Date,
            .StartTime = If(hasTime, ev.StartDateTime, DirectCast(Nothing, DateTime?)),
            .EndDate = ev.EndDateTime.Date,
            .EndTime = If(hasTime, ev.EndDateTime, DirectCast(Nothing, DateTime?)),
            .Address = ev.Address,
            .Place = ev.Place,
            .Url = ev.Url,
            .IsCanceled = ev.IsCanceled,
            .IsHidden = ev.IsHidden,
            .IsLocked = ev.IsLocked,
            .ParticipantsOfflineCount = ev.ParticipantsOfflineCount,
            .ParticipantsOnlineCount = ev.ParticipantsOnlineCount,
            .ReportMemo = ev.ReportMemo,
            .SpecialEventsSelectList = New SelectList(db.SpecialEvents.Where(Function(e) e.StartDateTime <= n AndAlso n <= e.EndDateTime), "Id", "Name"),
            .SpecialEventId = If(ev.SpecialEvents IsNot Nothing, ev.SpecialEvents.Id, Nothing),
            .PrefectureId = ev.Prefecture.Id,
            .PrefectureSelectList = New SelectList(db.Prefectures, "Id", "Name"),
            .CommunitiesSelectList = New SelectList(db.Communities.Where(Function(c) Not c.IsHidden).OrderBy(Function(c) c.Name), "Id", "Name"),
            .CommunityId = If(ev.Community IsNot Nothing, ev.Community.Id, Nothing)
            }

        Return View(viewModel)
    End Function

    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Function Edit(ByVal viewModel As EventDetailsViewModel) As ActionResult
        Dim n = Now
        viewModel.SpecialEventsSelectList = New SelectList(db.SpecialEvents.Where(Function(e) e.StartDateTime <= n AndAlso n <= e.EndDateTime), "Id", "Name")
        viewModel.PrefectureSelectList = New SelectList(db.Prefectures, "Id", "Name")
        viewModel.CommunitiesSelectList = New SelectList(db.Communities.Where(Function(c) Not c.IsHidden).OrderBy(Function(c) c.Name), "Id", "Name")

        If Not ModelState.IsValid Then
            Return View(viewModel)
        End If

        Try
            Dim ev = db.Events.Where(Function(c) c.Id = viewModel.Id).FirstOrDefault
            If ev Is Nothing Then
                Return View("Index")
            End If

            ' 編集権限の確認
            Dim appUser = UserManager.FindById(User.Identity.GetUserId)
            If Not CanEdit(appUser, ev) Then
                'TODO Return View("Details", New With {.id = id})
            End If

            ' 日時処理
            SetDateTime(viewModel.StartDate, viewModel.StartTime, viewModel.EndDate, viewModel.EndTime, ev.StartDateTime, ev.EndDateTime)
            If Not ModelState.IsValid Then
                Return View(viewModel)
            End If

            ' 単純なプロパティ更新
            UpdateModel(Of [Event])(ev)


            ev.Prefecture = db.Prefectures.Where(Function(p) p.Id = viewModel.PrefectureId).FirstOrDefault

            'ev.SpecialEvents = If(viewModel.SpecialEventId.HasValue, db.SpecialEvents.Where(Function(e) e.Id = viewModel.SpecialEventId.Value).FirstOrDefault, Nothing)

            'If viewModel.CommunityId.HasValue Then
            '    ev.Community = db.Communities.Where(Function(c) c.Id = viewModel.CommunityId.Value).FirstOrDefault
            'Else
            '    ev.Community = Nothing
            'End If

            ev.Community = Nothing

            If viewModel.SpecialEventId.HasValue Then
                ev.SpecialEvents = db.SpecialEvents.Where(Function(e) e.Id = viewModel.SpecialEventId.Value).FirstOrDefault
            Else
                ev.SpecialEvents = Nothing
            End If


            ev.LastUpdatedBy = appUser
            ev.LastUpdatedDateTime = Now

            db.SaveChanges()

            Return RedirectToAction("Details", New With {.id = ev.Id, .message = DetailsMessageId.Edit})

        Catch eEx As System.Data.Entity.Validation.DbEntityValidationException
            For Each er In eEx.EntityValidationErrors
                For Each e In er.ValidationErrors
                    Debug.Print(e.ErrorMessage)
                Next
            Next
            Return View(viewModel)
        Catch ex As Exception
            ModelState.AddInternalError(User, ex)
            Return View(viewModel)
        End Try
    End Function

    Public Enum DetailsMessageId
        Add
        Edit
    End Enum

    Private Function CanEdit(appUser As ApplicationUser, ev As [Event]) As Boolean
        If appUser Is Nothing Then
            Return False
        ElseIf Not ev.IsLocked OrElse User.IsInRole("Admin") Then
            Return True
        End If
        Return False
    End Function

    Private Sub SetDateTime(startDate As DateTime, startTime As DateTime?, endDate As DateTime, endTime As DateTime?, ByRef startDateTime As DateTime, ByRef endDateTime As DateTime)

        startDateTime = startDate.Date
        endDateTime = endDate.Date

        If startTime.HasValue Then
            startDateTime = startDateTime.AddHours(startTime.Value.Hour).AddMinutes(startTime.Value.Minute)
        End If

        If endTime.HasValue Then
            endDateTime = endDateTime.AddHours(endTime.Value.Hour).AddMinutes(endTime.Value.Minute)
        Else
            ' 終了時間未設定の場合、開始日時の時間を設定
            endDateTime = endDateTime.AddHours(startDate.Hour).AddMinutes(startDate.Minute)
        End If

        If startDateTime > endDateTime Then
            ModelState.AddModelError("EndDate", "開始日時より後の日時を設定してください。")
        End If

        If startDateTime < Now.AddYears(-1) Then
            ModelState.AddModelError("StartDate", "1年以上古い日時は登録できません。")
        End If

        If startDateTime > Now.AddYears(1) Then
            ModelState.AddModelError("StartDate", "1年以上先の日時は登録できません。")
        End If

    End Sub


    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing) Then
            db.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
End Class

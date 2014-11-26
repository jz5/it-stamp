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

<Authorize>
<RequireHttps>
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
    <AllowAnonymous>
    Function Index(page As Integer?, past As Boolean?, specialEvent As Integer?, message As DetailsMessage?) As ActionResult

        Dim results As IQueryable(Of [Event])
        Dim n = Now
        If past.HasValue AndAlso past.Value = True Then
            ' 過去
            results = db.Events.Where(Function(e) Not e.IsHidden AndAlso e.EndDateTime < n).OrderByDescending(Function(e) e.StartDateTime)
        Else
            ' 開催予定
            results = db.Events.Where(Function(e) Not e.IsHidden AndAlso e.StartDateTime >= n).OrderBy(Function(e) e.StartDateTime)
        End If

        ' さらに絞り込む
        If specialEvent.HasValue Then
            results = From item In results Where item.SpecialEvents.Id = specialEvent.Value
        End If

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

        ' Message
        Dim msg As String
        Select Case message
            Case DetailsMessage.Delete
                msg = "削除しました。"
            Case Else
                msg = ""
        End Select
        ViewBag.StatusMessage = msg

        viewModel.Results = results.Skip((viewModel.CurrentPage - 1) * count).Take(count).ToList

        Return View(viewModel)
    End Function

    ' GET: Events/5
    <AllowAnonymous>
    Async Function Details(ByVal id As Integer?, message As DetailsMessage?) As Task(Of ActionResult)
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

        ' チェックイン済みか
        ' TwitterやFacebookとの連携状況
        If appUser IsNot Nothing Then
            Dim ci = db.CheckIns.Where(Function(c) c.User.Id = appUser.Id AndAlso c.Event.Id = ev.Id).SingleOrDefault
            ViewBag.CheckIned = ci IsNot Nothing
            ViewBag.ShareTwitter = appUser.ShareTwitter
            ViewBag.ShareFacebook = appUser.ShareFacebook
            ViewBag.IsPrivateUser = appUser.IsPrivate
        Else
            ViewBag.CheckIned = False
            ViewBag.ShareTwitter = False
            ViewBag.ShareFacebook = False
            ViewBag.IsPrivateUser = False
        End If

        If ev.IsCanceled OrElse ev.IsHidden Then
            ViewBag.CanCheckIn = False
        Else
            ViewBag.CanChackIn = True
        End If

        ' フォロー済みか
        If appUser IsNot Nothing Then
            Dim followed = appUser.Favorites.Where(Function(f) f.Event.Id = ev.Id).Count > 0
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
                msg = "保存しました。"
            Case DetailsMessage.CheckIn
                msg = ev.Name & "にチェックイン！"
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

    <HttpPost>
    <ValidateAntiForgeryToken>
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
            Dim userId = User.Identity.GetUserId
            Dim appUser = Await db.Users.Where(Function(u) u.Id = userId).SingleOrDefaultAsync
            If appUser Is Nothing Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If

            ' 日時処理
            SetDateTime(viewModel.StartDate, viewModel.StartTime, viewModel.EndDate, viewModel.EndTime, ev.StartDateTime, ev.EndDateTime, Now)
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
            Return RedirectToAction("Details", New With {.id = newEvent.Id, .message = DetailsMessage.Add})

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
        If ev Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        ' 編集権限の確認
        Dim userId = User.Identity.GetUserId
        Dim appUser = Await db.Users.Where(Function(u) u.Id = userId).SingleOrDefaultAsync
        If Not CanEdit(appUser, ev) Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
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
            .CheckInCode = ev.CheckInCode,
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
            .CommunityId = If(ev.Community IsNot Nothing, ev.Community.Id, Nothing)
            }

        If ev.Community Is Nothing OrElse User.IsInRole("Admin") Then
            viewModel.CommunitiesSelectList = New SelectList(db.Communities.Where(Function(c) Not c.IsHidden).OrderBy(Function(c) c.Name), "Id", "Name")
        Else
            viewModel.CommunitiesSelectList = New SelectList(db.Communities.Where(Function(c) c.Id = ev.Community.Id), "Id", "Name")
        End If

        ViewBag.CanEditDetails = CanEditDetails(appUser, ev)
        ViewBag.CanDelete = CanDelete(appUser, ev)

        Return View(viewModel)
    End Function

    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Async Function Edit(ByVal viewModel As EventDetailsViewModel) As Task(Of ActionResult)
        Dim n = Now
        viewModel.SpecialEventsSelectList = New SelectList(db.SpecialEvents.Where(Function(e) e.StartDateTime <= n AndAlso n <= e.EndDateTime), "Id", "Name")
        viewModel.PrefectureSelectList = New SelectList(db.Prefectures, "Id", "Name")
        viewModel.CommunitiesSelectList = New SelectList(db.Communities.Where(Function(c) Not c.IsHidden).OrderBy(Function(c) c.Name), "Id", "Name")

        Try
            Dim ev = db.Events.Where(Function(c) c.Id = viewModel.Id).FirstOrDefault
            If ev Is Nothing Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If

            ' 編集権限の確認
            Dim id = User.Identity.GetUserId
            Dim appUser = Await db.Users.Where(Function(u) u.Id = id).SingleOrDefaultAsync
            If Not CanEdit(appUser, ev) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If

            ' ModelState check
            ViewBag.CanEditDetails = CanEditDetails(appUser, ev)
            ViewBag.CanDelete = CanDelete(appUser, ev)
            If Not ModelState.IsValid Then
                Return View(viewModel)
            End If

            ' 値修正
            viewModel.Name = If(viewModel.Name IsNot Nothing, viewModel.Name.Trim, "")
            viewModel.Description = If(viewModel.Description IsNot Nothing, viewModel.Description.Trim, "")
            viewModel.CheckInCode = If(viewModel.CheckInCode IsNot Nothing, viewModel.CheckInCode.Trim, "")
            viewModel.Url = If(viewModel.Url IsNot Nothing, viewModel.Url.Trim, "")
            viewModel.Address = If(viewModel.Address IsNot Nothing, viewModel.Address.Trim, "")
            viewModel.Place = If(viewModel.Place IsNot Nothing, viewModel.Place.Trim, "")

            ' 日時処理
            SetDateTime(viewModel.StartDate, viewModel.StartTime, viewModel.EndDate, viewModel.EndTime, ev.StartDateTime, ev.EndDateTime, ev.CreationDateTime)
            If Not ModelState.IsValid Then
                Return View(viewModel)
            End If

            ' プロパティ更新
            If CanEditDetails(appUser, ev) Then
                UpdateModel(Of [Event])(ev)

                ' SpecialEvent
                If viewModel.SpecialEventId.HasValue Then
                    ev.SpecialEvents = db.SpecialEvents.Where(Function(e) e.Id = viewModel.SpecialEventId.Value).FirstOrDefault
                Else
                    If ev.SpecialEvents IsNot Nothing Then
                        ev.SpecialEvents = Nothing
                    End If
                End If
            Else
                ' 一般ユーザーは更新項目に制限
                ev.Name = viewModel.Name
                ev.Description = viewModel.Description
                ev.Address = viewModel.Address
                ev.Place = viewModel.Place
                ev.Url = viewModel.Url
            End If

            ' 都道府県更新
            ev.Prefecture = db.Prefectures.Where(Function(p) p.Id = viewModel.PrefectureId).FirstOrDefault

            ' コミュニティの更新
            ' 一般ユーザーとコミュニティオーナー: コミュニティ未指定の場合のみ設定可能
            If ev.Community Is Nothing OrElse User.IsInRole("Admin") Then
                If viewModel.CommunityId.HasValue Then
                    ev.Community = db.Communities.Where(Function(c) c.Id = viewModel.CommunityId.Value).FirstOrDefault
                Else
                    If ev.Community IsNot Nothing Then
                        ev.Community = Nothing
                    End If
                End If
            End If

            ev.LastUpdatedBy = appUser
            ev.LastUpdatedDateTime = Now

            db.SaveChanges()

            Return RedirectToAction("Details", New With {.id = ev.Id, .message = DetailsMessage.Edit})

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


    Async Function CheckIn(id As Integer?) As Task(Of ActionResult)
        If Not id.HasValue Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim userId = User.Identity.GetUserId
        Dim appUser = Await db.Users.Where(Function(u) u.Id = userId).SingleOrDefaultAsync
        If appUser Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim ev = db.Events.Where(Function(e) e.Id = id.Value).SingleOrDefault
        If ev Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        ' チェックイン済みか
        Dim ci = db.CheckIns.Where(Function(c) c.User.Id = appUser.Id AndAlso c.Event.Id = ev.Id).SingleOrDefault
        ViewBag.CheckIned = ci IsNot Nothing

        Dim viewModel = New CheckInViewModel With {
            .Event = ev,
            .ShareFacebook = appUser.ShareFacebook,
            .ShareTwitter = appUser.ShareTwitter}

        If ev.IsCanceled OrElse ev.IsHidden Then
            ViewBag.StatusMessage = "このIT勉強会にはチェックインできません。"
            Return View(viewModel)
        End If

        Return View(viewModel)
    End Function

    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Async Function CheckIn(viewModel As CheckInViewModel) As Task(Of ActionResult)
        Dim userId = User.Identity.GetUserId
        Dim appUser = Await db.Users.Where(Function(u) u.Id = userId).SingleOrDefaultAsync
        If appUser Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim id = viewModel.Event.Id
        Dim ev = db.Events.Where(Function(e) e.Id = id).SingleOrDefault
        If ev Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        ' すでにチェックイン済みかどうかも調べる
        If db.CheckIns.Where(Function(c) c.User.Id = appUser.Id AndAlso c.Event.Id = ev.Id).SingleOrDefault IsNot Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Try
            viewModel.Event = ev

            If Not ModelState.IsValid Then
                Return View(viewModel)
            End If

            If ev.IsCanceled OrElse ev.IsHidden Then
                ViewBag.ErrorMessage = "このIT勉強会にはチェックインできません。"
                Return View(viewModel)
            End If

            If Now < ev.StartDateTime.AddHours(-1) Then
                ViewBag.ErrorMessage = "開始時間の1時間前からチェックインできるようになります。"
                Return View(viewModel)
            End If

            ' Stamp
            Dim stamp As Stamp = Nothing
            If ev.Community IsNot Nothing Then
                stamp = db.Stamps.Where(Function(s) s.Community.Id = ev.Community.Id).SingleOrDefault
            End If

            Dim ci = New CheckIn With {
                .Event = ev,
                .DateTime = Now,
                .User = appUser,
                .Stamp = stamp}

            db.CheckIns.Add(ci)
            ev.CheckIns.Add(ci)

            If viewModel.ShareTwitter Then
                ' TODO: Twitterへ投稿

            End If

            If viewModel.ShareFacebook Then
                ' TODO: Facebookへ投稿

            End If

            If viewModel.AdditionalMessage <> String.Empty Then
                Dim comment = New Comment() With {
                                .Content = viewModel.AdditionalMessage,
                                .CreatedBy = appUser,
                                .CreationDateTime = DateTime.Now,
                                .Event = ev,
                                .id = -1
                                }
                db.Comments.Add(comment)
                ev.Comments.Add(comment)
            End If

            Await db.SaveChangesAsync

            If Request.IsAjaxRequest Then
                Return Json(New With {.checkined = True})
            End If

            ' MEMO Ajax 処理のみになったの通常実行しない
            Return RedirectToAction("Details", "Events", New With {.id = ev.Id, .message = DetailsMessage.CheckIn})

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
            Return View(viewModel)
        End If

    End Function

    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Async Function Follow(model As [Event]) As Task(Of ActionResult)
        Dim userId = User.Identity.GetUserId
        Dim appUser = Await db.Users.Where(Function(u) u.Id = userId).SingleOrDefaultAsync
        If appUser Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim id = model.Id
        Dim ev = db.Events.Where(Function(e) e.Id = id).SingleOrDefault
        If ev Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Try
            ' フォロー済みか
            Dim fv = appUser.Favorites.Where(Function(f) f.Event.Id = id AndAlso f.User.Id = userId).SingleOrDefault
            Dim followed = fv IsNot Nothing
            ViewBag.Followd = followed

            If Not ModelState.IsValid Then
                Return View(model)
            End If

            If ev.IsHidden Then
                ViewBag.ErrorMessage = "このIT勉強会はフォローできません。"
                Return View(model)
            End If

            ' Switch following
            If followed Then
                db.Favorites.Remove(fv)
            Else
                Dim newFv = New Favorite() With {
                    .Event = ev,
                    .User = appUser,
                    .DateTime = Now}
                db.Favorites.Add(newFv)
                appUser.Favorites.Add(newFv)

            End If

            Await db.SaveChangesAsync

            ' for Ajax
            If Request.IsAjaxRequest Then
                Return Json(New With {.followed = Not followed})
            End If

            Return RedirectToAction("Details", "Events", New With {.id = ev.Id})

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
            Return View(model)
        End If

    End Function

    Async Function Today() As Task(Of ActionResult)

        'Dim userId = User.Identity.GetUserId
        'Dim appUser = Await db.Users.Where(Function(u) u.Id = userId).SingleOrDefaultAsync
        'If appUser Is Nothing Then
        '    Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        'End If

        'Dim n = Now.Date
        'Dim results = db.Events.Where(Function(e) Not e.IsHidden).OrderBy(Function(e) e.StartDateTime.Date <= n AndAlso n <= e.EndDateTime.Date)

        'Dim viewModel = New SearchEventsViewModel With {
        '    .TotalCount = results.Count
        '    }

        'Dim count = 10
        'Dim pagenationCount = 5

        '' Total page
        'viewModel.TotalPages = (results.Count - 1) \ count + 1

        '' Current page
        'If Not Page.HasValue OrElse viewModel.TotalPages > Page.Value Then
        '    viewModel.CurrentPage = 1
        'Else
        '    viewModel.CurrentPage = Page.Value
        'End If

        '' Start page
        'viewModel.StartPage = viewModel.CurrentPage - pagenationCount
        'If viewModel.StartPage < 1 Then
        '    viewModel.StartPage = 1
        'End If

        '' End page
        'viewModel.EndPage = viewModel.StartPage + pagenationCount - 1
        'If viewModel.EndPage > viewModel.TotalPages Then
        '    viewModel.EndPage = viewModel.TotalPages
        'End If

        'viewModel.Results = results.Skip((viewModel.CurrentPage - 1) * count).Take(count).ToList

        'Return View(viewModel)
    End Function


    Async Function Delete(id As Integer?) As Task(Of ActionResult)
        If Not id.HasValue Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim ev = db.Events.Where(Function(e) e.Id = id.Value).SingleOrDefault
        If ev Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim userId = User.Identity.GetUserId
        Dim appUser = Await db.Users.Where(Function(u) u.Id = userId).SingleOrDefaultAsync
        If appUser Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        ViewBag.CanDelete = CanDelete(appUser, ev)

        Return View(ev)
    End Function

    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Async Function Delete(model As [Event]) As Task(Of ActionResult)

        Dim userId = User.Identity.GetUserId
        Dim appUser = Await db.Users.Where(Function(u) u.Id = userId).SingleOrDefaultAsync

        Dim id = model.Id
        Dim ev = db.Events.Where(Function(e) e.Id = id).SingleOrDefault

        If Not CanDelete(appUser, ev) Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Try
            If Not ModelState.IsValid Then
                Return View(model)
            End If

            db.Events.Remove(ev)
            Await db.SaveChangesAsync

            Return RedirectToAction("Index", "Events", New With {.message = DetailsMessage.Delete})

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


    Public Enum DetailsMessage
        Add
        Edit
        Delete
        CheckIn
    End Enum

    Private Function CanEdit(appUser As ApplicationUser, ev As [Event]) As Boolean
        If appUser Is Nothing OrElse ev Is Nothing Then
            Return False
        ElseIf Not ev.IsLocked Then
            ' 一般ユーザー
            Return True
        ElseIf ev.Community IsNot Nothing AndAlso appUser.OwnerCommunities.Where(Function(c) c.Id = ev.Community.Id).Count > 0 Then
            ' コミュニティオーナー
            Return True
        ElseIf User.IsInRole("Admin") Then
            Return True
        End If
        Return False
    End Function

    Private Function CanEditDetails(appUser As ApplicationUser, ev As [Event]) As Boolean
        If appUser Is Nothing OrElse ev Is Nothing Then
            Return False
        ElseIf ev.Community IsNot Nothing AndAlso appUser.OwnerCommunities.Where(Function(c) c.Id = ev.Community.Id).Count > 0 Then
            ' コミュニティオーナー
            Return True
        ElseIf User.IsInRole("Admin") Then
            Return True
        End If
        Return False
    End Function

    Private Function CanDelete(appUser As ApplicationUser, ev As [Event]) As Boolean
        If appUser Is Nothing OrElse ev Is Nothing Then
            Return False
        ElseIf User.IsInRole("Admin") Then
            Return True
        ElseIf ev.CheckIns.Count > 0 OrElse ev.Favorites.Count > 0 OrElse ev.Comments.Count > 0 OrElse ev.IsReported Then
            ' チェックイン済み・フォロー済み・コメントあり・報告済みの場合削除不可
            Return False
        ElseIf ev.Community IsNot Nothing AndAlso appUser.OwnerCommunities.Where(Function(c) c.Id = ev.Community.Id).Count > 0 Then
            ' コミュニティオーナー
            Return True
        ElseIf ev.CreatedBy.Id = appUser.Id AndAlso Not ev.IsLocked Then
            ' 作成者
            Return True
        End If
        Return False
    End Function


    Private Sub SetDateTime(startDate As DateTime, startTime As DateTime?, endDate As DateTime, endTime As DateTime?, ByRef startDateTime As DateTime, ByRef endDateTime As DateTime, baseDateTime As DateTime)

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

        If startDateTime < baseDateTime.AddYears(-1) Then
            ModelState.AddModelError("StartDate", "1年以上古い日時は登録できません。")
        End If

        If startDateTime > baseDateTime.AddYears(1) Then
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

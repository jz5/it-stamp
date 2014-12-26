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
        Dim now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.ToUniversalTime(), "Tokyo Standard Time")
        Dim n = now.Date
        If past.HasValue AndAlso past.Value = True Then
            ' 過去
            results = db.Events.Where(Function(e) Not e.IsHidden AndAlso e.EndDateTime < n).OrderByDescending(Function(e) e.StartDateTime)
        Else
            ' 開催予定
            results = db.Events.Where(Function(e) Not e.IsHidden AndAlso e.EndDateTime >= n).OrderBy(Function(e) e.StartDateTime)
        End If

        ' さらに絞り込む
        If specialEvent.HasValue Then
            results = From item In results Where item.SpecialEvents.Where(Function(ev) ev.SpecialEvent.Id = specialEvent.Value).Count > 0
        End If

        Dim viewModel = New SearchEventsViewModel With {
            .TotalCount = results.Count,
            .Past = If(past, False)
            }

        Dim count = 20
        Dim pagenationCount = 5

        ' Total page
        viewModel.TotalPages = (results.Count - 1) \ count + 1

        ' Current page
        If Not page.HasValue OrElse viewModel.TotalPages < page.Value Then
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
        ViewBag.CanEditDetails = CanEditDetails(appUser, ev)

        ' チェックイン済みか
        ' TwitterやFacebookとの連携状況
        If appUser IsNot Nothing Then
            Dim ci = db.CheckIns.Where(Function(c) c.User.Id = appUser.Id AndAlso c.Event.Id = ev.Id).SingleOrDefault
            ViewBag.CheckIned = ci IsNot Nothing
            ViewBag.Twitter = appUser.Twitter
            ViewBag.ShareTwitter = appUser.ShareTwitter ' MEMO: ShareTwitter はツイートするかどうかの既定値用（予約）
            ViewBag.ShareFacebook = False ' TODO: Facebook対応
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
            Dim followed = appUser.Favorites.Any(Function(f) f.Event.Id = ev.Id)
            ViewBag.Followed = followed
        Else
            ViewBag.Followed = False
        End If

        ' Message
        Dim msg As String
        Select Case message
            Case DetailsMessage.Add
                msg = "登録しました。"
            Case DetailsMessage.Edit
                msg = "保存しました。"
            Case Else
                msg = ""
        End Select
        ViewBag.StatusMessage = msg

        Return View(ev)
    End Function


    ' GET: Events/Search
    Function Search() As ActionResult
        Dim viewModel = New AddEventViewModel With {
            .StartDate = TokyoTime.Now,
            .PrefectureSelectList = New SelectList(db.Prefectures, "Id", "Name")}

        Return View(viewModel)
    End Function

    ' POST: Events/Search
    <HttpPost>
    <ValidateAntiForgeryToken>
    Function Search(model As AddEventViewModel) As ActionResult
        If Not ModelState.IsValid Then
            model.PrefectureSelectList = New SelectList(db.Prefectures, "Id", "Name")
            Return View(model)
        End If

        Return RedirectToAction("Add", New With {.PrefectureId = model.PrefectureId, .StartDate = model.StartDate})
    End Function

    ' Post: Events/[Select]
    <HttpPost>
    <ValidateAntiForgeryToken>
    Async Function [Select](site As String, eventId As String) As Task(Of ActionResult)
        If site Is Nothing OrElse eventId Is Nothing Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim eventApi As EventApi
        Select Case site.ToLowerInvariant
            Case "atnd"
                eventApi = New Atnd
            Case "connpass"
                eventApi = New Connpass
            Case "doorkeeper"
                eventApi = New Doorkeeper
            Case Else
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End Select

        Dim result = Await eventApi.GetEvent(eventId)
        If result Is Nothing OrElse result.Event Is Nothing Then
            ' TODO:
            Return RedirectToAction("Search")
        End If

        ' 登録済みの勉強会
        Try
            Dim ev = db.Events.Where(Function(e) e.Url = result.Event.Url).FirstOrDefault
            If ev IsNot Nothing Then
                Return RedirectToAction("Details", New With {.id = ev.Id})
            End If
        Catch ex As Exception
            ' TODO: 
            Return RedirectToAction("Search")
        End Try

        ' 未登録の勉強会
        Try
            Dim ev = result.Event

            ' 編集権限の確認
            Dim userId = User.Identity.GetUserId
            Dim appUser = Await db.Users.Where(Function(u) u.Id = userId).SingleOrDefaultAsync
            If appUser Is Nothing Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If

            ' 値修正
            Dim prefId = ev.Prefecture.Id
            ev.Prefecture = db.Prefectures.Where(Function(p) p.Id = prefId).SingleOrDefault
            ev.Name = If(ev.Name IsNot Nothing, ev.Name.Trim, "")
            ev.Description = If(ev.Description IsNot Nothing, ev.Description.Trim, "")
            ev.CheckInCode = If(ev.CheckInCode IsNot Nothing, ev.CheckInCode.Trim, "")
            ev.Url = If(ev.Url IsNot Nothing, ev.Url.Trim, Nothing)
            ev.Address = If(ev.Address IsNot Nothing, ev.Address.Trim, "")
            ev.Place = If(ev.Place IsNot Nothing, ev.Place.Trim, "")

            ' Datetime
            Dim time = Now
            ev.CreatedBy = appUser
            ev.CreationDateTime = time
            ev.LastUpdatedBy = appUser
            ev.LastUpdatedDateTime = time

            Dim newEvent = db.Events.Add(ev)
            Await db.SaveChangesAsync()
            Return RedirectToAction("Details", New With {.id = newEvent.Id})

        Catch ex As Exception
            ' TODO: 
            Return RedirectToAction("Search")
        End Try

    End Function


    ' GET: Events/Add
    Function Add(prefectureId As Integer?, startDate As DateTime?) As ActionResult

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
    Async Function GetEvents(prefectureId As Integer?, startDate As DateTime?) As Task(Of ActionResult)

        If Not Request.IsAjaxRequest OrElse Not prefectureId.HasValue OrElse Not startDate.HasValue Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Dim apiResults = New List(Of ApiResult)
        Dim apis = New List(Of EventApi) From {New Atnd, New Connpass, New Doorkeeper}
        Dim pref = AddressHelper.GetPrefecture(prefectureId.Value)

        For Each a In apis
            Dim results = Await a.GetEvents(pref, startDate.Value)
            apiResults.AddRange(results)
        Next

        Return Json(New With {.ApiResults = apiResults, .Keyword = pref.Name})
    End Function

    <HttpPost>
    <ValidateAntiForgeryToken>
    Async Function Add(ByVal viewModel As AddEventDetailsViewModel) As Task(Of ActionResult)
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
            SetDateTime(viewModel.StartDate, viewModel.StartTime, viewModel.EndDate, viewModel.EndTime, ev.StartDateTime, ev.EndDateTime, TokyoTime.Now)

            ' Community
            If viewModel.CommunityName <> "" Then
                viewModel.CommunityName = viewModel.CommunityName.Trim
                If db.Communities.Where(Function(c) c.Name = viewModel.CommunityName).FirstOrDefault IsNot Nothing Then
                    ModelState.AddModelError("CommunityName", "既に登録されている名前です。")
                End If
            End If

            ' 値修正
            ev.Name = If(ev.Name IsNot Nothing, ev.Name.Trim, "")
            ev.Description = If(ev.Description IsNot Nothing, ev.Description.Trim, "")
            ev.CheckInCode = If(ev.CheckInCode IsNot Nothing, ev.CheckInCode.Trim, "")
            ev.Url = If(ev.Url IsNot Nothing, ev.Url.Trim, Nothing)
            ev.Address = If(ev.Address IsNot Nothing, ev.Address.Trim, "")
            ev.Place = If(ev.Place IsNot Nothing, ev.Place.Trim, "")

            ' ModalState
            If Not ModelState.IsValid Then
                Return View(viewModel)
            End If

            ' 単純なプロパティ更新
            UpdateModel(Of [Event])(ev)

            ' Community
            If viewModel.CommunityName <> "" Then
                ' 新規追加
                Dim newCom = New Community With {
                    .Name = viewModel.CommunityName}

                Dim n = now
                newCom.CreatedBy = appUser
                newCom.CreationDateTime = n
                newCom.LastUpdatedBy = appUser
                newCom.LastUpdatedDateTime = n

                ' Random icon
                newCom.IconPath = "Icons/icon" & ((New Random).Next(47) + 1).ToString("00") & ".png"

                Dim com = db.Communities.Add(newCom)
                Await db.SaveChangesAsync

                ev.Community = com

            ElseIf viewModel.CommunityId.HasValue Then
                ev.Community = db.Communities.Where(Function(c) c.Id = viewModel.CommunityId.Value).FirstOrDefault
            End If

            ' Datetime
            Dim time = now
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

        Dim now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.ToUniversalTime(), "Tokyo Standard Time")
        Dim n = now
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
            .IsReported = ev.IsReported,
            .ParticipantsOfflineCount = ev.ParticipantsOfflineCount,
            .ParticipantsOnlineCount = ev.ParticipantsOnlineCount,
            .ReportMemo = ev.ReportMemo,
            .SpecialEventsSelectList = New SelectList(db.SpecialEvents.Where(Function(e) n <= e.EndDateTime), "Id", "Name"),
            .SpecialEventId = If(ev.SpecialEvents IsNot Nothing AndAlso ev.SpecialEvents.Count > 0, ev.SpecialEvents.First.SpecialEvent.Id, Nothing),
            .PrefectureId = If(ev.Prefecture IsNot Nothing, ev.Prefecture.Id, Nothing),
            .PrefectureSelectList = New SelectList(db.Prefectures, "Id", "Name"),
        .CommunityId = If(ev.Community IsNot Nothing, ev.Community.Id, Nothing)
            }
        ' TODO: SpecialEvents 複数対応

        If ev.Community Is Nothing OrElse User.IsInRole("Admin") Then
            viewModel.CommunitiesSelectList = New SelectList(db.Communities.Where(Function(c) Not c.IsHidden).OrderBy(Function(c) c.Name), "Id", "Name")
        Else
            'viewModel.CommunitiesSelectList = New SelectList(db.Communities.Where(Function(c) c.Id = ev.Community.Id), "Id", "Name")
            viewModel.Community = db.Communities.Where(Function(c) c.Id = ev.Community.Id).SingleOrDefault
        End If

        ViewBag.CanEditDetails = CanEditDetails(appUser, ev)
        ViewBag.CanDelete = CanDelete(appUser, ev)

        Return View(viewModel)
    End Function

    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Async Function Edit(ByVal viewModel As EventDetailsViewModel) As Task(Of ActionResult)
        Dim now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.ToUniversalTime(), "Tokyo Standard Time")
        Dim n = now
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

            ' 値修正
            viewModel.Name = If(viewModel.Name IsNot Nothing, viewModel.Name.Trim, "")
            viewModel.Description = If(viewModel.Description IsNot Nothing, viewModel.Description.Trim, "")
            viewModel.CheckInCode = If(viewModel.CheckInCode IsNot Nothing, viewModel.CheckInCode.Trim, "")
            viewModel.Url = If(viewModel.Url IsNot Nothing, viewModel.Url.Trim, Nothing)
            viewModel.Url = If(viewModel.Url = "", Nothing, viewModel.Url)
            viewModel.Address = If(viewModel.Address IsNot Nothing, viewModel.Address.Trim, "")
            viewModel.Place = If(viewModel.Place IsNot Nothing, viewModel.Place.Trim, "")

            ' ModelState check
            ViewBag.CanEditDetails = CanEditDetails(appUser, ev)
            ViewBag.CanDelete = CanDelete(appUser, ev)
            If Not ModelState.IsValid Then
                Return View(viewModel)
            End If

            ' 日時処理
            SetDateTime(viewModel.StartDate, viewModel.StartTime, viewModel.EndDate, viewModel.EndTime, ev.StartDateTime, ev.EndDateTime, ev.CreationDateTime)
            If Not ModelState.IsValid Then
                Return View(viewModel)
            End If

            ' プロパティ更新
            If CanEditDetails(appUser, ev) Then
                UpdateModel(Of [Event])(ev)

                ' SpecialEvent
                ' TODO: SpecialEvent 複数対応
                If viewModel.SpecialEventId.HasValue Then
                    If ev.SpecialEvents.Any AndAlso ev.SpecialEvents.First.SpecialEvent.Id = viewModel.SpecialEventId.Value Then
                        ' 追加済み
                        ' Do nothing
                    Else
                        ' 新規追加
                        Dim ue = New UserEvent With {
                            .Event = ev,
                            .SpecialEvent = db.SpecialEvents.Where(Function(e) e.Id = viewModel.SpecialEventId.Value).SingleOrDefault}
                        ev.SpecialEvents.Add(ue)
                    End If
                Else
                    ' 削除
                    If ev.SpecialEvents.Any Then
                        ' Delete userEvent
                        Dim userEventId = ev.SpecialEvents.First.Id
                        Dim ue = db.UserEvents.Where(Function(e) e.Id = userEventId).Single
                        db.UserEvents.Remove(ue)
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
            ev.LastUpdatedDateTime = now

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

    ' GET: Events/EditReport
    Async Function EditReport(id As Integer?) As Task(Of ActionResult)
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
        If Not CanEditDetails(appUser, ev) Then
            Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
        End If

        Return View(ev)
    End Function

    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Async Function EditReport(ByVal model As [Event]) As Task(Of ActionResult)
        Try
            Dim ev = db.Events.Where(Function(c) c.Id = model.Id).FirstOrDefault
            If ev Is Nothing Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If

            ' 編集権限の確認
            Dim id = User.Identity.GetUserId
            Dim appUser = Await db.Users.Where(Function(u) u.Id = id).SingleOrDefaultAsync
            If Not CanEditDetails(appUser, ev) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If

            ' ModalState
            If Not ModelState.IsValid Then
                Return View(ev)
            End If

            ' 値修正
            model.ReportMemo = If(model.ReportMemo IsNot Nothing, model.ReportMemo.Trim, "")

            ' プロパティ更新
            ev.ParticipantsOfflineCount = model.ParticipantsOfflineCount
            ev.ParticipantsOnlineCount = model.ParticipantsOnlineCount
            ev.ReportMemo = model.ReportMemo
            ev.IsReported = True

            Dim now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.ToUniversalTime(), "Tokyo Standard Time")
            ev.LastUpdatedBy = appUser
            ev.LastUpdatedDateTime = now

            db.SaveChanges()

            Return RedirectToAction("Details", New With {.id = ev.Id, .message = DetailsMessage.Edit})

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

            Dim now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.ToUniversalTime(), "Tokyo Standard Time")
            If now < ev.StartDateTime.AddHours(-1) Then
                ViewBag.ErrorMessage = "開始時間の1時間前からチェックインできるようになります。"
                Return View(viewModel)
            End If

            ' Stamp
            'Dim stamp = New UserStamp
            'If ev.Community IsNot Nothing Then
            '    If ev.Community.DefaultStamp IsNot Nothing Then
            '        stamp.Stamp = ev.Community.DefaultStamp
            '    End If
            '    ' TODO Stamp expression 処理
            'End If

            Dim ci = New CheckIn With {
                .Event = ev,
                .DateTime = now,
                .User = appUser}

            ev.CheckIns.Add(ci)

            If viewModel.ShareTwitter Then
                ' Tweet
                Dim words = New List(Of String)
                words.Add(String.Format("http://it-stamp.jp/e/{0}", ev.Id))
                words.Add(If(ev.Hashtag <> "", "#" & ev.Hashtag.Trim, ""))
                words.Add("#itstamp")
                Dim footer = String.Join(" ", words.ToArray)

                Dim msg = If(viewModel.AdditionalMessage <> "", viewModel.AdditionalMessage.Trim, "")
                If (msg & " " & footer).Length > 140 Then
                    msg = msg.Substring(0, 140 - footer.Length - 1)
                End If

                Try
                    Await SocialHelpers.Tweet(Await UserManager.GetClaimsAsync(userId), msg & " " & footer)
                Catch ex As Exception
                    ' Ignore
                End Try
            End If

            If viewModel.ShareFacebook Then
                ' TODO: Facebookへ投稿

            End If

            If viewModel.PostComment AndAlso viewModel.AdditionalMessage <> "" Then
                Dim comment = New Comment() With {
                                .Content = viewModel.AdditionalMessage.Trim,
                                .CreatedBy = appUser,
                                .CreationDateTime = now,
                                .Event = ev}
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
    Async Function Comment(viewModel As CheckInViewModel) As Task(Of ActionResult)
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

        ' 連投制限
        Dim now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.ToUniversalTime(), "Tokyo Standard Time")
        Dim lastComment = db.Comments.Where(Function(c) c.CreatedBy.Id = appUser.Id AndAlso c.Event.Id = ev.Id).OrderByDescending(Function(c) c.CreationDateTime).FirstOrDefault
        If lastComment IsNot Nothing AndAlso lastComment.CreationDateTime.AddMinutes(1) > now Then
            Return View(viewModel) ' TODO: 連投制限処理
        End If

        Try
            viewModel.Event = ev

            If Not ModelState.IsValid Then
                Return View(viewModel)
            End If

            If viewModel.ShareTwitter Then
                ' Tweet
                Dim words = New List(Of String)
                words.Add(If(viewModel.AdditionalMessage <> "", viewModel.AdditionalMessage.Trim, ""))
                words.Add(String.Format("http://it-stamp.jp/e/{0}", ev.Id))
                words.Add(If(ev.Hashtag <> "", "#" & ev.Hashtag.Trim, ""))
                words.Add("#itstamp")

                Try
                    Await SocialHelpers.Tweet(Await UserManager.GetClaimsAsync(userId), String.Join(" ", words.ToArray))
                Catch ex As Exception
                    ' Ignore
                End Try
            End If

            If viewModel.ShareFacebook Then
                ' TODO: Facebookへ投稿

            End If

            If viewModel.AdditionalMessage <> "" Then
                Dim c = New Comment() With {
                                .Content = viewModel.AdditionalMessage.Trim,
                                .CreatedBy = appUser,
                                .CreationDateTime = now,
                                .Event = ev}
                ev.Comments.Add(c)
            End If

            Await db.SaveChangesAsync

            If Request.IsAjaxRequest Then
                Return Json(New With {.checkined = True})
            End If

            ' MEMO Ajax 処理のみになったの通常実行しない
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
            ViewBag.Followed = followed

            If Not ModelState.IsValid Then
                Return View(ev)
            End If

            If ev.IsHidden Then
                ViewBag.ErrorMessage = "このIT勉強会はフォローできません。"
                Return View(model)
            End If

            ' Switch following
            If followed Then
                db.Favorites.Remove(fv)
            Else
                Dim now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.ToUniversalTime(), "Tokyo Standard Time")
                Dim newFv = New Favorite() With {
                    .Event = ev,
                    .User = appUser,
                    .DateTime = now}
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
        Try
            Dim userId = User.Identity.GetUserId
            Dim appUser = Await db.Users.Where(Function(u) u.Id = userId).SingleOrDefaultAsync

            Dim id = model.Id
            Dim ev = db.Events.Where(Function(e) e.Id = id).SingleOrDefault

            If Not CanDelete(appUser, ev) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If

            If Not ModelState.IsValid Then
                Return View(ev)
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
        ElseIf ev.Community IsNot Nothing AndAlso appUser.OwnerCommunities.Any(Function(c) c.Id = ev.Community.Id) Then
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
        ElseIf ev.Community IsNot Nothing AndAlso appUser.OwnerCommunities.Any(Function(c) c.Id = ev.Community.Id) Then
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
        ElseIf ev.CheckIns.Any OrElse ev.Favorites.Any OrElse ev.Comments.Any OrElse ev.IsReported Then
            ' チェックイン済み・フォロー済み・コメントあり・報告済みの場合削除不可
            Return False
        ElseIf ev.Community IsNot Nothing AndAlso appUser.OwnerCommunities.Any(Function(c) c.Id = ev.Community.Id) Then
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

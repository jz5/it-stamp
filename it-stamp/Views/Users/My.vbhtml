﻿@ModelType ApplicationUser
@Imports Microsoft.AspNet.Identity
@Code

    Layout = "_UserLayout.vbhtml"
    ViewBag.Title = Model.DisplayName & "さん"
    Dim now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.ToUniversalTime(), "Tokyo Standard Time")

End Code
<ul class="nav nav-tabs nav-justified myapge-tabs">
    <li role="presentation" class="active"><a href="@Href("~/Users/" & Model.UserName & "/My")">ホーム</a></li>
    <li role="presentation"><a href="@Href("~/Users/" & Model.UserName & "/MyFollowing")">フォロー</a></li>
    <li role="presentation"><a href="@Href("~/Users/" & Model.UserName & "/Manage")">管理</a></li>
</ul>
<h2>📢 開催予定のIT勉強会 <small>フォローしているIT勉強会とコミュニティ</small></h2>
<div>
    @If ViewBag.Events.Count = 0 Then
        @<p class="text-muted">開催予定のIT勉強会はありません。</p>
    Else
        @<table class="table">
            <tbody>
                @For Each ev As [Event] In ViewBag.Events
                    @<tr>
                        <td style="width:32px;"><a href="@Href("/Events/")@ev.Id"><img class="img-rounded icon24" src="@Href(ev.GetIconPath)" /></a></td>
                        <td style="width:35%;">
                            @If ev.StartDateTime.Date <= now.Date AndAlso now.Date <= ev.EndDateTime.Date Then
                                @<span class="badge badge-primary">&nbsp;今日&nbsp;</span>
                            ElseIf ev.StartDateTime.Date = now.Date.AddDays(1) OrElse ev.EndDateTime.Date = now.Date.AddDays(1) Then
                                @<span class="badge badge-default">&nbsp;明日&nbsp;</span>
                            End If
                            <time class="text-muted small">@ev.FriendlyDateTime</time>
                        </td>
                        <td><a href="@Href("~/Events/" & ev.Id)">@ev.Name</a></td>
                    </tr>
                Next
            </tbody>
        </table>
    End If
</div>
<h2>✅ チェックイン <span class="badge badge-primary @(If(Model.CheckIns.Count = 0, "hidden", ""))">@Model.CheckIns.Count</span></h2>
<div>
    @If Model.CheckIns.Count = 0 Then
        @<p class="text-muted">まだチェックインしていません。</p>
    Else
        @<table class="table">
            <tbody>
                @For Each item In Model.CheckIns
                    @<tr>
                        <td style="width:32px;"><a href="@Href("/Events/")@item.Event.Id"><img class="img-rounded icon24" src="@Href(item.Event.GetIconPath)" /></a></td>
                        <td><a href="@Href("/Events/")@item.Event.Id">@item.Event.Name</a></td>
                        <td><time class="text-muted small" datetime="@item.DateTime.ToString("yyyy-MM-ddTH:mm:ssK")">@item.DateTime.ToString("yyyy/MM/dd HH:mm")</time></td>
                    </tr>
                Next
            </tbody>
        </table>
    End If
</div>

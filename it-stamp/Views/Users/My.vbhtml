@ModelType ApplicationUser
@Imports Microsoft.AspNet.Identity
@Code

    Layout = "_UserLayout.vbhtml"
    ViewBag.Title = Model.FriendlyName

End Code

<ul class="nav nav-tabs nav-justified myapge-tabs">
    <li role="presentation" class="active"><a href="@Href("~/Users/" & Model.UserName & "/My")">ホーム</a></li>
    <li role="presentation"><a href="@Href("~/Users/" & Model.UserName & "/MyFollowing")">フォロー</a></li>
    <li role="presentation"><a href="@Href("~/Users/" & Model.UserName & "/Manage")">管理</a></li>
</ul>

<h2>📢 開催予定のIT勉強会 <small>フォローしているIT勉強会とコミュニティ</small></h2>
<div>
    @If ViewBag.Events.Count = 0 Then
        @<p class="text-muted">開催予定の勉強会はありません。</p>
    Else
    @<table class="table">
            <tbody>
            @For Each ev As [Event] In ViewBag.Events
            @<tr>
                <td style="width:35%;"><time class="text-muted small">@ev.FriendlyDateTime</time></td>
                <td><a href="@Href("~/Events/" & ev.Id)">@ev.Name</a></td>
            </tr>
            Next
            </tbody>
    </table>
    End If
</div>

<h2>✅ チェックイン</h2>
<div>
    @If Model.CheckIns.Count = 0 Then
        @<p class="text-muted">まだチェックインしていません。</p>
    Else
        @<table class="table">
            <tbody>
                @For Each item In Model.CheckIns
                Dim src = If(item.Event.Community IsNot Nothing AndAlso item.Event.Community.IconPath <> "", Href("/Uploads/" & item.Event.Community.IconPath), Href("/Uploads/Icons/no-community.png"))
                    @<tr>
                        <td style="border-top-width:0;width:32px;"><a href="@Href("/Events/")@item.Event.Id"><img class="img-rounded icon24" src="@src" /></a></td>
                        <td style="border-top-width:0;vertical-align:bottom;"><a href="@Href("/Events/")@item.Event.Id">@item.Event.Name</a></td>
                        <td style="border-top-width:0;vertical-align:bottom;"><time class="text-muted small" datetime="@item.DateTime.ToString("yyyy-MM-ddTH:mm:ssK")">@item.DateTime.ToString("yyyy/MM/dd HH:mm")</time></td>
                    </tr>
                Next
            </tbody>
        </table>
    End If
</div>


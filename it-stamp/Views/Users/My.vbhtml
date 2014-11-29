@ModelType ApplicationUser
@Imports Microsoft.AspNet.Identity
@Code

    Layout = "_UserLayout.vbhtml"
    ViewBag.Title = Model.FriendlyName

End Code

<h2>開催予定のIT勉強会 <small>フォローしているIT勉強会とコミュニティ</small></h2>
<div>
    <p class="text-muted">（未実装）</p>
</div>

<h2>✅ チェックイン</h2>
<div>
    @If Model.CheckIns.Count = 0 Then
        @<p class="text-muted">まだチェックインしていません。</p>
    Else
        @<table class="table">
            <tbody>
                @For Each item In Model.CheckIns
                Dim src = If(item.Event.Community IsNot Nothing AndAlso item.Event.Community.IconPath <> "", Href("/Uploads/" & item.Event.Community.IconPath), "http://placehold.it/96x96")
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
<h2>フォロー コミュニティ</h2>
<div>
    @If Model.Communities.Count = 0 Then
        @<p class="text-muted">フォローしているコミュニティはありません。</p>
    Else
        @<table class="table">
            <tbody>
                @For Each item In Model.Communities
                Dim src = If(item.IconPath <> "", Href("/Uploads/" & item.IconPath), "http://placehold.it/96x96")
                    @<tr>
                        <td style="border-top-width:0;width:32px;"><a href="@Href("/Communities/")@item.Id"><img class="img-rounded icon24" src="@src" /></a></td>
                        <td style="border-top-width:0;vertical-align:bottom;"><a href="@Href("/Communities/")@item.Id">@item.Name</a></td>
                    </tr>
                Next
            </tbody>
        </table>
    End If
</div>

<h2>フォロー IT勉強会</h2>
<div>
    @If Model.Favorites.Count = 0 Then
        @<p class="text-muted">フォローしているIT勉強会はありません。</p>
    Else
        @<table class="table">
            <tbody>
                @For Each item In Model.Favorites
                Dim src = If(item.Event.Community IsNot Nothing AndAlso item.Event.Community.IconPath <> "", Href("/Uploads/" & item.Event.Community.IconPath), "http://placehold.it/96x96")
                    @<tr>
                        <td style="border-top-width:0;width:32px;"><a href="@Href("/Communities/")@item.Id"><img class="img-rounded icon24" src="@src" /></a></td>
                        <td style="border-top-width:0;vertical-align:bottom;"><time class="text-muted small">@item.Event.FriendlyDateTime</time></td>
                        <td style="border-top-width:0;vertical-align:bottom;"><a href="@Href("/Communities/")@item.Id">@item.Event.Name</a></td>
                    </tr>
                Next
            </tbody>
        </table>
    End If
</div>

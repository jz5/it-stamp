@ModelType ApplicationUser
@Imports Microsoft.AspNet.Identity
@Code

    Layout = "_UserLayout.vbhtml"
    ViewBag.Title = Model.DisplayName & "さん"

End Code

<ul class="nav nav-tabs nav-justified myapge-tabs">
    <li role="presentation"><a href="@Href("~/Users/" & Model.UserName & "/My")">ホーム</a></li>
    <li role="presentation" class="active"><a href="@Href("~/Users/" & Model.UserName & "/MyFollowing")">フォロー</a></li>
    <li role="presentation"><a href="@Href("~/Users/" & Model.UserName & "/Manage")">管理</a></li>
</ul>

<h2>コミュニティ <span class="badge badge-primary @(If(Model.Communities.Count = 0, "hidden", ""))">@Model.Communities.Count</span></h2>
<div>
    @If Model.Communities.Count = 0 Then
        @<p class="text-muted">フォローしているコミュニティはありません。</p>
    Else
        @<table class="table">
            <tbody>
                @For Each item In Model.Communities
                    @<tr>
                        <td style="width:32px;"><a href="@Href("/Communities/")@item.Id"><img class="img-rounded icon24" src="@Href(item.GetIconPath)" /></a></td>
                        <td><a href="@Href("/Communities/")@item.Id">@item.Name</a></td>
                    </tr>
                Next
            </tbody>
        </table>
    End If
</div>

<h2>IT勉強会 <span class="badge badge-primary @(If(Model.Favorites.Count = 0, "hidden", ""))">@Model.Favorites.Count</span></h2>
<div>
    @If Model.Favorites.Count = 0 Then
        @<p class="text-muted">フォローしているIT勉強会はありません。</p>
    Else
        @<table class="table">
            <tbody>
                @For Each item In Model.Favorites
                    @<tr>
                        <td style="width:32px;"><a href="@Href("/Events/")@item.Event.Id"><img class="img-rounded icon24" src="@Href(item.Event.GetIconPath)" /></a></td>
                        <td style="width:35%;"><time class="text-muted small">@item.Event.FriendlyDateTime</time></td>
                        <td><a href="@Href("/Events/")@item.Event.Id">@item.Event.Name</a></td>
                    </tr>
                Next
            </tbody>
        </table>
    End If
</div>

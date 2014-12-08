@ModelType ApplicationUser
@Imports Microsoft.AspNet.Identity
@Code

    Layout = "_UserLayout.vbhtml"
    ViewBag.Title = Model.DisplayName & "さん"

End Code
<ul class="nav nav-tabs nav-justified myapge-tabs">
    <li role="presentation"><a href="@Href("~/Users/" & Model.UserName & "/My")">ホーム</a></li>
    <li role="presentation"><a href="@Href("~/Users/" & Model.UserName & "/MyFollowing")">フォロー</a></li>
    <li role="presentation" class="active"><a href="@Href("~/Users/" & Model.UserName & "/Manage")">管理</a></li>
</ul>
@If Model.OwnerCommunities.Any Then
    @<h2>管理者権限のあるコミュニティ <span class="badge badge-primary @(If(Model.OwnerCommunities.Count = 0, "hidden", ""))">@Model.OwnerCommunities.Count</span></h2>
    @<div>
        <table class="table">
            <tbody>
                @For Each item In Model.OwnerCommunities
                    @<tr>
                        <td style="width:32px;"><a href="@Href("/Communities/")@item.Id"><img class="img-rounded icon24" src="@Href(item.GetIconPath))" /></a></td>
                        <td><a href="@Href("/Communities/")@item.Id">@item.Name</a></td>
                    </tr>
                Next
            </tbody>
        </table>
    </div>
End If
<h2>作成したコミュニティ <span class="badge badge-primary @(If(ViewBag.Communities.Count = 0, "hidden", ""))">@ViewBag.Communities.Count</span></h2>
<div>
    @If ViewBag.Communities.Count = 0 Then
        @<p class="text-muted">作成したコミュニティはありません。</p>
    Else
        @<table class="table">
            <tbody>
                @For Each item As Community In ViewBag.Communities
                    @<tr>
                        <td style="width:32px;"><a href="@Href("/Communities/")@item.Id"><img class="img-rounded icon24" src="@Href(item.GetIconPath)" /></a></td>
                        <td><a href="@Href("/Communities/")@item.Id">@item.Name</a></td>
                    </tr>
                Next
            </tbody>
        </table>
    End If
</div>
<h2>作成したIT勉強会 <span class="badge badge-primary @(If(ViewBag.Events.Count = 0, "hidden", ""))">@ViewBag.Events.Count</span></h2>
<div>
    @If ViewBag.Events.Count = 0 Then
        @<p class="text-muted">作成したIT勉強会はありません。</p>
    Else
        @<table class="table">
            <tbody>
                @For Each item As [Event] In ViewBag.Events
                    @<tr>
                        <td style="width:32px;"><a href="@Href("/Events/")@item.Id"><img class="img-rounded icon24" src="@Href(item.GetIconPath)" /></a></td>
                        <td style="width:35%;"><time class="text-muted small">@item.FriendlyDateTime</time></td>
                        <td><a href="@Href("/Events/")@item.Id">@item.Name</a></td>
                    </tr>
                Next
            </tbody>
        </table>
    End If
</div>

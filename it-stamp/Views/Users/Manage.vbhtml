@ModelType ApplicationUser
@Imports Microsoft.AspNet.Identity
@Code

    Layout = "_UserLayout.vbhtml"
    ViewBag.Title = Model.FriendlyName

End Code

<ul class="nav nav-tabs nav-justified myapge-tabs">
    <li role="presentation"><a href="@Href("~/Users/" & Model.UserName & "/My")">ホーム</a></li>
    <li role="presentation"><a href="@Href("~/Users/" & Model.UserName & "/MyFollowing")">フォロー</a></li>
    <li role="presentation" class="active"><a href="@Href("~/Users/" & Model.UserName & "/Manage")">管理</a></li>
</ul>

@If Model.OwnerCommunities.Any Then
    @<h2>管理者権限のあるコミュニティ</h2>
    @<div>
        <table class="table">
            <tbody>
                @For Each item In Model.Communities
                        Dim src = If(item.IconPath <> "", Href("/Uploads/" & item.IconPath), Href("/Uploads/Icons/no-community.png"))
                    @<tr>
                        <td style="border-top-width:0;width:32px;"><a href="@Href("/Communities/")@item.Id"><img class="img-rounded icon24" src="@src" /></a></td>
                        <td style="border-top-width:0;vertical-align:bottom;"><a href="@Href("/Communities/")@item.Id">@item.Name</a></td>
                    </tr>
                Next
            </tbody>
        </table>
        End If
    </div>
End If

<h2>作成したコミュニティ</h2>
<div>
    @If ViewBag.Communities.Count = 0 Then
        @<p class="text-muted">作成したコミュニティはありません。</p>
    Else
        @<table class="table">
            <tbody>
                @For Each item In ViewBag.Communities
                Dim src = If(item.IconPath <> "", Href("/Uploads/" & item.IconPath), Href("/Uploads/Icons/no-community.png"))
                    @<tr>
                        <td style="border-top-width:0;width:32px;"><a href="@Href("/Communities/")@item.Id"><img class="img-rounded icon24" src="@src" /></a></td>
                        <td style="border-top-width:0;vertical-align:bottom;"><a href="@Href("/Communities/")@item.Id">@item.Name</a></td>
                    </tr>
                Next
            </tbody>
        </table>
    End If
</div>

<h2>作成したIT勉強会</h2>
<div>
    @If ViewBag.Events.Count = 0 Then
        @<p class="text-muted">フォローしているIT勉強会はありません。</p>
    Else
        @<table class="table">
            <tbody>
                @For Each item In ViewBag.Events
                Dim src = If(item.Community IsNot Nothing AndAlso item.Community.IconPath <> "", Href("/Uploads/" & item.Community.IconPath), Href("/Uploads/Icons/no-community.png"))
                    @<tr>
                        <td style="border-top-width:0;width:32px;"><a href="@Href("/Events/")@item.Id"><img class="img-rounded icon24" src="@src" /></a></td>
                        <td style="border-top-width:0;vertical-align:bottom;"><time class="text-muted small">@item.FriendlyDateTime</time></td>
                        <td style="border-top-width:0;vertical-align:bottom;"><a href="@Href("/Events/")@item.Id">@item.Name</a></td>
                    </tr>
                Next
            </tbody>
        </table>
    End If
</div>

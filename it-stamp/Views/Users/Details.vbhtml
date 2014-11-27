@ModelType ApplicationUser
@Imports Microsoft.AspNet.Identity
@Code

    ViewBag.Title = Model.FriendlyName

    Dim icon = If(Model.IconPath <> "", Href("/Uploads/" & Model.IconPath), "http://placehold.it/96x96")
End Code
@*@Html.Partial("_TopBanner")*@
<div class="row">
    <div class="col-md-8">
        <h1>@ViewBag.Title</h1>
        @If Model.IsPrivate Then
            @<div class="alert alert-info fade in" role="alert">
                 🚫 プライベートモードのユーザーです。
            </div>
        Else
            @<text>
                <div class="media">
                    @If Model.Url <> "" Then
                        @<a class="pull-left" href="@Model.Url" target="_blank">
                            <img class="media-object img-rounded" src="@icon" alt="@Model.FriendlyName">
                        </a>
                    Else
                        @<img class="pull-left media-object img-rounded" src="@icon" alt="@Model.FriendlyName">
                    End If
                    <div class="media-body">
                        <p>@Html.Raw(Html.Encode(Model.Description).Replace(vbCrLf, "<br />"))</p>
                        @If Model.Url <> "" Then
                            @<p><a href="@Model.Url" target="_blank">@Model.Url</a></p>
                        End If
                    </div>
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
                <h2>コミュニティ</h2>
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

                @Html.Partial("_SocialButtons")
            </text>

        End If

    </div>
    <div class="col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

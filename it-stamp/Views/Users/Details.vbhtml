@ModelType ApplicationUser
@Imports Microsoft.AspNet.Identity
@Code

    ViewBag.Title = Model.FriendlyName

    Dim icon = If(Model.IconPath <> "", Href("/Uploads/" & Model.IconPath), "http://placehold.it/96x96")
End Code
<div class="row">
    <div class="col-md-8">
        @If Model.IsRemoved Then
            @<div class="alert alert-info fade in" role="alert">
                🚫 退会したユーザーです。
            </div>
        Else
            @<h1>@ViewBag.Title</h1>
            @If Model.IsPrivate Then
                @<div class="alert alert-info fade in" role="alert">
                    🔒 プライベートモードのユーザーです。
                </div>
            Else
                @<text>
                    <div class="media">
                        <div class="pull-left">
                            @If Model.Url <> "" Then
                                @<a href="@Model.Url" target="_blank">
                                    <img class="media-object img-rounded" src="@icon" alt="@Model.FriendlyName">
                                </a>
                            Else
                                @<img class="media-object img-rounded" src="@icon" alt="@Model.FriendlyName">
                            End If

                            @If Request.IsAuthenticated AndAlso Not ViewBag.IsMe Then
                                @<div class="text-center">
                                    @Using Ajax.BeginForm("Follow", "Users", New With {.userName = Model.UserName}, New AjaxOptions With {.HttpMethod = "POST", .OnSuccess = "onSuccess", .OnBegin = "onBegin"}, New With {.class = "form-horizontal", .role = "form"})
                                        @Html.AntiForgeryToken()

                                        @<div class="form-group">
                                            <div class="form-inline">
                                                <input id="follow-btn" type="submit" value="@(If(ViewBag.Followed, "フォロー中", "フォロー"))" class="btn btn-default" style="min-width:96px;width:96px;font-size:14px;" />
                                            </div>
                                        </div>
                                    End Using
                                </div>
                            End If
                        </div>
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

                    @Html.Partial("_SocialButtons")
                </text>

            End If
        End If
    </div>
    <div class="col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>
@section Styles
    <link href="@Href("~/Content/animate.css")" rel="stylesheet" />
End Section
@section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    <script>
        var followed = @(If(ViewBag.Followed, "true", "false"));
        function onSuccess(result) {
            if (result) {
                followed = result.followed;
                $("#follow-btn")
                    .val(result.followed ? "フォロー中" : "フォロー")
                    .addClass("animated bounceIn")
                    .one("webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend", function() {
                        $(this).removeClass("animated bounceIn");
                    });
            }
        }

        function onBegin() {
        }

        (function ($) {
            $("#follow-btn").hover(function () {
                if (followed) {
                    $(this).val("解除").removeClass("btn-default").addClass("btn-primary");
                } else {
                    $(this).removeClass("btn-default").addClass("btn-primary");
                }
            },function () {
                if (followed) {
                    $(this).val("フォロー中").removeClass("btn-primary").addClass("btn-default");
                } else {
                    $(this).removeClass("btn-primary").addClass("btn-default");
                }
            });
        })(jQuery);
    </script>
End Section
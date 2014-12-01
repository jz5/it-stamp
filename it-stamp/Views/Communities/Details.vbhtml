@ModelType Community
@Imports Microsoft.AspNet.Identity
@Code

    Dim userIcon = Href("/Uploads/Icons/anon.png")
    ViewBag.Title = Model.Name
    ViewData("Details") = True

End Code
@section head
    <meta name="twitter:card" content="summary" />
    <meta name="twitter:site" content="@Html.Raw("@itstamp")" />
    <meta name="twitter:title" content="@ViewBag.Title" />
    <meta name="twitter:description" content="@(If(Model.Description <> "", Model.Description.Excerpt(200), "IT勉強会スタンプは、IT勉強会の参加を記録できるWebサービスです。"))" />
End Section
<div class="row">
    <div class="col-md-8">

        <h1>@ViewBag.Title</h1>

        @If ViewBag.StatusMessage <> "" AndAlso Request.IsAuthenticated Then
            @<div class="alert alert-success fade in" role="alert">
                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                @ViewBag.StatusMessage
            </div>
        End If

        @Html.Partial("_CommunityCard", Model, ViewData)

        <h2>📢 開催予定のIT勉強会 <span class="badge badge-primary @(If(ViewBag.FutureEvents Is Nothing OrElse ViewBag.FutureEvents.Count = 0, "hidden", ""))">@ViewBag.FutureEvents.Count</span></h2>
        @If ViewBag.FutureEvents Is Nothing OrElse ViewBag.FutureEvents.Count = 0 Then
            @<p class="text-muted">開催予定のIT勉強会はまだありません。</p>
        Else
            @<table class="table">
                <tbody>
                    @For Each ev In ViewBag.FutureEvents
                        @<tr>
                            <td style="width:35%;"><time class="text-muted small">@ev.FriendlyDateTime</time></td>
                            <td><a href="@Href("~/Events/" & ev.Id)">@ev.Name</a></td>
                        </tr>
                    Next
                </tbody>
            </table>
        End If

        <h2>📢 開催したIT勉強会 <span class="badge badge-primary @(If(ViewBag.PastEvents Is Nothing OrElse ViewBag.PastEvents.Count = 0, "hidden", ""))">@ViewBag.PastEvents.Count</span></h2>
        @If ViewBag.PastEvents Is Nothing OrElse ViewBag.PastEvents.Count = 0 Then
            @<p class="text-muted">開催したIT勉強会はまだありません。</p>
        Else
            @<table class="table">
                <tbody>
                    @For Each ev In ViewBag.PastEvents
                        @<tr>
                            <td style="width:35%;"><time class="text-muted small">@ev.FriendlyDateTime</time></td>
                            <td><a href="@Href("~/Events/" & ev.Id)">@ev.Name</a></td>
                        </tr>
                    Next
                </tbody>
            </table>
        End If

        <h2>フォロワー <span class="badge badge-primary @(If(Model.Members.Count = 0, "hidden", ""))">@Model.Members.Count</span></h2>
        <div>
            @If Model.Members.Count = 0 Then
                If Request.IsAuthenticated Then
                @<p class="text-muted">最初のフォロワーになりませんか？</p>
                Else
                @<p class="text-muted">ログインして最初のフォロワーになりませんか？</p>
                End If
            Else
                @For Each m In Model.Members.Where(Function(u) Not u.IsPrivate)
                    @<a href="@Href("~/Users/" & m.UserName)"><img src="@(If(M.IconPath <> "", Href("/Uploads/" & m.IconPath), "http://placehold.it/16x16"))" class="img-rounded icon24" alt="" title="@m.FriendlyName" /></a>
                Next

                If Model.Members.Any(Function(u) u.IsPrivate) Then
                @<img src="@userIcon" class="img-rounded icon24" alt="" title="プライベートユーザー（ひとり以上）" />
                End If

            End If
        </div>

        @If ViewBag.StatusMessage = "" Then
            @Html.Partial("_SocialButtons")
        End If

        <aside class="edit-menu-bar">
            @If ViewBag.CanEdit Then
                @<a href="@Url.Action("Edit", "Communities", New With {.id = Model.Id})">✏ 編集</a>
            ElseIf Model.IsLocked Then
                @<span>🔒 編集が制限されています。</span>
            ElseIf Not Request.IsAuthenticated Then
                @<span>✏ <a href="@Url.Action("Edit", "Communities", New With {.id = Model.Id})">ログイン</a>して編集しませんか？</span>
            End If
        </aside>

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
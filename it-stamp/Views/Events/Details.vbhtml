@ModelType [Event]
@Imports Microsoft.AspNet.Identity
@code
    ViewBag.Title = Model.Name

    Dim icon = If(Model.Community IsNot Nothing AndAlso Model.Community.IconPath <> "", "/Uploads/" & Model.Community.IconPath, "http://placehold.it/96x96")
    Dim userIcon = Href("/Uploads/Icons/anon.png")

    Dim searchAddress As String = Nothing
    If Model.IsOnline Then
        'オンライン
        searchAddress = Nothing
    ElseIf Model.Address IsNot Nothing Then
        '会場（住所）
        searchAddress = If(Model.Prefecture.Id < 49, Model.Prefecture.Name, "") & Model.Address
    ElseIf Model.Address Is Nothing Then
        searchAddress = Model.Place
    End If

    Dim privateCommentCount = (From item In Model.Comments Where item.CreatedBy.IsPrivate).Count

End Code
<div class="row">
    <div class="col-md-8">
        <h1>@ViewBag.Title</h1>
        @If ViewBag.StatusMessage <> "" AndAlso Request.IsAuthenticated Then
            @<div class="alert alert-success fade in" role="alert">
                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                @ViewBag.StatusMessage
            </div>
        End If
        @If Model.IsCanceled Then
            @<div class="alert alert-warning fade in" role="alert">
                中止になったIT勉強会です。
            </div>
        ElseIf Model.EndDateTime < Now.Date Then
            @<div class="alert alert-warning fade in" role="alert">
                終了したIT勉強会です。
            </div>
        End If
        @If Model.IsHidden Then
            @<div class="alert alert-warning fade in" role="alert">
                IT勉強会一覧に表示されないIT勉強会です。このステータスのIT勉強会にはチェックインできません。
            </div>
        End If
        <div class="media">
            <div class="pull-left">
                @If Model.Url <> "" Then
                    @<a href="@Model.Url">
                        <img class="media-object img-rounded" src="@icon" alt="@Model.Name">
                    </a>
                Else
                    @<img class="media-object img-rounded" src="@icon" alt="@Model.Name">
                End If
                @If Request.IsAuthenticated AndAlso Not Model.IsHidden Then
                    @<div class="text-center">
                        @Using Ajax.BeginForm("Follow", "Events", New With {.id = Model.Id}, New AjaxOptions With {.HttpMethod = "POST", .OnSuccess = "onFollowSuccess", .OnBegin = "onFollowBegin"}, New With {.class = "form-horizontal", .role = "form"})
                            @Html.AntiForgeryToken()
                            @Html.HiddenFor(Function(m) m.Id)
                            @Html.HiddenFor(Function(m) m.Name)

                            @<div class="form-group">
                                <div class="form-inline">
                                    <input id="follow-btn" type="submit" value="@(if(ViewBag.Followd,"フォロー中","フォロー"))" class="btn btn-default" style="min-width:auto;width:96px;font-size:14px;" />
                                </div>
                            </div>
                        End Using
                    </div>
                End If
            </div>
            <div class="media-body">
                <p>@Html.Raw(Html.Encode(Model.Description).Replace(vbCrLf, "<br />"))</p>
            </div>
        </div>
        <div class="row">
            <div id="details" class="col-md-12">
                <table class="table">
                    <tbody>
                        <tr>
                            <td style="border-top-width:0;">📅 日時</td>
                            <td style="border-top-width:0;">
                                @Model.FriendlyDateTime
                                @If Model.StartDateTime.Date <= Now.Date AndAlso Now.Date <= Model.EndDateTime.Date Then
                                    @<span class="text-primary small">（今日）</span>
                                ElseIf Model.StartDateTime.Date = Now.Date.AddDays(1) OrElse Model.EndDateTime.Date = Now.Date.AddDays(1) Then
                                    @<span class="text-muted small">（明日）</span>
                                ElseIf Model.EndDateTime.Date.AddDays(1) = Now.Date Then
                                    @<span class="text-muted small">（昨日）</span>
                                End If
                            </td>
                        </tr>
                        <tr>
                            <td>📍 会場</td>
                            @If Model.IsOnline Then
                                'オンライン
                                @<td>@Model.Prefecture.Name</td>
                            ElseIf Model.Address IsNot Nothing Then
                                '会場（住所）
                                @<td>@Model.Place（@(If(Model.Prefecture.Id < 49, Model.Prefecture.Name, ""))@Model.Address）</td>
                            ElseIf Model.Address Is Nothing Then
                                If Model.Place IsNot Nothing Then
                                @<td>@Model.Place</td>
                                Else
                                @<td><span class="text-muted">未登録</span></td>
                                End If
                            End If
                        </tr>
                        <tr>
                            <td>🌏 Webサイト</td>
                            @If Model.Url Is Nothing Then
                                @<td><span class="text-muted">未登録</span></td>
                            Else
                                @<td><a href="@Model.Url" target="_blank">@Model.Url</a></td>
                            End If
                        </tr>
                        <tr>
                            <td>コミュニティ</td>
                            @If Model.Community Is Nothing Then
                                @<td><span class="text-muted">未登録</span></td>
                            Else
                                @<td>@Html.ActionLink(Model.Community.Name, "Details", "Communities", New With {.id = Model.Community.Id}, Nothing)</td>
                            End If
                        </tr>
                        @If Model.SpecialEvents IsNot Nothing Then
                            @<tr>
                                <td colspan="2"><span>⭐</span> @Html.ActionLink(Model.SpecialEvents.Name, Model.SpecialEvents.Id.ToString, "SpecialEvents")対象のIT勉強会です。</td>
                            </tr>
                        End If
                        @If Model.IsReported Then
                            @<tr>
                                <td>参加人数（オフライン）</td>
                                <td>@(Model.ParticipantsOfflineCount.ToString & "名")</td>
                            </tr>
                            @<tr>
                                <td>参加人数（オンライン）</td>
                                <td>@(Model.ParticipantsOnlineCount.ToString & "名")</td>
                            </tr>
                            @<tr>
                                <td>備考</td>
                                <td>@Html.Raw(Html.Encode(Model.ReportMemo).Replace(vbCrLf, "<br />"))</td>
                            </tr>
                        End If
                    </tbody>
                </table>
            </div>
            @If searchAddress <> "" Then
                @<div id="map-col" class="col-md-4" style="display:none;">
                    <div id="map" style="width: 100%; height: 150px"></div>
                </div>
            End If
        </div>
        <h2>✅ チェックイン <span class="badge badge-primary @(If(Model.CheckIns.Count = 0, "hidden", ""))">@Model.CheckIns.Count</span></h2>
        <div style="margin-bottom: 20px;">
            @If Model.CheckIns.Count = 0 Then

            Else
                @For Each m In Model.CheckIns.Where(Function(c) Not c.User.IsPrivate).Select(Function(c) c.User)
                    @<a href="@Href("~/Users/" & m.UserName)"><img src="@(If(M.IconPath <> "", Href("/Uploads/" & m.IconPath), userIcon))" class="img-rounded icon24" alt="" title="@m.FriendlyName" /></a>
                Next
                                                                                                                                                                                                                                                                                                                                                                                                                If Model.CheckIns.Where(Function(c) c.User.IsPrivate).Count > 0 Then
                @<img src="@userIcon" class="img-rounded icon24" alt="" title="プライベートユーザー（ひとり以上）" />
                End If
            End If
        </div>
        @Using Ajax.BeginForm("CheckIn", "Events", New With {.id = Model.Id}, New AjaxOptions() With {.HttpMethod = "POST", .OnSuccess = "onCheckInSuccess", .OnBegin = "onCheckInBegin"}, New With {.class = "form-horizontal", .id = "checkin-form", .role = "form"})
            @Html.AntiForgeryToken()
            @Html.Hidden("Event.Id", Model.Id)
            @Html.Hidden("Event.Name", Model.Name)

            @If ViewBag.CheckIned Then
                @<p>チェックイン済み</p>

            ElseIf Model.IsCanceled OrElse Model.IsHidden Then
                @<p>チェックインできません。</p>

            ElseIf Not ViewBag.CanChackIn Then
                @<p>このIT勉強会にはチェックインできません。</p>

            ElseIf Model.StartDateTime.AddHours(-1) <= Now Then
                If Not Request.IsAuthenticated Then
                @<p>@Html.ActionLink("ログイン", "Login", "Account", New With {.ReturnUrl = If(Request.RawUrl.ToLower.Contains("login"), "", Request.RawUrl)}, Nothing) してチェックイン！</p>
                Else

                @<div class="form-group">
                    <div class="form-inline">
                        <button type="button" class="btn btn-default" data-toggle="modal" data-target="#confirm-modal">チェックイン</button>
                    </div>
                </div>
                    'Modal Window
                @<div class="modal fade" id="confirm-modal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                <h4 class="modal-title" id="modalLabel">チェックイン</h4>
                            </div>
                            <div class="modal-body">
                                @If ViewBag.IsPrivateUser Then
                                    @<p>💡 プライベートモードを「OFF」にするとチェックインをシェアできます。</p>
                                Else
                                    @Html.TextArea("AdditionalMessage", Model.Name + "にチェックイン！", New With {.maxlength = 256, .class = "form-control", .style = "max-width:none;"})
                                    @<ul class="list-unstyled">
                                        @If False Then@*TODO コメント実装*@
                                        @<li>
                                            <div class="checkbox">
                                                @Html.CheckBox("PostComment")
                                                @Html.Label("コメントを投稿", New With {.for = "PostComment"})
                                            </div>
                                        </li>
                                        End If
                                        @If ViewBag.ShareTwitter Then
                                            @<li>
                                                <div class="checkbox">
                                                    @Html.CheckBox("ShareTwitter")
                                                    @Html.Label("Twitterへシェア", New With {.for = "ShareTwitter"})
                                                </div>
                                            </li>
                                        End If
                                        @If ViewBag.ShareFacebook Then
                                            @<li>
                                                <div class="checkbox">
                                                    @Html.CheckBox("ShareFacebook")
                                                    @Html.Label("Facebookへシェア", New With {.for = "ShareFacebook"})
                                                </div>
                                            </li>
                                        End If
                                    </ul>
                                End If
                            </div>
                            <div class="modal-footer">
                                <input type="submit" value="チェックイン" class="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>

                End If
            Else
                @<p>開始時間の1時間前からチェックインできます。</p>
            End If
        End Using
        <h2>💬 コメント</h2>
        @If Model.Comments IsNot Nothing AndAlso Model.Comments.Count > 0 Then
            @<div class="comment-container">
                @For Each item As Comment In Model.Comments
                If item.CreatedBy.IsPrivate Then
                    Continue For
                End If
                    @<div class="comment">
                        <div class="comment-time">
                            <time datetime="@item.CreationDateTime.ToString("yyyy-MM-ddTH:mm:ssK")">@(item.CreationDateTime.ToRelativeTimeString())前</time>
                        </div>
                        <div class="comment-body">
                            <p>
                                <a href="@Href("/Users/" & item.CreatedBy.UserName)">
                                    <img src="@(If(item.CreatedBy.IconPath <> "", Href("/Uploads/" & item.CreatedBy.IconPath), "http://placehold.it/96x96"))">
                                </a>
                                @item.Content
                            </p>
                        </div>
                    </div>
                Next
            </div>
            @If privateCommentCount > 0 Then
                @<p>他、@(privateCommentCount)人のプライベートユーザー</p>
            End If
        Else
            @<p>コメントは投稿されていません</p>
        End If
        @If Model.Community IsNot Nothing Then
            @<h2>このコミュニティのその他のIT勉強会</h2>
            @<p>（未実装）</p>
        End If

        @If ViewBag.StatusMessage = "" Then
            @Html.Partial("_SocialButtons")
        End If

        <aside class="edit-menu-bar">
            @If ViewBag.CanEdit Then
                @<a href="@Url.Action("Edit", "Events" , new with {.id=Model.Id})">✏ 編集</a>
            ElseIf Model.IsLocked Then
                @<span>🔒 編集が制限されています。</span>
            ElseIf Not Request.IsAuthenticated Then
                @<span>✏ <a href="@Url.Action("Edit", "Events" , new with {.id=Model.Id})">ログイン</a>して編集しませんか？</span>
            End If
        </aside>
    </div>
    <div class="col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

@section Styles
    @Styles.Render("~/Content/skins/square/blue.css")
    @Styles.Render("~/Content/animate.css")
    <style>
        .comment-time time {
            color: gray;
            font-size: 0.8em;
        }

        .comment img {
            width: 30px;
            height: 30px;
        }
    </style>

End Section
@section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    <script>
        function onCheckInSuccess(result) {
            if (result && $) {
                if ($('body').hasClass('modal-open')) {
                    $('#confirm-modal').modal('hide');
                    $('body').removeClass('modal-open');
                    $('.modal-backdrop').remove();
                }

                $("#checkin-form")
                    .empty()
                    .append($("<p>チェックイン済み</p>")
                        .addClass("animated fadeIn")
                        .one("webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend", function () {
                            $(this).removeClass("animated fadeIn");
                        }));
            }
        }

        function onCheckInBegin() {
        }

        var followed = @(If(ViewBag.Followd, "true", "false"));
        function onFollowSuccess(result) {
            if (result && $) {
                followed = result.followed;
                $("#follow-btn")
                    .val(result.followed ? "フォロー中" : "フォロー")
                    .addClass("animated bounceIn")
                    .one("webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend", function() {
                        $(this).removeClass("animated bounceIn");
                    });
            }
        }

        function onFollowBegin() {
        }

        (function ($) {
            $('input').iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-blue'
            });

            $("#follow-btn").hover(function () {
                if (followed) {
                    $(this).val("解除").removeClass("btn-default").addClass("btn-primary");
                } else {
                    $(this).removeClass("btn-default").addClass("btn-primary");
                }
            }, function () {
                if (followed) {
                    $(this).val("フォロー中").removeClass("btn-primary").addClass("btn-default");
                } else {
                    $(this).removeClass("btn-primary").addClass("btn-default");
                }
            });
        }(jQuery));
    </script>
    @If searchAddress <> "" Then
        @<script src='//maps.google.com/maps/api/js?key=AIzaSyAXIgOzni2RmLCFYZH3G6ubKRExWYwd338&sensor=false&language=ja'></script>
        @<script>
            (function ($) {
                var geocoder = new google.maps.Geocoder();
                geocoder.geocode({ "address": "@searchAddress" }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK && results.length > 0) {
                        $("#details").attr("class", "col-md-8");
                        $("#map-col").show();

                        var map = new google.maps.Map(document.getElementById("map"), {
                            center: new google.maps.LatLng(35.658, 139.745),
                            zoom: 16,
                            mapTypeId: google.maps.MapTypeId.ROADMAP,
                            scaleControl: true
                        });

                        map.setCenter(results[0].geometry.location);

                        var marker = new google.maps.Marker({
                            position: results[0].geometry.location,
                            animation: google.maps.Animation.DROP
                        });
                        marker.setMap(map);
                    }
                });

            }(jQuery));
        </script>
    End If
End Section
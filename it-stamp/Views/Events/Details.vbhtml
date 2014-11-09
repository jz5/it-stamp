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
    
    Dim twitterCheckBoxOption = Nothing
    Dim facebookCheckBoxOption = Nothing
    
    If Not ViewBag.AssosiatedTwitter Then
        twitterCheckBoxOption = New With {.disabled = "disabled"}
    End If
    
    If Not ViewBag.AssosiatedFacebook Then
        facebookCheckBoxOption = New With {.disabled = "disabled"}
    End If
    
End Code

@Html.Partial("_TopBanner")

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
                キャンセルされたIT勉強会です。
            </div>
        End If

        @If Model.IsHidden Then
            @<div class="alert alert-warning fade in" role="alert">
                IT勉強会一覧に表示されないIT勉強会です。このステータスのIT勉強会にはチェックインできません。
            </div>
        End If

        <div class="media">
            @If Model.Url <> "" Then
                @<a class="pull-left" href="@Model.Url">
                    <img class="media-object img-rounded" src="@icon" alt="@Model.Name">
                </a>
            Else
                @<img class="pull-left media-object img-rounded" src="@icon" alt="@Model.Name">
            End If
            <div class="media-body">
                <p>@Html.Raw(Html.Encode(Model.Description).Replace(vbCrLf, "<br />"))</p>
            </div>
        </div>


        <div class="row">
            <div id="details" class="@(If(searchAddress <> "", "col-md-8", "col-md-12"))">
                <table class="table">
                    <tbody>
                        <tr>
                            <td style="border-top-width:0;"><i class="glyphicon glyphicon-calendar"></i> 日時</td>
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
                            <td><i class="glyphicon glyphicon-map-marker"></i> 会場</td>
                            @If Model.IsOnline Then
                                'オンライン
                                @<td>@Model.Prefecture.Name</td>
                            ElseIf Model.Address IsNot Nothing Then
                                '会場（住所）
                                @<td>@Model.Place（@(If(Model.Prefecture.Id < 49, Model.Prefecture.Name, ""))@Model.Address）</td>
                            ElseIf Model.Address Is Nothing Then
                                @<td>@Model.Place</td>
                            End If
                        </tr>
                        <tr>
                            <td><i class="glyphicon glyphicon-globe"></i> Webサイト</td>
                            @If Model.Url Is Nothing Then
                                @<td></td>
                            Else
                                @<td><a href="@Model.Url" target="_blank">@Model.Url</a></td>
                            End If
                        </tr>
                        <tr>
                            <td>コミュニティ</td>
                            @If Model.Community Is Nothing Then
                                @<td></td>
                            Else
                                @<td>@Html.ActionLink(Model.Community.Name, "Details", "Communities", New With {.id = Model.Id}, Nothing)</td>
                            End If
                        </tr>
                        @If Model.SpecialEvents IsNot Nothing Then
                            @<tr>
                                <td colspan="2">@Html.ActionLink(Model.SpecialEvents.Name, "Details", "SpecialEvents") 対象のIT勉強会です。</td>
                            </tr>
                        End If
                    </tbody>

                </table>
            </div>
            @If searchAddress <> "" Then
                @<div id="map-col" class="col-md-4">
                    <div id="map" style="width: 100%; height: 150px"></div>
                </div>
            End If
        </div>

        <h2><i class="glyphicon glyphicon-ok"></i> チェックイン <span class="badge badge-primary @(If(Model.CheckIns.Count = 0, "hidden", ""))">@Model.CheckIns.Count</span></h2>
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


        @Using Ajax.BeginForm("CheckIn", "Events", New With {.id = Model.Id}, New AjaxOptions() With {.HttpMethod = "POST", .OnSuccess = "onSuccess", .OnBegin = "onBegin"}, New With {.class = "form-horizontal", .id = "checkin-form", .role = "form"})
            @Html.AntiForgeryToken()
            @Html.Hidden("Event.Id", Model.Id)
            @Html.Hidden("Event.Name", Model.Name)
            @Html.Hidden("QuickCheckInEnabled", "True")

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
                        <input type="submit" value="クイック チェックイン" class="btn btn-primary" />
                        <button type="button" class="btn btn-default" data-toggle="modal" data-target="#confirm-modal">チェックイン</button>
                        @*@Html.ActionLink("チェックイン", "CheckIn", "Events", New With {.id = Model.Id}, New With {.class = "btn btn-default"})*@
                    </div>
                </div>
                @<!-- Modal Window -->
                @<div class="modal fade" id="confirm-modal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                <h4 class="modal-title" id="myModalLabel">チェックイン</h4>
                            </div>
                            <div class="modal-body">
                                @If ViewBag.IsPrivateUser Then
                                    @<p>チェックインしますか？<br />（共有コメントの投稿にはプライベートモードを解除する必要があります）</p>
                                Else
                                    @Html.TextArea("AdditionalMessage", Model.Name + "にチェックイン！ #itstamp", New With {.maxlength = 1023})
                                    @<ul class="share-options">
                                        <li>
                                            @Html.CheckBox("ShareTwitter", CBool(ViewBag.ShareTwitter AndAlso ViewBag.AssosiatedTwitter), twitterCheckBoxOption)
                                            @If ViewBag.AssosiatedTwitter Then
                                                @Html.Label("Twitterへシェア")
                                            Else
                                                @Html.Label("Twitterへシェア（未設定）")
                                            End If
                                        </li>
                                        <li>
                                            @Html.CheckBox("ShareFacebook", CBool(ViewBag.ShareFacebook AndAlso ViewBag.AssosiatedFacebook), facebookCheckBoxOption)
                                            @If ViewBag.AssosiatedFacebook Then
                                                @Html.Label("Facebookへシェア")
                                            Else
                                                @Html.Label("Facebookへシェア（未設定）")
                                            End If
                                        </li>
                                    </ul>
                                End If
                             </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-default" data-dismiss="modal">キャンセル</button>
                                <input type="submit" value="チェックイン" class="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>

                End If
            Else
                @<p>開始時間の1時間前からチェックインできるようになります。</p>
            End If
        End Using

       


        @*<div class="modal fade">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                        <h4 class="modal-title"><i class="glyphicon glyphicon-ok"></i> チェックイン</h4>
                    </div>
                    <div class="modal-body">
                        <p>One fine body&hellip;</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary">チェックイン</button>
                    </div>
                </div>
            </div>
        </div>*@

        <h2><i class="glyphicon glyphicon-comment"></i> コメント</h2>
        <p>（未実装）</p>


        <h2>このコミュニティのIT勉強会</h2>
        <p>（未実装）</p>

        @Html.Partial("_SocialButtons")

        @If ViewBag.CanEdit Then
            @<a href="@Url.Action("Edit", "Events", New With {.id = Model.Id})"><i class="glyphicon glyphicon-pencil"></i> 編集</a>
        Else
            @<i class="glyphicon glyphicon-pencil" title="編集権限がありません"></i>
        End If

    </div>
    <div class="col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>






@section Styles
<link href="@Href("~/Content/animate.css")" rel="stylesheet" />
    <style>
        #AdditionalMessage {
            margin: 0 auto;
            max-width: none;
            min-height: 5em;
            width: 100%;
        }

        .share-options>li {
            list-style: none;
        }


    </style>
End Section
@section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    <script>
        @*var followed = @(If(ViewBag.Followd, "true", "false"));*@
        function onSuccess(result) {
            if (result) {
                if ($('body').hasClass('modal-open')) {
                    $('#confirm-modal').modal('hide');
                    $('body').removeClass('modal-open');
                    $('.modal-backdrop').remove();
                }

                $("#checkin-form")
                    .empty()
                    .append($("<p>チェックイン済み</p>")
                        .addClass("animated bounceIn")
                        .one("webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend", function () {
                            $(this).removeClass("animated bounceIn");
                        }));
            }


            //if (result) {
            //    followed = result.followed;
            //    $("#follow-btn")
            //        .val(result.followed ? "フォロー中" : "フォロー")
            //        .addClass("animated bounceIn")
            //        .one("webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend", function() {
            //            $(this).removeClass("animated bounceIn");
            //        });
            //}
        }

        function onBegin() {
        }

        (function ($) {
            //$("#follow-btn").hover(function () {
            //    if (followed) {
            //        $(this).val("解除").removeClass("btn-default").addClass("btn-primary");
            //    } else {
            //        $(this).removeClass("btn-default").addClass("btn-primary");
            //    }
            //},function () {
            //    if (followed) {
            //        $(this).val("フォロー中").removeClass("btn-primary").addClass("btn-default");
            //    } else {
            //        $(this).removeClass("btn-primary").addClass("btn-default");
            //    }
            //});
        })(jQuery);
    </script>

@If searchAddress <> "" Then
        @<script src='http://maps.google.com/maps/api/js?key=AIzaSyAXIgOzni2RmLCFYZH3G6ubKRExWYwd338&sensor=false&language=ja'></script>
        @<script>
            (function ($) {

                var map = new google.maps.Map(document.getElementById("map"), {
                    center: new google.maps.LatLng(35.658, 139.745),
                    zoom: 16,
                    mapTypeId: google.maps.MapTypeId.ROADMAP,
                    scaleControl: true
                });

                var geocoder = new google.maps.Geocoder();
                geocoder.geocode({ "address": "@searchAddress" }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK && results.length > 0) {
                        map.setCenter(results[0].geometry.location);

                        var marker = new google.maps.Marker({
                            position: results[0].geometry.location,
                            animation: google.maps.Animation.DROP()
                        });
                        marker.setMap(map);
                    } else {
                        $("#map-col").hide();
                        $("#details").attr("class", "col-md-12");
                    }
                });

            }(jQuery));
        </script>
    End If

End Section
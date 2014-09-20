<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@(If(ViewBag.Title = "", "IT勉強会スタンプ", ViewBag.Title & " | IT勉強会スタンプ"))</title>
    <link rel="stylesheet" href="https://netdna.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css">
    @Styles.Render("~/Content/css")
    @Scripts.Render("~/bundles/modernizr")
    @RenderSection("styles", required:=False)
    <meta name="description" content="IT勉強会スタンプは、IT勉強会の参加を記録できるWebサービスです。" />
</head>
<body>
    <div class="navbar navbar-default navbar-custom navbar-fixed-top">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a href="@Href("~/")"><img src="@Href("~/images/logo.png")" srcset="@Href("~/images/logo2x.png") 2x" class="navbar-brand" /></a>
            </div>
            <div class="navbar-collapse collapse">
                <ul class="nav navbar-nav">
                    <li>@Html.ActionLink("About", "About", "Home")</li>
                    <li>@Html.ActionLink("IT勉強会", "Index", "Events")</li>
                    <li>@Html.ActionLink("スタンプラリー", "Details", "SpecialEvents", New With {.id = 1}, Nothing)</li>

                    @If Request.IsAuthenticated AndAlso User.IsInRole("Admin") Then
                        @<li class="dropdown">
                            <a href="#" class="dropdown-toggle" data-toggle="dropdown">Admin <span class="caret"></span></a>
                            <ul class="dropdown-menu" role="menu">
                                <li>@Html.ActionLink("Roles", "Index", "RolesAdmin")</li>
                                <li>@Html.ActionLink("Users", "Index", "UsersAdmin")</li>
                                <li>@Html.ActionLink("Communities", "Index", "CommunitiesAdmin")</li>
                                <li>@Html.ActionLink("SpecialEvents", "Index", "SpecialEventsAdmin")</li>
                            </ul>
                        </li>
                    End If
                </ul>
                @Html.Partial("_LoginPartial")
            </div>
        </div>
    </div>
    <div class="container">
        @RenderBody()
    </div>
    <div class="footer">
        <div class="container">
            <div class="row">
                <div class="col-md-4">
                    <h3>IT勉強会スタンプ</h3>
                    <p>IT勉強会スタンプは、IT勉強会の参加を記録できるWebサービスです。</p>
                    <ul class="list-unstyled">
                        <li><a href="#">About</a></li>
                        <li>@Html.ActionLink("IT勉強会一覧", "Index", "Events")</li>
                        <li>@Html.ActionLink("コミュニティ一覧", "Index", "Communities")</li>
                        <li><a href="#">コミュニティの参加</a></li>
                        <li><a href="#">素材</a></li>
                        <li><a href="#">登録</a>・<a href="#">ログイン</a></li>
                        <li><a href="#">お問い合わせ</a></li>
                        <li><a href="#">利用規約・Privacy Policy</a></li>
                    </ul>
                </div>
                <div class="col-md-4">
                    <h3>IT勉強会スタンプラリー</h3>
                    <p>IT勉強会スタンプラリー開催中です。</p>
                    <ul class="list-unstyled">
                        <li><a href="#">IT勉強会スタンプラリー</a></li>
                        <li><a href="#">スポンサー</a></li>
                    </ul>
                </div>
                <div class="col-md-4">
                    <h3>IT勉強会スタンプラリー</h3>
                    <p>IT勉強会スタンプラリー開催中です。</p>
                    <ul class="list-unstyled">
                        <li><a href="#">IT勉強会スタンプラリー</a></li>
                        <li><a href="#">スポンサー</a></li>
                    </ul>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 small" style="margin: 20px 0 20px;">
                    © 2014 IT勉強会スタンプラリー運営委員会 ・ <a href="">GitHub</a>
                </div>
            </div>
        </div>
    </div>

    <script src="https://code.jquery.com/jquery-1.11.0.min.js"></script>
    <script src="https://netdna.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
    @Scripts.Render("~/bundles/script")
    <script>
        (function ($) {
            $(".banner-img").show();
            $(".bxslider").bxSlider({
                auto: true
            });

            $('input').iCheck({
                checkboxClass: 'icheckbox_flat',
                radioClass: 'iradio_flat'
            });

        }(jQuery));
    </script>
    @RenderSection("scripts", required:=False)
</body>
</html>

<!DOCTYPE html>
<html lang="ja">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@(If(ViewBag.Title = "", "IT勉強会スタンプ", ViewBag.Title & " | IT勉強会スタンプ"))</title>
    <link rel="stylesheet" href="https://netdna.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" />
    @Styles.Render("~/Content/css")
    @Scripts.Render("~/bundles/modernizr")
    @RenderSection("styles", required:=False)
    <meta name="description" content="IT勉強会スタンプは、IT勉強会の参加を記録できるWebサービスです。" />
</head>
<body>
    <div class="navbar navbar-default navbar-fixed-top">
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
                    <li>@Html.ActionLink("🔰 初めての方", "About", "Home")</li>
                    <li>@Html.ActionLink("IT勉強会", "Index", "Events")</li>
                    <li><a href="@Href("~/Stamprally/2015/")">スタンプラリー</a></li>
                   
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
                        <li>@Html.ActionLink("🔰 初めての方", "About", "Home")</li>
                        <li>@Html.ActionLink("IT勉強会一覧", "Index", "Events")・@Html.ActionLink("登録", "Add", "Events")</li>
                        <li>@Html.ActionLink("コミュニティ一覧", "Index", "Communities")・@Html.ActionLink("登録", "Add", "Communities")</li>
                        <li>@Html.ActionLink("アカウント登録", "Register", "Account")・@Html.ActionLink("ログイン", "Login", "Account")</li>
                        <li>@Html.ActionLink("Q & A", "QA", "Home")</li>
                        <li>@Html.ActionLink("お問い合わせ", "Contact", "Home")</li>
                        <li>@Html.ActionLink("利用規約・プライバシーポリシー", "TOS", "Home")</li>
                    </ul>
                </div>
                <div class="col-md-4">
                    <h3>IT勉強会スタンプラリー 2015</h3>
                    <p>IT勉強会スタンプラリーを開催中です。</p>
                    <ul class="list-unstyled">
                        <li><a href="@href("~/Stamprally/2015/")">IT勉強会スタンプラリー</a></li>
                        <li><a href="@Href("~/Stamprally/2015/Events")">IT勉強会一覧</a></li>
                        <li><a href="@Href("~/Stamprally/2015/Communities")">参加コミュニティ一覧</a></li>
                        <li><a href="@Href("~/Stamprally/2015/QA")">Q &amp; A</a></li>
                        <li><a href="@Href("~/Stamprally/2015/Sponsors")">スポンサー紹介</a></li>
                        <li><a href="@Href("~/Stamprally/2015/Join")">コミュニティの参加</a></li>
                        <li><a href="@Href("~/Stamprally/2015/Committee")">運営委員会</a></li>
                        <li><a href="@Href("~/Stamprally/2015/Resources")">素材</a></li>
                        <li><a href="@Href("~/Stamprally/2015/Contact")">お問い合わせ</a></li>
                    </ul>
                </div>
                <div class="col-md-4">
                    <a class="twitter-timeline" href="https://twitter.com/search?q=%23itstamp%20OR%20%22IT%E5%8B%89%E5%BC%B7%E4%BC%9A%E3%82%B9%E3%82%BF%E3%83%B3%E3%83%97%E3%83%A9%E3%83%AA%E3%83%BC%22" data-widget-id="535866409834659841">#itstamp OR "IT勉強会スタンプラリー"に関するツイート</a>
                    <script>!function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0], p = /^http:/.test(d.location) ? 'http' : 'https'; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = p + "://platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs); } }(document, "script", "twitter-wjs");</script>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 small" style="margin: 20px 0 20px;">
                    ©2014 IT勉強会スタンプ ©2014 IT勉強会スタンプラリー運営委員会
                </div>
            </div>
        </div>
    </div>

    @*<script src="https://code.jquery.com/jquery-1.11.0.min.js"></script>*@
    @Scripts.Render("~/bundles/jquery")
    <script src="https://netdna.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
    <script src="//twemoji.maxcdn.com/twemoji.min.js"></script>
    @Scripts.Render("~/bundles/script")
    <script>
    twemoji.parse(document.body, {
        callback: function (icon, options, variant) {
            switch (icon) {
                case 'a9':      // copyright
                case 'ae':      // trademark
                    return false;
            }
            return ''.concat(options.base, options.size, '/', icon, options.ext);
        }
    });
    </script>
    @RenderSection("scripts", required:=False)
</body>
</html>

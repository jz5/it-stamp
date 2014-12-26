@ModelType SearchEventsViewModel
@Imports Microsoft.AspNet.Identity
@Code
    ViewBag.Title = "IT勉強会一覧"
    Dim now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.ToUniversalTime(), "Tokyo Standard Time")

End Code

<div class="row">
    <div class="col-md-8">

        <h1>📢 @ViewBag.Title</h1>
        @If ViewBag.StatusMessage <> "" AndAlso Request.IsAuthenticated Then
            @<div class="alert alert-success fade in" role="alert">
                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>
                @ViewBag.StatusMessage
            </div>
        End If

        <div class="clearfix">
            <div class="pull-left">
                <span><small>@(Model.TotalCount)件@(If(Model.CurrentPage > 1, " " & Model.CurrentPage & "ページ目", ""))</small></span>
            </div>

            <div class="pull-right">
                <p>
                    <div class="dropdown">
                        <a data-toggle="dropdown" href="#">検索条件 <span class="caret"></span></a>
                        <ul class="dropdown-menu" role="menu">
                            <li>@Html.ActionLink("開催予定の勉強会", "Index", "Events", Nothing, Nothing)</li>
                            <li>@Html.ActionLink("開催予定の勉強会（スタンプラリー対象）", "Index", "Events", New With {.SpecialEvent = 1}, Nothing)</li>
                            <li>@Html.ActionLink("過去の勉強会", "Index", "Events", New With {.past = True}, Nothing)</li>
                        </ul>
                    </div>
                </p>
            </div>
        </div>

        @If Not Request.IsAuthenticated Then
            @<div class="jumbotron">
                <div class="jumbotron-contents">
                    <p><a href="@Href("~/Home/About/")">IT勉強会スタンプ</a>は、IT勉強会の参加を記録できるWebサービスです。</p>
                    <p>IT勉強会に参加してスタンプを集める “<a href="@Href("~/Stamprally/2015/")">IT勉強会スタンプラリー</a>” を開催中！　<a href="@Href("~/Events/?SpecialEvent=1")">対象のIT勉強会</a>を探してみよう！（※ 開催中のスタンプラリーは、台紙を使います。Webサービスの記録機能とは関連していません。）</p>

                    @Html.ActionLink("アカウント登録", "Register", "Account", Nothing, New With {.class = "btn btn-primary"})
                </div>
            </div>
        Else
            @<div class="jumbotron">
                <div class="jumbotron-contents">
                    <p>✅ IT勉強会を選んで、チェックインしましょう！</p>
                    @Html.ActionLink("IT勉強会の検索・登録", "Search", "Events", Nothing, New With {.class = "btn btn-primary"})
                </div>
            </div>
        End If

        @Html.Partial("_EventResults")

        <aside>
            @If Request.IsAuthenticated Then
                @<span>💡 チェックインするIT勉強会が見つかりませんか？　あなたが、<a href="@Url.Action("Search", "Events")">IT勉強会を登録してください</a>。</span>
            Else
                @<span>💡 チェックインするIT勉強会が見つかりませんか？　<a href="@Url.Action("Search", "Events")">ログイン</a>してIT勉強会を登録しましょう。</span>
            End If
        </aside>


    </div>
    <div class="col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

@Section Styles
    @Styles.Render("~/Content/datepicker3.css")
End Section

@Section Scripts

    @Scripts.Render("~/bundles/jqueryval")
    @Scripts.Render("~/Scripts/bootstrap-datepicker.js")
    @Scripts.Render("~/Scripts/locales/bootstrap-datepicker.ja.js")

    <script>
        $('.input-group.date').datepicker({
            startDate: "@now.AddYears(-1).ToString("yyyy/M/d")",
            endDate: "@now.AddYears(1).ToString("yyyy/M/d")",
            todayBtn: "linked",
            language: "ja",
            autoclose: true,
            todayHighlight: true
        }).datepicker("update", "@now.ToString("yyyy/MM/dd")");

    </script>
End Section






@ModelType SearchEventsViewModel

@Code
    Dim icon = "http://placehold.it/96x96"

End Code


@Html.Partial("_TopBanner")


<div class="row">
    <div class="col-sm-12 col-md-8">

        @If Not Request.IsAuthenticated Then
            @<div class="jumbotron">
                <div class="jumbotron-contents">
                    <p><a href="@Href("~/Home/About/")">IT勉強会スタンプ</a> は、IT勉強会の参加を記録できるWebサービスです。</p>
                    <p style="margin-bottom:0;">IT勉強会に参加してスタンプを集める “<a href="#">IT勉強会スタンプラリー</a>” を開催中！（※ 台紙でスタンプを集めます。Webサービスの記録機能とは関連していません。）</p>
                    <hr />
                    @Html.ActionLink("アカウント登録", "Register", "Account", Nothing, New With {.class = "btn btn-primary"})
                </div>
            </div>

        End If

        <h2 @(If(Request.IsAuthenticated, Html.Raw("style=""margin-top:0;"""), ""))>開催予定のIT勉強会</h2>

        @Html.Partial("_EventResults")

        @If Request.IsAuthenticated Then
            @<div>
                <a href="@Url.Action("Add", "Events")"><i class="glyphicon glyphicon-plus"></i> IT勉強会の登録</a>
            </div>
        End If

        <h2>外部サービスでIT勉強会を探す</h2>

        <ul class="list-unstyled">
            <li><a href="https://www.google.com/calendar/embed?src=ZnZpanZvaG05MXVpZnZkOWhyYXRlaGY2NWtAZ3JvdXAuY2FsZW5kYXIuZ29vZ2xlLmNvbQ" target="_blank">IT勉強会カレンダー</a></li>
            <li><a href="http://utf-8.jp/tool/calsearch.html" target="_blank">IT勉強会カレンダー検索</a></li>
        </ul>

        <h3>イベント登録サイト</h3>
        <ul class="list-inline">
            <li><a href="https://atnd.org/" target="_blank">ATND</a></li>
            <li><a href="http://connpass.com/" target="_blank">connpass</a></li>
            <li><a href="http://www.doorkeeper.jp/" target="_blank">Doorkeeper</a></li>
            <li><a href="http://partake.in/" target="_blank">PARTAKE</a></li>
            <li><a href="http://www.zusaar.com/" target="_blank">Zusaar</a></li>
            <li><a href="http://kokucheese.com/" target="_blank">こくちーず</a></li>
        </ul>

        @Html.Partial("_SocialButtons")

    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

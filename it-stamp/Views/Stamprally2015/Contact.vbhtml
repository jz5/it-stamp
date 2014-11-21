@Code
    ViewBag.Title = "お問い合わせ"
End Code

<div class="row">
    <div class="col-sm-12 col-md-8">
        <img src="@Href("~/images/stamprally2015/stamprally-logo.png")" alt="" class="img-responsive" />

        <h1>お問い合わせ</h1>
        <h2>メール</h2>
        <p>
            IT勉強会スタンプラリーに関わる内容は <mark>admin@@it-stamp.jp</mark> までお問い合わせください。
        </p>
        <h2>Twitter</h2>
        <p><a href="https://twitter.com/itstamp">@@itstamp</a> へもお気軽にツイートしてください。</p>

        <h2>コミュニティの参加について</h2>
        <p>参加を希望・検討されているコミュニティの方は、<a href="@Href("~/Stamprally/2015/Join")">コミュニティの参加</a> をご確認ください。</p>

    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

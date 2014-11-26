@Code
    ViewBag.Title = "IT勉強会スタンプラリー運営委員会"
End Code

<div class="row">
    <div class="col-sm-12 col-md-8">
        <img src="@Href("~/images/stamprally2015/stamprally-logo.png")" alt="" class="img-responsive" />

        <h1>IT勉強会スタンプラリー運営委員会</h1>
        <p>IT勉強会を主催しているコミュニティメンバーが集まり有志で運営しています。</p>

        <h2>組織</h2>
        <ul>
            <li>IT勉強会スタンプラリー運営委員会
                <ul>
                    <li>運営事務局： 参加者やコミュニティとの窓口</li>
                </ul>
            </li>
        </ul>

        <h2>運営委員</h2>
        <ul>
            <li>代表： @@jz5（<a href="http://pronama.jp/">プログラミング生放送</a>）</li>
            <li>副代表： @@kiyokura（<a href="http://oitec.vbstation.net/">Okayama IT Engineers Community</a>）</li>
            <li>他、若干名</li>
        </ul>

    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

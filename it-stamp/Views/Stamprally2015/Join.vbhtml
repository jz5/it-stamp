@Code
    ViewBag.Title = "コミュニティの参加"
End Code
<div class="row">
    <div class="col-sm-12 col-md-8">
        <a href="@Href("~/Stamprally/2015")"><img src="@Href("~/images/stamprally2015/stamprally-logo.png")" alt="" class="img-responsive" /></a>
        <h1>@ViewBag.Title</h1>
        <p>IT勉強会スタンプラリーに参加いただけるコミュニティを募集中です。</p>
        <p>参加コミュニティには、スタンプラリーの台紙とステッカーを送ります。また、スポンサーのノベルティのプレゼントもあります（開催時期などにより用意できない場合もあります）。</p>
        <h2>コミュニティの参加条件</h2>
        <p>
            1⃣ 参加は無料ですが、スタンプもしくはシール（オリジナル・既製品問わず）はご用意ください。
        </p>
        <p>
            2⃣ 参加した場合、スタンプラリーの運営に関わる作業（IT勉強会の登録および報告、スタンプの押印など）を実施していただくことになります。
            詳細は、下記の資料でご確認ください。
        </p>
        <div style="margin:50px 0 50px;">
            <iframe src="//www.slideshare.net/slideshow/embed_code/42118168" width="510" height="420" frameborder="0" marginwidth="0" marginheight="0" scrolling="no" style="border:1px solid #CCC; border-width:1px; margin-bottom:5px; max-width: 100%;" allowfullscreen> </iframe> <div style="margin-bottom:5px"> <strong> <a href="//www.slideshare.net/it-stamp/it-2015" title="IT勉強会スタンプラリー 2015 コミュニティ向け情報" target="_blank">IT勉強会スタンプラリー 2015 コミュニティ向け情報</a> </strong> from <strong><a href="//www.slideshare.net/it-stamp" target="_blank">IT 勉強会スタンプラリー</a></strong> </div>
        </div>
        <h2>コミュニティの参加方法</h2>
        <p>
            参加希望のコミュニティは、資料をご確認いただいた上で、<a href="@Href("~/Stamprally/2015/Contact")">運営事務局</a>までご連絡ください。
            <ul>
                <li>IT勉強会スタンプのユーザー名</li>
                <li>Googleグループに登録するアカウント</li>
                <li>コミュニティ情報（IT勉強会スタンプにコミュニティを登録してください）</li>
            </ul>
            参加検討されているコミュニティからの質問などもお待ちしています。
        </p>

        <h2>"参加済み"のコミュニティ向け情報</h2>
        <h3>台紙と記念品（ステッカー）の受け取り手順</h3>
        <p>下記の内容に追記して、admin@@it-stamp.jp へメールしてください。</p>
        <p class="text-muted">※ 複数のコミュニティを同一の方が管理しているなど、別コミュニティで宛先が同じになる場合は、コミュニティ・勉強会情報を複数記載するなどして、発送回数を少なくできるようご協力お願いします。</p>
        <script src="https://gist.github.com/jz5/741b2f75f6f4e3b4e015.js"></script>
        <h3>プライバシーポリシー</h3>
        <ul>
            <li><a href="http://www.microsoft.com/privacystatement/ja-jp/core/default.aspx">日本マイクロソフト株式会社 プライバシー</a></li>
            <li><a href="http://www.microsoft.com/ja-jp/mscorp/privacy/default.aspx">日本マイクロソフト株式会社 個人情報のお取り扱いについて</a></li>
            <li><a href="https://www.gmo.jp/privacy/">GMOインターネット株式会社 個人情報の取り扱いについて</a></li>
            <li><a href="http://leverages.jp/privacypolicy/">レバレジーズ株式会社 個人情報保護方針</a></li>
            <li><a href="http://cybozu.co.jp/company/copyright/privacy_policy.html">サイボウズ株式会社 個人情報保護の取り組みについて</a></li>
        </ul>
        @Html.Partial("_SocialButtons")
    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

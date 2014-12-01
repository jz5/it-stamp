@code
    Dim action = ViewContext.RouteData.GetRequiredString("action")
End Code
<div class="sidebar">
    <div class="row">
        <div class="col-xs-12 col-sm-12 hidden-md hidden-lg">
            <hr />
        </div>
    </div>

    <div class="list-group">
        <a href="@href("~/Stamprally/2015/")" class="list-group-item@(If(action = "Index", " active", ""))">IT勉強会スタンプラリー</a>
        <a href="@Href("~/Stamprally/2015/Schedule")" class="list-group-item@(If(action = "Schedule", " active", ""))">スケジュール</a>
        <a href="@Href("~/Stamprally/2015/Events")" class="list-group-item@(If(action = "Events", " active", ""))">IT勉強会一覧</a>
        <a href="@Href("~/Stamprally/2015/Communities")" class="list-group-item@(If(action = "Communities", " active", ""))">参加コミュニティ一覧</a>
        <a href="@Href("~/Stamprally/2015/QA")" class="list-group-item@(If(action = "QA", " active", ""))">Q &amp; A</a>
        <a href="@Href("~/Stamprally/2015/Sponsors")" class="list-group-item@(If(action = "Sponsors", " active", ""))">スポンサー紹介</a>
        <a href="@Href("~/Stamprally/2015/Join")" class="list-group-item@(If(action = "Join", " active", ""))">コミュニティの参加</a>
        <a href="@Href("~/Stamprally/2015/Committee")" class="list-group-item@(If(action = "Committee", " active", ""))">運営委員会</a>
        <a href="@Href("~/Stamprally/2015/Resources")" class="list-group-item@(If(action = "Resources", " active", ""))">素材</a>
        <a href="@Href("~/Stamprally/2015/Contact")" class="list-group-item@(If(action = "Contact", " active", ""))">お問い合わせ</a>
    </div>

    <div style="margin:40px 0">
        <script async src="//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js"></script>
        <!-- ITstamp2 -->
        <ins class="adsbygoogle"
             style="display:block"
             data-ad-client="ca-pub-5991185227226997"
             data-ad-slot="5499270866"
             data-ad-format="auto"></ins>
        <script>
            (adsbygoogle = window.adsbygoogle || []).push({});
        </script>
    </div>

    <h4><img src="@Href("~/images/stamprally2015/platinum.png")" alt="プラチナスポンサー" class="img-responsive" /></h4>

    <a href="@Href("~/Stamprally/2015/Sponsors")"><img src="@Href("~/images/stamprally2015/logo-microsoft.png")" srcset="@Href("~/images/stamprally2015/logo-microsoft2x.png") 2x" alt="日本マイクロソフト株式会社" class="img-responsive" style="max-width: 250px;margin:20px 0 40px;" /></a>

    <a href="@Href("~/Stamprally/2015/Sponsors")"><img src="@Href("~/images/stamprally2015/logo-conoha.png")" srcset="@Href("~/images/stamprally2015/logo-conoha2x.png") 2x" alt="GMOインターネット株式会社 ConoHa" class="img-responsive" style="max-width: 230px;margin:20px 0 40px;" /></a>

    <a href="@Href("~/Stamprally/2015/Sponsors")"><img src="@Href("~/images/stamprally2015/logo-forest.png")" srcset="@Href("~/images/stamprally2015/logo-forest2x.png") 2x" alt="窓の杜" class="img-responsive" style="max-width: 200px;margin:20px 0 40px;" /></a>

    <h4><img src="@Href("~/images/stamprally2015/gold.png")" alt="ゴールドスポンサー" class="img-responsive" /></h4>

    <a href="@Href("~/Stamprally/2015/Sponsors")"><img src="@Href("~/images/stamprally2015/logo-cybozulive.png")" srcset="@Href("~/images/stamprally2015/logo-cybozulive2x.png") 2x" alt="サイボウズLive" class="img-responsive" style="max-width: 190px;margin:20px 0 40px;" /></a>

    <h4><img src="@Href("~/images/stamprally2015/silver.png")" alt="シルバースポンサー" class="img-responsive" /></h4>

    <a href="@Href("~/Stamprally/2015/Sponsors")"><img src="@Href("~/images/stamprally2015/logo-gihyo.png")" srcset="@Href("~/images/stamprally2015/logo-gihyo2x.png") 2x" alt="技術評論社" class="img-responsive" style="max-width: 150px;margin:20px 0 40px;" /></a>

    <div style="margin:40px 0">
        <script async src="//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js"></script>
        <!-- ITstamp2 -->
        <ins class="adsbygoogle"
             style="display:block"
             data-ad-client="ca-pub-5991185227226997"
             data-ad-slot="5499270866"
             data-ad-format="auto"></ins>
        <script>
            (adsbygoogle = window.adsbygoogle || []).push({});
        </script>
    </div>

</div>
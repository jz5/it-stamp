@Code
    ViewBag.Title = "スポンサー紹介"
End Code

<div class="row">
    <div class="col-sm-12 col-md-8">
        <img src="@Href("~/images/stamprally2015/stamprally-logo.png")" alt="" class="img-responsive" />

        <h1>スポンサー紹介</h1>
        <p>IT勉強会スタンプラリー 2015 に協賛いただいているスポンサーを紹介します。スタンプラリーの運営は、協賛金および当サイトの収入により行っています。</p>


        <h2><img src="@Href("~/images/stamprally2015/platinum.png")" alt="プラチナスポンサー" class="img-responsive" /></h2>

        <div class="row">
            <div class="col-md-4" style="vertical-align:top;">
                <img src="@Href("~/images/stamprally2015/logo-microsoft.png")" srcset="@Href("~/images/stamprally2015/logo-microsoft2x.png") 2x" alt="日本マイクロソフト株式会社" class="img-responsive" style="margin-top:30px;" />
            </div>
            <div class="col-md-8">
                <h3>日本マイクロソフト株式会社</h3>
                <h4><a href="http://www.microsoft.com/ja-jp/communities/mvp/default.aspx">マイクロソフト MVP アワード プログラム</a></h4>
                <p>MVP (Most Valuable Professional) アワード プログラムは、マイクロソフトの製品やテクノロジーに関する豊富な知識と経験を持ち、オンラインまたはオフラインのコミュニティや、メディアなどを通して、その優れた能力を幅広いユーザーと共有している個人を表彰するものです。MVP アワードの表彰は全世界で行われており、現在は、世界 90 か国以上、4,000 名を超える方々が MVP として精力的な活動を続けています。</p>
            </div>
        </div>

        <div class="row" style="margin-top:60px;">

            <div class="col-md-4">
                <a href="https://www.conoha.jp/"><img src="@Href("~/images/stamprally2015/logo-conoha.png")" srcset="@Href("~/images/stamprally2015/logo-conoha2x.png") 2x" alt="GMOインターネット株式会社 ConoHa" class="img-responsive" style="margin-top:30px;" /></a>
            </div>
            <div class="col-md-8">
                <h3>GMOインターネット株式会社</h3>
                <h4><a href="https://www.conoha.jp/">ConoHa</a></h4>
                <p>準備中</p>
            </div>

        </div>

        <div class="row" style="margin-top:60px;">

            <div class="col-md-4">
                <a href="http://www.forest.impress.co.jp/"><img src="@Href("~/images/stamprally2015/logo-forest.png")" srcset="@Href("~/images/stamprally2015/logo-forest2x.png") 2x" alt="窓の杜" class="img-responsive" style="margin-top:30px;" /></a>
            </div>
            <div class="col-md-8">
                <h3>株式会社インプレス</h3>
                <h4><a href="http://www.forest.impress.co.jp/">窓の杜</a></h4>
                <p>窓の杜（まどのもり）は、株式会社インプレスが運営するWindows向けオンラインソフトの情報サイトです。1996年よりオンラインソフトに関するニュースやレビューの配信と、ソフトウェアライブラリサービスの提供を続けています。</p>
            </div>
        </div>


        <h2><img src="@Href("~/images/stamprally2015/gold.png")" alt="ゴールドスポンサー" class="img-responsive" /></h2>


        <div class="row">

            <div class="col-md-4">
                <a href="http://live.cybozu.co.jp/"><img src="@Href("~/images/stamprally2015/logo-cybozulive.png")" srcset="@Href("~/images/stamprally2015/logo-cybozulive2x.png") 2x" alt="サイボウズLive" class="img-responsive" style="margin-top:30px;" /></a>
            </div>
            <div class="col-md-8">

                <h3>サイボウズ株式会社</h3>
                <h4><a href="http://live.cybozu.co.jp/">サイボウズLive</a></h4>
                <p>サイボウズ Live は無料で使えるコラボレーションツールです。メンバーだけでファイル共有やディスカッションができる情報共有スペースを、インターネット上で簡単に作ることができます。ソフトウェア開発プロジェクトの情報共有にぴったりのサービスです。</p>

            </div>
        </div>


        <h2><img src="@Href("~/images/stamprally2015/silver.png")" alt="シルバースポンサー" class="img-responsive" /></h2>



        <div class="row">

            <div class="col-md-4">
                <a href="http://gihyo.jp/"><img src="@Href("~/images/stamprally2015/logo-gihyo.png")" srcset="@Href("~/images/stamprally2015/logo-gihyo2x.png") 2x" alt="技術評論社" class="img-responsive" style="max-width: 150px;margin:20px 0 40px;" /></a>
            </div>
            <div class="col-md-8">

                <h3>株式会社技術評論社</h3>
                <h4><a href="http://gihyo.jp/">gihyo.jp</a></h4>
                <p>技術評論社は、1969年の創業以来、IT や理工を中心とした専門分野に特化した書籍、『Software Design』『WEB+DB PRESS』の二大看板雑誌、ビジネスや趣味などを対象とした優良な刊行物を提供し続け、そして新たに電子出版分野に積極的に取り組んでいる出版社です。</p>

            </div>
        </div>


    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

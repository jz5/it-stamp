@Code
    ViewBag.Title = "IT勉強会スタンプラリー 2015"
End Code
@section head
    <meta name="twitter:card" content="summary" />
    <meta name="twitter:site" content="@Html.Raw("@itstamp")" />
    <meta name="twitter:title" content="@ViewBag.Title" />
    <meta name="twitter:description" content="IT勉強会スタンプラリーは、IT勉強会に参加してスタンプを集める無料のイベントです。" />
    <meta name="twitter:image:src" content="https://itstamp.azurewebsites.net/images/stamprally2015/nareru2.jpg">
End Section
<div class="row">
    <div class="col-sm-12 col-md-8">

        <img src="@Href("~/images/stamprally2015/nareru2.jpg")" class="img-responsive img-rounded" />

        <h2>🎉 開催概要</h2>
        <div>
            <p>IT勉強会スタンプラリーは、IT勉強会に参加してスタンプを集める無料のイベントです。</p>

            <h3>IT 勉強会に参加すると楽しい！</h3>


            <p>
                毎日各所でIT勉強会が開催されています。まだ参加したことのない人はぜひ参加してみてください。
            </p>

            <h3>スタンプを集めると楽しい！</h3>

            <p>対象のIT勉強会に参加するとスタンプがもらえます。また、集めたスタンプの数によって参加記念品がもらえます。</p>

            <h3>IT勉強会スタンプラリーとは</h3>

            <ul>
                <li>IT勉強会に参加してスタンプ（またはシール）を集めるイベントです。</li>
                <li>全国で 2014月12月 から 2015年6月 まで開催します（予定）。</li>
                <li>対象の IT 勉強会 に参加すると台紙にスタンプを押してもらえます。</li>
                <li>スタンプを集めると参加記念品がもらえます。</li>
                <li>誰でも参加でき、参加は無料です。</li>
            </ul>
        </div>


        <h2>🎈 参加してみよう！</h2>
        <div>

            <h3>参加方法</h3>
            <ol>
                <li>スタンプラリーを実施しているIT勉強会に参加します。</li>
                <li>はじめて参加した場合、台紙をもらいます。</li>
                <li>スタンプを押してもらいます。</li>
                <li>2回目の参加から台紙を持参してください（※ 忘れても大丈夫です）。</li>
            </ol>

            <h3>スタンプラリーのルール</h3>

            <ul>
                <li>対象のIT勉強会に参加すると1個スタンプがもらえます。</li>
                <li>参加時と、記念品の引き渡し期間中に、参加記念品がもらえます。</li>
                <li>すべてのスタンプを集める必要はありません。</li>
                <li>台紙を忘れても続けられます。IT勉強会で新しい台紙とその回のスタンプをもらってください。記念品の受け渡し時、複数の台紙がある場合はスタンプを合算して計算します。</li>
            </ul>

            <h3>🎁 参加記念品</h3>
            <p>1個集めると（1回参加すると）、ステッカーをプレゼント！<br />ステッカーも台紙も数種類あるので、いろいろなIT勉強会に参加してみてください。</p>
            <p>2個以上は、クリアフォルダーを予定しています（郵送でお渡し）。</p>
            <p>スタンプ数上位者と、抽選による、参加記念品も用意します。</p>

            <h3>記念品の受け取り方法</h3>
            <p>記念品の受け取りは、台紙の写真のアップロードと宛先などをこのサイトで登録してください（記念品の発送以外で個人情報は利用しません）。</p>
            <p>詳細は、このサイトでお知らせします。</p>
        </div>


        <h2>参加コミュニティ募集中！</h2>
        <div>

            <p>IT勉強会スタンプラリーに参加いただけるコミュニティを募集中です。</p>
            <p>参加コミュニティには、スタンプラリーの台紙を送ります。また、スポンサーのノベルティのプレゼントもあります（開催時期などにより用意できない場合もあります）。</p>

            <a href="@Href("~/Stamprally/2015/Join")" class="btn btn-default">コミュニティの参加</a>

        </div>


        @Html.Partial("_SocialButtons")
    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

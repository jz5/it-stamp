<aside class="social-buttons">
    <ul class="list-inline" style="margin-left:0;">
        <li>
            <a href="http://b.hatena.ne.jp/entry/@Request.Url"
               class="hatena-bookmark-button"
               data-hatena-bookmark-title="@(If(ViewBag.Title = "", "IT勉強会スタンプ", ViewBag.Title & " | IT勉強会スタンプ"))"
               data-hatena-bookmark-layout="vertical-balloon"
               title="このエントリーをはてなブックマークに追加">
                <img src="https://b.hatena.ne.jp/images/entry-button/button-only.gif"
                     alt="このエントリーをはてなブックマークに追加"
                     width="20" height="20" style="border: none;" />
            </a>
        </li>
        <li>
            <a href="https://twitter.com/share" class="twitter-share-button" data-via="itstamp" data-lang="ja" data-related="itstamp" data-hashtags="itstamp" data-count="vertical">ツイート</a>
        </li>
        <li><div class="fb-like" data-href="@Request.Url" data-layout="box_count" data-action="like" data-show-faces="false" data-share="false"></div></li>
        <li>
            <div class="g-plusone" data-size="tall"></div>
        </li>
        <li>
            <a data-pocket-label="pocket" data-pocket-count="vertical" class="pocket-btn" data-lang="en"></a>
        </li>
    </ul>

    <script type="text/javascript"
            src="https://b.hatena.ne.jp/js/bookmark_button.js"
            charset="utf-8"
            async="async">
    </script>
    <script>!function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0], p = /^http:/.test(d.location) ? 'http' : 'https'; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = p + '://platform.twitter.com/widgets.js'; fjs.parentNode.insertBefore(js, fjs); } }(document, 'script', 'twitter-wjs');</script>
    <div id="fb-root"></div>
    <script>
        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) return;
            js = d.createElement(s); js.id = id;
            js.src = "//connect.facebook.net/ja_JP/sdk.js#xfbml=1&appId=767727909916605&version=v2.0";
            fjs.parentNode.insertBefore(js, fjs);
        }(document, 'script', 'facebook-jssdk'));</script>

    <script src="https://apis.google.com/js/platform.js" async defer>
        { lang: 'ja' }
    </script>
    <script type="text/javascript">!function (d, i) { if (!d.getElementById(i)) { var j = d.createElement("script"); j.id = i; j.src = "https://widgets.getpocket.com/v1/j/btn.js?v=1"; var w = d.getElementById(i); d.body.appendChild(j); } }(document, "pocket-btn-js");</script>

</aside>
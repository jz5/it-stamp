@code

    Dim db = New ApplicationDbContext
    Dim checkIns = db.CheckIns.OrderByDescending(Function(c) c.DateTime).Take(10).ToList
    Dim anonIcon = Href("/Uploads/Icons/anon.png")

End Code
<div class="sidebar">
    <div class="row">
        <div class="col-xs-12 col-sm-12 hidden-md hidden-lg">
            <hr />
        </div>
    </div>
    <div style="margin:0 0 30px">
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
    <h3><a href="@Href("~/Stamprally/2015/")"><img src="@Href("~/images/stamprally2015/stamprally-logo.png")" class="img-responsive" alt="IT勉強会スタンプラリー" /></a></h3>
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
    <div>
        <h2>✅ みんなのチェックイン</h2>
        <ul class="list-unstyled">
            @For Each item In checkIns
                @<li style="margin-bottom:10px;">
                    @If item.User.IsPrivate Then
                        @<img class="img-rounded icon24" title="プライベートユーザー" src="@anonIcon" />
                    Else
                        @<a href="@Href("/Users/")@item.User.UserName" title="@item.User.FriendlyName"><img class="img-rounded icon24" src="@Href(item.User.GetIconPath)" /></a>
                    End If
                    <a href="@Href("/Events/")@item.Event.Id">@item.Event.Name</a> に ✅
                    <time class="text-muted small" datetime="@item.DateTime.ToString("yyyy-MM-ddTH:mm:ssK")">@item.DateTime.ToString("MM/dd HH:mm")</time>
                </li>
            Next
        </ul>
    </div>
</div>
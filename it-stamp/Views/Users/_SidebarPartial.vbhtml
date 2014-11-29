@Imports Microsoft.AspNet.Identity
@code
    Dim action = ViewContext.RouteData.GetRequiredString("action")
    Dim userName = User.Identity.Name
End Code
<div class="sidebar">

    @*<div class="list-group">
            <a href="@Href("~/Users/" & userName & "/CheckIns/")" class="list-group-item@(If(action = "CheckIns", " active", ""))">チェックイン</a>
        </div>*@

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
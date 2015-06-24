@Code
    ViewBag.Title = "記念品の交換"
End Code

<div class="row">
    <div class="col-sm-12 col-md-8">
        <a href="@Href("~/Stamprally/2015")"><img src="@Href("~/images/stamprally2015/stamprally-logo.png")" alt="" class="img-responsive" /></a>

        <h1>@ViewBag.Title</h1>

        <p class="text-muted">スタンプが2個以上集めた方は、記念品をおくります。</p>
        <p class="text-muted">近日公開。</p>


        @Html.Partial("_SocialButtons")
    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

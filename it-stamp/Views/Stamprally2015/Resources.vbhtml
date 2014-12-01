@Code
    ViewBag.Title = "素材"
End Code

<div class="row">
    <div class="col-sm-12 col-md-8">
        <a href="@Href("~/Stamprally/2015")"><img src="@Href("~/images/stamprally2015/stamprally-logo.png")" alt="" class="img-responsive" /></a>

        <h1>@ViewBag.Title</h1>
        <p>IT勉強会スタンプラリーの紹介に使える、画像やスライドをダウンロードできます。ご自由にお使いください。</p>

        <p class="text-muted">準備中</p>

        @Html.Partial("_SocialButtons")
    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

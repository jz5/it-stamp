@Code
    ViewBag.Title = "スケジュール"
End Code

<div class="row">
    <div class="col-sm-12 col-md-8">
        <a href="@Href("~/Stamprally/2015")"><img src="@Href("~/images/stamprally2015/stamprally-logo.png")" alt="" class="img-responsive" /></a>

        <h1>📅 @ViewBag.Title</h1>

        <p>開催期間は、2014/12/1～2015/6/30です（予定）。</p>
        <table class="table">
            <tr>
                <td>2014年12月1日</td>
                <td>IT勉強会スタンプラリー 2015 開始</td>
            </tr>
            <tr>
                <td>2015年3月下旬</td>
                <td>記念品（クリアフォルダー）の受け取り手続きをWebで受け付け</td>
            </tr>
            <tr>
                <td>2015年4月上旬</td>
                <td>記念品の発送</td>
            </tr>
            <tr>
                <td>2015年6月下旬</td>
                <td>記念品の受け取り手続きをWebで受け付け</td>
            </tr>
            <tr>
                <td>2015年6月30日</td>
                <td>IT勉強会スタンプラリー 2015 終了（予定）</td>
            </tr>
            <tr>
                <td>2015年6月上旬</td>
                <td>記念品の発送</td>
            </tr>
        </table>

    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

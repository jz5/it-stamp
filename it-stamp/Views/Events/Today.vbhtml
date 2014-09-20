@Code
    ViewBag.Title = "今日のIT勉強会"
End Code

@Html.Partial("_TopBanner")



<div class="row">
    <div class="col-sm-12 col-md-8">

        <h1>@ViewBag.Title</h1>

        @Html.Partial("_EventResults")


    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

@Code
    ViewData("Title") = "Today"
End Code

@Html.Partial("_TopBanner")



<div class="row">
    <div class="col-sm-12 col-md-8">

        <h2 @(If(Request.IsAuthenticated, Html.Raw("style=""margin-top:0;"""), ""))>開催予定のIT勉強会</h2>

        @Html.Partial("_EventResults")


    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

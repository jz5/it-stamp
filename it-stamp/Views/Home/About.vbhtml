@Code
    ViewBag.Title = "About"
End Code

<div class="row">
    <div class="col-sm-12 col-md-8">

        <h1>@ViewBag.Title</h1>

        <p>準備中</p>
        

    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

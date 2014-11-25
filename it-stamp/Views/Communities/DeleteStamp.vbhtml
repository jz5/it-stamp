@ModelType Stamp

@Code
    ViewData("Title") = "スタンプの削除"
End Code

@Html.Partial("_TopBanner")
<div class="row">
    <div class="col-sm-12 col-md-8">
        <h1>@ViewBag.Title</h1>
        <div class="panel">
            <div class="panel-body">
                @Html.Label(Model.Name)
                <p><img class="img-rounded" src="@Href("/Uploads/" & Model.Path)" /></p>
                @Html.Label(Model.Expression)
            </div>
        </div>
        <p>削除してもよろしいですか？</p>
        @Using Html.BeginForm("RemoveStamp", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
            @Html.AntiForgeryToken()
            @Html.Hidden("communityId", Model.Community.Id)
            @Html.Hidden("stampId", Model.Id)

            @Html.ValidationSummary(False, "", New With {.class = "text-danger"})

            @<div class="form-group">
                <div class="form-inline">
                    <input id="follow-btn" type="submit" value="削除" class="btn btn-primary" />
                </div>
            </div>

        End Using
        <hr />
        @Html.ActionLink("戻る", "Edit", "Communities", New With {.id = Model.Community.Id}, Nothing)
    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

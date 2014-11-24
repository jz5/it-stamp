@ModelType ApplicationUser
@Code
    ViewData("Title") = "管理者の除外"
End Code

<h2>DeleteOwner</h2>

@Html.Partial("_TopBanner")
<div class="row">
    <div class="col-sm-12 col-md-8">
        <h1>@ViewBag.Title</h1>
        <div class="panel">
            <div class="panel-body">
                <p><img class="img-rounded" src="@(If(Model.IconPath <> "", Href("/Uploads/" & Model.IconPath), "http://placehold.it/96x96"))" /></p>
                @Html.Label(Model.FriendlyName)
            </div>
        </div>
        <p>コミュニティの管理者から除外してもよろしいですか？</p>
        @Using Html.BeginForm("RemoveOwner", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
            @Html.AntiForgeryToken()
            @Html.Hidden("communityId", ViewBag.CommunityId)
            @Html.Hidden("userId", Model.Id)

            @Html.ValidationSummary(False, "", New With {.class = "text-danger"})

            @<div class="form-group">
                <div class="form-inline">
                    <input id="follow-btn" type="submit" value="削除" class="btn btn-primary" />
                </div>
            </div>

        End Using
        <hr />
        @Html.ActionLink("戻る", "Edit", "Communities", New With {.id = ViewBag.CommunityId}, Nothing)
    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>

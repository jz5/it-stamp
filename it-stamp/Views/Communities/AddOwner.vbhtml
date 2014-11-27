@ModelType AddCommunityOwnerViewModel
@Code
    ViewBag.Title = "管理者の追加"
End Code

<h1>@ViewBag.Title</h1>
@Using Html.BeginForm("AddOwner", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
    @Html.AntiForgeryToken()
    @<text>
        @Html.Hidden("id", Model.Id)
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
        <div class="form-group">
            @Html.LabelFor(Function(m) m.UserName, New With {.class = "control-label"})
            <span class="text-muted">（英数字）</span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.UserName, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.UserName, "", New With {.class = "text-danger"})
            </div>
        </div>
        <div class="form-group">
            <input type="submit" class="btn btn-primary" value="登録" />
        </div>
    </text>
End Using
<hr />
@Html.ActionLink("戻る", "Edit", "Communities", New With {.id = Model.Id}, Nothing)

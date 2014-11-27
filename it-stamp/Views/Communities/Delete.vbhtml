@ModelType Community

@Code
    ViewBag.Title = "コミュニティの削除"

End Code

<h1>@ViewBag.Title</h1>
<div class="jumbotron">
    <div class="jumbotron-contents">
        @Html.Partial("_CommunityCard", Model)
    </div>
</div>

<p>💡 削除すると元に戻せません。</p>

@If ViewBag.CanDelete Then
    @Using Html.BeginForm("Delete", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
        @Html.AntiForgeryToken()
        @Html.HiddenFor(Function(m) m.Id)
        @Html.HiddenFor(Function(m) m.Name)

        @Html.ValidationSummary(False, "", New With {.class = "text-danger"})

        @<div class="form-group">
            <div class="form-inline">
                <input id="follow-btn" type="submit" value="削除" class="btn btn-primary" />
            </div>
        </div>

    End Using
Else
    @<p>削除できません。</p>
End If

<hr />
@Html.ActionLink("戻る", "Edit", "Communities", New With {.id = Model.Id}, Nothing)


@ModelType Community
@Imports Microsoft.AspNet.Identity
@Code
    ViewBag.Title = "コミュニティの編集"
    Dim icon = If(Model.IconPath <> "", Href("/Uploads/" & Model.IconPath), "http://placehold.it/96x96")

End Code

<h1>@ViewBag.Title</h1>

@Using Html.BeginForm("Edit", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
    @Html.AntiForgeryToken()
    @<text>
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
        @Html.HiddenFor(Function(m) m.IconPath)

        <div class="form-group">
            @Html.LabelFor(Function(m) m.Name, New With {.class = "control-label"}) <span class="text-primary">*</span>
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.Name, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Name, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(m) m.Description, New With {.class = "control-label"})
            <span class="text-muted">（簡潔な紹介）</span>
            <div class="form-inline">
                @Html.EditorFor(Function(m) m.Description, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(m) m.Description, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(m) m.Url, New With {.class = "control-label"})
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.Url, New With {.type = "url", .class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Url, "", New With {.class = "text-danger"})
            </div>
        </div>

        @If ViewBag.IsOwner Then
        @<div class="form-group">
            @Html.LabelFor(Function(m) m.IsLocked, New With {.class = "control-label"})
            <span class="text-muted"></span>
            @Html.CheckBoxFor(Function(m) m.IsLocked)
            @Html.ValidationMessageFor(Function(m) m.IsLocked, "", New With {.class = "text-danger"})
        </div>

        @<div class="form-group">
            @Html.LabelFor(Function(m) m.IsHidden, New With {.class = "control-label"})
            <span class="text-muted"></span>
            @Html.CheckBoxFor(Function(m) m.IsHidden)
            @Html.ValidationMessageFor(Function(m) m.IsHidden, "", New With {.class = "text-danger"})
        </div>
            
        End If

        <div class="form-group">
            <input type="submit" value="保存" class="btn btn-primary" />
        </div>
    </text>
        End Using


<hr />
<img class="media-object img-rounded" src="@icon" alt="@Model.Name" style="margin-bottom: 30px;" />
@Html.ActionLink("変更", "Upload", "Communities", New With {.id = Model.Id}, New With {.class = "btn btn-default"})

<hr />

@If ViewBag.IsOwner Then
    @<h2>コミュニティスタンプの管理</h2>
    @Html.ActionLink("編集", "EditStamps", "Communities", New With {.id = Model.Id}, New With {.class = "btn btn-default"})
    @<hr />
    @<h2>コミュニティ管理者</h2>
    @Html.ActionLink("編集", "EditOwners", "Communities", New With {.id = Model.Id}, New With {.class = "btn btn-default"})
    @<hr />
End If
<hr />
<h2>コミュニティの削除</h2>
@Html.ActionLink("削除", "Delete", "Communities", New With {.id = Model.Id}, New With {.class = "btn btn-default"})

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section

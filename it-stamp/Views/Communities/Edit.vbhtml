@ModelType Community
@Imports Microsoft.AspNet.Identity
@Code
    ViewBag.Title = "コミュニティの編集"
    Dim icon = If(Model.IconPath <> "", Href("/Uploads/" & Model.IconPath), Href("/Uploads/Icons/no-community.png"))

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
        @If ViewBag.CanEditDetails Then
            @<div class="form-group">
                <div class="checkbox">
                    @Html.CheckBoxFor(Function(m) m.IsLocked)
                    @Html.LabelFor(Function(m) m.IsLocked, New With {.class = "control-label"})
                    <span class="text-muted"></span>
                    @Html.ValidationMessageFor(Function(m) m.IsLocked, "", New With {.class = "text-danger"})
                </div>
            </div>

            @<div class="form-group">
                <div class="checkbox">
                    @Html.CheckBoxFor(Function(m) m.IsHidden)
                    @Html.LabelFor(Function(m) m.IsHidden, New With {.class = "control-label"})
                    <span class="text-muted"></span>
                    @Html.ValidationMessageFor(Function(m) m.IsHidden, "", New With {.class = "text-danger"})
                </div>
            </div>
        End If

        <div class="form-group">
            <input type="submit" value="保存" class="btn btn-primary" />
        </div>
    </text>
End Using

<h2>アイコン</h2>
<img class="media-object img-rounded" src="@icon" alt="@Model.Name" style="margin-bottom: 30px;" />
<div class="form-group">
    @Html.ActionLink("変更", "Upload", "Communities", New With {.id = Model.Id}, New With {.class = "btn btn-default"})
</div>

@If ViewBag.CanEditDetails Then
    @<h2>スタンプ</h2>
    @<div class="form-group">
        @Html.ActionLink("編集", "EditStamps", "Communities", New With {.id = Model.Id}, New With {.class = "btn btn-default"})
    </div>
    @<h2>コミュニティ管理者</h2>
    @<div class="form-group">
        @Html.ActionLink("編集", "EditOwners", "Communities", New With {.id = Model.Id}, New With {.class = "btn btn-default"})
    </div>
End If

@If ViewBag.CanDelete Then
    @<h2>コミュニティの削除</h2>
    @<div class="form-group">
        @Html.ActionLink("削除", "Delete", "Communities", New With {.id = Model.Id}, New With {.class = "btn btn-default"})
    </div>
End If

@If Not ViewBag.CanEditDetails Then
    @<aside>
        <span>💡 @(Model.Name)の方ですか？　<a href="@Href("~/Home/Contact/")">申請</a>すると詳細情報を編集できます。</span>
    </aside>
End If

@Section Styles
    @Styles.Render("~/Content/skins/square/blue.css")
End Section

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    <script>
        (function ($) {
            $('input').iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-blue'
            });
        })(jQuery);
    </script>
End Section

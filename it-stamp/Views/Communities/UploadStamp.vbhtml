@ModelType UploadCommunityStampViewModel
@Imports Microsoft.AspNet.Identity
@Code
    ViewBag.Title = "スタンプの登録"
End Code
<h1>@ViewBag.Title</h1>
@Using Html.BeginForm("UploadStamp", "Communities", FormMethod.Post, New With {.class = "form-horizontal", .role = "form", .enctype = "multipart/form-data"})
    @Html.AntiForgeryToken()
    @<text>
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
        <div class="form-group">
            @Html.LabelFor(Function(m) m.Name, New With {.class = "control-label"}) <span class="text-primary">*</span>
            <span class="text-muted"></span>
            <div class="form-inline">
                @Html.TextBoxFor(Function(m) m.Name, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Name, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            <div style="margin: 30px 0 15px;">
                <div class="form-inline">
                    @Html.TextBoxFor(Function(m) m.File, New With {.class = "form-control", .type = "file"})
                    @Html.ValidationMessageFor(Function(m) m.File, "", New With {.class = "text-danger"})
                </div>
            </div>
        </div>
        <ul class="text-info">
            <li>PNG/JPEG形式のみ</li>
            <li>96x96ピクセル推奨</li>
            <li>他者の権利を侵害する画像をアップロードしないでください。</li>
        </ul>
    </text>
End Using

<hr />
@Html.ActionLink("戻る", "Edit", "Communities", New With {.id = Model.Id}, Nothing)
@Section Styles
    @Styles.Render("~/Content/fileinput.css")
End Section

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    @Scripts.Render("~/Scripts/fileinput.js")
    <script>
        $("#File").fileinput({
            browseLabel: "選択...",
            uploadLabel: "アップロード",
            uploadClass: "btn btn-primary",
            removeLabel: "解除",
            browseClass: "btn btn-default",
            maxFileSize: 1024,
            maxFileCount: 1,
            msgSizeTooLarge: '<b>{maxSize} KB</b> 以下のファイル選択してください。"{name}" (<b>{size} KB</b>) '
        });
    </script>
End Section
